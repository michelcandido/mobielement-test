//
//  CoordinateSystem.m
//  MCHP MFI
//
//  Created by Tony on 7/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CoordinateSystem.h"
#import <QuartzCore/QuartzCore.h>
#import "Constants.h"

#define xAxisLength 280
#define yAxisLength 250

@implementation CoordinateSystem
@synthesize appDelegate;

- (id)initWithFrame:(CGRect)frame {
    
    self = [super initWithFrame:frame];
    if (self) {
		self.backgroundColor = [UIColor clearColor];
        startPoint = CGPointMake(STARTPOINTX + 1, 253.0f);
    }
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    return self;
}


// 绘制坐标轴X
- (void)drawAxisX:(CGContextRef)context {
	CGContextMoveToPoint(context, startPoint.x, startPoint.y - yAxisLength);
	CGContextAddLineToPoint(context, startPoint.x, startPoint.y);
	CGContextStrokePath(context);
	
	// 绘制坐标轴X端帽
	CGContextMoveToPoint(context, startPoint.x - 5, startPoint.y - yAxisLength + 5);
	CGContextAddLineToPoint(context, startPoint.x, startPoint.y - yAxisLength);
	CGContextAddLineToPoint(context, startPoint.x + 5, startPoint.y - yAxisLength + 5);
	CGContextStrokePath(context);
}

// 绘制坐标轴Y
- (void)drawAxisY:(CGContextRef)context {
	CGContextMoveToPoint(context, startPoint.x, startPoint.y);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength, startPoint.y);
	CGContextStrokePath(context);
	
	// 绘制坐标轴Y端帽
	CGContextMoveToPoint(context, startPoint.x + xAxisLength - 5, startPoint.y - 5);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength, startPoint.y);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength - 5, startPoint.y + 5);
	CGContextStrokePath(context);
	
	// 绘制纵轴的坐标轴
    NSString *str;
	for (int i = 30; i < yAxisLength; i += 30) {
		CGContextMoveToPoint(context, startPoint.x, startPoint.y - i + 1);
		CGContextAddLineToPoint(context, startPoint.x + 5, startPoint.y - i + 1);
		CGContextStrokePath(context);
		
		CGContextSelectFont(context, "Helvetica", 10, kCGEncodingMacRoman);
		//CGContextSetTextMatrix(context, CGAffineTransformMakeTranslation(-80, 80));
		CGContextSetTextMatrix(context, CGAffineTransformMakeScale(1.0, -1.0));
		
		str = [[NSString stringWithFormat:@"%d", appDelegate.unitMode?i + 10 * (i / 30):i/15] autorelease];
		CGContextShowTextAtPoint(context, 3, startPoint.y - i + 5, [str UTF8String], [str length]);
        
	}
	
	str = [[NSString stringWithFormat:@"%d", 0] autorelease];
	CGContextShowTextAtPoint(context, 3, startPoint.y, [str UTF8String], [str length]);
    
    CGContextSetStrokeColorWithColor(context, [UIColor orangeColor].CGColor);
    int yMax = [self getScreenY: appDelegate.maxTarget];
    CGContextMoveToPoint(context, startPoint.x, yMax);
    CGContextAddLineToPoint(context, startPoint.x+xAxisLength-12, yMax);
    CGContextStrokePath(context);
    str = [[NSString stringWithFormat:appDelegate.unitMode?@"%.0f":@"%.1f", appDelegate.maxTarget] autorelease];
	CGContextShowTextAtPoint(context, startPoint.x+xAxisLength-10, yMax+2, [str UTF8String], [str length]);
    
    int yMin = [self getScreenY: appDelegate.minTarget];
    CGContextMoveToPoint(context, startPoint.x, yMin);
    CGContextAddLineToPoint(context, startPoint.x+xAxisLength-12, yMin);
    CGContextStrokePath(context);
    str = [[NSString stringWithFormat:appDelegate.unitMode?@"%.0f":@"%.1f", appDelegate.minTarget] autorelease];
	CGContextShowTextAtPoint(context, startPoint.x+xAxisLength-10, yMin+2, [str UTF8String], [str length]);
    
}

-(int) getScreenY:(float) value {
    int y;
    if  (appDelegate.unitMode) {
        y = startPoint.y - 0.75 * value + 1;
    } else {
        y = startPoint.y - 15 * value + 1;
    }
    return y;
}
// 绘制坐标系统
- (void)drawCoordinateSystem:(CGContextRef)context {
	//[self drawDetialInfo:context];
	
	
	CGContextSetLineCap(context, kCGLineCapRound);
	CGContextSetLineWidth(context, 2);
	[self drawAxisX:context];
	[self drawAxisY:context];
}


// 绘制视图中要显示的内容
- (void)drawRect:(CGRect)rect {
    //CGContextRef context = UIGraphicsGetCurrentContext();
	myContext = UIGraphicsGetCurrentContext();
	[self drawCoordinateSystem:myContext];
}

- (void)dealloc {
    [appDelegate release];
    [super dealloc];
}


@end
