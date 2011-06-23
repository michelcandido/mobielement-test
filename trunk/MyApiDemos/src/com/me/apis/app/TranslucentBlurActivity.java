package com.me.apis.app;



import android.app.Activity;
import android.os.Bundle;
import android.view.WindowManager;

import com.me.apis.R;

public class TranslucentBlurActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		getWindow().setFlags(WindowManager.LayoutParams.FLAG_BLUR_BEHIND,
                WindowManager.LayoutParams.FLAG_BLUR_BEHIND);

		
		setContentView(R.layout.translucent_background);
	}

}
