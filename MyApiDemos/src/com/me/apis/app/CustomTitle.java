package com.me.apis.app;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.me.apis.R;

public class CustomTitle extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.getWindow().requestFeature(Window.FEATURE_CUSTOM_TITLE);
		this.setContentView(R.layout.customtitle);
		CustomTitle.this.getWindow().setFeatureInt(Window.FEATURE_CUSTOM_TITLE, R.layout.customtitle_title);
							
		updateLeftTitle();
		updateRightTitle();
		
		((Button)(findViewById(R.id.btnLeft))).setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View arg0) {
				updateLeftTitle();
			}});
		((Button)(findViewById(R.id.btnRight))).setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View arg0) {
				updateRightTitle();
			}});
	}

	private void updateLeftTitle() {
		String title = ((EditText)(CustomTitle.this.findViewById(R.id.textLeft))).getText().toString();
		((TextView)(CustomTitle.this.findViewById(R.id.left_title))).setText(title);
	}
	
	private void updateRightTitle() {
		String title = ((EditText)(CustomTitle.this.findViewById(R.id.textRight))).getText().toString();				
		((TextView)(CustomTitle.this.findViewById(R.id.right_title))).setText(title);
	}
	
	@Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
		
	}

}
