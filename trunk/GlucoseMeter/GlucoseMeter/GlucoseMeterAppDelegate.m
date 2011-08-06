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
    NSLog(@"[PZ]: app finish launching with options...");
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
    //protocol initialization; not detecting accessory yet
    _protocol = [[protocolDemo1 alloc] init ];
    accWasConnected = 0; //not connected
    accPreviousStripStatus = -1; //null strip state
    
    return (_protocol != nil);
}

- (BOOL) detectAndInitAccessory
{
    // Start to detect accessory and init a session
    if(!isDemoMode){
        @synchronized(_protocol)
        {

            // start the external accessory manager
            if(_protocol == nil)
            {
                _protocol = [[protocolDemo1 alloc] init ];
                if(_protocol == nil)
                {
                    NSLog(@"Ini protocol failed. Please close the app and try again.");
                    return FALSE;
                }
            }
            if([_protocol isConnected])  // connected means we have a session
            {
                _protocol.notifDelegate = self;
                //[_protocol notifDelegate];
                [self NotifyAppOfAccessoryStatusChanges]; //Update app's protocol state
                return TRUE;
            }
            // Session not init yet; now init
            if (![_protocol findMatchingAccessory]) // got a session?
            {
                [self showAccessoryNotFoundDialog];
                return FALSE;

            }
            else
            {
                _protocol.notifDelegate = self;
                //[_protocol notifDelegate];
                [self NotifyAppOfAccessoryStatusChanges]; //Update app's protocol state
                return TRUE;
   
            }
            
          }
    }
    return TRUE; // Demo mode; always TRUE
    
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    NSLog(@"[PZ]: app will resign active ...");
    /*
     Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
     Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
     */
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
         NSLog(@"[PZ]: app entering background...");
    /*
     Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later. 
     If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
     */
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
     NSLog(@"[PZ]: app entering foreground...");
    //PZ To support suspend and resume ...
    /*
    accWasConnected = 0;
    accPreviousStripStatus = -1;
    
    if(!isDemoMode){
        // startup the external accessory manager
        _protocol = [[protocolDemo1 alloc] init ];
        if( _protocol != nil)
        {
            _protocol.notifDelegate = self;
            [_protocol notifDelegate];
        }
    }
     */
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
    [ tsView updateView:curTestStep];
    
}


// configure a periodic timer to poll the accessory
// the timer func calls isConnected to check
- (void)NotifyAppOfAccessoryStatusChanges
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
            
            // Now update the view 
            if(tabController.selectedIndex == 1)
                [self updateTestView];
            
            
        }
        else if(accWasConnected == 1 && (![_protocol isConnected]))
        {
            accWasConnected = 0;
            accPreviousStripStatus = -1;
            curTestStep = 0;
            [self showAccessoryNotFoundDialog];
        }
        else if (accWasConnected == 0 && (![_protocol isConnected]))
        {
            curTestStep = 0;
            [self showAccessoryNotFoundDialog];
         }
    }
    
}

- (void) showAccessoryNotFoundDialog
{
    // Just disconnected! Show alert
    alertView = [[UIAlertView alloc] initWithTitle:@"Glucose Meter Not Found"
                                           message:@"No matching accessory hardware is attached. Attach it if you want to use it."
                                          delegate:self 
                                 cancelButtonTitle:@"OK"
                                 otherButtonTitles:nil]; 
    [alertView  show];

}

- (void)applicationWillTerminate:(UIApplication *)application
{
     NSLog(@"[PZ]: App will terminate ...");
    /*
     Called when the application is about to terminate.
     Save data if appropriate.
     See also applicationDidEnterBackground:.
     */
}

- (void)dealloc
{
    NSLog(@"[PZ]: Deallc app ...");
    [_protocol release];
    [_window release];
    [tabController release];
    [super dealloc];
}

@end
