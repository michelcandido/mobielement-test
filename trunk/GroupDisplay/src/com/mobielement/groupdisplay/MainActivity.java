package com.mobielement.groupdisplay;

import com.mobielement.groupdisplay.R;
import com.mobielement.groupdisplay.service.GroupDisplayService;
import com.mobielement.groupdisplay.service.GroupDisplayService.GroupDisplayServiceBinder;
import com.mobielement.groupdisplay.service.GroupDisplayService.IGroupDisplayServiceListener;

import android.os.Bundle;
import android.os.IBinder;
import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.util.Log;
import android.view.Menu;

public class MainActivity extends Activity implements IGroupDisplayServiceListener{
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "MainActivity : ";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Log.v(TAG, TAGClass + "onCreate");
		setContentView(R.layout.activity_main);
		
		startGDService();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}
	
	// **********************************************************************
    // Using Service
    // **********************************************************************
    private GroupDisplayService mChordService = null;

    private ServiceConnection mConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName name, IBinder service) {
            // TODO Auto-generated method stub
            Log.d(TAG, TAGClass + "onServiceConnected()");
            GroupDisplayServiceBinder binder = (GroupDisplayServiceBinder)service;
            mChordService = binder.getService();
            try {
                mChordService.initialize(MainActivity.this);
            } catch (Exception e) {
                e.printStackTrace();
            }

            refreshInterfaceType();
            mChannelTestFragment.setService(mChordService);
            mDataTestFragment.setService(mChordService);
        }

        @Override
        public void onServiceDisconnected(ComponentName name) {
            // TODO Auto-generated method stub
            Log.i(TAG, TAGClass + "onServiceDisconnected()");
            mChordService = null;
        }
    };
	
	private void startGDService() {
        Log.i(TAG, TAGClass + "startGDService()");
        Intent intent = new Intent("com.mobielement.groupdisplay.service.GroupDisplayService.SERVICE_START");
        startService(intent);
    }
	
	private void bindGDService() {
        Log.i(TAG, TAGClass + "bindGDService()");
        if (mChordService == null) {
            Intent intent = new Intent(
                    "com.mobielement.groupdisplay.service.GroupDisplayService.SERVICE_BIND");
            bindService(intent, mConnection, Context.BIND_AUTO_CREATE);
        }
    }
	
	private void unbindGDService() {
        Log.i(TAG, TAGClass + "unbindGDService()");

        if (null != mChordService) {
            unbindService(mConnection);
        }
        mChordService = null;
    }

	@Override
	public void onReceiveMessage(String node, String channel, String message) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onFileWillReceive(String node, String channel, String fileName,
			String exchangeId) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onFileProgress(boolean bSend, String node, String channel,
			int progress, String exchangeId) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onFileCompleted(int reason, String node, String channel,
			String exchangeId, String fileName) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onNodeEvent(String node, String channel, boolean bJoined) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onNetworkDisconnected() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onUpdateNodeInfo(String nodeName, String ipAddress) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onConnectivityChanged() {
		// TODO Auto-generated method stub
		
	}

}
