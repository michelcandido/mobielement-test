//
//  GlucoseMeterAppDelegate.h
//  GlucoseMeter
//
//  Created by Tony on 7/3/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
//PZ
#import "protocolDemo1.h" 
#import "Constants.h"
#define isDemoMode TRUE

typedef enum {
    STEP_INSERT_STRIP = 0,
    STEP_SELECT_MEAL_STATUS = 1,
    STEP_DROP_BLOOD = 2,
    STEP_JUST_WAIT = 3,
    STEP_CHECK_RESULT = 4,
    
    STEP_NUM_STEPS = 5
} tagTestSteps;

@interface GlucoseMeterAppDelegate : NSObject <UIApplicationDelegate> {
    float maxAlarm,minAlarm,maxTarget,minTarget;
    int testInterval;
    float testResult;
    int unitMode;
    NSString *contactEmail;
    NSString *contactPhone;
    
    IBOutlet UITabBarController *tabController;
	NSMutableArray *monthlyReadings;	
    //PZ
    protocolDemo1 *_protocol;
    NSTimer *updateTimer;    
    uint8_t accWasConnected;
    UIAlertView *alertView;
    uint8_t curTestStep;
    tagTestSteps TestSteps;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) UITabBarController *tabController;

@property (nonatomic) float maxAlarm;
@property (nonatomic) float minAlarm;
@property (nonatomic) float maxTarget;
@property (nonatomic) float minTarget;
@property (nonatomic) int testInterval;
@property (nonatomic) float testResult;
@property (nonatomic) int unitMode;
@property (nonatomic,retain) NSString *contactEmail;
@property (nonatomic,retain) NSString *contactPhone;

@property (nonatomic, retain) NSMutableArray *monthlyReadings;
@property (nonatomic, retain) protocolDemo1 *_protocol;
@property (nonatomic, assign) uint8_t accWasConnected;
@property (nonatomic, assign) uint8_t accPreviousStripStatus;
@property (readwrite) uint8_t curTestStep;
@property (nonatomic, readonly) tagTestSteps TestSteps;

-(void) updateTestView;           
           
@end
