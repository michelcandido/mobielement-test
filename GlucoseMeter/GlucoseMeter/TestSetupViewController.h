//
//  TestSetupViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TestResultViewController.h"

#define kStepTag 1
#define kInstructionTag 2

@interface TestSetupViewController : UIViewController
<UITableViewDelegate, UITableViewDataSource, TestResultViewControllerDelegate> 
{
    NSArray *testInstructions; 
    IBOutlet UITableViewCell *tvCell;
    int currentStep;
    IBOutlet UITableView *theTableView;
    GlucoseMeterAppDelegate *appDelegate;
} 
@property (nonatomic, retain) NSArray *testInstructions;
@property (nonatomic, retain) UITableViewCell *tvCell;
@property (nonatomic) int currentStep;
@property (nonatomic, retain) UITableView *theTableView;
@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

-(IBAction)readyBtnTapped:(id)sender;
-(void)nextStep;

@end
