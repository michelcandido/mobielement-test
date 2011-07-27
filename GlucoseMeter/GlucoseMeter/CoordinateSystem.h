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
}

@property (nonatomic, retain) GlucoseMeterAppDelegate *appDelegate;

@end
