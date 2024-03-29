//
//  TestResultViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "TestResultViewController.h"
#import "QuartzCore/QuartzCore.h"
#import "EventKit/EventKit.h"

@implementation TestResultViewController
@synthesize note;
@synthesize appDelegate;

@synthesize maxAlarm;
@synthesize minAlarm;
@synthesize maxTarget;
@synthesize minTarget;
@synthesize midTarget;
@synthesize testResult;

@synthesize scrollView;
@synthesize keyboardIsShown;
@synthesize delegate;

-(IBAction)doneWithResult:(id)sender
{
    //[self dismissModalViewControllerAnimated:true];
    if ([sender selectedSegmentIndex] == 0) {
        EKEventStore *eventDB = [[EKEventStore alloc] init];
        
        EKEvent *myEvent  = [EKEvent eventWithEventStore:eventDB];
        
        myEvent.title     = @"Glucose Test";
        NSDate *startDate = [[NSDate alloc] initWithTimeIntervalSinceNow:3600*appDelegate.testInterval];
        NSDate *endDate = [[NSDate alloc] initWithTimeInterval:600 sinceDate:startDate];
        myEvent.startDate = startDate;
        myEvent.endDate   = endDate;
        myEvent.notes = note.text;
        [startDate release];
        [endDate release];
        
        
        [myEvent setCalendar:[eventDB defaultCalendarForNewEvents]];
        
        NSError *err;        
        [eventDB saveEvent:myEvent span:EKSpanThisEvent error:&err];
        
        NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
        [dateFormat setDateFormat:@"MMM dd, yyyy HH:mm"];
        NSString *dateString = [NSString stringWithFormat:@"Your next glucose test will be at %@",[dateFormat stringFromDate:myEvent.startDate]];
        [dateFormat release];
        
        NSString *title = [NSString stringWithString:@"Reminder Created"];
        NSString *btn = [NSString stringWithString:@"OK"];
        if (err == noErr) {
            UIAlertView *alert = [[UIAlertView alloc]
                                  initWithTitle:title
                                  message:dateString
                                  delegate:nil
                                  cancelButtonTitle:btn
                                  otherButtonTitles:nil];
            [alert show];
            [alert release];
        }
        
        [eventDB release];
        
        [delegate didDismissTestResultView:false];
    }
    else
        [delegate didDismissTestResultView:true];
}

-(IBAction)doneEditing:(id)sender
{
    [sender resignFirstResponder];
}

-(IBAction) bgTouched:(id) sender { 
    [note resignFirstResponder]; 
}

//---before the View window appears--- 
-(void) viewWillAppear:(BOOL)animated { 
    //---registers the notifications for keyboard--- 
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidShow:) name:UIKeyboardDidShowNotification object:self.view.window];  
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidHide:) name:UIKeyboardDidHideNotification object:nil]; 
    [super viewWillAppear:animated];
}  



//---when the keyboard appears--- 
-(void) keyboardDidShow:(NSNotification *) notification { 
    if (keyboardIsShown) 
        return;  
    NSDictionary* info = [notification userInfo];  
    //---obtain the size of the keyboard---
    NSValue *aValue = [info objectForKey:UIKeyboardFrameEndUserInfoKey]; 
    CGRect keyboardRect = [self.view convertRect:[aValue CGRectValue] fromView:nil];  
    //NSLog(@"%f", keyboardRect.size.height);  
    //---resize the scroll view (with keyboard)--- 
    CGRect viewFrame = [scrollView frame]; 
    viewFrame.size.height -= keyboardRect.size.height; 
    scrollView.frame = viewFrame;  
    //---scroll to the current text field--- 
    CGRect textFieldRect = [note frame]; 
    [scrollView scrollRectToVisible:textFieldRect animated:YES];  
    keyboardIsShown = YES; 
}

//---when the keyboard disappears--- 
-(void) keyboardDidHide:(NSNotification *) notification { 
    NSDictionary* info = [notification userInfo];  
    //---obtain the size of the keyboard--- 
    NSValue* aValue = [info objectForKey:UIKeyboardFrameEndUserInfoKey]; 
    CGRect keyboardRect = [self.view convertRect:[aValue CGRectValue] fromView:nil];  
    //---resize the scroll view back to the original size 
    // (without keyboard)--- 
    CGRect viewFrame = [scrollView frame]; 
    viewFrame.size.height += keyboardRect.size.height; 
    scrollView.frame = viewFrame;  
    keyboardIsShown = NO; 
}

//---before the View window disappear--- 
-(void) viewWillDisappear:(BOOL)animated { 
    //---removes the notifications for keyboard--- 
    [[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillShowNotification object:nil];  
    [[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillHideNotification object:nil]; 
   
    [super viewWillDisappear:animated];
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
    note.layer.borderWidth = 1;
    note.layer.borderColor = [[UIColor grayColor] CGColor];
    note.layer.cornerRadius = 8;
    
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    float midTargetValue = (appDelegate.maxTarget + appDelegate.minTarget) / 2;
    
    if (appDelegate.unitMode) {
        maxAlarm.text = [[[NSString alloc] initWithFormat:@"%.0f",appDelegate.maxAlarm] autorelease];
        maxTarget.text = [[[NSString alloc] initWithFormat:@"%.0f",appDelegate.maxTarget] autorelease];
        minAlarm.text = [[[NSString alloc] initWithFormat:@"%.0f",appDelegate.minAlarm] autorelease];
        minTarget.text = [[[NSString alloc] initWithFormat:@"%.0f",appDelegate.minTarget] autorelease];
        midTarget.text = [[[NSString alloc] initWithFormat:@"%.0f",midTargetValue] autorelease];
    } else {
        maxAlarm.text = [[[NSString alloc] initWithFormat:@"%.1f",appDelegate.maxAlarm] autorelease];
        maxTarget.text = [[[NSString alloc] initWithFormat:@"%.1f",appDelegate.maxTarget] autorelease];
        minAlarm.text = [[[NSString alloc] initWithFormat:@"%.1f",appDelegate.minAlarm] autorelease];
        minTarget.text = [[[NSString alloc] initWithFormat:@"%.1f",appDelegate.minTarget] autorelease];        
        midTarget.text = [[[NSString alloc] initWithFormat:@"%.1f",midTargetValue] autorelease];            
    }
    
    
    @synchronized (appDelegate)
    {
        if(isDemoMode)
            appDelegate.testResult = arc4random() % ((int)appDelegate.maxAlarm + 20 - (int)appDelegate.minAlarm + 20)+appDelegate.minAlarm - 20;
        
        NSLog(@"[PZ]: we will show result: %d", (int)appDelegate.testResult);
        
        if (appDelegate.unitMode)
            testResult.text = [[[NSString alloc] initWithFormat:@"%.0f",appDelegate.testResult] autorelease];
        else
            testResult.text = [[[NSString alloc] initWithFormat:@"%.1f",appDelegate.testResult] autorelease];
    }
    
    if (appDelegate.testResult <= appDelegate.minAlarm || appDelegate.testResult >= appDelegate.maxAlarm)
        testResult.textColor = [UIColor redColor];
    else if (appDelegate.testResult < appDelegate.minTarget && appDelegate.testResult > appDelegate.minAlarm)
        testResult.textColor = [UIColor orangeColor];
    else if (appDelegate.testResult <= appDelegate.maxTarget && appDelegate.testResult >= appDelegate.minTarget)
        testResult.textColor = [UIColor greenColor];
    else if (appDelegate.testResult < appDelegate.maxAlarm && appDelegate.testResult > appDelegate.maxTarget)
        testResult.textColor = [UIColor orangeColor];
    
    scrollView.frame = CGRectMake(0, 0, 320, 460); 
    [scrollView setContentSize:CGSizeMake(320, 460)];
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

- (void)dealloc { 
    [scrollView release]; 
    [note release];
    [maxAlarm release];
    [maxTarget release];
    [minAlarm release];
    [minTarget release];
    [testResult release];
    //[appDelegate release];
    [super dealloc]; }

@end
