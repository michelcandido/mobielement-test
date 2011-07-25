//
//  TestResultViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "TestResultViewController.h"
#import "QuartzCore/QuartzCore.h"

@implementation TestResultViewController
@synthesize textView;
@synthesize appDelegate;

@synthesize maxAlarm;
@synthesize minAlarm;
@synthesize maxTarget;
@synthesize minTarget;
@synthesize midTarget;
@synthesize testResult;
@synthesize delegate;

-(IBAction)doneWithResult:(id)sender
{
    //[self dismissModalViewControllerAnimated:true];
    if ([sender selectedSegmentIndex] == 0)
        [delegate didDismissTestResultView:false];
    else
        [delegate didDismissTestResultView:true];
}

-(IBAction)doneEditing:(id)sender
{
    [sender resignFirstResponder];
}

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)didReceiveMemoryWarning
{
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    textView.layer.borderWidth = 1;
    textView.layer.borderColor = [[UIColor grayColor] CGColor];
    textView.layer.cornerRadius = 8;
    
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    maxAlarm.text = [[[NSString alloc] initWithFormat:@"%d",appDelegate.maxAlarm] autorelease];
    maxTarget.text = [[[NSString alloc] initWithFormat:@"%d",appDelegate.maxTarget] autorelease];
    minAlarm.text = [[[NSString alloc] initWithFormat:@"%d",appDelegate.minAlarm] autorelease];
    minTarget.text = [[[NSString alloc] initWithFormat:@"%d",appDelegate.minTarget] autorelease];
    int midTargetValue = (appDelegate.maxTarget + appDelegate.minTarget) / 2;
    midTarget.text = [[[NSString alloc] initWithFormat:@"%d",midTargetValue] autorelease];
    
    appDelegate.testResult = arc4random() % 100;
    testResult.text = [[[NSString alloc] initWithFormat:@"%d",appDelegate.testResult] autorelease];
    if (appDelegate.testResult <= appDelegate.minAlarm || appDelegate.testResult >= appDelegate.maxAlarm)
        testResult.textColor = [UIColor redColor];
    else if (appDelegate.testResult < appDelegate.minTarget && appDelegate.testResult > appDelegate.minAlarm)
        testResult.textColor = [UIColor yellowColor];
    else if (appDelegate.testResult <= appDelegate.maxTarget && appDelegate.testResult >= appDelegate.minTarget)
        testResult.textColor = [UIColor greenColor];
    else if (appDelegate.testResult < appDelegate.maxAlarm && appDelegate.testResult > appDelegate.maxTarget)
        testResult.textColor = [UIColor yellowColor];
    
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

@end
