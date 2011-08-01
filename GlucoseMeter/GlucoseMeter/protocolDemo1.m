//
//  protocolDemo1.m
//  MCHP MFI
//
//  Created by Pei Zheng
//  
//

#import "protocolDemo1.h"
@implementation protocolDemo1

@synthesize AccStatus;
@synthesize StripStatus;
@synthesize StripStatusTable;
@synthesize AccStatusTable;

@synthesize LSB;
@synthesize MSB;

@synthesize AccMajor;
@synthesize AccMinor;
@synthesize AccRev;
@synthesize BoardID;



- (void) sendInitCommand
{
    uint8_t buffer[6];
	bzero(buffer, sizeof(buffer));
	
    
	buffer[0] = 0; // command id
    buffer[1] = 1; // app major version
    buffer[2] = 0; // app minor version
    buffer[3] = 0; // app revsion
	
	@synchronized (self)
	{
        [self queueTxBytes:[NSData dataWithBytes:buffer length:6]];
	}
    NSLog(@"[PZ]: Get init command enqueued.");    
}


- (void) sendGetStatusCommand
{
	uint8_t buffer[6];
	bzero(buffer, sizeof(buffer));
	

	buffer[0] = 4;
	
	@synchronized (self)
	{
	  [self queueTxBytes:[NSData dataWithBytes:buffer length:6]];
	}
    NSLog(@"[PZ]: Get status command enqueued.");
}

- (uint8_t) getPrintStripStatus
{
    switch (StripStatus)
    {
        case STRIP_STATUS_IDLE:
            NSLog(@"[PZ]: Strip Status: STRIP_STATUS_IDLE");
            break;
        case STRIP_STATUS_PRESENT:
            NSLog(@"[PZ]: Strip Status: STRIP_STATUS_PRESENT");
            break;
        case STRIP_STATUS_COMPUTING:
            NSLog(@"[PZ]: Strip Status: STRIP_STATUS_COMPUTING");
            break;
        case STRIP_STATUS_FINISHED:
            NSLog(@"[PZ]: Strip Status: STRIP_STATUS_FINISHED");
            break;
        default:
            NSLog(@"[PZ]: Wrong strip status. Check the value from acc");
            break;
    }
    return StripStatus;
    
}

- (uint8_t) getPrintAccStatus
{
    switch (AccStatus) {
        case ACC_STATUS_SUCCESS:
            NSLog(@"[PZ]: Acc Status Success");
            break;
        case ACC_STATUS_ERROR:
            NSLog(@"[PZ]: Acc Status ERROR");
            break;
        case ACC_STATUS_UNSUPPORTED_APP_VERSION:
            NSLog(@"[PZ]: Acc Status Unsupported App Version");
            break;
            
        default:
            NSLog(@"[PZ]: Wrong acc status. Check the value from acc");
            break;
    }
    return AccStatus;
}

- (void) sendGetResultCommand
{
	uint8_t buffer[6];
	bzero(buffer, sizeof(buffer));

	buffer[0] = 5;

	@synchronized (self)
	{
		[self queueTxBytes:[NSData dataWithBytes:buffer length:6]];
	}
    NSLog(@"[PZ]: Get result command enqueued.");

}

- (void) sendCommand: (uint8_t) cmdID
{
    switch (cmdID)
    {
        case  0:
            self.sendInitCommand;
            break;
        case  4:
            self.sendGetStatusCommand;
            break;
        case  5:
            self.sendGetResultCommand;
            break;
        default:
            NSLog(@"[PZ]: wrong command id!");
            break;
    }
}


#define kUPDATERATE_FAST 0.500 // seconds
#define kUPDATERATE_SLOW 1.000 // seconds

// there will be some delay when the accessory is connected 
// so we need to check it periodically
- (void) updateData
{
    float updateRate = kUPDATERATE_SLOW;
    thePool = [[NSAutoreleasePool alloc] init];
    
    while (eas == nil){
        @synchronized (self){
            AccStatus = -1;
            StripStatus = -1;
            id initX = [super initWithProtocol:@"com.microchip.ipodaccessory.glucose_meter"];
            if(initX == nil)
            {   
                NSLog(@"[PZ]: initProtocol failed in update timer. Recheck soon ...");
                [NSThread sleepForTimeInterval:updateRate];
            }   
            else
            {
                
                [self sendCommand:0];
                    NSLog(@"[PZ]: initProtocol succeeded in update timer. Sending init command.");
                break;
            }   
 

        }
        
    }
        
    
}


- (int) readData:(NSData *) data
{
	int ret;
	ret = 0;
    NSLog(@"[PZ]: Received Data: %@", data);
    NSLog(@"[PZ]: Length: %d", [data length]);
	if([data length] >= 6)
	{
		NSRange r;
		uint8_t buf[8];
		r.location = 0;
		r.length = 6;
		ret = r.length;
		[data getBytes:buf length:6]; // Extract the complete Packet
		// process data received from the accessory
		switch(buf[0])
		{
			case 1: // Accessory Ready
				@synchronized (self)
				{
					AccStatus = buf[1];
					AccMajor = buf[2];
					AccMinor = buf[3];
					AccRev = buf[4];
					BoardID = buf[5];
                    NSLog(@"[PZ]: Acc Status: %d", AccStatus);
                    [self getPrintAccStatus];
				}
				break;
			case 2: // Strip status
				@synchronized (self)
				{
					StripStatus = buf[1];
                    NSLog(@"[PZ]: Strip Status: %d", StripStatus);
                    [self getPrintStripStatus];
                    // query after acc says finished
                    if (StripStatus == STRIP_STATUS_FINISHED) {
                        [self sendCommand:5];
                    }
				}
				break;
			case 3: // Result
				{
					uint lsb, msb;
					lsb = buf[1];
					msb = buf[2];
					
					@synchronized (self)
                    {
                        LSB = lsb;
                        MSB = msb;
                    }
                    NSLog(@"[PZ]: **********Result LSB/MSB: %d/%d*******", \
                          LSB, MSB);
                }
				break;
			default: // unknown command
				NSLog(@"[PZ]: %@ : Unknown Command %d",theProtocol,buf[0]);
				break;
		}
	}
	return ret;
}

// init will be called when program starts, and when it resumes

- (id) init
{
    NSLog(@"[PZ]:----------- Start Init -------------");
    
    StripStatus = -1;
    AccStatus = -1;
    
	id initX = [super initWithProtocol:@"com.microchip.ipodaccessory.glucose_meter"];
    if(initX == nil)
    {
        NSLog(@"[PZ]: initProtocol failed.");
    }
    else
    {
        // Send init command 
        [self sendCommand:0];
        
        NSLog(@"[PZ]: initProtocol succeeded. Sending init command.");
    }
	NSLog(@"[PZ]: starting update thread");
	updateThread = [[NSThread alloc] initWithTarget:self selector:@selector(updateData) object:nil];
	[updateThread start];

	BoardID = MFI_UNKNOWN_HW;
	 
	return self;
	
}

-(void) dealloc
{
    [super dealloc];
	[thePool drain];
}

@end
