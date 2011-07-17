//
//  TestSetupViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "TestSetupViewController.h"
#import "TestResultViewController.h"

@implementation TestSetupViewController
@synthesize testInstructions;
@synthesize tvCell;
@synthesize currentStep;
@synthesize theTableView;


- (IBAction)readyBtnTapped:(id)sender 
{ 
    UIButton *senderButton = (UIButton *)sender;      
    NSUInteger buttonRow = senderButton.tag; 
    NSString *buttonText = [[senderButton titleLabel] text]; 
    
    if (buttonRow == currentStep && [buttonText caseInsensitiveCompare:@"Done"] == NSOrderedSame)
    {
        [senderButton setTitle:@"Ready" forState:UIControlStateNormal];
        [self nextStep];    
    }
    
    if (buttonRow == 4) 
    {
        TestResultViewController *resultView = [[TestResultViewController alloc] initWithNibName:@"testResultViewController" bundle:nil];
        [self presentModalViewController:resultView animated:true];
        [resultView release];
    }
    
    [buttonText release];    
}

-(void)nextStep
{
    UITableViewCell* cell = [theTableView cellForRowAtIndexPath:[NSIndexPath indexPathForRow:currentStep inSection:0]];
    [cell setSelected:false animated:true];
    
    currentStep++;
    if (currentStep == 1)
        currentStep++;
    
    //NSArray *newIndexPaths = [NSArray arrayWithObjects:[NSIndexPath indexPathForRow:currentStep-1 inSection:0],nil];
    cell = [theTableView cellForRowAtIndexPath:[NSIndexPath indexPathForRow:currentStep inSection:0]];
    [cell setSelected:true animated:true];
    
}

- (void)viewDidAppear:(BOOL)animated
{
    NSIndexPath *index = [NSIndexPath indexPathForRow:0 inSection:0]; 
    [theTableView selectRowAtIndexPath:index animated:true scrollPosition:UITableViewScrollPositionNone];
    currentStep = 0;
    
    [index release];
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
    UITableViewCell* cell = [theTableView cellForRowAtIndexPath:[NSIndexPath indexPathForRow:currentStep inSection:0]];
    [cell setSelected:false animated:false];

    for (int i = 0; i < 5; i++) {
        if (i != 1) {
            UITableViewCell* cell = [theTableView cellForRowAtIndexPath:[NSIndexPath indexPathForRow:i inSection:0]];
            UIButton *button = (UIButton*)cell.accessoryView;
            [button setTitle:@"Done" forState:UIControlStateNormal];
        }
    }
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
    NSDictionary *step1 = [[NSDictionary alloc] initWithObjectsAndKeys: @"1", @"Step", @"Insert strip.", @"Instruction", nil];
    NSDictionary *step2 = [[NSDictionary alloc] initWithObjectsAndKeys: @"2", @"Step", @"Select meal status.", @"Instruction", nil];
    NSDictionary *step3 = [[NSDictionary alloc] initWithObjectsAndKeys: @"3", @"Step", @"Provide blood sample.", @"Instruction", nil];
    NSDictionary *step4 = [[NSDictionary alloc] initWithObjectsAndKeys: @"4", @"Step", @"Sufficient blood has been collected.", @"Instruction", nil];
    NSDictionary *step5 = [[NSDictionary alloc] initWithObjectsAndKeys: @"5", @"Step", @"Run the test.", @"Instruction", nil];
    
    NSArray *array = [[NSArray alloc] initWithObjects:step1, step2, step3, step4, step5, nil]; 
    self.testInstructions = array;  
    [step1 release]; [step2 release]; [step3 release]; [step4 release]; [step5 release]; [array release];
    
    currentStep = 0;
   
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
    self.testInstructions = nil; 
    self.tvCell = nil;
    self.theTableView = nil;
}

- (void)dealloc 
{ 
    [testInstructions release]; 
    [tvCell release];
    [theTableView release];
    [super dealloc]; 
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

#pragma mark - 
#pragma mark Table View Data Source Methods 
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section 
{ 
    return [self.testInstructions count];
}  

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath 
{
    static NSString *TestCellIdentifier = @"TestCellIdentifier";  
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier: TestCellIdentifier]; 
    if (cell == nil) 
    { 
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"TestCell" owner:self options:nil]; 
        if ([nib count] > 0) 
        { 
            cell = self.tvCell; 
        } 
        else 
        { 
            NSLog(@"failed to load TestCell nib file!"); 
        } 
    }
    
    NSUInteger row = [indexPath row]; 
    NSDictionary *rowData = [self.testInstructions objectAtIndex:row];
    
    UILabel *stepLabel = (UILabel *)[cell viewWithTag:kStepTag]; 
    stepLabel.text = [rowData objectForKey:@"Step"];  
    UILabel *instructionLabel = (UILabel *)[cell viewWithTag:kInstructionTag]; 
    instructionLabel.text = [rowData objectForKey:@"Instruction"];    
    
    if (row == 1)
    {
        UISwitch *switchview = [[UISwitch alloc] initWithFrame:CGRectZero];    
        cell.accessoryView = switchview;
        [switchview release];
    }
    else
    {
        UIButton *buttonView = [UIButton buttonWithType:UIButtonTypeRoundedRect];
        buttonView.frame =  CGRectMake(0.0, 0.0,80.0,30.0);
        [buttonView setTitle:@"Done" forState:UIControlStateNormal];
        [buttonView setTag:row];
        [buttonView addTarget:self action:@selector(readyBtnTapped:) forControlEvents:UIControlEventTouchUpInside];
        cell.accessoryView = buttonView;
        [buttonView release];
    }
    
    if (row == 0 && currentStep == 1)
        [cell setSelected:true];
        
    return cell;
} 

#pragma mark - 
#pragma mark Table Delegate Methods

-(NSIndexPath *)tableView:(UITableView *)tableView willSelectRowAtIndexPath:(NSIndexPath *)indexPath 
{ 
    return nil;
    
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath { return 70; }
@end
