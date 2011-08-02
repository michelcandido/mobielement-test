//
//  SettingViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GlucoseMeterAppDelegate.h"

@interface SettingViewController : UIViewController {
    IBOutlet UITextField *maxTarget;
    IBOutlet UITextField *minTarget;
    IBOutlet UITextField *maxAlarm;
    IBOutlet UITextField *minAlarm;
    IBOutlet UITextField *interval;
    IBOutlet UITextField *phone;
    IBOutlet UITextField *email;
    IBOutlet UILabel *targetUnit;
    IBOutlet UILabel *alarmUnit;
    IBOutlet UISwitch *unitSwitch;
    
    IBOutlet UIScrollView *scrollView;
    
    UITextField *currentTextField;
    BOOL keyboardIsShown;
    
    GlucoseMeterAppDelegate *appDelegate;
    
}

@property (nonatomic, retain)  UITextField *maxTarget;
@property (nonatomic, retain)  UITextField *minTarget;
@property (nonatomic, retain)  UITextField *maxAlarm;
@property (nonatomic, retain)  UITextField *minAlarm;
@property (nonatomic, retain)  UITextField *interval;
@property (nonatomic, retain)  UITextField *phone;
@property (nonatomic, retain)  UITextField *email;
@property (nonatomic, retain)  UILabel *targetUnit;
@property (nonatomic, retain)  UILabel *alarmUnit;
@property (nonatomic, retain)  UISwitch *unitSwitch;

@property (nonatomic, retain) UITextField *currentTextField;
@property (nonatomic) BOOL keyboardIsShown;
@property (nonatomic, retain)  UIScrollView *scrollView;

@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

-(IBAction) bgTouched:(id) sender;
-(IBAction) unitChanged:(id) sender;

@end
