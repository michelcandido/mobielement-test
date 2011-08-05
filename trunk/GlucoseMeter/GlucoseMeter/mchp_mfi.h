//
//  mchp_mfi.h
//  MCHP MFI
//
//  Created by Pei Zheng 
//  
//

#import <Foundation/Foundation.h>
#import <ExternalAccessory/ExternalAccessory.h>


extern NSString *EADSessionDataReceivedNotification;

@protocol AccessoryProtocolDelegate <NSObject>
@required
-(void)NotifyAppOfAccessoryStatusChanges;
@end

@interface mchp_mfi : NSObject <EAAccessoryDelegate, NSStreamDelegate>
{
    id<AccessoryProtocolDelegate> notifDelegate;
    
    uint8_t appMajorVersion;
    uint8_t appMinorVersion;
    uint8_t appRev;
    
	NSString *_protocolString;
	EAAccessory *_accessory;
    EASession *_session;
    BOOL streamReady;
    BOOL receivedAccPkt;
    NSMutableData *rxData;
    NSMutableData *txData;
    NSData *FSresponse_data;
   
}

@property (retain) id notifDelegate;

- (BOOL) initWithProtocol:(NSString *)protocol;

- (void) queueTxBytes:(NSData *)buf;
- (void) txBytes;
- (void) rxBytes:(const void *)buf length:(int)len;

- (BOOL) openSession;
- (void) closeSession;

- (void)accessoryDidDisconnect:(NSNotification *)notification;
- (void)accessoryDidConnect:(NSNotification *)notification;
- (bool) isConnected;
- (NSString *) name;
- (NSString *) manufacturer;
- (NSString *) modelNumber;
- (NSString *) serialNumber;
- (NSString *) firmwareRevision;
- (NSString *) hardwareRevision;
// You implement this function

- (int) readData:(NSData *) data;

@end
