//
//  protocolDemo1.h
//  MCHP MFI
//
//  Created by Joseph Julicher on 6/10/10.
//  Copyright 2010 Microchip Technology. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "mchp_mfi.h"

#define MFI_UNKNOWN_HW 0
#define	MFI_GLUCOSE_DEMO_BOARD 1

typedef enum{
    STRIP_STATUS_IDLE = 0,
    STRIP_STATUS_PRESENT = 1,
    STRIP_STATUS_COMPUTING = 2,
    STRIP_STATUS_FINISHED = 3,
    
    STRIP_STATUS_NUM_STATES 
} tagStripStatusTable;

typedef enum {
    ACC_STATUS_SUCCESS = 0,
    ACC_STATUS_ERROR = 1,
    ACC_STATUS_UNSUPPORTED_APP_VERSION = 2,
    
    ACC_STATUS_NUM_STATES
} tagAccStatusTable;




@interface protocolDemo1 : mchp_mfi {
      
    uint8_t AccStatus;
    uint8_t StripStatus;
    tagStripStatusTable StripStatusTable;
    tagAccStatusTable AccStatusTable;
    
    uint8_t AccMajor;
    uint8_t AccMinor;
    uint8_t AccRev;
    int BoardID;
	
	//NSAutoreleasePool *thePool;
	NSThread *updateThread;
	
    int LSB;
    int MSB;
}

@property (readonly) uint8_t AccStatus;
@property (readonly) uint8_t StripStatus;
@property (readonly) tagStripStatusTable StripStatusTable;
@property (readonly) tagAccStatusTable AccStatusTable;
@property (readonly) uint8_t AccMajor;
@property (readonly) uint8_t AccMinor;
@property (readonly) uint8_t AccRev;
@property (readonly) int BoardID;

@property (readonly) int LSB;
@property (readonly) int MSB;


- (void) updateData;
- (uint8_t) getPrintStripStatus;
- (uint8_t) getPrintAccStatus;

- (void) sendCommand: (uint8_t) cmdID;
- (void) sendInitCommand;
- (void) sendGetStatusCommand;
- (void) sendGetResultCommand;

@end
