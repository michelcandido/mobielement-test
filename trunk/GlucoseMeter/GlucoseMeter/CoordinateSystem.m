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
#import "TestReading.h"

#define xAxisLength 280
#define yAxisLength 250

@implementation CoordinateSystem
@synthesize appDelegate;
@synthesize dayMode;
@synthesize mealMode;
@synthesize sampleSize;

- (id)initWithFrame:(CGRect)frame {
    
    self = [super initWithFrame:frame];
    if (self) {
		self.backgroundColor = [UIColor clearColor];
        startPoint = CGPointMake(STARTPOINTX + 1, 253.0f);
    }
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    dayMode = 0;
    mealMode = 0;
    sampleSize = 6;
    
    return self;
}

-(void)drawCurve:(CGContextRef) context {
    time_t rawtime;
	struct tm * timeinfo;
	time ( &rawtime );
	timeinfo = localtime ( &rawtime );
    int x, y, x1,y1, step;
    TestReading *aReading, *nextReading;
    
    CGContextSetStrokeColorWithColor(context, [UIColor greenColor].CGColor);
    CGContextSelectFont(context, "Helvetica", 10, kCGEncodingMacRoman);
    CGContextSetTextMatrix(context, CGAffineTransformMakeScale(1.0, -1.0));
    
    if (dayMode == 0) {        
        NSMutableArray *dailyReadings = [appDelegate.monthlyReadings objectAtIndex:(timeinfo->tm_mday - 1)];
        for (int i = 0; i < NUMBER_OF_READINGS; i++) {
            aReading = [dailyReadings objectAtIndex:i];
            if (mealMode == 1 && aReading.mealMode == 2) {                
                continue;
            }
            else if (mealMode == 2 && aReading.mealMode == 1) {
                continue;
            }
            else {                
                x = [self getScreenX:i];
                y = [self getScreenY:aReading.reading];
                CGContextMoveToPoint(context, x, y);
                CGContextAddEllipseInRect(context, CGRectMake(x-2, y-2, 5, 5));
            }
            if (i + 1 < NUMBER_OF_READINGS) {
                nextReading = [dailyReadings objectAtIndex:i+1];
                x1 = [self getScreenX:i+1];
                y1 = [self getScreenY:nextReading.reading];
                CGContextAddLineToPoint(context, x1, y1);
            }
            
            CGContextStrokePath(context);
            CGContextShowTextAtPoint(context, x - (10 / (dayMode + 1)), startPoint.y + 12, [aReading.date UTF8String], [aReading.date length]);
            CGContextShowTextAtPoint(context, x - (12 / (dayMode + 1)), startPoint.y + 25, [aReading.time UTF8String], [aReading.time length]);
        }        
    }
    
    //[aReading release];
    //[nextReading release];
}
 
- (void)drawAxisX:(CGContextRef)context {
	CGContextMoveToPoint(context, startPoint.x, startPoint.y - yAxisLength);
	CGContextAddLineToPoint(context, startPoint.x, startPoint.y);
	CGContextStrokePath(context);
	
	// draw end
	CGContextMoveToPoint(context, startPoint.x - 5, startPoint.y - yAxisLength + 5);
	CGContextAddLineToPoint(context, startPoint.x, startPoint.y - yAxisLength);
	CGContextAddLineToPoint(context, startPoint.x + 5, startPoint.y - yAxisLength + 5);
	CGContextStrokePath(context);
    
    time_t rawtime;
	struct tm * timeinfo;
	time ( &rawtime );
	timeinfo = localtime ( &rawtime );
    
    for(int i = 0; i < sampleSize; i++) {
		int x = [self getScreenX:i];
		CGContextMoveToPoint(context, x, startPoint.y);
		CGContextAddLineToPoint(context, x, startPoint.y - 5);
		CGContextStrokePath(context);
		
		CGContextSelectFont(context, "Helvetica", 10, kCGEncodingMacRoman);
		CGContextSetTextMatrix(context, CGAffineTransformMakeScale(1.0, -1.0));
		/*
		NSString *date = [NSString stringWithString:@"8/1"];
		NSString *time = [NSString stringWithString:@"12:10"];
        
        if (dayMode == 2 && i % 3 != 0)
            continue;
        
		CGContextShowTextAtPoint(context, x - (10 / (dayMode + 1)), startPoint.y + 12, [date UTF8String], [date length]);
		CGContextShowTextAtPoint(context, x - (15 / (dayMode + 1)), startPoint.y + 25, [time UTF8String], [time length]);
         */
	}
}

- (void)drawAxisY:(CGContextRef)context {
	CGContextMoveToPoint(context, startPoint.x, startPoint.y);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength, startPoint.y);
	CGContextStrokePath(context);
	
	// draw end
	CGContextMoveToPoint(context, startPoint.x + xAxisLength - 5, startPoint.y - 5);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength, startPoint.y);
	CGContextAddLineToPoint(context, startPoint.x + xAxisLength - 5, startPoint.y + 5);
	CGContextStrokePath(context);
	
	// coordinate
    NSString *str;
	for (int i = 30; i < yAxisLength; i += 30) {
		CGContextMoveToPoint(context, startPoint.x, startPoint.y - i + 1);
		CGContextAddLineToPoint(context, startPoint.x + 5, startPoint.y - i + 1);
		CGContextStrokePath(context);
		
		CGContextSelectFont(context, "Helvetica", 10, kCGEncodingMacRoman);
		//CGContextSetTextMatrix(context, CGAffineTransformMakeTranslation(-80, 80));
		CGContextSetTextMatrix(context, CGAffineTransformMakeScale(1.0, -1.0));
		
		str = [NSString stringWithFormat:@"%d", appDelegate.unitMode?i + 10 * (i / 30):i/15];
		CGContextShowTextAtPoint(context, 3, startPoint.y - i + 5, [str UTF8String], [str length]);
        
	}
	
	str = [NSString stringWithFormat:@"%d", 0];
    CGContextShowTextAtPoint(context, 3, startPoint.y, [str UTF8String], [str length]);
    
    CGContextSetStrokeColorWithColor(context, [UIColor orangeColor].CGColor);
    int yMax = [self getScreenY: appDelegate.maxTarget];
    CGContextMoveToPoint(context, startPoint.x, yMax);
    CGContextAddLineToPoint(context, startPoint.x+xAxisLength-12, yMax);
    CGContextStrokePath(context);
    str = [NSString stringWithFormat:appDelegate.unitMode?@"%.0f":@"%.1f", appDelegate.maxTarget];
	CGContextShowTextAtPoint(context, startPoint.x+xAxisLength-10, yMax+2, [str UTF8String], [str length]);
    
    int yMin = [self getScreenY: appDelegate.minTarget];
    CGContextMoveToPoint(context, startPoint.x, yMin);
    CGContextAddLineToPoint(context, startPoint.x+xAxisLength-12, yMin);
    CGContextStrokePath(context);
    str = [NSString stringWithFormat:appDelegate.unitMode?@"%.0f":@"%.1f", appDelegate.minTarget];
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

-(int) getScreenX:(int) index {
    int x;    
    x = startPoint.x + xAxisLength * (index + 1) / (sampleSize + 1);
    return x;
}

- (void)drawCoordinateSystem:(CGContextRef)context {
	//[self drawDetialInfo:context];
	
	
	CGContextSetLineCap(context, kCGLineCapRound);
	CGContextSetLineWidth(context, 2);
	[self drawAxisX:context];
	[self drawAxisY:context];
    [self drawCurve:context];
}



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
