package com.me.apis.app;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.me.apis.R;

public class SendResult extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.sendresult);
		
		((Button)this.findViewById(R.id.btnCorky)).setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View v) {
				SendResult.this.setResult(RESULT_OK, (new Intent()).setAction("Corky!"));
				finish();
			}});
		
		((Button)this.findViewById(R.id.btnViolet)).setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View v) {
				SendResult.this.setResult(RESULT_OK, (new Intent()).setAction("Violet!"));
				finish();				
			}});
	}

}
