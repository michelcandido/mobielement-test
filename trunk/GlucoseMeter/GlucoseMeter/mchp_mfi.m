//
//  mchp_mfi.m
//  MCHP MFI
//


#import "mchp_mfi.h"

NSString *EADSessionDataReceivedNotification = @"EADSessionDataReceivedNotification";

@implementation mchp_mfi

@synthesize notifDelegate;


// OVERLOAD ME!!
- (int) readData:(NSData *) data
{
	return [data length];
}

- (void) dealloc
{
    NSLog(@"[PZ]: dealloc in mchp_mfi");

    [self closeSession];
    [_accessory release];
    [_protocolString release];
    
    [super dealloc];
    
}

- (BOOL) initWithProtocol:(NSString *)protocol
{
    // redirect stderr to file
    if(LOG_TO_FILE == 1)
    {
        NSString *cacheDir = [NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES) objectAtIndex:0];
        NSString *logPath = [cacheDir stringByAppendingPathComponent:@"application.log"];
        freopen([logPath cStringUsingEncoding:NSASCIIStringEncoding], "a+", stderr);
	}
    _protocolString = protocol;
    [_accessory release];
    [_session release];
    
    
	// see if an accessory is already attached
    NSLog(@"[PZ]: Getting a list of accessories");
    NSArray *accessories = [[EAAccessoryManager sharedAccessoryManager]
                            connectedAccessories];
	
	NSLog(@"[PZ]: Found %d accessories",[accessories count]);
    NSLog(@"[PZ]: Looking for %@",_protocolString);
    
	for (EAAccessory *obj in accessories)
    {
		NSArray *sa = [obj protocolStrings];
		//NSLog(@"a string dump of %d strings",[sa count]);
		
		for(NSString *s in sa)
		{
			NSLog(@"[PZ]: %@",s);
		}
		
        if ([sa containsObject:_protocolString])
        {
            _accessory = obj;
            [_accessory retain];
            NSLog(@"[PZ]: Found accessory matching the protocol");
            break;
        }
        
    }
    if (_accessory != nil) {
        NSLog(@"[PZ]: Found a compatible accessory.");
        return TRUE;
    }
    else {
        NSLog(@"[PZ]: Failed to find a compatible accessory.");
        return FALSE;
    }
        
    
 }

// This is called by app to check accessory status
- (bool) isConnected
{
    
    if(_session!= nil)
        return [[_session accessory] isConnected];
    else{
        // we don't do init here. The protocol has its own timer to do this
        //[self init];
        //NSLog(@"[PZ]: chekcing if accessory is connected: session object not initialized.");
        return FALSE;
    }
 
}

- (NSString *) name { return (_session)?[[_session accessory] name]:@"-"; }
- (NSString *) manufacturer { return (_session)?[[_session accessory] manufacturer]:@"-";}
- (NSString *) modelNumber {return (_session)?[[_session accessory] modelNumber]:@"-";}
- (NSString *) serialNumber {return (_session)?[[_session accessory] serialNumber]:@"-";}
- (NSString *) firmwareRevision {return (_session)?[[_session accessory] firmwareRevision]:@"-";}
- (NSString *) hardwareRevision {return (_session)?[[_session accessory] hardwareRevision]:@"-";}

/***********************************************************************/
#pragma mark External Accessory Basic Identification 
/***********************************************************************/

- (BOOL)openSession 
{
    [_accessory setDelegate:self];
    
    if(!_session){
        NSLog(@"[PZ]: opening the session for this accessory");
        _session = [[EASession alloc] initWithAccessory:_accessory forProtocol:_protocolString];
                    
    }
      
    if (_session)
    {
        NSLog(@"[PZ]: opening the streams for this accessory");
        [[_session inputStream] setDelegate:self];
        [[_session inputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop]
                                         forMode:NSDefaultRunLoopMode];
        [[_session inputStream] open];
        [[_session outputStream] setDelegate:self];
        [[_session outputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop]
                                          forMode:NSDefaultRunLoopMode];
        [[_session outputStream] open];
      
        streamReady = true;
        receivedAccPkt = true;
        
    }
    
    else
        NSLog(@"[PZ]: Create Session Failed.");
    return (_session != nil);


}

- (void) closeSession {
    
    NSLog(@"[PZ]: protocol closing session...");
    [[_session inputStream] removeFromRunLoop:[NSRunLoop currentRunLoop]
                                      forMode:NSDefaultRunLoopMode];
    [[_session inputStream] setDelegate:nil];
    [[_session inputStream] close];
     
    [[_session outputStream] removeFromRunLoop:[NSRunLoop currentRunLoop]
                                       forMode:NSDefaultRunLoopMode];
    [[_session outputStream] setDelegate:nil];
    [[_session outputStream] close];
    
    [_session release];
    _session = nil;
    
    [_accessory setDelegate:nil];
    [_accessory release];
    _accessory = nil;
  
}



#pragma mark Stream Processing
// Handle communications from the streams.
- (void)stream:(NSStream*)stream handleEvent:(NSStreamEvent)streamEvent
{
    uint8_t buf[36];
    bzero(buf, sizeof(buf));
    unsigned int len = 0;
	int count = 0;
    
    switch (streamEvent)
    {
            
        case NSStreamEventHasBytesAvailable:
            // Process the incoming stream data.
            // get the new bytes from the stream
            while((len = [(NSInputStream *)stream read:buf maxLength:sizeof(buf)]))
            {
				if(rxData == nil) rxData = [[NSMutableData data] retain];
				[rxData appendBytes:buf length:len];
				count += len;
			}
            
            NSLog(@"[PZ]: Pick up: %@", rxData);
			
			// loop here to unload the receive queue
			// find a preamble
			const char preambleBytes[] = {0x5A,0xA5};
			NSRange r = {0,sizeof(preambleBytes)};
			NSData	*pd = [NSData dataWithBytes:preambleBytes length:2];

			r.length = 2;
			while ((r.length + r.location) < [rxData length])
			{
				if([[rxData subdataWithRange:r] isEqualToData:pd])
				{
					if( (r.location + r.length) > [rxData length])
						break;
					r.location += 2;
					r.length = [rxData length] - r.location;
					len = [self readData:[rxData subdataWithRange:r]];
					r.location += len;
				}
				else
				{
					r.location ++;
				}
				
				r.length = 2;
			}
            
			if(r.location >= [rxData length])
			{
				[rxData release];
				rxData = nil;
			}
			else 
			{
				// reset the receiver with the bytes that were not consumed.
				[rxData setData:[rxData subdataWithRange:r]];
			}

			//NSLog(@"[PZ]: After parsing: %@", rxData);
            break;
        case NSStreamEventHasSpaceAvailable:
            // Send the next queued command.
			@synchronized(self)
			{
				[self txBytes];
			}
            break;
        case NSStreamEventErrorOccurred:
            NSLog(@"[PZ]: Stream Error Occured");
            break;
        default:
            break;
    }
}


#pragma mark accessory notifications

/* this is the notification function if we register for the did disconnect notification event */
- (void)accessoryDidDisconnect:(NSNotification *)notification
{
    NSLog(@"[PZ]: Accessory Disconnected. Notifying app. Close session. ");
    
    [[self notifDelegate] NotifyAppOfAccessoryStatusChanges];
    
    [self closeSession];
}

- (void)accessoryDidConnect:(NSNotification *)notification
{
    NSLog(@"[PZ]: Accessory Connected. Notifying app");
    [[self notifDelegate] NotifyAppOfAccessoryStatusChanges];

    if (_session == nil) {
        NSLog(@"[PZ]: null session. init ...");
        [self init];
        [self openSession];
        // The update thread will take care of polling the accesory
        return;
    }
}

#pragma mark Stream Handling TX and RX functions

-(void) queueTxBytes:(NSData *)buf
{
    unsigned char p[] = {0x5A,0xA5};
	
	if([self isConnected])
	{
		if(txData!=nil)
		{
			[txData appendData:[NSData dataWithBytes:p length:sizeof(p)]];
			[txData appendData:buf];
		}
		else
		{
			txData = [NSMutableData dataWithBytes:p length:sizeof(p)];
			[txData appendData:buf];
			[txData retain];
			if([[_session outputStream] hasSpaceAvailable])
			{
				@synchronized(self)
				{
					[self txBytes]; // jumpstart the transmitter
				}
			}
		}
	}    
}

-(void) txBytes
{
    int len;
    
    if(txData!=nil)
    {
        if([txData length])
        {
            len = [[_session outputStream] write:[txData bytes] maxLength:[txData length]];

            if (len < [txData length])
			{
				NSRange range;
				range.location = len;
				range.length = [txData length] - len;
                // some data remaining
				[txData setData:[txData subdataWithRange:range]];
            }
            else
			{ // no data remaining
				[txData release];
				txData = nil;
            }
        }
    }	
}

typedef enum{ find_preamble,get_com_cmd,get_mddfs_cmd,get_mddfs_data } mddfs_rx_states_t;
-(void) rxBytes:(const void*)buf length:(int)len
{
    static mddfs_rx_states_t receive_state = find_preamble;
	
    if (receive_state != find_preamble) 
    {
        [rxData appendBytes:buf length:len];
    }
    else // look for the preamble
    {
        for(int x=1;x<len;x++)
        {
            if(((uint8_t *)buf)[x-1] == 0x5A && ((uint8_t *)buf)[x] == 0xA5)
            {
                receive_state = get_com_cmd;
                rxData = [[NSMutableData data] retain];
                [rxData appendBytes:&((uint8_t *)buf)[x+1] length:len-x-1];
                break;
            }
        }            
    }
}

@end
