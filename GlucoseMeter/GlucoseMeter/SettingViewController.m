//
//  SettingViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "SettingViewController.h"

@implementation SettingViewController
@synthesize minTarget;
@synthesize maxTarget;
@synthesize minAlarm;
@synthesize maxAlarm;
@synthesize interval;
@synthesize email;
@synthesize phone;
@synthesize targetUnit;
@synthesize alarmUnit;
@synthesize unitSwitch;

@synthesize currentTextField;
@synthesize keyboardIsShown;
@synthesize scrollView;
@synthesize appDelegate;

-(IBAction) bgTouched:(id) sender 
{ 
    [minTarget resignFirstResponder];
    [maxTarget resignFirstResponder];
    [minAlarm resignFirstResponder];
    [maxAlarm resignFirstResponder];
    [interval resignFirstResponder];
    [email resignFirstResponder];
    [phone resignFirstResponder];
}

-(IBAction) unitChanged:(id)sender
{
    
    appDelegate.unitMode = ((UISwitch*)sender).isOn? 1:0;
    NSString *strUnit = [[NSString alloc] initWithFormat:@"(%@)",appDelegate.unitMode?@"mg/dl":@"mmol/l"];
    targetUnit.text = strUnit;
    alarmUnit.text = strUnit;
    [strUnit release];
    
    if (appDelegate.unitMode) {
        appDelegate.minTarget *= 18.0;        
        appDelegate.maxTarget *= 18.0;
        appDelegate.minAlarm *= 18.0;
        appDelegate.maxAlarm *= 18.0;
    } else {
        appDelegate.minTarget /= 18.0;
        appDelegate.maxTarget /= 18.0;
        appDelegate.minAlarm /= 18.0;
        appDelegate.maxAlarm /= 18.0;        
    }
    minTarget.text = [[[NSString alloc] initWithFormat:@"%3.1f", appDelegate.minTarget] autorelease];
    maxTarget.text = [[[NSString alloc] initWithFormat:@"%3.1f", appDelegate.maxTarget] autorelease];
    minAlarm.text = [[[NSString alloc] initWithFormat:@"%3.1f", appDelegate.minAlarm] autorelease];
    maxAlarm.text = [[[NSString alloc] initWithFormat:@"%3.1f", appDelegate.maxAlarm] autorelease];
}

//---before the View window appears--- 
-(void) viewWillAppear:(BOOL)animated { 
    //---registers the notifications for keyboard--- 
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidShow:) name:UIKeyboardDidShowNotification object:self.view.window];  
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardDidHide:) name:UIKeyboardDidHideNotification object:nil]; 
    [super viewWillAppear:animated];
}  

//---when a TextField view begins editing--- 
-(void) textFieldDidBeginEditing:(UITextField *)textFieldView { 
    currentTextField = textFieldView; 
}  

//---when the user taps on the return key on the keyboard--- 
-(BOOL) textFieldShouldReturn:(UITextField *) textFieldView { 
    [textFieldView resignFirstResponder]; 
    return NO; 
}  

//---when a TextField view is done editing--- 
-(void) textFieldDidEndEditing:(UITextField *) textFieldView { 
    currentTextField = nil; 
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
    CGRect textFieldRect = [currentTextField frame]; 
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
    
    appDelegate.minTarget = [minTarget.text floatValue];
    appDelegate.maxTarget = [maxTarget.text floatValue];
    appDelegate.minAlarm = [minAlarm.text floatValue];
    appDelegate.maxAlarm = [maxAlarm.text floatValue];
    appDelegate.testInterval = [interval.text intValue];
    appDelegate.contactEmail = email.text;
    appDelegate.contactPhone = phone.text;
    
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
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    NSString *strUnit = [[NSString alloc] initWithFormat:@"(%@)",appDelegate.unitMode?@"mg/dl":@"mmol/l"];
    targetUnit.text = strUnit;
    alarmUnit.text = strUnit;
    
    minTarget.text = [[[[NSNumber alloc] initWithInt:appDelegate.minTarget] autorelease]  stringValue]; 
    maxTarget.text = [[[[NSNumber alloc] initWithInt:appDelegate.maxTarget] autorelease]  stringValue];
    minAlarm.text = [[[[NSNumber alloc] initWithInt:appDelegate.minAlarm] autorelease]  stringValue];
    maxAlarm.text = [[[[NSNumber alloc] initWithInt:appDelegate.maxAlarm] autorelease]  stringValue];
    interval.text = [[[[NSNumber alloc] initWithInt:appDelegate.testInterval] autorelease]  stringValue];
    
    scrollView.frame = CGRectMake(0, 0, 320, 460); 
    [scrollView setContentSize:CGSizeMake(320, 460)];
    
    [unitSwitch setOn:appDelegate.unitMode?true:false];
        
    [strUnit release];
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
    [maxTarget release];
    [minTarget release];
    [maxAlarm release];
    [minAlarm release];
    [interval release];
    [phone release];
    [email release];
    [targetUnit release];
    [alarmUnit release];
    [unitSwitch release];
    
    [scrollView release];
    [currentTextField release];
    
    [appDelegate release];
     
    [super dealloc]; 
}

@end
