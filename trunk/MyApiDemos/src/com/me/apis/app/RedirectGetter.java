package com.me.apis.app;

import android.app.Activity;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

import com.me.apis.R;

public class RedirectGetter extends Activity {
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.redirectgetter);	
		
		Button btnApply = (Button)findViewById(R.id.btnRedirectApply);
		btnApply.setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View arg0) {
				SharedPreferences prefs = RedirectGetter.this.getSharedPreferences("RedirectData", MODE_PRIVATE);
				EditText et = (EditText)findViewById(R.id.etRedirectInput);
				Editor editor = prefs.edit();
				editor.putString("text", et.getText().toString());
				if (editor.commit())
					setResult(RESULT_OK);
				finish();
			}});
	}

}
