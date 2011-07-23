//
//  SettingViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GlucoseMeterAppDelegate.h"

@interface SettingViewController : UIViewController

@property (nonatomic, retain) IBOutlet UITextField *maxTarget;
@property (nonatomic, retain) IBOutlet UITextField *minTarget;
@property (nonatomic, retain) IBOutlet UITextField *maxAlarm;
@property (nonatomic, retain) IBOutlet UITextField *minAlarm;
@property (nonatomic, retain) IBOutlet UITextField *interval;
@property (nonatomic, retain) IBOutlet UITextField *phone;
@property (nonatomic, retain) IBOutlet UITextField *email;
@property (nonatomic, retain) IBOutlet UILabel *targetUnit;
@property (nonatomic, retain) IBOutlet UILabel *alarmUnit;

@property (nonatomic, retain) UITextField *currentTextField;
@property (nonatomic) BOOL keyboardIsShown;
@property (nonatomic, retain) IBOutlet UIScrollView *scrollView;

@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

-(IBAction) bgTouched:(id) sender;
-(IBAction) unitChanged:(id) sender;

@end
