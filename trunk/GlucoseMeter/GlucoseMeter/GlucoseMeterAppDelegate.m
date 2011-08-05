//
//  GlucoseMeterAppDelegate.m
//  GlucoseMeter
//
//  Created by Tony on 7/3/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "GlucoseMeterAppDelegate.h"

//PZ
# import "TestSetupViewController.h" //to congtrol the setup view
@implementation GlucoseMeterAppDelegate

@synthesize window = _window;
@synthesize tabController;

@synthesize maxAlarm;
@synthesize minAlarm;
@synthesize maxTarget;
@synthesize minTarget;
@synthesize testInterval;
@synthesize contactEmail;
@synthesize contactPhone;
@synthesize testResult;
@synthesize unitMode;

//PZ
@synthesize _protocol;
@synthesize accWasConnected;
@synthesize accPreviousStripStatus;
@synthesize curTestStep;
@synthesize TestSteps;



#define kAppUpdateTimer 1.5
@synthesize monthlyReadings;

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    [NSThread sleepForTimeInterval: 0.0];
    // Override point for customization after application launch.
    [self.window addSubview:tabController.view];
    [self.window makeKeyAndVisible];
    
    maxAlarm = 250;
    minAlarm = 70;
    maxTarget = 150;
    minTarget = 90;
    testResult = 120;
    testInterval = 6;
    unitMode = 1;
    
    //monthlyReadings = [NSMutableArray arrayWithCapacity:NUMBER_OF_DAYS]; 
    monthlyReadings = [[NSMutableArray alloc] initWithCapacity:NUMBER_OF_DAYS];
    contactEmail = @"contact@microchip.com";
    contactPhone = @"480-792-7200";
    //PZ
    accWasConnected = 0;
    accPreviousStripStatus = -1;
    
    if(!isDemoMode){
        // startup the external accessory manager
        _protocol = [[protocolDemo1 alloc] init];
        if( _protocol != nil)
        {
            _protocol.notifDelegate = self;
            [_protocol notifDelegate];
        }
    }
    return YES;
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    /*
     Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
     Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
     */
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    /*
     Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later. 
     If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
     */
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    /*
     Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
     */
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    /*
     Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
     */
    
    //PZ
    
    NSLog(@"App Became Active");

    


    
    
}
//PZ
- (void) updateTestView {
    
    if (tabController.selectedIndex != 1 ) return;
    
    TestSetupViewController *tsView = (TestSetupViewController*) [tabController selectedViewController];
    [ tsView setStep:curTestStep];
    
}


// configure a periodic timer to poll the accessory
// the timer func calls isConnected to check
- (void)NotifyAppOfAccessoryStatusChanges/*:(NSTimer*)theTimer*/
{
    
    if(isDemoMode)
        return;
    
    @synchronized(_protocol)
    {
        if([_protocol isConnected] )
        {
            if(accWasConnected == 0){
                // Just connected! Update UI
                accWasConnected = 1;
            }
            
            if (_protocol.StripStatus == accPreviousStripStatus \
                /*&& tabController.selectedIndex == 1*/)
                return; //wait more
            
            if (_protocol.StripStatus != accPreviousStripStatus){
                NSLog(@"[PZ]: AppDelegate: Strip Status Changed.");
                accPreviousStripStatus = _protocol.StripStatus;
            }
            
            // Switch to the test setup view
            //tabController.selectedIndex = 1;
            
            if(_protocol.StripStatus == STRIP_STATUS_IDLE) {
                
                // Insert a strip please
                curTestStep = STEP_INSERT_STRIP;
                
            }
            
            else if (_protocol.StripStatus == STRIP_STATUS_PRESENT) {
                
                // Select a meal status, drop blood
                curTestStep = STEP_DROP_BLOOD;
                
            }
            
            else if ( _protocol.StripStatus == STRIP_STATUS_COMPUTING) {
                
                // Meter is computing
                curTestStep = STEP_JUST_WAIT;
                
            }
            
            else if ( _protocol.StripStatus == STRIP_STATUS_FINISHED) {
                
                // Done! Show result
                curTestStep = STEP_CHECK_RESULT;
                testResult = _protocol.LSB + _protocol.MSB * 256;
                
            }
            
            else if (_protocol.AccStatus == ACC_STATUS_ERROR) {
                NSLog(@"[PZ]: Accessory Error.");
            }
            
            else if (_protocol.AccStatus == ACC_STATUS_UNSUPPORTED_APP_VERSION) {
                NSLog(@"[PZ]: Accssory has unsupported app version.");
            }
            
            else {
                NSLog(@"[PZ]: Accssory unknown state.");
            }
            if(tabController.selectedIndex == 1)
                [self updateTestView];
            
            
        }
        else if(accWasConnected == 1 && (![_protocol isConnected]))
        {
            // Just disconnected! Show alert
            alertView = [[UIAlertView alloc] initWithTitle:@"Glucose Meter Not Found"
                                                   message:@"No matching accessory hardware is attached. Attach it if you want to use it."
                                                  delegate:self 
                                         cancelButtonTitle:@"OK"
                                         otherButtonTitles:nil]; 
            accWasConnected = 0;
            accPreviousStripStatus = -1;
            curTestStep = 0;
            [alertView show];                            
            
        }
    }
    
}


- (void)applicationWillTerminate:(UIApplication *)application
{
    /*
     Called when the application is about to terminate.
     Save data if appropriate.
     See also applicationDidEnterBackground:.
     */
}

- (void)dealloc
{
    [_window release];
    [tabController release];
    [super dealloc];
}

@end
