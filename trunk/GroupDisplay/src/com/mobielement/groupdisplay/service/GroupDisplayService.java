package com.mobielement.groupdisplay.service;

import com.samsung.chord.ChordManager;
import com.samsung.chord.ChordManager.INetworkListener;



import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.Environment;
import android.os.IBinder;
import android.util.Log;

public class GroupDisplayService extends Service {
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "GDService : ";
    
    public static final String chordFilePath = Environment.getExternalStorageDirectory().getAbsolutePath() + "/Chord";
    
    private ChordManager mChord = null;
    
    private IGroupDisplayServiceListener mListener = null;
    
    private final IBinder mBinder = new GroupDisplayServiceBinder();
    
    // for notifying event to Activity
    public interface IGroupDisplayServiceListener {
        void onReceiveMessage(String node, String channel, String message);

        void onFileWillReceive(String node, String channel, String fileName, String exchangeId);

        void onFileProgress(boolean bSend, String node, String channel, int progress,
                String exchangeId);

        public static final int SENT = 0;

        public static final int RECEIVED = 1;

        public static final int CANCELLED = 2;

        public static final int REJECTED = 3;

        public static final int FAILED = 4;

        void onFileCompleted(int reason, String node, String channel, String exchangeId,
                String fileName);

        void onNodeEvent(String node, String channel, boolean bJoined);

        void onNetworkDisconnected();

        void onUpdateNodeInfo(String nodeName, String ipAddress);

        void onConnectivityChanged();
    }
    
	public GroupDisplayService() {
	}
	
	public class GroupDisplayServiceBinder extends Binder {
        public GroupDisplayService getService() {
            return GroupDisplayService.this;
        }
    }

	@Override
	public IBinder onBind(Intent intent) {
		Log.d(TAG, TAGClass + "onBind()");
		return mBinder;
	}
	
	@Override
    public void onCreate() {
        Log.d(TAG, TAGClass + "onCreate()");
        super.onCreate();
    }

    @Override
    public void onDestroy() {
        Log.d(TAG, TAGClass + "onDestroy()");
        super.onDestroy();
        try {
            release();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onRebind(Intent intent) {
        Log.d(TAG, TAGClass + "onRebind()");
        super.onRebind(intent);
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(TAG, TAGClass + "onStartCommand()");
        return super.onStartCommand(intent, START_NOT_STICKY, startId);
    }
    
    @Override
    public boolean onUnbind(Intent intent) {
        Log.d(TAG, TAGClass + "onUnbind()");
        return super.onUnbind(intent);
    }
    
 // Initialize chord
    public void initialize(IGroupDisplayServiceListener listener) throws Exception {
        if (mChord != null) {
            return;
        }

        // #1. GetInstance
        mChord = ChordManager.getInstance(this);
        Log.d(TAG, TAGClass + "[Initialize] Chord Initialized");

        mListener = listener;

        // #2. set some values before start
        mChord.setTempDirectory(chordFilePath);
        mChord.setHandleEventLooper(getMainLooper());

        // Optional.
        // If you need listening network changed, you can set callback before
        // starting chord.
        mChord.setNetworkListener(new INetworkListener() {

            @Override
            public void onConnected(int interfaceType) {
                if (null != mListener) {
                    mListener.onConnectivityChanged();
                }
            }

            @Override
            public void onDisconnected(int interfaceType) {
                if (null != mListener) {
                    mListener.onConnectivityChanged();
                }
            }

        });
    }
    // Release chord
    public void release() throws Exception {
        if (mChord != null) {
            mChord.stop();
            mChord.setNetworkListener(null);
            mChord = null;
            Log.d(TAG, TAGClass + "[UNREGISTER] Chord unregistered");
        }

    }
    
    
}
