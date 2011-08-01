//
//  mchp_mfi.m
//  MCHP MFI
//
//  Created by Joseph Julicher on 6/10/10.
//  Copyright 2010 Microchip Technology. All rights reserved.
//

#import "mchp_mfi.h"

@implementation mchp_mfi


// OVERLOAD ME!!
- (int) readData:(NSData *) data
{
	return [data length];
}

- (id) initWithProtocol:(NSString *)protocol
{
    // redirect stderr to file
    /*
    NSString *cacheDir = [NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES) objectAtIndex:0];
    NSString *logPath = [cacheDir stringByAppendingPathComponent:@"application.log"];
    freopen([logPath cStringUsingEncoding:NSASCIIStringEncoding], "a+", stderr);
	*/
    theProtocol = protocol;
	// see if an accessory is already attached
	eas = [self openSessionForProtocol:theProtocol];
    if(eas == nil)
    {
        // we did not find an appropriate accessory
        NSLog(@"[PZ]: openSessionForProtocol return nil");
        return nil;
        
    }
	// install notification events.
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(accessoryDidConnect:)
												 name:EAAccessoryDidConnectNotification object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(accessoryDidDisconnect:)
                                                 name:EAAccessoryDidDisconnectNotification object:nil];
    
    [[EAAccessoryManager sharedAccessoryManager] registerForLocalNotifications];
    return self;	
}

- (bool) isConnected
{
    if(eas != nil)
        return [[eas accessory] isConnected];
    else{
        // we don't do init here. The protocol has its own timer to do this
         NSLog(@"[PZ]: session object not initialized.");
         return FALSE;
    }

       
}

- (NSString *) name { return (eas)?[[eas accessory] name]:@"-"; }
- (NSString *) manufacturer { return (eas)?[[eas accessory] manufacturer]:@"-";}
- (NSString *) modelNumber {return (eas)?[[eas accessory] modelNumber]:@"-";}
- (NSString *) serialNumber {return (eas)?[[eas accessory] serialNumber]:@"-";}
- (NSString *) firmwareRevision {return (eas)?[[eas accessory] firmwareRevision]:@"-";}
- (NSString *) hardwareRevision {return (eas)?[[eas accessory] hardwareRevision]:@"-";}

/***********************************************************************/
#pragma mark External Accessory Basic Identification 
/***********************************************************************/

- (EASession *)openSessionForProtocol:(NSString *)protocolString 
{
    NSLog(@"[PZ]: Getting a list of accessories");
    NSArray *accessories = [[EAAccessoryManager sharedAccessoryManager]
                            connectedAccessories];
	
    EAAccessory *accessory = nil;
    EASession *session = nil;
    
	NSLog(@"[PZ]: Found %d accessories",[accessories count]);
    NSLog(@"[PZ]: Looking for %@",protocolString);
    
	for (EAAccessory *obj in accessories)
    {
		NSArray *sa = [obj protocolStrings];
		//NSLog(@"a string dump of %d strings",[sa count]);
		
		for(NSString *s in sa)
		{
			NSLog(@"[PZ]: %@",s);
		}
		
        if ([sa containsObject:protocolString])
        {
            accessory = obj;
            NSLog(@"[PZ]: Found accessory matching the protocol");
            break;
        }
        accessory = obj;
    }
    if (accessory != nil) {
        NSLog(@"[PZ]: Found a compatible accessory.");
    }
    else 
        NSLog(@"[PZ]: Failed to find a compatible accessory.");
    
    if (accessory)
    {
        session = [[EASession alloc] initWithAccessory:accessory
                                           forProtocol:protocolString];
        if (session)
        {
            NSLog(@"[PZ]: opening the streams for this accessory");
            [[session inputStream] setDelegate:self];
            [[session inputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop]
                                             forMode:NSDefaultRunLoopMode];
            [[session inputStream] open];
            [[session outputStream] setDelegate:self];
            [[session outputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop]
                                              forMode:NSDefaultRunLoopMode];
            [[session outputStream] open];
            [session retain];
            streamReady = true;
            receivedAccPkt = true;
            
           
        }
        else {
            NSLog(@"[PZ]: Init acc failed for %@.", protocolString);
        }
    }
    else {
        NSLog(@"[PZ]: failed to find acc that matches the protocol.");
    }
        
    
        
    return session;
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
    NSLog(@"[PZ]: Accessory Disconnected");
    [[eas inputStream] close];
    [[eas outputStream] close];
    [eas release];
    eas = nil;
}

- (void)accessoryDidConnect:(NSNotification *)notification
{
    NSLog(@"[PZ]: Accessory Connected");
    if (eas == nil) {
        [self init];
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
			if([[eas outputStream] hasSpaceAvailable])
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
            len = [[eas outputStream] write:[txData bytes] maxLength:[txData length]];

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
