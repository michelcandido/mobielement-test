package com.me.apis.app;


import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

import com.me.apis.R;

public class RedirectMain extends Activity {

	static final int INIT_TEXT_REQUEST = 0;
    static final int NEW_TEXT_REQUEST = 1;
    
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		if (requestCode == INIT_TEXT_REQUEST) {
			if (resultCode == RESULT_CANCELED) {
				finish();
			} else {
				loadPrefs();
			}
		} else if (requestCode == NEW_TEXT_REQUEST) {
            if (resultCode != RESULT_CANCELED) {
                loadPrefs();
            }

        }
		
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.redirectmain);
		if (!loadPrefs()) {
			Intent intent = new Intent(this, RedirectGetter.class);
			startActivityForResult(intent, INIT_TEXT_REQUEST);
		}
		
		Button btnClear = (Button)findViewById(R.id.btnRedirectClear);
		btnClear.setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View v) {
				SharedPreferences prefs = RedirectMain.this.getSharedPreferences("RedirectData", MODE_PRIVATE);
				Editor editor = prefs.edit();
				editor.remove("text");
				editor.commit();
				finish();
				
			}});
		
		Button btnNew = (Button)findViewById(R.id.btnRedirectNew);
		btnNew.setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View v) {
				Intent intent = new Intent(RedirectMain.this, RedirectGetter.class);
	            startActivityForResult(intent, NEW_TEXT_REQUEST);
				
			}});
	}
	
	private boolean loadPrefs() {
		SharedPreferences prefs = this.getSharedPreferences("RedirectData", MODE_PRIVATE);
		String text = prefs.getString("text", null);
		if (text != null) {
			TextView tv = (TextView) findViewById(R.id.tvRedirectResult);
			tv.setText(text);
			return true;
		} else 
			return false;
	}

}
