package com.me.apis.app;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.me.apis.R;

public class RedirectEnter extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.redirectenter);
		
		Button btnGo = (Button)this.findViewById(R.id.btnRedirectGo);
		btnGo.setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View arg0) {
				Intent intent = new Intent(RedirectEnter.this, RedirectMain.class);
				startActivity(intent);
			}});
	}

}
