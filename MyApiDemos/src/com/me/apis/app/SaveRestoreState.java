package com.me.apis.app;


import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.EditText;

import com.me.apis.R;

public class SaveRestoreState extends Activity {

	@Override
	protected void onPause() {
		// TODO Auto-generated method stub
		super.onPause();
		SharedPreferences saves = this.getPreferences(Context.MODE_PRIVATE);
		SharedPreferences.Editor editor = saves.edit();
		editor.putString("savedText", ((EditText)(this.findViewById(R.id.save_yes))).getText().toString());
		editor.commit();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.saverestorestate);
		
		SharedPreferences saves = this.getPreferences(Context.MODE_PRIVATE);
		String savedText = saves.getString("savedText", "Initial text.");
		((EditText)(this.findViewById(R.id.save_yes))).setText(savedText);
	}

	@Override
	protected void onSaveInstanceState(Bundle outState) {
		// TODO Auto-generated method stub
		super.onSaveInstanceState(outState);
		
	}

}
