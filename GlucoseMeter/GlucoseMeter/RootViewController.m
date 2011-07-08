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
    
    [UIView beginAnimations:@"View Flip" context:nil]; 
    [UIView setAnimationDuration:1.25]; 
    [UIView setAnimationCurve:UIViewAnimationCurveEaseInOut];
    
    [UIView setAnimationTransition: UIViewAnimationTransitionFlipFromRight forView:self.view cache:YES];  
    [currentViewController viewWillAppear:YES]; 
    
    if ([titleText caseInsensitiveCompare:@"Run Test"] == NSOrderedSame)
    {
        if (self.testViewController.view.superview == nil) 
        { 
            if (self.testViewController == nil) 
            { 
                TestViewController *testController = [[TestViewController alloc] initWithNibName:@"TestView" bundle:nil]; 
                self.testViewController = testController;  
                [testController release]; 
            } 
            [testViewController viewWillDisappear:YES];
            [currentViewController.view removeFromSuperview];
            [self.view insertSubview:testViewController.view atIndex:0]; 
            [testViewController viewDidDisappear:YES]; 
            [currentViewController viewDidAppear:YES];
            currentViewController = testViewController;
        }
        
    } else if ([titleText caseInsensitiveCompare:@"View Log"] == NSOrderedSame)
    {
        if (self.logViewController.view.superview == nil) 
        { 
            if (self.logViewController == nil) 
            { 
                LogViewController *logController = [[LogViewController alloc] initWithNibName:@"LogView" bundle:nil]; 
                self.logViewController = logController;  
                [logController release]; 
            } 
            [logViewController viewWillDisappear:YES];            
            [currentViewController.view removeFromSuperview];
            [self.view insertSubview:logViewController.view atIndex:0]; 
            [logViewController viewDidDisappear:YES]; 
            [currentViewController viewDidAppear:YES];
            currentViewController = logViewController;
        }
        
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
            [settingViewController viewWillDisappear:YES];            
            [currentViewController.view removeFromSuperview];
            [self.view insertSubview:settingViewController.view atIndex:0];
            [settingViewController viewDidDisappear:YES]; 
            [currentViewController viewDidAppear:YES];
            currentViewController = settingViewController;
        }
    } else if ([titleText caseInsensitiveCompare:@"Home"] == NSOrderedSame)
    {
        if (self.homeViewController.view.superview == nil) 
        { 
            if (self.homeViewController == nil) 
            { 
                HomeViewController *homeController = [[HomeViewController alloc] initWithNibName:@"HomeView" bundle:nil]; 
                self.homeViewController = homeController;  
                [homeController release]; 
            } 
            [homeViewController viewWillDisappear:YES];            
            [currentViewController.view removeFromSuperview];
            [self.view insertSubview:homeViewController.view atIndex:0];            
            [homeViewController viewDidDisappear:YES]; 
            [currentViewController viewDidAppear:YES];
            currentViewController = homeViewController;
        }
    }
    
    [UIView commitAnimations];
    [titleText release];
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
