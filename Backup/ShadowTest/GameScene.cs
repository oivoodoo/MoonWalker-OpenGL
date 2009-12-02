
using System;
using System.Collections.Generic;
using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Voodoo.Game
{

	public class GameScene
	{
	    #region [  GameScene params  ]

		private static int widthScreen = 1280;
		private static int heightScreen = 768;
		private float[] Plane = {0,1,0,0};			// the plane is simple here, this is the normal for the plane, 0,1,0
		private float[] LightAmbient = {0.2f, 0.2f, 0.2f, 1.0f};
		private float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f}; 
		private float[] LightSpecular = {1.0f, 1.0f, 1.0f, 1.0f};
		private float[] LightPosition = {2.0f, 3.1f, 2.0f, 1.0f};
		private static float viewerPositionY = 3.0f;
		private float rotation = 0.0f;
		private float positionX = -1.7f;
		private float positionY = 0.5f;
		private float positionZ = 4.0f;
        private float[] fShadowMatrix = new float[16];
		private int frame = 0;
		private const int AsteroidCount = 5;
		private float rotatingY = 0.0f;
		private int[] textures = new int[3]; // TODO: Loading all textures and set them we are rending the scene.
		private const float STEP = 0.2f;

		#endregion

        #region [  Models  ]

        private MoonWalker walker = new MoonWalker();
		private List<Asteroid> asteroids = new List<Asteroid>();
		private Planet planet = new Planet();

		#endregion

		public void Run()
		{
			Glut.glutInit();
			Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);
			Glut.glutInitWindowSize(widthScreen, heightScreen);
			Glut.glutCreateWindow("Moonwalker v.0.3");
			Glut.glutFullScreen();

			InitializeObjects();
			InitializeTextures();

			Glut.glutDisplayFunc(new Glut.DisplayCallback(OnDisplay));
			Glut.glutIdleFunc(new Glut.IdleCallback(OnIdle));
			Glut.glutReshapeFunc(new Glut.ReshapeCallback(OnReshape));
			Glut.glutKeyboardFunc(new Glut.KeyboardCallback(OnKeyboard));

			Glut.glutMainLoop();
		}

		private void InitializeObjects()
		{
			Gl.glDisable(Gl.GL_DEPTH_TEST); 									 // Выключаем тест глубины
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glShadeModel(Gl.GL_FLAT);
			Gl.glShadeModel(Gl.GL_SMOOTH);

			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

			Gl.glEnable(Gl.GL_NORMALIZE);
			Gl.glEnable(Gl.GL_COLOR_MATERIAL);

			walker.Init();
			planet.Init();
			CreateAsteroids();

			SetShadowMatrix(fShadowMatrix, LightPosition, Plane);
		}

		private void CreateAsteroids()
		{
			Random random = new Random();

			for(int i = 0; i < AsteroidCount; i++)
			{
				Asteroid asteroid = new Asteroid();
				asteroid.Init();
				float x = (float)(random.Next(-10, 10)  + 1.0f) / 3.0f;
				float y = (float)(random.Next(0, 20)  + 1.0f) / 3.0f;
				float z = (float)(random.Next(-20, 20)  + 1.0f) / 3.0f;
				asteroid.SetPosition(x, y, z);
				asteroids.Add(asteroid);
			}
		}

		private void InitializeScene()
		{
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glLoadIdentity();
			Glu.gluLookAt(0.0f, 55.0f, 8.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
		}

		private void InitializeTextures()
		{
            textures = TextureUtility.LoadGLTextures(new String[]{"moon.bmp", "asteroid.bmp", "moon.bmp"});
		}

    	private void OnDisplay()
		{
		    InitializeScene();
			RenderScene();
            rotation += 1.0f;
            frame++;
            Gl.glFlush();
			Glut.glutSwapBuffers();
		}

        private void RenderScene()
		{
			InitializeLights();

            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
		    Gl.glDepthMask(Gl.GL_FALSE);

            Gl.glEnable(Gl.GL_STENCIL_TEST);
            Gl.glStencilFunc(Gl.GL_ALWAYS, 1, 0xFFFFFFFF);
            Gl.glStencilOp(Gl.GL_REPLACE, Gl.GL_REPLACE, Gl.GL_REPLACE);

            RenderFloor();

            Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
            Gl.glDepthMask(Gl.GL_TRUE);

            Gl.glStencilFunc(Gl.GL_EQUAL, 1, 0xFFFFFFFF);
            Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_KEEP);

            RenderFloor();

            Gl.glPushMatrix();
                Gl.glColor4f(0.0f, 0.0f, 0.0f, 0.5f);
                Gl.glDisable(Gl.GL_TEXTURE);
                Gl.glDisable(Gl.GL_TEXTURE_2D);
                Gl.glDisable(Gl.GL_LIGHTING);
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                Gl.glEnable(Gl.GL_BLEND);
                Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_INCR);
                Gl.glMultMatrixf(fShadowMatrix);
                Gl.glPushMatrix();
                    RenderFrame(true);
                Gl.glPopMatrix();
                Gl.glEnable(Gl.GL_TEXTURE);
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                Gl.glDisable(Gl.GL_BLEND);
                Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_STENCIL_TEST);

            Gl.glPushMatrix();
                RenderFrame(false);
            Gl.glPopMatrix();
        }

        private void RenderFloor()
        {
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            DrawFloor(-5, 0, 0);
        }

        private void RenderFrame(bool hasShadow)
		{
			if (hasShadow)
			{
				Gl.glColor4f(0.0f, 0.0f, 0.0f, 0.1f);
			}

			Gl.glPushMatrix();
				Gl.glRotatef(rotatingY, 0.0f, 1.0f, 0.0f);
				Gl.glPushMatrix();
					Gl.glTranslatef(positionX, positionY, -1.0f);
					// Gl.glRotatef(180, 0.0f, 1.0f, 0.0f);
					// walker.Render(hasShadow);
					DrawCube(0, 2, 0);
				Gl.glPopMatrix();
			Gl.glPopMatrix();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glPushMatrix();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[0]);
                    planet.Render(hasShadow);
                Gl.glPopMatrix();
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glPushMatrix();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[1]);
			        foreach(Asteroid asteroid in asteroids)
			        {
                        asteroid.Render(hasShadow);
                    }
                Gl.glPopMatrix();
           Gl.glDisable(Gl.GL_TEXTURE_2D);
        }
		
		
		private void DrawCube(float x, float y, float z)
		{
			Gl.glPushMatrix();
				Gl.glBegin(Gl.GL_QUADS);
					// top
					Gl.glNormal3f(0.0f, 1.0f, 0.0f);
					Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(1.0f+y), (float)(1.0f+z));
					Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f((float)(1.0f+x), (float)(1.0f+y), (float)(1.0f+z));
					Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f((float)(1.0f+x), (float)(1.0f+y), (float)(-1.0f+z));
					Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(1.0f+y), (float)(-1.0f+z));
			
					// bottom
					Gl.glNormal3f(0.0f, -1.0f, 0.0f);
					Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f((float)(-1.0f+x),(float)( -1.0f+y), (float)(-1.0f+z));
					Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f((float)(1.0f+x), (float)(-1.0f+y), (float)(-1.0f+z));
					Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f((float)(1.0f+x), (float)(-1.0f+y), (float)(1.0f+z));
					Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(-1.0f+y), (float)(1.0f+z));
			
					// left
					Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
					Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(-1.0f+y), (float)(-1.0f+z));
					Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(-1.0f+y), (float)(1.0f+z));
					Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(1.0f+y), (float)(1.0f+z));
					Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f((float)(-1.0f+x), (float)(1.0f+y), (float)(-1.0f+z));
			
					// right
					Gl.glNormal3f(1.0f, 0.0f, 0.0f);
					Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f((float)(1.0f+x), (float)( -1.0f+y),(float)( 1.0f+z));
					Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f((float)(1.0f+x), (float)(-1.0f+y), (float)(-1.0f+z));
					Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f((float)(1.0f+x), (float)(1.0f+y), -(float)(1.0f+z));
					Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f((float)(1.0f+x), (float)(1.0f+y), (float)(1.0f+z));
				Gl.glEnd();
			Gl.glPopMatrix();
		}

		private void InitializeLights()
		{
			Gl.glLightiv(Gl.GL_LIGHT0, Gl.GL_SPOT_EXPONENT, new int[] { 128 });
            Gl.glLightiv(Gl.GL_LIGHT0, Gl.GL_SPOT_CUTOFF, new int[] { 180 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, LightAmbient);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, LightDiffuse);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, LightSpecular);

			Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
		}

		private void OnKeyboard(byte key, int x, int y)
		{
    		if (key == 27)
			{
				Environment.Exit(0);
			} else if (key == (byte) 'w') {
				positionX -= STEP;
			} else if (key == (byte) 's') {
				positionX += STEP;
			} else if (key == (byte) 'a') {
				rotatingY += -2.0f;			} else if (key == (byte) 'd') {
				rotatingY += 2.0f;
			} else if (key == (byte) 'q') {
				positionZ += STEP;
			} else if (key == (byte) 'e') {
				positionZ -= STEP;
			} else if (key == (byte) 'f') {
			}
		}

	    private void DrawFloor(int fCenterX, int fCenterY, int fCenterZ)
		{
			Gl.glPushMatrix();
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[2]);
				    Gl.glBegin(Gl.GL_QUADS);
					    Gl.glNormal3f(0.0f, 1.0f, 0.0f);
					    float x = fCenterX - 5.0f, z = fCenterZ - 7.0f;
					    for (float i = 0.0f; i < 20.0f; i++, x += 1.0f)
					    {
						    for (float j = 0.0f; j < 24.0f; j += 1.0f, z += 1.0f)
						    {
							    // draw the plane slightly offset so the shadow shows up
							    Gl.glTexCoord2f(0.0f, 0.0f);
							    Gl.glVertex3f(x, fCenterY, z);
							    Gl.glTexCoord2f(1.0f, 0.0f);
							    Gl.glVertex3f(x + 1.0f, fCenterY, z);
							    Gl.glTexCoord2f(1.0f, 1.0f);
							    Gl.glVertex3f(x + 1.0f, fCenterY, z + 1.0f);
							    Gl.glTexCoord2f(0.0f, 1.0f);
							    Gl.glVertex3f(x, fCenterY, z + 1.0f);
						    }
						    z = fCenterZ - 7.0f;
					    }
				    Gl.glEnd();
                Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glPopMatrix();
		}

		private void SetShadowMatrix(float[] fDestMat,float[] fLightPos,float[] fPlane)
		{
			float dot;

			// dot product of plane and light position
			dot =	fPlane[0] * fLightPos[0] +
					fPlane[1] * fLightPos[1] +
					fPlane[1] * fLightPos[2] +
					fPlane[3] * fLightPos[3];

			// first column
			fDestMat[0] = dot - fLightPos[0] * fPlane[0];
			fDestMat[4] = 0.0f - fLightPos[0] * fPlane[1];
			fDestMat[8] = 0.0f - fLightPos[0] * fPlane[2];
			fDestMat[12] = 0.0f - fLightPos[0] * fPlane[3];

			// second column
			fDestMat[1] = 0.0f - fLightPos[1] * fPlane[0];
			fDestMat[5] = dot - fLightPos[1] * fPlane[1];
			fDestMat[9] = 0.0f - fLightPos[1] * fPlane[2];
			fDestMat[13] = 0.0f - fLightPos[1] * fPlane[3];

			// third column
			fDestMat[2] = 0.0f - fLightPos[2] * fPlane[0];
			fDestMat[6] = 0.0f - fLightPos[2] * fPlane[1];
			fDestMat[10] = dot - fLightPos[2] * fPlane[2];
			fDestMat[14] = 0.0f - fLightPos[2] * fPlane[3];

			// fourth column
			fDestMat[3] = 0.0f - fLightPos[3] * fPlane[0];
			fDestMat[7] = 0.0f - fLightPos[3] * fPlane[1];
			fDestMat[11] = 0.0f - fLightPos[3] * fPlane[2];
			fDestMat[15] = dot - fLightPos[3] * fPlane[3];
		}

		private void OnIdle()
		{
			// render the scene
			Glut.glutPostRedisplay();
			// TODO: Update positions of our models.

		}

		private void OnReshape(int width, int height)
		{
			// save the new window size
            widthScreen = width;
            heightScreen = height;
            // map the view port to the client area
            Gl.glViewport(0, 0, width, height);
            // set the matrix mode to project
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // load the identity matrix
            Gl.glLoadIdentity();
            // create the viewing frustum
            Glu.gluPerspective(30.0, (float) width / (float) height, 1.0, 300.0);
            // set the matrix mode to modelview
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            // load the identity matrix
            Gl.glLoadIdentity();
            // position the view point
            Glu.gluLookAt(0.0f, viewerPositionY, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
		}
	}
}

