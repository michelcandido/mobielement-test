//
//  GlucoseMeterAppDelegate.h
//  GlucoseMeter
//
//  Created by Tony on 7/3/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Constants.h"

@interface GlucoseMeterAppDelegate : NSObject <UIApplicationDelegate> {
    float maxAlarm,minAlarm,maxTarget,minTarget;
    int testInterval;
    float testResult;
    int unitMode;
    NSString *contactEmail;
    NSString *contactPhone;
    
    IBOutlet UITabBarController *tabController;
    NSMutableArray *monthlyReadings;
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
           
@end
