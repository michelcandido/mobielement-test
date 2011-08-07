//
//  CoordinateSystem.h
//  MCHP MFI
//
//  Created by Tony on 7/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GlucoseMeterAppDelegate.h"

@interface CoordinateSystem : UIView {
	CGPoint startPoint;
	CGContextRef myContext;
    GlucoseMeterAppDelegate *appDelegate;
    int dayMode; //0: today, 1:weekly, 2:monthly
    int mealMode; //0: all, 1:pre-meal, 2:post-meal
    int sampleSize;  //how many data points for the selected daymode and mealmode
    NSMutableArray *displayPoints; //host all data points to be drawed
}

@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;
@property (nonatomic) int dayMode;
@property (nonatomic) int mealMode;
@property (nonatomic) int sampleSize;
@property (nonatomic, retain) NSMutableArray *displayPoints;

-(int) getScreenY:(float) value; // get the y value for a given reading
-(int) getScreenX:(int) index; // get the x value for a given index
-(void) updateData; //update data for graph
-(int) getTouchIndex:(int) x; // get the index for long press x
@end
