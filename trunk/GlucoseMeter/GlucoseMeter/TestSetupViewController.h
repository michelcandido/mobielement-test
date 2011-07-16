//
//  TestSetupViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#define kStepTag 1
#define kInstructionTag 2

@interface TestSetupViewController : UIViewController
<UITableViewDelegate, UITableViewDataSource> 
{
    NSArray *testInstructions; 
    UITableViewCell *tvCell;
} 
@property (nonatomic, retain) NSArray *testInstructions;
@property (nonatomic, retain) IBOutlet UITableViewCell *tvCell;
@property (nonatomic) int currentStep;

-(IBAction)readyBtnTapped:(id)sender;
-(void)nextStep;

@end
