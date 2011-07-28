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
    
    // generate data for logs
    
    if ([appDelegate.monthlyReadings count] == 0) {        
        for (int i = 0; i < NUMBER_OF_DAYS; i++) {
            NSMutableArray *dailyReadings = [NSMutableArray arrayWithCapacity:NUMBER_OF_READINGS];
            for (int j = 0; j < NUMBER_OF_READINGS/2; j++) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 40+appDelegate.testResult - 20;
                aReading.mealMode = 0;
                [dailyReadings insertObject:aReading atIndex:j];
                [aReading release];
            }
            for (int j = 1; j < NUMBER_OF_READINGS; j+=2) {
                TestReading *aReading = [[TestReading alloc] init];
                aReading.reading =  arc4random() % 40+appDelegate.testResult - 20;
                aReading.mealMode = 1;
                [dailyReadings insertObject:aReading atIndex:j];
                [aReading release];
            }
            [appDelegate.monthlyReadings addObject:dailyReadings];
            //[dailyReadings release];
        }
    }
    for (int i = 0; i < NUMBER_OF_DAYS; i++) {
        NSMutableArray *dailyReadings = [appDelegate.monthlyReadings objectAtIndex:i];
        for (int j = 0; j < NUMBER_OF_READINGS; j++) {
            TestReading *aReading = [dailyReadings objectAtIndex:j];
            NSLog(@"day:%d #%d reading:%.1f mealmode:%d",i,j,aReading.reading,aReading.mealMode);
        }
        //[dailyReadings release];
    }
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    CoordinateSystem *system = [[CoordinateSystem alloc] initWithFrame:CGRectMake(0, 80, 320, 280)];
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
