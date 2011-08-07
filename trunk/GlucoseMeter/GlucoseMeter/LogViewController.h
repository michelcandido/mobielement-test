//
//  LogViewController.h
//  GlucoseMeter
//
//  Created by Tony on 7/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "CoordinateSystem.h"
#import "TestReading.h"
#import "Constants.h"

@interface LogViewController : UIViewController {
    CoordinateSystem *coordinateSystem;
    GlucoseMeterAppDelegate *appDelegate;    
}
@property (nonatomic, retain) CoordinateSystem *coordinateSystem;
@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

-(IBAction) doneWithSelection:(id)sender; //update daymode and mealmode
@end
