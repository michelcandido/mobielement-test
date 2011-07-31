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

    NSArray *array = [[NSArray alloc] initWithObjects:@"6:30", @"8:00", @"12:00", @"13:00", @"19:00", @"20:00",nil]; 
    // generate data for logs        
    if ([appDelegate.monthlyReadings count] == 0) {        
        for (int i = 0; i < NUMBER_OF_DAYS; i++) {
            NSMutableArray *dailyReadings = [NSMutableArray arrayWithCapacity:NUMBER_OF_READINGS];
            for (int j = 0; j < NUMBER_OF_READINGS/2; j++) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 40+appDelegate.testResult - 20;
                if (i == timeinfo->tm_mday - 1 && j == 0)
                    aReading.reading = appDelegate.testResult;
                aReading.mealMode = 1;
                aReading.date = [NSString stringWithFormat:@"%d/%d",timeinfo->tm_mon+1,i+1];
                aReading.time = [array objectAtIndex:j*2];
                [dailyReadings insertObject:aReading atIndex:j];                
                [aReading release];
            }
            for (int j = 1; j < NUMBER_OF_READINGS; j+=2) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 50+appDelegate.testResult - 10;
                aReading.mealMode = 2;
                aReading.date = [NSString stringWithFormat:@"%d/%d",timeinfo->tm_mon+1,i+1];                
                aReading.time = [array objectAtIndex:j];
                [dailyReadings insertObject:aReading atIndex:j];
                [aReading release];
            }
            [appDelegate.monthlyReadings addObject:dailyReadings];
            //[dailyReadings release];
        }
    }
    /*
    for (int i = 0; i < NUMBER_OF_DAYS; i++) {
        NSMutableArray *dailyReadings = [appDelegate.monthlyReadings objectAtIndex:i];
        for (int j = 0; j < NUMBER_OF_READINGS; j++) {
            TestReading *aReading = [dailyReadings objectAtIndex:j];
            NSLog(@"reading:%.1f mealmode:%d date:%@ time:%@",aReading.reading,aReading.mealMode, aReading.date, aReading.time);
            [aReading release];
        }
        //[dailyReadings release];
    }
    */
    [array release];
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
