
using System;
using System.IO;
using System.Collections.Generic;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Ode;

namespace Voodoo.Game
{

    public class MoonWalker : IGameObject
    {
        private List<float> ball = new List<float>();
        private List<float> head = new List<float>();
        private List<float> walker = new List<float>();
        private List<float> antenna01 = new List<float>();
        private List<float> antenna02 = new List<float>();

        public MoonWalker()
        {
            ModelUtility.LoadModel(ball, "ball");
            ModelUtility.LoadModel(head, "head");
            ModelUtility.LoadModel(walker, "walker");
            ModelUtility.LoadModel(antenna01, "antenna01");
            ModelUtility.LoadModel(antenna02, "antenna02");
        }

        public void Init()
        {
            Gl.glNewList(1, Gl.GL_COMPILE_AND_EXECUTE);
	            Gl.glRotatef(270, 1.0f, 0.0f, 0.0f);
	            Gl.glScalef(0.5f, 0.5f, 0.5f);
	            ModelUtility.RenderModel(walker);
            Gl.glEndList();
	            Gl.glNewList(2, Gl.GL_COMPILE_AND_EXECUTE);
	            // Gl.glScalef(0.5f, 0.5f, 0.5f);
	            ModelUtility.RenderModel(ball);
            Gl.glEndList();
            Gl.glNewList(3, Gl.GL_COMPILE_AND_EXECUTE);
				// Gl.glScalef(0.5f, 0.5f, 0.5f);
				ModelUtility.RenderModel(antenna01);
            Gl.glEndList();
	            Gl.glNewList(4, Gl.GL_COMPILE_AND_EXECUTE);
	            // Gl.glScalef(0.5f, 0.5f, 0.5f);
	            ModelUtility.RenderModel(antenna02);
            Gl.glEndList();
            Gl.glNewList(5, Gl.GL_COMPILE_AND_EXECUTE);
				// Gl.glScalef(0.5f, 0.5f, 0.5f);
	            ModelUtility.RenderModel(head);
            Gl.glEndList();
        }

        public void Render(bool isColor)
        {
            if(!isColor)
            {
                Gl.glColor4f(0.0f, 0.0f, 0.0f, 0.7f);
            }
            if (isColor)
            {
                Gl.glColor3f(0.177f, 0.160f, 0.137f);
            }
            Gl.glPushMatrix();
            	Gl.glCallList(1);
				Gl.glScalef(1.0f, 1.0f, 1.0f);
				if (isColor)
				{
					Gl.glColor3f(1.0f, 0.5f, 0.3f);
				}
				Gl.glTranslatef(0.0f, -0.6f, 0.0f);
				Gl.glRectf(0.0f, 0.0f, 5.8f, 3.6f); // ðèñóåì îñü
				if (isColor)
				{
					Gl.glColor3f(0.3f, 0.5f, 0.3f);
				}
				Gl.glTranslatef(5.3f, 0.5f, 2.6f);
				Gl.glRotatef(45.0f, 0.0f, 0.1f, 0.0f);
				Gl.glRectf(0.0f, 0.4f, 1.8f, 2.2f); // ðèñóåì ëîáîâîå ñòåêëî
            Gl.glPopMatrix();

            if (isColor)
            {
                Gl.glColor3f(0.136f, 0.099f, 0.046f);
            }
            Gl.glPushMatrix();
	            Gl.glScalef(0.8f, 0.8f, 0.8f);
				Gl.glTranslatef(1.0f, 0.0f, 0.0f);
	            for (int j = 0; j < 4; j++)
	            {
	                Gl.glPushMatrix();
		                Gl.glTranslatef(-j * 1.0f, -0.4f, 1.0f);
		                Gl.glCallList(2);
	                Gl.glPopMatrix();
	            }
	            Gl.glRotatef(180, 2.0f, 0.0f, 0.0f);
	            for (int j = 0; j < 4; j++)
	            {
	                Gl.glPushMatrix();
		                Gl.glTranslatef(-j * 1.0f, -0.4f, 2.7f);
		                Gl.glCallList(2);
	                Gl.glPopMatrix();
				}
            Gl.glPopMatrix();

            if (isColor)
            {
                Gl.glColor3f(0.036f, 0.099f, 0.046f);
            }
            Gl.glPushMatrix();
	            Gl.glTranslatef(1.0f, 0.0f, 0.25f);
	            Gl.glScalef(0.4f, 0.4f, 0.4f);
	            for (int j = 0; j < 4; j++)
	            {
	                Gl.glPushMatrix();
						Gl.glTranslatef(2.1f, 0.0f, 0.0f);
		                Gl.glTranslatef(-j * 1.9666f, -0.4f, 1.0f);
		                Gl.glCallList(2);
	                Gl.glPopMatrix();
	            }
	            Gl.glRotatef(180, 2.0f, 0.0f, 0.0f);
				// Gl.glRotatef(180, 0.0f, 0.0f, 2.0f);
	            for (int j = 0; j < 4; j++)
	            {
	                Gl.glPushMatrix();
						Gl.glTranslatef(2.1f, 0.0f, 0.0f);
	                	Gl.glTranslatef(-j * 1.9666f, -0.4f, 5.6f);
	                	Gl.glCallList(2);
	                Gl.glPopMatrix();
				}
            Gl.glPopMatrix();

            if (isColor)
            {
                Gl.glColor4f(1.0f, 0.0f, 0.2f, 1.0f);
            }
            Gl.glPushMatrix();
				Gl.glRotatef(90, 1.0f, 0.0f, 0.0f);
				Gl.glTranslatef(1.0f, -2.3f, -2.0f);
				Gl.glCallList(3);
            Gl.glPopMatrix();

            if (isColor)
            {
                Gl.glColor3f(0.3f, 0.3f, 0.3f);
            }
            Gl.glPushMatrix();
	            Gl.glRotatef(300, 1.0f, 0.0f, 0.0f);
	            Gl.glTranslatef(0.7f, 0.25f, -0.042f);
	            if (isColor)
	            {
	                Gl.glColor3f(0.5f, 0.4f, 0.2f);
	            }
	            Gl.glCallList(4);
            Gl.glPopMatrix();

            if (isColor)
            {
                Gl.glColor3f(0.1f, 0.1f, 0.1f);
            }
            Gl.glPushMatrix();
				Gl.glScalef(0.3f, 0.3f, 0.3f);
				Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
				Gl.glRotatef(300.0f, 0.0f, 1.0f, 0.0f);
				Gl.glTranslatef(-8.6f, -3.0f, -1.4f);
				Gl.glCallList(5);
            Gl.glPopMatrix();
        }
    }
}

