package com.mobielement.groupdisplay;

import java.util.List;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.Toast;

import com.mobielement.groupdisplay.service.GroupDisplayService;
import com.samsung.android.sdk.chord.SchordManager;

/**
 * A simple {@link android.support.v4.app.Fragment} subclass. Activities that
 * contain this fragment must implement the
 * {@link StartFragment.OnFragmentInteractionListener} interface to handle
 * interaction events. Use the {@link StartFragment#newInstance} factory method
 * to create an instance of this fragment.
 * 
 */
public class StartFragment extends Fragment implements OnClickListener{
	
	private static final String TAG = "[ME][GroupDisplay]";

    private static final String TAGClass = "StartFragment : ";
    
    private Button mBtnJoin;
    
    private Button mBtnSetup;
    
    private Button mBtnTest;
    
    private GroupDisplayService mChordService = null;
    
    
	// TODO: Rename parameter arguments, choose names that match
	// the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
	private static final String ARG_PARAM1 = "param1";
	private static final String ARG_PARAM2 = "param2";

	// TODO: Rename and change types of parameters
	private String mParam1;
	private String mParam2;

	

	/**
	 * Use this factory method to create a new instance of this fragment using
	 * the provided parameters.
	 * 
	 * @param param1
	 *            Parameter 1.
	 * @param param2
	 *            Parameter 2.
	 * @return A new instance of fragment StartFragment.
	 */
	// TODO: Rename and change types and number of parameters
	public static StartFragment newInstance(String param1, String param2) {
		StartFragment fragment = new StartFragment();
		Bundle args = new Bundle();
		args.putString(ARG_PARAM1, param1);
		args.putString(ARG_PARAM2, param2);
		fragment.setArguments(args);
		return fragment;
	}

	public StartFragment() {
		// Required empty public constructor
	}

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		if (getArguments() != null) {
			mParam1 = getArguments().getString(ARG_PARAM1);
			mParam2 = getArguments().getString(ARG_PARAM2);
		}
	}

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		// Inflate the layout for this fragment
		View view = inflater.inflate(R.layout.fragment_start, container, false);
		
		mBtnJoin = (Button)view.findViewById(R.id.buttonJoinGroup);
        mBtnSetup = (Button)view.findViewById(R.id.buttonSetupGroup);
        mBtnTest = (Button)view.findViewById(R.id.buttonTest);
        
        mBtnJoin.setEnabled(true);
        mBtnSetup.setEnabled(true);
        mBtnTest.setEnabled(true);
        
        mBtnJoin.setOnClickListener(this);
        mBtnSetup.setOnClickListener(this);
        mBtnTest.setOnClickListener(this);
        
		return view;
	}

	@Override
    public void onActivityCreated(Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
    }

	@Override
    public void onClick(View v) {
		switch (v.getId()) {
			case R.id.buttonJoinGroup:
				Log.d(TAG, TAGClass + "setOnClickListener() - Join Group");
				mChordService.start(SchordManager.INTERFACE_TYPE_WIFI);
				break;
			case R.id.buttonSetupGroup:
				Log.d(TAG, TAGClass + "setOnClickListener() - Setup Group");
				break;
			case R.id.buttonTest:
				Log.d(TAG, TAGClass + "setOnClickListener() - Setup Group");
				List<String> nodes = mChordService.getChordManager().getJoinedChannel(mChordService.getChannel()).getJoinedNodeList();
				for (String node : nodes) {
					Log.d(TAG, TAGClass + "node: " + node);
				}
				Toast.makeText(this.getActivity(), nodes.toString(), Toast.LENGTH_LONG).show();
				break;
			 default:
	            Log.d(TAG, TAGClass + "setOnClickListener() - default");
	            break;
		}
	}
	
	public void setService(GroupDisplayService chordService) {
        mChordService = chordService;
    }
}
