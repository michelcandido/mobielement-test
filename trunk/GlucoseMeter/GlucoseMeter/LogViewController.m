//
//  LogViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "LogViewController.h"

@implementation LogViewController

@synthesize coordinateSystem;
//@synthesize monthlyReadings;
@synthesize appDelegate;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

-(IBAction) doneWithSelection:(id)sender
{
    if (((UIView*)sender).tag == 1)
        coordinateSystem.dayMode = [sender selectedSegmentIndex];
    else if (((UIView*)sender).tag == 2) 
        coordinateSystem.mealMode = [sender selectedSegmentIndex];
    
    if (coordinateSystem.dayMode == 0)
        coordinateSystem.sampleSize = NUMBER_OF_READINGS;
    else if (coordinateSystem.dayMode == 1)
        coordinateSystem.sampleSize = 7;
    else if (coordinateSystem.dayMode == 2)
        coordinateSystem.sampleSize = 31;
    
    [coordinateSystem setNeedsDisplay];
}

-(void)showDetails:(UILongPressGestureRecognizer *)recognizer { 
    if (recognizer.state == UIGestureRecognizerStateBegan && (coordinateSystem.dayMode == 1 || coordinateSystem.dayMode == 2)){
        CGPoint touchPoint = [recognizer locationInView:coordinateSystem];
        int index = [coordinateSystem getTouchIndex:touchPoint.x];
        NSArray *dailyReadings = [NSArray arrayWithArray:[appDelegate.monthlyReadings objectAtIndex:index]];
        NSString *title = [NSString stringWithFormat:@"Readings for date: %@",((TestReading *)[dailyReadings objectAtIndex:0]).date];
        NSString *msg = [NSString stringWithString:@""];
        for (int i = 0; i < [dailyReadings count]; i++)
        {
            TestReading *aReading = (TestReading *)[dailyReadings objectAtIndex:i];
            msg = [msg stringByAppendingFormat:@"(%d) %@ at %@: %.1f %@\n", i, (aReading.mealMode == 1)?@"Pre-meal":@"Post-meal", aReading.time, (appDelegate.unitMode)?aReading.reading:aReading.reading/18, (appDelegate.unitMode)?@"mg/dl":@"mmol/l"];
        }
                            
       
        UIAlertView *alert = [[UIAlertView alloc]
                              initWithTitle:title
                              message:msg
                              delegate:nil
                              cancelButtonTitle:@"OK"
                              otherButtonTitles:nil];
        ((UILabel*)[[alert subviews] objectAtIndex:1]).textAlignment = UITextAlignmentLeft;
        [alert show];
        [alert release];
    } 
}
- (void)didReceiveMemoryWarning
{
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

-(void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
    [coordinateSystem setNeedsDisplay];
    
    time_t rawtime;
	struct tm * timeinfo;
	time ( &rawtime );
	timeinfo = localtime ( &rawtime );
    
    NSTimeInterval secondsPerDay = 24*60*60;
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"M/dd"];

    NSArray *array = [[NSArray alloc] initWithObjects:@"06:30", @"08:00", @"12:00", @"13:00", @"19:00", @"20:00",nil]; 
    // generate data for logs        
    if ([appDelegate.monthlyReadings count] == 0) {        
        for (int i = 0; i < NUMBER_OF_DAYS; i++) {
            NSDate *day = [[NSDate alloc] initWithTimeIntervalSinceNow:-secondsPerDay*(NUMBER_OF_DAYS - i - 1)];
            NSString *dayStr = [dateFormatter stringFromDate:day];
            NSLog(@"date:%@",dayStr);
            NSMutableArray *dailyReadings = [NSMutableArray arrayWithCapacity:NUMBER_OF_READINGS];
            for (int j = 0; j < NUMBER_OF_READINGS/2; j++) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 40+appDelegate.testResult - 20;
                if (i == timeinfo->tm_mday - 1 && j == 0)
                    aReading.reading = appDelegate.testResult;
                aReading.mealMode = 1;
                //aReading.date = [NSString stringWithFormat:@"%d/%d",timeinfo->tm_mon+1,i+1];
                aReading.date = [dateFormatter stringFromDate:day];
                aReading.time = [array objectAtIndex:j*2];
                [dailyReadings insertObject:aReading atIndex:j];                
                [aReading release];
            }
            for (int j = 1; j < NUMBER_OF_READINGS; j+=2) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 50+appDelegate.testResult - 10;
                aReading.mealMode = 2;
                //aReading.date = [NSString stringWithFormat:@"%d/%d",timeinfo->tm_mon+1,i+1];                
                aReading.date = [dateFormatter stringFromDate:day];
                aReading.time = [array objectAtIndex:j];
                [dailyReadings insertObject:aReading atIndex:j];
                [aReading release];
            }
            [appDelegate.monthlyReadings addObject:dailyReadings];
            [day release];
        }
    }
    [array release];
    [dateFormatter release];
}

-(void)viewDidDisappear:(BOOL)animated
{
    [super viewDidDisappear:animated];
    [appDelegate.monthlyReadings removeAllObjects];
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    CoordinateSystem *system = [[CoordinateSystem alloc] initWithFrame:CGRectMake(0, 60, 320, 320)];
    self.coordinateSystem = system;
    [system release];
    [self.view addSubview:self.coordinateSystem];
    
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    
    UILongPressGestureRecognizer *longPress = [[[UILongPressGestureRecognizer alloc] initWithTarget:self action:@selector(showDetails:)] autorelease];   
    longPress.minimumPressDuration = 0.5;
    [self.view addGestureRecognizer:longPress];
   
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
    [coordinateSystem release];
    //[monthlyReadings release];
    [appDelegate release];
    [super dealloc];
}

@end
