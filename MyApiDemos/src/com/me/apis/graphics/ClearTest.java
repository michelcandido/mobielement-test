package com.me.apis.graphics;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.FloatBuffer;

import javax.microedition.khronos.egl.EGLConfig;
import javax.microedition.khronos.opengles.GL10;

import android.app.Activity;
import android.content.Context;
import android.opengl.GLSurfaceView;
import android.opengl.GLU;
import android.os.Bundle;
import android.os.SystemClock;
import android.view.MotionEvent;

public class ClearTest extends Activity {
	private GLSurfaceView mGLView;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mGLView = new ClearGLSurfaceView(this);
		setContentView(mGLView);
	}

	@Override
	protected void onPause() {
		super.onPause();
		mGLView.onPause();
	}

	@Override
	protected void onResume() {
		super.onResume();
		mGLView.onResume();
	}

}

class ClearGLSurfaceView extends GLSurfaceView {
	ClearRenderer mRenderer;

	private final float TOUCH_SCALE_FACTOR = 180.0f / 320;
	private float mPreviousX;
	private float mPreviousY;

	public ClearGLSurfaceView(Context context) {
		super(context);
		mRenderer = new ClearRenderer();
		this.setRenderer(mRenderer);

		setRenderMode(GLSurfaceView.RENDERMODE_WHEN_DIRTY);
	}

	public boolean onTouchEvent(final MotionEvent event) {
		queueEvent(new Runnable() {

			@Override
			public void run() {
				mRenderer.setColor(event.getX() / getWidth(), event.getY()
						/ getHeight(), 1.0f);

				float x = event.getX();
				float y = event.getY();

				switch (event.getAction()) {
				case MotionEvent.ACTION_MOVE:

					float dx = x - mPreviousX;
					float dy = y - mPreviousY;

					// reverse direction of rotation above the mid-line
					if (y > getHeight() / 2) {
						dx = dx * -1;
					}

					// reverse direction of rotation to left of the mid-line
					if (x < getWidth() / 2) {
						dy = dy * -1;
					}

					mRenderer.mAngle += (dx + dy) * TOUCH_SCALE_FACTOR;
					requestRender();
				}
				
				mPreviousX = x;
		        mPreviousY = y;
			}

		});
		return true;
	}

}

class ClearRenderer implements GLSurfaceView.Renderer {
	private float mRed;
	private float mGreen;
	private float mBlue;

	private FloatBuffer triangleVB;
	public float mAngle;

	private void initShapes() {

		float triangleCoords[] = {
				// X, Y, Z
				-0.5f, -0.25f, 0, 0.5f, -0.25f, 0, 0.0f, 0.559016994f, 0 };

		// initialize vertex Buffer for triangle
		ByteBuffer vbb = ByteBuffer.allocateDirect(
		// (# of coordinate values * 4 bytes per float)
				triangleCoords.length * 4);
		vbb.order(ByteOrder.nativeOrder());// use the device hardware's native
											// byte order
		triangleVB = vbb.asFloatBuffer(); // create a floating point buffer from
											// the ByteBuffer
		triangleVB.put(triangleCoords); // add the coordinates to the
										// FloatBuffer
		triangleVB.position(0); // set the buffer to read the first coordinate

	}

	public void setColor(float r, float g, float b) {
		mRed = r;
		mGreen = g;
		mBlue = b;
	}

	@Override
	public void onDrawFrame(GL10 gl) {
		gl.glClear(GL10.GL_COLOR_BUFFER_BIT | GL10.GL_DEPTH_BUFFER_BIT);
		gl.glClearColor(mRed, mGreen, mBlue, 1.0f);

		// Set GL_MODELVIEW transformation mode
		gl.glMatrixMode(GL10.GL_MODELVIEW);
		gl.glLoadIdentity(); // reset the matrix to its default state

		// When using GL_MODELVIEW, you must set the view point
		GLU.gluLookAt(gl, 0, 0, -5, 0f, 0f, 0f, 0f, 1.0f, 0.0f);

		// Create a rotation for the triangle
		/*
		 * long time = SystemClock.uptimeMillis() % 4000L; float angle = 0.090f
		 * * ((int) time); gl.glRotatef(angle, 0.0f, 0.0f, 1.0f);
		 */
		gl.glRotatef(mAngle, 0.0f, 0.0f, 1.0f);

		gl.glColor4f(0.63671875f, 0.76953125f, 0.22265625f, 0.0f);
		gl.glVertexPointer(3, GL10.GL_FLOAT, 0, triangleVB);
		gl.glDrawArrays(GL10.GL_TRIANGLES, 0, 3);
	}

	@Override
	public void onSurfaceChanged(GL10 gl, int width, int height) {
		gl.glViewport(0, 0, width, height);

		// make adjustments for screen ratio
		float ratio = (float) width / height;
		gl.glMatrixMode(GL10.GL_PROJECTION); // set matrix to projection mode
		gl.glLoadIdentity(); // reset the matrix to its default state
		gl.glFrustumf(-ratio, ratio, -1, 1, 3, 7); // apply the projection
													// matrix

	}

	@Override
	public void onSurfaceCreated(GL10 gl, EGLConfig config) {
		initShapes();
		gl.glEnableClientState(GL10.GL_VERTEX_ARRAY);
	}

}