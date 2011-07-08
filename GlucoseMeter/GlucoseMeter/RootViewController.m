//
//  RootViewController.m
//  GlucoseMeter
//
//  Created by Tony on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "RootViewController.h"
#import "TestViewController.h"
#import "LogViewController.h"
#import "SettingViewController.h"
#import "GlucoseMeterAppDelegate.h"

@implementation RootViewController
@synthesize testViewController;
@synthesize logViewController;
@synthesize settingViewController;

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

- (IBAction)switchViews:(id)sender
{
    GlucoseMeterAppDelegate *appDelegate = (GlucoseMeterAppDelegate *)[[UIApplication sharedApplication] delegate];
    
    NSString *titleText = [[NSString alloc] initWithString:((UIButton*)sender).titleLabel.text];
     
    if ([titleText caseInsensitiveCompare:@"Test"] == NSOrderedSame)
    {
        
    } else if ([titleText caseInsensitiveCompare:@"Log"] == NSOrderedSame)
    {
        
    } else if ([titleText caseInsensitiveCompare:@"Settings"] == NSOrderedSame)
    {
        if (self.settingViewController.view.superview == nil) 
        { 
            if (self.settingViewController == nil) 
            { 
                SettingViewController *settingController = [[SettingViewController alloc] initWithNibName:@"SettingView" bundle:nil]; 
                self.settingViewController = settingController;  
                [settingController release]; 
            } 
            
            //[blueViewController.view removeFromSuperview]; 
            //[self.view insertSubview:settingViewController.view atIndex:0]; 
            [appDelegate.window addSubview:settingViewController.view];
            [appDelegate.window makeKeyAndVisible];
        }
    }
    
    [titleText release];
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
