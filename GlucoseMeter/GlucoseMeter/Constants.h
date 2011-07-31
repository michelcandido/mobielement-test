//
//  Constants.h
//  MCHP MFI
//
//  Created by Tony on 7/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

// 得到测试数据更新通知
#define DATAUPDATENOTIFICATION @"MEASUREDATAUPDATE"

// 得到设备准备好的通知,然后去里面取版本信息
#define ACCREADYNOTIFICATION @"READYWITHVERSIONNOTIFICATION"

#define IAMATLINENOTIFICATION @"IAMATLINENOTIFICATION"

// 坐标轴的起始坐标点
#define STARTPOINTX 30
#define STARTPOINTY 30

// 坐标轴X上刻度的间隔
#define GAPFORX 30

// 坐标轴Y上的刻度的间隔
#define GAPFORY 30

// Days of a month
#define NUMBER_OF_DAYS 31

// Readings of a day
#define NUMBER_OF_READINGS 6