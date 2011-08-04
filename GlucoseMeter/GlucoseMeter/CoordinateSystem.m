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
        NSLog(@"%.1f,%.1f",startPoint.x,startPoint.y);
    }
    appDelegate = (GlucoseMeterAppDelegate*)[[UIApplication sharedApplication] delegate];
    dayMode = 0;
    mealMode = 0;
    sampleSize = 6;
    
    return self;
}

-(void)updateData {
    time_t rawtime;
	struct tm * timeinfo;
	time ( &rawtime );
	timeinfo = localtime ( &rawtime );
    NSPredicate *predicate = [NSPredicate predicateWithFormat:@"mealMode == %d", mealMode];
    if (dayMode == 0) {
        displayPoints = [NSMutableArray arrayWithCapacity:sampleSize];
        NSArray *dailyReadings = [NSArray arrayWithArray:[appDelegate.monthlyReadings objectAtIndex:(timeinfo->tm_mday - 1)]];
        if (mealMode == 1 || mealMode == 2) {            
            dailyReadings = [dailyReadings filteredArrayUsingPredicate:predicate];
        }
        for (int i = 0; i < [dailyReadings count]; i++)
            [displayPoints addObject:[dailyReadings objectAtIndex:i]];
        sampleSize = [displayPoints count];
    } else if (dayMode == 1) {
        displayPoints =  [[NSMutableArray alloc] initWithCapacity:sampleSize];
        for (int i = 0; i < sampleSize; i++) {
            NSArray *dailyReadings = [NSArray arrayWithArray:[appDelegate.monthlyReadings objectAtIndex:(timeinfo->tm_mday >= 7)?(timeinfo->tm_mday - 7 + i):(timeinfo->tm_mday + i)]];
            if (mealMode == 1 || mealMode == 2)
                dailyReadings = [dailyReadings filteredArrayUsingPredicate:predicate];
            [displayPoints addObject:dailyReadings];
        }
    } else if (dayMode == 2) {
        displayPoints = [[NSMutableArray alloc] initWithCapacity:sampleSize];
        for (int i = 0; i < sampleSize; i++) {
            NSArray *dailyReadings = [NSArray arrayWithArray:[appDelegate.monthlyReadings objectAtIndex:i]];
            if (mealMode == 1 || mealMode == 2)
                dailyReadings = [dailyReadings filteredArrayUsingPredicate:predicate];
            [displayPoints addObject:dailyReadings];            
        }
    }
}

-(void)drawCurve:(CGContextRef) context {
    
    int x, y, x1,y1;
    TestReading *aReading, *nextReading;
    
    CGContextSelectFont(context, "Helvetica", 10, kCGEncodingMacRoman);
    CGContextSetTextMatrix(context, CGAffineTransformMakeScale(1.0, -1.0));
        
    
    if (dayMode == 0) {
        CGContextSetStrokeColorWithColor(context, [UIColor greenColor].CGColor);
        for (int i = 0; i < [displayPoints count]; i++) {
            aReading = [displayPoints objectAtIndex:i];
                            
            x = [self getScreenX:i];
            y = [self getScreenY:aReading.reading];
            CGContextMoveToPoint(context, x, y);
            CGContextAddEllipseInRect(context, CGRectMake(x-2, y-2, 5, 5));
        
            if (i + 1 < [displayPoints count]) {
                nextReading = [displayPoints objectAtIndex:i+1];
                x1 = [self getScreenX:i+1];
                y1 = [self getScreenY:nextReading.reading];
                CGContextAddLineToPoint(context, x1, y1);
            }            
            CGContextStrokePath(context);
            CGContextShowTextAtPoint(context, x - (10 / (dayMode + 1)), startPoint.y + 12, [aReading.date UTF8String], [aReading.date length]);
            CGContextShowTextAtPoint(context, x - (12 / (dayMode + 1)), startPoint.y + 25, [aReading.time UTF8String], [aReading.time length]);
        }        
    } else if (dayMode == 1 || dayMode == 2) {
        NSMutableArray *min, *max, *ave;
        min = [NSMutableArray arrayWithCapacity:sampleSize];
        max = [NSMutableArray arrayWithCapacity:sampleSize];
        ave = [NSMutableArray arrayWithCapacity:sampleSize];
        for (int i = 0; i < sampleSize; i++) {
            NSArray *dailyReadings = [NSArray arrayWithArray:[displayPoints objectAtIndex:i]];
            NSArray *sortedArray = [dailyReadings sortedArrayUsingComparator:^(id obj1, id obj2) {
                NSNumber *num1 = [NSNumber numberWithFloat:[(TestReading*)obj1 reading]];
                NSNumber *num2 = [NSNumber numberWithFloat:[(TestReading*)obj2 reading]];
                return (NSComparisonResult)[num1 compare:num2];
            }];
            [min addObject:[sortedArray objectAtIndex:0]];
            [max addObject:[sortedArray lastObject]];
            float average = 0;
            for (int j = 0; j < [sortedArray count]; j++) {
                TestReading *t = [sortedArray objectAtIndex:j];
                average += t.reading;
                [t release];
            }
            TestReading *aveReading = [TestReading alloc];
            aveReading.reading = average / [sortedArray count];
            [ave addObject:aveReading];
        }
        for (int i = 0; i < sampleSize; i++) {
            CGContextSetStrokeColorWithColor(context, [UIColor yellowColor].CGColor);
            aReading = [min objectAtIndex:i];
            x = [self getScreenX:i];
            y = [self getScreenY:aReading.reading];
            CGContextMoveToPoint(context, x, y);
            CGContextAddEllipseInRect(context, CGRectMake(x-2, y-2, 5, 5));
            
            if (i + 1 < [min count]) {
                nextReading = [min objectAtIndex:i+1];
                x1 = [self getScreenX:i+1];
                y1 = [self getScreenY:nextReading.reading];
                CGContextAddLineToPoint(context, x1, y1);
            }
            CGContextStrokePath(context);
            if (dayMode == 1 || (dayMode == 2 && (i % 4 == 0)))
                CGContextShowTextAtPoint(context, x - (10 / (dayMode + 1)), startPoint.y + 12, [aReading.date UTF8String], [aReading.date length]);
            
            CGContextSetStrokeColorWithColor(context, [UIColor redColor].CGColor);
            aReading = [max objectAtIndex:i];
            x = [self getScreenX:i];
            y = [self getScreenY:aReading.reading];
            CGContextMoveToPoint(context, x, y);
            CGContextAddEllipseInRect(context, CGRectMake(x-2, y-2, 5, 5));
            
            if (i + 1 < [max count]) {
                nextReading = [max objectAtIndex:i+1];
                x1 = [self getScreenX:i+1];
                y1 = [self getScreenY:nextReading.reading];
                CGContextAddLineToPoint(context, x1, y1);
            }
            CGContextStrokePath(context);
            
            CGContextSetStrokeColorWithColor(context, [UIColor greenColor].CGColor);
            aReading = [ave objectAtIndex:i];
            x = [self getScreenX:i];
            y = [self getScreenY:aReading.reading];
            CGContextMoveToPoint(context, x, y);
            CGContextAddEllipseInRect(context, CGRectMake(x-2, y-2, 5, 5));
            
            if (i + 1 < [ave count]) {
                nextReading = [ave objectAtIndex:i+1];
                x1 = [self getScreenX:i+1];
                y1 = [self getScreenY:nextReading.reading];
                CGContextAddLineToPoint(context, x1, y1);
            }
            CGContextStrokePath(context);
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
    
    str = [NSString stringWithString:appDelegate.unitMode?@"mg/dl":@"mmol/l"];
    CGContextSelectFont(context, "Helvetica", 12, kCGEncodingMacRoman);
    CGContextShowTextAtPoint(context, 15, startPoint.y+25, [str UTF8String], [str length]);
    
    CGContextSetStrokeColorWithColor(context, [UIColor orangeColor].CGColor);
    int yMax = [self getScreenY: appDelegate.unitMode?appDelegate.maxTarget:appDelegate.maxTarget * 18];
    CGContextMoveToPoint(context, startPoint.x, yMax);
    CGContextAddLineToPoint(context, startPoint.x+xAxisLength-12, yMax);
    CGContextStrokePath(context);
    str = [NSString stringWithFormat:appDelegate.unitMode?@"%.0f":@"%.1f", appDelegate.maxTarget];
	CGContextShowTextAtPoint(context, startPoint.x+xAxisLength-10, yMax+2, [str UTF8String], [str length]);
    
    int yMin = [self getScreenY: appDelegate.unitMode?appDelegate.minTarget:appDelegate.minTarget * 18];
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
        y = startPoint.y - 15 * value /18 + 1;
    }
    return y;
}

-(int) getScreenX:(int) index {
    int x;    
    x = startPoint.x + xAxisLength * (index + 1) / (sampleSize + 1);
    return x;
}

-(int) getTouchIndex:(int) x {
    int index;
    index = (x - startPoint.x) * (sampleSize + 1) / xAxisLength - 1;
    if (dayMode == 2)
        return index;
    else {
        time_t rawtime;
        struct tm * timeinfo;
        time ( &rawtime );
        timeinfo = localtime ( &rawtime );
        
        if (timeinfo->tm_mday >= 7)
            return timeinfo->tm_mday - 7 + index;
        else
            return timeinfo->tm_mday + index;
    }
}

- (void)drawCoordinateSystem:(CGContextRef)context {
	//[self drawDetialInfo:context];
	
	
	CGContextSetLineCap(context, kCGLineCapRound);
	CGContextSetLineWidth(context, 2);
	
	[self updateData];
    
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
    [displayPoints release];
    [super dealloc];
}


@end
