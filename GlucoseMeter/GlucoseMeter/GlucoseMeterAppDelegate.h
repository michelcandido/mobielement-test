//
//  GlucoseMeterAppDelegate.h
//  GlucoseMeter
//
//  Created by Tony on 7/3/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GlucoseMeterAppDelegate : NSObject <UIApplicationDelegate>

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet UITabBarController *tabController;

@property (nonatomic) int maxAlarm;
@property (nonatomic) int minAlarm;
@property (nonatomic) int maxTarget;
@property (nonatomic) int minTarget;
@property (nonatomic) int testResult;
           
@end
