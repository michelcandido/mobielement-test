//
//  GlucoseMeterAppDelegate.m
//  GlucoseMeter
//
//  Created by Tony on 7/3/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "GlucoseMeterAppDelegate.h"

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
