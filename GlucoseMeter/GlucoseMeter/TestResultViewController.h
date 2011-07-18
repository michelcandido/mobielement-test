//
//  TestResultViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GlucoseMeterAppDelegate.h"

@interface TestResultViewController : UIViewController

@property (nonatomic, retain) IBOutlet UITextView *textView;
@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

@property (nonatomic, retain) IBOutlet UILabel *maxAlarm;
@property (nonatomic, retain) IBOutlet UILabel *minAlarm;
@property (nonatomic, retain) IBOutlet UILabel *maxTarget;
@property (nonatomic, retain) IBOutlet UILabel *minTarget;
@property (nonatomic, retain) IBOutlet UILabel *midTarget;
@property (nonatomic, retain) IBOutlet UILabel *testResult;

@end
