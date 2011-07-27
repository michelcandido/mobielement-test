//
//  TestResultViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GlucoseMeterAppDelegate.h"

@protocol TestResultViewControllerDelegate <NSObject>
-(void)didDismissTestResultView:(BOOL)cancel;
@end

@interface TestResultViewController : UIViewController {
    id<TestResultViewControllerDelegate> delegate;
    
    IBOutlet UITextView *note;
    GlucoseMeterAppDelegate *appDelegate;
    
    IBOutlet UILabel *maxAlarm;
    IBOutlet UILabel *minAlarm;
    IBOutlet UILabel *maxTarget;
    IBOutlet UILabel *minTarget;
    IBOutlet UILabel *midTarget;
    IBOutlet UILabel *testResult;
    
    BOOL keyboardIsShown;
    IBOutlet UIScrollView *scrollView;

}

@property (nonatomic, retain)  UITextView *note;
@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

@property (nonatomic, retain)  UILabel *maxAlarm;
@property (nonatomic, retain)  UILabel *minAlarm;
@property (nonatomic, retain)  UILabel *maxTarget;
@property (nonatomic, retain)  UILabel *minTarget;
@property (nonatomic, retain)  UILabel *midTarget;
@property (nonatomic, retain)  UILabel *testResult;

@property (nonatomic) BOOL keyboardIsShown;
@property (nonatomic, retain)  UIScrollView *scrollView;

@property (nonatomic, assign) id<TestResultViewControllerDelegate> delegate;

-(IBAction) doneWithResult:(id)sender;
-(IBAction) doneEditing:(id) sender;
-(IBAction) bgTouched:(id) sender;

@end


