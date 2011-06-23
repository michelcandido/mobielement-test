package com.me.apis.app;



import android.app.Activity;
import android.os.Bundle;
import android.view.Window;

import com.me.apis.R;

public class DialogActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		requestWindowFeature(Window.FEATURE_LEFT_ICON);
		//requestWindowFeature(Window.FEATURE_CUSTOM_TITLE);
		
		setContentView(R.layout.dialog);
        
		
        getWindow().setFeatureDrawableResource(Window.FEATURE_LEFT_ICON, android.R.drawable.ic_dialog_alert);
        
        
		//getWindow().setFeatureInt(Window.FEATURE_CUSTOM_TITLE, R.layout.helloworld);
		
	}

}
