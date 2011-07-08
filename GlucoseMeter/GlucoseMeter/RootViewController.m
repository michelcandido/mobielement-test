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
#import "HomeViewController.h"
#import "GlucoseMeterAppDelegate.h"

@implementation RootViewController
@synthesize testViewController;
@synthesize logViewController;
@synthesize settingViewController;
@synthesize homeViewController;
@synthesize currentViewController;

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
    
    NSString *titleText = [[NSString alloc] initWithString:((UIBarButtonItem*)sender).title];
    NSInteger tag = ((UIView*)sender).tag;
        
    //if ([titleText caseInsensitiveCompare:@"Run Test"] == NSOrderedSame)
    switch (tag) {
        case 1:
            if (self.homeViewController.view.superview == nil) 
            { 
                if (self.homeViewController == nil) 
                { 
                    HomeViewController *homeController = [[HomeViewController alloc] initWithNibName:@"HomeView" bundle:nil]; 
                    self.homeViewController = homeController;  
                    [homeController release]; 
                }             
                [self animateSwitch:homeViewController];
            }
            break;
        case 2:
            if (self.testViewController.view.superview == nil) 
            { 
                if (self.testViewController == nil) 
                { 
                    TestViewController *testController = [[TestViewController alloc] initWithNibName:@"TestView" bundle:nil]; 
                    self.testViewController = testController;  
                    [testController release]; 
                } 
                [self animateSwitch:testViewController];
            }
            break;
        case 3:
            if (self.logViewController.view.superview == nil) 
            { 
                if (self.logViewController == nil) 
                { 
                    LogViewController *logController = [[LogViewController alloc] initWithNibName:@"LogView" bundle:nil]; 
                    self.logViewController = logController;  
                    [logController release]; 
                }     
                [self animateSwitch:logViewController];
            }
            break;
        case 4:
            if (self.settingViewController.view.superview == nil) 
            { 
                if (self.settingViewController == nil) 
                { 
                    SettingViewController *settingController = [[SettingViewController alloc] initWithNibName:@"SettingView" bundle:nil]; 
                    self.settingViewController = settingController;  
                    [settingController release]; 
                }            
                [self animateSwitch:settingViewController];
            }
            break;
        default:
            break;
    }
    
    [appDelegate release];
    [titleText release];
}

- (void)animateSwitch:(UIViewController*)targetViewController
{
    [UIView beginAnimations:@"View Flip" context:nil]; 
    [UIView setAnimationDuration:1.25]; 
    [UIView setAnimationCurve:UIViewAnimationCurveEaseInOut];
    
    [UIView setAnimationTransition: UIViewAnimationTransitionFlipFromRight forView:self.view cache:YES];  
    [currentViewController viewWillAppear:YES];     
    [targetViewController viewWillDisappear:YES];            
    [currentViewController.view removeFromSuperview];
    [self.view insertSubview:targetViewController.view atIndex:0];            
    [targetViewController viewDidDisappear:YES]; 
    [currentViewController viewDidAppear:YES];
    currentViewController = targetViewController;
    
    [UIView commitAnimations];
}

#pragma mark - View lifecycle

/*
// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView
{
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad
{
    HomeViewController *homeController = [[HomeViewController alloc] initWithNibName:@"HomeView" bundle:nil];
    self.homeViewController = homeController;
    [self.view insertSubview:homeController.view atIndex:0];
    currentViewController = homeViewController;
    [homeController release];
    [super viewDidLoad];
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
