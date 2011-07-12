//
//  TestViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

#define kStepTag 1
#define kInstructionTag 2

@interface TestViewController : UIViewController
<UITableViewDelegate, UITableViewDataSource> 
{
    NSArray *testInstructions; 
    UITableViewCell *tvCell;
} 
@property (nonatomic, retain) NSArray *testInstructions;
@property (nonatomic, retain) IBOutlet UITableViewCell *tvCell;
-(IBAction)readyBtnTapped:(id)sender;
@end
