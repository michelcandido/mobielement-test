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
    int dayMode;
    int mealMode;
    int sampleSize;
    NSMutableArray *displayPoints;
}

@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;
@property (nonatomic) int dayMode;
@property (nonatomic) int mealMode;
@property (nonatomic) int sampleSize;

-(int) getScreenY:(float) value;
-(int) getScreenX:(int) index;
-(void) updateData;
-(int) getTouchIndex:(int) x;
@end
