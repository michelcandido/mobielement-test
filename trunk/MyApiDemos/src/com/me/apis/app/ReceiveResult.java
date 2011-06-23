package com.me.apis.app;


import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.text.Editable;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.me.apis.R;

public class ReceiveResult extends Activity {

	private static int CODE_RECEIVE = 0;
	private TextView mResults;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		this.setContentView(R.layout.receiveresult);
				 
		((Button)this.findViewById(R.id.btnReceiveResult)).setOnClickListener(new OnClickListener(){

			@Override
			public void onClick(View arg0) {
				Intent intent = new Intent(ReceiveResult.this, SendResult.class);				
				ReceiveResult.this.startActivityForResult(intent, CODE_RECEIVE);
			}});
		
		mResults = (EditText)this.findViewById(R.id.etReceiveResult);
	}
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		super.onActivityResult(requestCode, resultCode, data);
		if (CODE_RECEIVE == requestCode) {
			Editable text = (Editable)mResults.getText();
			if (RESULT_CANCELED == resultCode)
				text.append("(cancelled)");
			else {
				text.append("(okay ");
                text.append(Integer.toString(resultCode));
                text.append(") ");
                if (data != null) {
                    text.append(data.getAction());
                }
			}
			text.append("\n");
		}
	}

	
}
