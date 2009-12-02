using System;
using System.IO;
using System.Collections.Generic;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Ode;

namespace Voodoo.Game
{

	public class Planet : IGameObject
	{
		#region IGameObject implementation

		public void Init ()
		{
			Gl.glNewList(200, Gl.GL_COMPILE_AND_EXECUTE);
				Glut.glutSolidSphere(1.0f, 30, 30);
                Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.65f, 0.0f);
                    Glut.glutSolidSphere(0.3f, 30, 30);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.65f, 0.3f);
                    Glut.glutSolidSphere(0.3f, 30, 30);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glTranslatef(0.3f, 0.65f, 0.1f);
                    Glut.glutSolidSphere(0.3f, 30, 30);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glTranslatef(0.3f, 0.55f, 0.1f);
                    Glut.glutSolidSphere(0.3f, 30, 30);
                Gl.glPopMatrix();
			Gl.glEndList();
		}

		public void Render (bool isColor)
		{
            Gl.glColor4f(0.317f, 0.5392f, 0.603f, 0.3f);
	        Gl.glCallList(200);
		}

		#endregion
	}
}

