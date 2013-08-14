package com.mobielement.groupdisplay.service;

import com.samsung.chord.ChordManager;
import com.samsung.chord.IChordChannel;
import com.samsung.chord.IChordChannelListener;
import com.samsung.chord.IChordManagerListener;
import com.samsung.chord.ChordManager.INetworkListener;



import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.os.Binder;
import android.os.Environment;
import android.os.IBinder;
import android.os.PowerManager;
import android.util.Log;

public class GroupDisplayService extends Service {
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "GDService : ";
    
    public static final String chordFilePath = Environment.getExternalStorageDirectory().getAbsolutePath() + "/Chord";
    
    private ChordManager mChord = null;
    
    private IGroupDisplayServiceListener mListener = null;
    
    private final IBinder mBinder = new GroupDisplayServiceBinder();
    
    private PowerManager.WakeLock mWakeLock = null;
    
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
    
    // Start chord
    public int start(int interfaceType) {
    	
    	//acqureWakeLock();
        // #3. set some values before start
        return mChord.start(interfaceType, new IChordManagerListener() {
            @Override
            public void onStarted(String name, int reason) {
                Log.d(TAG, TAGClass + "onStarted chord");

                if (null != mListener)
                    mListener.onUpdateNodeInfo(name, mChord.getIp());

                if (STARTED_BY_RECONNECTION == reason) {
                    Log.e(TAG, TAGClass + "STARTED_BY_RECONNECTION");
                    return;
                }
                // if(STARTED_BY_USER == reason) : Returns result of calling
                // start

                // #4.(optional) listen for public channel
                IChordChannel channel = mChord.joinChannel(ChordManager.PUBLIC_CHANNEL,
                        mChannelListener);

                if (null == channel) {
                    Log.e(TAG, TAGClass + "fail to join public");
                }
            }

            @Override
            public void onNetworkDisconnected() {
                Log.e(TAG, TAGClass + "onNetworkDisconnected()");
                if (null != mListener)
                    mListener.onNetworkDisconnected();
            }

            @Override
            public void onError(int error) {
                // TODO Auto-generated method stub

            }

        });
    }
    
    // This interface defines a listener for chord channel events.
    private IChordChannelListener mChannelListener = new IChordChannelListener() {

		@Override
		public void onDataReceived(String arg0, String arg1, String arg2,
				byte[][] arg3) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileChunkReceived(String arg0, String arg1, String arg2,
				String arg3, String arg4, String arg5, long arg6, long arg7) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileChunkSent(String arg0, String arg1, String arg2,
				String arg3, String arg4, String arg5, long arg6, long arg7,
				long arg8) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileFailed(String arg0, String arg1, String arg2,
				String arg3, String arg4, int arg5) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileReceived(String arg0, String arg1, String arg2,
				String arg3, String arg4, String arg5, long arg6, String arg7) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileSent(String arg0, String arg1, String arg2,
				String arg3, String arg4, String arg5) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onFileWillReceive(String arg0, String arg1, String arg2,
				String arg3, String arg4, String arg5, long arg6) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onNodeJoined(String arg0, String arg1) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onNodeLeft(String arg0, String arg1) {
			// TODO Auto-generated method stub
			
		}};
    
    // Release chord
    public void release() throws Exception {
        if (mChord != null) {
            mChord.stop();
            mChord.setNetworkListener(null);
            mChord = null;
            Log.d(TAG, TAGClass + "[UNREGISTER] Chord unregistered");
        }

    }
    
    public String getPublicChannel() {
        return ChordManager.PUBLIC_CHANNEL;
    }
    
    private void acqureWakeLock(){
		if(null == mWakeLock){
			PowerManager powerMgr = (PowerManager)getSystemService(Context.POWER_SERVICE);
			mWakeLock = powerMgr.newWakeLock(PowerManager.FULL_WAKE_LOCK, "ChordApiDemo Lock");
			Log.d(TAG, "acqureWakeLock : new");
		}
	  
		if(mWakeLock.isHeld()){
			Log.w(TAG, "acqureWakeLock : already acquire");
			mWakeLock.release();
		}
	  
		 Log.d(TAG, "acqureWakeLock : acquire");
		 mWakeLock.acquire();
	}
    
}
