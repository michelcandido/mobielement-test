//
//  TestReading.h
//  GlucoseMeter
//
//  Created by Tony on 7/26/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TestReading : NSObject {
    float reading;
    int mealMode; //0: pre-meal; 1: post-meal
    NSString *time;
    NSString *date;
}

@property (nonatomic) float reading;
@property (nonatomic) int mealMode;
@property (nonatomic, retain) NSString *time;
@property (nonatomic, retain) NSString *date;

@end
