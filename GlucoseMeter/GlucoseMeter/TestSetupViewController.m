//
//  TestSetupViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "TestSetupViewController.h"
#import "TestResultViewController.h"

//PZ
#import "GlucoseMeterAppDelegate.h"
@implementation TestSetupViewController
@synthesize testInstructions;
@synthesize tvCell;
@synthesize currentStep;
@synthesize theTableView;
@synthesize appDelegate;
//PZ
@synthesize bCancelResultView;


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
        resultView.delegate = self;
        [self presentModalViewController:resultView animated:true];
        [resultView release];
    }
    
       
}

-(void)didDismissTestResultView:(BOOL)cancel
{
    // From the result view: 
    // cancel:0  user clicked Home
    // cancel:1  user cliecked Cancel
    [self dismissModalViewControllerAnimated:true];
    //PZ
    bCancelResultView = TRUE;
    if (!cancel) 
    {
        appDelegate.tabController.selectedIndex = 0;
    }
}

//PZ
-(void)setStep: (uint8_t) step
{
    currentStep = step;
    
    /* Obtain the cell to set */
    /*
    UITableViewCell* cell = [theTableView cellForRowAtIndexPath:[NSIndexPath indexPathForRow:currentStep inSection:0]];
    UILabel *stepLabel = (UILabel *)[cell viewWithTag:kStepTag];
    */
    
    // update the buttons and text on the test setup UI
    switch (step) {
        case STEP_CHECK_RESULT:
        {
            if (!bCancelResultView){
                
                TestResultViewController *resultView = [[TestResultViewController alloc] initWithNibName:@"TestResultViewController" bundle:nil];
                resultView.delegate = self;
                [self presentModalViewController:resultView animated:true];
                [resultView release];
            }
            
            break;
        }

        case STEP_INSERT_STRIP:
            [theTableView selectRowAtIndexPath:[NSIndexPath indexPathForRow:0 inSection:0] animated:FALSE scrollPosition:UITableViewScrollPositionMiddle];
            break;
        case STEP_SELECT_MEAL_STATUS:
            [theTableView selectRowAtIndexPath:[NSIndexPath indexPathForRow:1 inSection:0] animated:FALSE scrollPosition:UITableViewScrollPositionMiddle];
            break;
        case STEP_DROP_BLOOD:
            [theTableView selectRowAtIndexPath:[NSIndexPath indexPathForRow:2 inSection:0] animated:FALSE scrollPosition:UITableViewScrollPositionMiddle];
            break;
        case STEP_JUST_WAIT:
            [theTableView selectRowAtIndexPath:[NSIndexPath indexPathForRow:3 inSection:0] animated:FALSE scrollPosition:UITableViewScrollPositionMiddle];
            break;
        default:
            break;
    }
    
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
    //PZ
    // In case we return to this view, we need to update the view
    // Note the AppDelegate always set currentStep to appropriate value even the TestSetupView is not on screen
    if(isDemoMode){
        currentStep = STEP_CHECK_RESULT;
    }
        
    @synchronized (self)
    {
        [self setStep:currentStep];
        
        // Reset to be able to see result view if a test is performed
        bCancelResultView = FALSE;
    }
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
    NSDictionary *step2 = [[NSDictionary alloc] initWithObjectsAndKeys: @"2", @"Step", @"Post-meal test?", @"Instruction", nil];
    NSDictionary *step3 = [[NSDictionary alloc] initWithObjectsAndKeys: @"3", @"Step", @"Provide blood sample.", @"Instruction", nil];
    NSDictionary *step4 = [[NSDictionary alloc] initWithObjectsAndKeys: @"4", @"Step", @"Sufficient blood has been collected.", @"Instruction", nil];
    NSDictionary *step5 = [[NSDictionary alloc] initWithObjectsAndKeys: @"5", @"Step", @"Run the test.", @"Instruction", nil];
    
    NSArray *array = [[NSArray alloc] initWithObjects:step1, step2, step3, step4, step5, nil]; 
    self.testInstructions = array;  
    [step1 release]; [step2 release]; [step3 release]; [step4 release]; [step5 release]; [array release];
    
    currentStep = 0;
    
    //PZ
    bCancelResultView = FALSE;
    
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
   
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
        UIImage *btnImage = [UIImage imageNamed:@"Step_Button.png"];
        UIButton *buttonView = [UIButton buttonWithType:UIButtonTypeCustom];
        buttonView.frame =  CGRectMake(0.0, 0.0,80.0,30.0);
        [buttonView setBackgroundImage:btnImage forState:UIControlStateNormal];
        buttonView.adjustsImageWhenDisabled = NO;
        buttonView.adjustsImageWhenHighlighted = NO;
        [buttonView setTitle:@"Done" forState:UIControlStateNormal];
        [buttonView setTag:row];
        [buttonView addTarget:self action:@selector(readyBtnTapped:) forControlEvents:UIControlEventTouchUpInside];
        cell.accessoryView = buttonView;
        [buttonView release];
        [btnImage release];
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

-(void)dealloc {
    [testInstructions release]; 
    [tvCell release];
    [theTableView release];
    [appDelegate release];
    [super dealloc];
}

@end
