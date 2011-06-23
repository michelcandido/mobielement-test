package com.me.apis;

import java.text.Collator;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import android.app.ListActivity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.SimpleAdapter;

public class MyApiDemos extends ListActivity {
	private static String PACKAGE_FILTER = MyApiDemos.class.getPackage().getName();
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        Intent intent = getIntent();
        String path = intent.getStringExtra("com.example.android.apis.Path");
        
        if (path == null) {
            path = "";
        }
        
        SimpleAdapter adapter = new SimpleAdapter(this, getData(path), android.R.layout.simple_list_item_1, new String[]{"title"}, new int[] { android.R.id.text1 });
        this.setListAdapter(adapter);
        
        getListView().setTextFilterEnabled(true);
    }
    
    protected List<Map<String, Object>> getData(String prefix) {
    	List<Map<String, Object>> myData = new ArrayList<Map<String, Object>>();
    	
    	PackageManager pm = this.getPackageManager();
    	Intent sampleIntent = new Intent(Intent.ACTION_MAIN);
    	sampleIntent.addCategory(Intent.CATEGORY_SAMPLE_CODE);
    	List<ResolveInfo> sampleActivities = pm.queryIntentActivities(sampleIntent, 0);
    	
    	List<ResolveInfo> temp = new ArrayList<ResolveInfo>();
    	for(ResolveInfo activity:sampleActivities){
    		String packageName = activity.activityInfo.packageName;
    		if (PACKAGE_FILTER.equalsIgnoreCase(packageName))
    			temp.add(activity);
    	}
    	sampleActivities = temp;
    	
    	if (null == sampleActivities)
    		return myData;
    	
    	String[] prefixPath;        
        if (prefix.equals("")) {
            prefixPath = null;
        } else {
            prefixPath = prefix.split("/");
        }
        
    	Map<String, Boolean> entries = new HashMap<String, Boolean>();
    	
    	for(ResolveInfo activity:sampleActivities){
    		CharSequence labelSeq = activity.loadLabel(pm);
    		String label;
    		if (labelSeq != null)
    			label = labelSeq.toString();
    		else
    			label = activity.activityInfo.name;
    		
    		if (prefix.length() == 0 || label.startsWith(prefix)) {
    			String[] labelPath = label.split("/");
    			String nextLabel;
    			if (prefixPath == null)
    				nextLabel = labelPath[0];
    			else
    				nextLabel = labelPath[prefixPath.length];
    			
    			if ((prefixPath != null ? prefixPath.length : 0) == labelPath.length - 1) {
                    addItem(myData, nextLabel, activityIntent(
                    		activity.activityInfo.applicationInfo.packageName,
                    		activity.activityInfo.name));
                } else {
                    if (entries.get(nextLabel) == null) {
                        addItem(myData, nextLabel, browseIntent(prefix.equals("") ? nextLabel : prefix + "/" + nextLabel));
                        entries.put(nextLabel, true);
                    }
                }
    		}    		    		
    	}
    	Collections.sort(myData, sDisplayNameComparator);
    	return myData;
    }
    
    private final static Comparator<Map<String, Object>> sDisplayNameComparator = new Comparator<Map<String, Object>>() {
        private final Collator   collator = Collator.getInstance();

        public int compare(Map<String, Object> map1, Map<String, Object> map2) {
            return collator.compare(map1.get("title"), map2.get("title"));
        }
    };
    
    protected Intent activityIntent(String pkg, String componentName) {
        Intent result = new Intent();
        result.setClassName(pkg, componentName);
        return result;
    }
    
    protected Intent browseIntent(String path) {
        Intent result = new Intent();
        result.setClass(this, MyApiDemos.class);
        result.putExtra("com.example.android.apis.Path", path);
        return result;
    }

    protected void addItem(List<Map<String, Object>> data, String name, Intent intent) {
        Map<String, Object> temp = new HashMap<String, Object>();
        temp.put("title", name);
        temp.put("intent", intent);
        data.add(temp);
    }
    
    @Override
    protected void onListItemClick(ListView l, View v, int position, long id) {
        Map<String, Object> map = (Map<String, Object>) l.getItemAtPosition(position);

        Intent intent = (Intent) map.get("intent");
        startActivity(intent);
    }
}