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
@class HomeViewController;

@interface RootViewController : UIViewController

@property (retain, nonatomic) TestViewController *testViewController;
@property (retain, nonatomic) LogViewController *logViewController;
@property (retain, nonatomic) SettingViewController *settingViewController;
@property (retain, nonatomic) HomeViewController *homeViewController;
@property (retain, nonatomic) UIViewController *currentViewController;


-(IBAction)switchViews:(id)sender;
-(void)animateSwitch:(UIViewController*)targetViewController;

@end
