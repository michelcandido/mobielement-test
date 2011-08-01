//
//  TestSetupViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TestResultViewController.h"

//PZ
#import "protocolDemo1.h"
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
    //PZ
    bool bCancelResultView;
} 
@property (nonatomic, retain) NSArray *testInstructions;
@property (nonatomic, retain) UITableViewCell *tvCell;
@property (nonatomic) int currentStep;
@property (nonatomic, retain) UITableView *theTableView;
@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

//PZ
@property (nonatomic) bool bCancelResultView;
-(IBAction)readyBtnTapped:(id)sender;
-(void)nextStep;

//PZ
-(void)setStep:(uint8_t)step;
@end
