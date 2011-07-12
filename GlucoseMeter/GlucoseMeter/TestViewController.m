//
//  TestViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "TestViewController.h"

@implementation TestViewController
@synthesize testInstructions;
@synthesize tvCell;


- (IBAction)readyBtnTapped:(id)sender 
{ 
    UIButton *senderButton = (UIButton *)sender;      
    NSUInteger buttonRow = senderButton.tag; 
    NSString *buttonTitle = [testInstructions objectAtIndex:buttonRow]; 
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"You tapped the button" message:[NSString stringWithFormat: @"You tapped the button for %@", buttonTitle] delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil]; 
    [alert show]; 
    [alert release]; 
    
    [senderButton setTitle:@"Ready" forState:UIControlStateNormal];
}


- (void)viewDidLoad 
{
    NSDictionary *step1 = [[NSDictionary alloc] initWithObjectsAndKeys: @"1", @"Step", @"Insert strip.", @"Instruction", nil];
    NSDictionary *step2 = [[NSDictionary alloc] initWithObjectsAndKeys: @"2", @"Step", @"Select meal status.", @"Instruction", nil];
    NSDictionary *step3 = [[NSDictionary alloc] initWithObjectsAndKeys: @"3", @"Step", @"Provide blood sample.", @"Instruction", nil];
    NSDictionary *step4 = [[NSDictionary alloc] initWithObjectsAndKeys: @"4", @"Step", @"Sufficient blood has been collected.", @"Instruction", nil];
    NSDictionary *step5 = [[NSDictionary alloc] initWithObjectsAndKeys: @"5", @"Step", @"Run the test.", @"Instruction", nil];
    
    NSArray *array = [[NSArray alloc] initWithObjects:step1, step2, step3, step4, step5, nil]; 
    self.testInstructions = array;  
    [step1 release]; [step2 release]; [step3 release]; [step4 release]; [step5 release]; [array release];
    
    
    [super viewDidLoad]; 
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

/*
// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView
{
}
*/

/*
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad
{
    [super viewDidLoad];
}
*/

- (void)viewDidUnload
{
    
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
    self.testInstructions = nil; 
    self.tvCell = nil;
    [super viewDidUnload];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

- (void)dealloc 
{ 
    [testInstructions release]; 
    [tvCell release];
    [super dealloc]; 
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
    
    return cell; 
} 

#pragma mark - 
#pragma mark Table Delegate Methods

-(NSIndexPath *)tableView:(UITableView *)tableView willSelectRowAtIndexPath:(NSIndexPath *)indexPath 
{ 
    return nil;
    /*
    NSUInteger row = [indexPath row];  
    if (row == 0) 
        return nil;  
    return indexPath;
     */
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath { return 70; }
@end
