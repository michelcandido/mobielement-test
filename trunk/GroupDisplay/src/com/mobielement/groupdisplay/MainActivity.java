package com.mobielement.groupdisplay;

import com.mobielement.groupdisplay.R;
import com.mobielement.groupdisplay.service.GroupDisplayService;
import com.mobielement.groupdisplay.service.GroupDisplayService.GroupDisplayServiceBinder;
import com.mobielement.groupdisplay.service.GroupDisplayService.IGroupDisplayServiceListener;


import android.os.Bundle;
import android.os.IBinder;
import android.app.Activity;
import android.app.FragmentTransaction;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.util.Log;
import android.view.Menu;
import android.view.Window;
import android.view.WindowManager.LayoutParams;

public class MainActivity extends Activity implements IGroupDisplayServiceListener{
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "MainActivity : ";
    
    private static final int START_FRAGMENT = 1001;
    
    private int mCurrentFragment = START_FRAGMENT;
    
    private StartFragment mStartFragment;
    
    private FragmentTransaction mFragmentTransaction;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Log.v(TAG, TAGClass + "onCreate");
		setContentView(R.layout.activity_main);
		
		// keep the screen on
		Window window = this.getWindow();
		window.setFlags(LayoutParams.FLAG_FULLSCREEN, LayoutParams.FLAG_FULLSCREEN);
		window.addFlags(LayoutParams.FLAG_KEEP_SCREEN_ON);
		
		startGDService();
		bindGDService();
		
		mStartFragment = (StartFragment)getFragmentManager().findFragmentById(R.id.fragment_start);
		
		setFragment(START_FRAGMENT);
	}

	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}
	
	@Override
    protected void onResume() {
        // TODO Auto-generated method stub
        super.onResume();
        Log.v(TAG, TAGClass + "onResume");
        //refreshInterfaceType();
    }

    @Override
    protected void onDestroy() {
        // TODO Auto-generated method stub
        super.onDestroy();
        unbindGDService();
        stopGDService();
        Log.v(TAG, TAGClass + "onDestroy");
    }
    
    private void setFragment(int currentFragment) {
        mCurrentFragment = currentFragment;
        mFragmentTransaction = getFragmentManager().beginTransaction();
        if (mCurrentFragment == START_FRAGMENT) {
            mFragmentTransaction.show(mStartFragment);
            //mFragmentTransaction.hide(mChannelTestFragment);
            //mFragmentTransaction.hide(mDataTestFragment);
        } /*else if (mCurrentFragment == CHANNELTEST_FRAGMENT) {
            mFragmentTransaction.show(mChannelTestFragment);
            mFragmentTransaction.hide(mInterfaceTestFragment);
            mFragmentTransaction.hide(mDataTestFragment);
        } else if (mCurrentFragment == DATATEST_FRAGMENT) {
            mFragmentTransaction.show(mDataTestFragment);
            mFragmentTransaction.hide(mInterfaceTestFragment);
            mFragmentTransaction.hide(mChannelTestFragment);
        }*/
        mFragmentTransaction.commit();
    }
	
	// **********************************************************************
    // Using Service
    // **********************************************************************
    private GroupDisplayService mGDService = null;

    private ServiceConnection mConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName name, IBinder service) {
            // TODO Auto-generated method stub
            Log.d(TAG, TAGClass + "onServiceConnected()");
            GroupDisplayServiceBinder binder = (GroupDisplayServiceBinder)service;
            mGDService = binder.getService();
            try {
            	mGDService.initialize(MainActivity.this);
            } catch (Exception e) {
                e.printStackTrace();
            }

            //refreshInterfaceType();
            //mChannelTestFragment.setService(mChordService);
            //mDataTestFragment.setService(mChordService);
        }

        @Override
        public void onServiceDisconnected(ComponentName name) {
            // TODO Auto-generated method stub
            Log.i(TAG, TAGClass + "onServiceDisconnected()");
            mGDService = null;
        }
    };
	
	private void startGDService() {
        Log.i(TAG, TAGClass + "startGDService()");
        Intent intent = new Intent("com.mobielement.groupdisplay.service.GroupDisplayService.SERVICE_START");
        startService(intent);
    }
	
	private void stopGDService() {
        Log.i(TAG, TAGClass + "stopService()");
        Intent intent = new Intent("com.mobielement.groupdisplay.service.GroupDisplayService.SERVICE_STOP");
        stopService(intent);
    }
	
	private void bindGDService() {
        Log.i(TAG, TAGClass + "bindGDService()");
        if (mGDService == null) {
            Intent intent = new Intent(
                    "com.mobielement.groupdisplay.service.GroupDisplayService.SERVICE_BIND");
            bindService(intent, mConnection, Context.BIND_AUTO_CREATE);
        }
    }
	
	private void unbindGDService() {
        Log.i(TAG, TAGClass + "unbindGDService()");

        if (null != mGDService) {
            unbindService(mConnection);
        }
        mGDService = null;
    }

	// **********************************************************************
    // IChordServiceListener
    // **********************************************************************
    @Override
    public void onReceiveMessage(String node, String channel, String message) {
        //mDataTestFragment.onMessageReceived(node, channel, message);
    }

    @Override
    public void onFileWillReceive(String node, String channel, String fileName, String exchangeId) {
        //mDataTestFragment.onFileNotify(node, channel, fileName, exchangeId);
    }

    @Override
    public void onFileProgress(boolean bSend, String node, String channel, int progress,
            String exchangeId) {
        //mDataTestFragment.onFileProgress(bSend, node, channel, progress, exchangeId);

    }

    @Override
    public void onFileCompleted(int reason, String node, String channel, String exchangeId,
            String fileName) {
        //mDataTestFragment.onFileCompleted(reason, node, channel, fileName, exchangeId);
    }

    @Override
    public void onNodeEvent(String node, String channel, boolean bJoined) {
        if (bJoined) {
            if (channel.equals(mGDService.getPublicChannel())) {
                //mChannelTestFragment.onPublicChannelNodeJoined(node);
                //mDataTestFragment.onNodeJoined(node, channel);
            } else {
                //mChannelTestFragment.onJoinedChannelNodeJoined(node);
                //mDataTestFragment.onNodeJoined(node, channel);
            }
            return;
        }

        if (channel.equals(mGDService.getPublicChannel())) {
            //mChannelTestFragment.onPublicChannelNodeLeaved(node);
            //mDataTestFragment.onNodeLeaved(node, channel);
        } else {
            //mChannelTestFragment.onJoinedChannelNodeLeaved(node);
            //mDataTestFragment.onNodeLeaved(node, channel);
        }
    }

    @Override
    public void onNetworkDisconnected() {
        //mChannelTestFragment.onNetworkDisconnected();
        //mDataTestFragment.onNetworkDisconnected();
    }

    @Override
    public void onUpdateNodeInfo(String nodeName, String ipAddress) {
        //mChannelTestFragment.setMyNodeInfo(nodeName, ipAddress);
        //mDataTestFragment.setMyNodeInfo(nodeName, ipAddress);
    }

    @Override
    public void onConnectivityChanged() {
        //refreshInterfaceType();
    }

}
