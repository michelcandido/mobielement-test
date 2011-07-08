//
//  RootViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class TestViewController;
@class LogViewController;
@class SettingViewController;

@interface RootViewController : UIViewController

@property (retain, nonatomic) TestViewController *testViewController;
@property (retain, nonatomic) LogViewController *logViewController;
@property (retain, nonatomic) SettingViewController *settingViewController;

-(IBAction)switchViews:(id)sender;

@end
