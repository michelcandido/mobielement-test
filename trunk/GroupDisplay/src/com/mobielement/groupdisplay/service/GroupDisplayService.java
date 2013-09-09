package com.mobielement.groupdisplay.service;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.os.Binder;
import android.os.Environment;
import android.os.IBinder;
import android.os.PowerManager;
import android.util.Log;
import android.widget.Toast;

import com.samsung.android.sdk.SsdkUnsupportedException;
import com.samsung.android.sdk.chord.InvalidInterfaceException;
import com.samsung.android.sdk.chord.Schord;
import com.samsung.android.sdk.chord.SchordChannel;
import com.samsung.android.sdk.chord.SchordManager;
import com.samsung.android.sdk.chord.SchordManager.NetworkListener;


public class GroupDisplayService extends Service {
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "GDService : ";
    
    public static final String chordFilePath = Environment.getExternalStorageDirectory().getAbsolutePath() + "/Chord";
    
    private static final String CHORD_GROUP_DISPLAY_CHANNEL = "com.mobielement.groupdisplay.service.GROUPDISPLAYCHANNEL";
    
    private static final String CHORD_GROUP_DISPLAY_MESSAGE_TYPE = "com.mobielement.groupdisplay.service.MESSAGE_TYPE";
    
    private Schord mChord = null;
    
    private SchordManager mChordManager = null;
    
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

        /****************************************************
         * 1. GetInstance
         ****************************************************/
        mChord = new Schord();
        try {
        	mChord.initialize(this);
        } catch (SsdkUnsupportedException e) {
            if (e.getType() == SsdkUnsupportedException.VENDOR_NOT_SUPPORTED) {
                // Vender is not SAMSUNG
                return;
            }
        }      
        mChordManager = new SchordManager(this);        
        Log.d(TAG, TAGClass + "[Initialize] Chord Initialized");
        
        /****************************************************
         * 2. Set some values before start If you want to use secured channel,
         * you should enable SecureMode. Please refer
         * UseSecureChannelFragment.java mChordManager.enableSecureMode(true);
         * 
         *
         * Once you will use sendFile or sendMultiFiles, you have to call setTempDirectory  
         * mChordManager.setTempDirectory(Environment.getExternalStorageDirectory().getAbsolutePath()
         *       + "/Chord");
         ****************************************************/
        mChordManager.setTempDirectory(Environment.getExternalStorageDirectory().getAbsolutePath()+ "/Chord");
        mChordManager.setLooper(this.getMainLooper());

        /**
         * Optional. If you need listening network changed, you can set callback
         * before starting chord.
         */
        mListener = listener;
        mChordManager.setNetworkListener(new NetworkListener() {

            @Override
            public void onDisconnected(int interfaceType) {
                if (null != mListener) {
                    Toast.makeText(GroupDisplayService.this,
                            getInterfaceName(interfaceType) + " is disconnected",
                            Toast.LENGTH_SHORT).show();
                    mListener.onConnectivityChanged();
                }
            }

            @Override
            public void onConnected(int interfaceType) {
                if (null != mListener) {
                    Toast.makeText(GroupDisplayService.this,
                            getInterfaceName(interfaceType) + " is connected",
                            Toast.LENGTH_SHORT).show();
                    mListener.onConnectivityChanged();
                }
            }
        });        
    }
    
    // Start chord
    public void start(int interfaceType) {
    	
    	//acqureWakeLock();
        // #3. Start Chord
    	try {
            mChordManager.start(interfaceType, mManagerListener);            
            Log.d(TAG, TAGClass + "    start(" + getInterfaceName(interfaceType) + ")");            
        } catch (IllegalArgumentException e) {
        	Log.d(TAG, TAGClass + "    Fail to start -" + e.getMessage());
        } catch (InvalidInterfaceException e) {
        	Log.d(TAG, TAGClass + "    There is no such a connection.");
        } catch (Exception e) {
        	Log.d(TAG, TAGClass + "    Fail to start -" + e.getMessage());
        }    	        
    }
    
    /**
     * ChordManagerListener
     */
    SchordManager.StatusListener mManagerListener = new SchordManager.StatusListener() {

        @Override
        public void onStarted(String nodeName, int reason) {
            /**
             * 4. Chord has started successfully
             */            
            if (reason == STARTED_BY_USER) {
                // Success to start by calling start() method
            	Log.d(TAG, TAGClass + "    >onStarted(" + nodeName + ", STARTED_BY_USER)");
                joinGroupDisplayChannel();
            } else if (reason == STARTED_BY_RECONNECTION) {
                // Re-start by network re-connection.
            	Log.d(TAG, TAGClass + "    >onStarted(" + nodeName + ", STARTED_BY_RECONNECTION)");
            }

        }

        @Override
        public void onStopped(int reason) {
            /**
             * 8. Chord has stopped successfully
             */            
            if (STOPPED_BY_USER == reason) {
                // Success to stop by calling stop() method
            	Log.d(TAG, TAGClass + "    >onStopped(STOPPED_BY_USER)");
            } else if (NETWORK_DISCONNECTED == reason) {
                // Stopped by network disconnected
            	Log.d(TAG, TAGClass + "    >onStopped(NETWORK_DISCONNECTED)");
            }
        }
    };

    private void joinGroupDisplayChannel() {
        SchordChannel channel = null;
        
        /**
         * 5. Join my channel
         */
        Log.d(TAG, TAGClass + "    joinChannel");
        channel = mChordManager.joinChannel(CHORD_GROUP_DISPLAY_CHANNEL, mChannelListener);

        if (channel == null) {
        	Log.d(TAG, TAGClass + "    Fail to joinChannel");
        }
    }

    private void release() throws Exception {
        if (mChordManager == null)
            return;

        /**
         * If you registered NetworkListener, you should unregister it.
         */
        mChordManager.setNetworkListener(null);

        /**
         * 7. Stop Chord. You can call leaveChannel explicitly.
         * mChordManager.leaveChannel(CHORD_HELLO_TEST_CHANNEL);
         */
        Log.d(TAG, TAGClass + "    stop");
        mChordManager.stop();        
    }
    
 
    
 // ***************************************************
    // ChordChannelListener
    // ***************************************************
    private SchordChannel.StatusListener mChannelListener = new SchordChannel.StatusListener() {

        /**
         * Called when a node leave event is raised on the channel.
         */
        @Override
        public void onNodeLeft(String fromNode, String fromChannel) {
        	Log.d(TAG, TAGClass + "    >onNodeLeft(" + fromNode + ")");
        }

        /**
         * Called when a node join event is raised on the channel
         */
        @Override
        public void onNodeJoined(String fromNode, String fromChannel) {
        	Log.d(TAG, TAGClass + "    >onNodeJoined(" + fromNode + ")");

            /**
             * 6. Send data to joined node
             */
            byte[][] payload = new byte[1][];
            payload[0] = "Hello chord!".getBytes();

            SchordChannel channel = mChordManager.getJoinedChannel(fromChannel);
            channel.sendData(fromNode, CHORD_GROUP_DISPLAY_MESSAGE_TYPE, payload);
            Log.d(TAG, TAGClass + "    sendData(" + fromNode + ", " + new String(payload[0]) + ")");
        }

        /**
         * Called when the data message received from the node.
         */
        @Override
        public void onDataReceived(String fromNode, String fromChannel, String payloadType,
                byte[][] payload) {

            /**
             * 6. Received data from other node
             */
            if(payloadType.equals(CHORD_GROUP_DISPLAY_MESSAGE_TYPE)){
            	Log.d(TAG, TAGClass + "    >onDataReceived(" + fromNode + ", " + new String( payload[0]) + ")");
            }
        }

        /**
         * The following callBacks are not used in this Fragment. Please refer
         * to the SendFilesFragment.java
         */
        @Override
        public void onMultiFilesWillReceive(String fromNode, String fromChannel, String fileName,
                String taskId, int totalCount, String fileType, long fileSize) {

        }

        @Override
        public void onMultiFilesSent(String toNode, String toChannel, String fileName,
                String taskId, int index, String fileType) {

        }

        @Override
        public void onMultiFilesReceived(String fromNode, String fromChannel, String fileName,
                String taskId, int index, String fileType, long fileSize, String tmpFilePath) {

        }

        @Override
        public void onMultiFilesFinished(String node, String channel, String taskId, int reason) {

        }

        @Override
        public void onMultiFilesFailed(String node, String channel, String fileName, String taskId,
                int index, int reason) {

        }

        @Override
        public void onMultiFilesChunkSent(String toNode, String toChannel, String fileName,
                String taskId, int index, String fileType, long fileSize, long offset,
                long chunkSize) {

        }

        @Override
        public void onMultiFilesChunkReceived(String fromNode, String fromChannel, String fileName,
                String taskId, int index, String fileType, long fileSize, long offset) {

        }

        @Override
        public void onFileWillReceive(String fromNode, String fromChannel, String fileName,
                String hash, String fileType, String exchangeId, long fileSize) {

        }

        @Override
        public void onFileSent(String toNode, String toChannel, String fileName, String hash,
                String fileType, String exchangeId) {

        }

        @Override
        public void onFileReceived(String fromNode, String fromChannel, String fileName,
                String hash, String fileType, String exchangeId, long fileSize, String tmpFilePath) {

        }

        @Override
        public void onFileFailed(String node, String channel, String fileName, String hash,
                String exchangeId, int reason) {

        }

        @Override
        public void onFileChunkSent(String toNode, String toChannel, String fileName, String hash,
                String fileType, String exchangeId, long fileSize, long offset, long chunkSize) {

        }

        @Override
        public void onFileChunkReceived(String fromNode, String fromChannel, String fileName,
                String hash, String fileType, String exchangeId, long fileSize, long offset) {

        }

    };
    
    
    
    public String getChannel() {
        return CHORD_GROUP_DISPLAY_CHANNEL;
    }
    
    public SchordManager getChordManager() {
    	return mChordManager;
    }        
    
    private String getInterfaceName(int interfaceType) {
        if (SchordManager.INTERFACE_TYPE_WIFI == interfaceType)
            return "Wi-Fi";
        else if (SchordManager.INTERFACE_TYPE_WIFI_AP == interfaceType)
            return "Mobile AP";
        else if (SchordManager.INTERFACE_TYPE_WIFI_P2P == interfaceType)
            return "Wi-Fi Direct";

        return "UNKNOWN";
    }
    
}
