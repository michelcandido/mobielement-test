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

@property (nonatomic) float maxAlarm;
@property (nonatomic) float minAlarm;
@property (nonatomic) float maxTarget;
@property (nonatomic) float minTarget;
@property (nonatomic) int testInterval;
@property (nonatomic) int testResult;
@property (nonatomic) int unitMode;
@property (nonatomic,retain) NSString *contactEmail;
@property (nonatomic,retain) NSString *contactPhone;
           
@end
