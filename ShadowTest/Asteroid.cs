
using System;
using System.IO;
using System.Collections.Generic;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Ode;

namespace Voodoo.Game
{


	public class Asteroid : IGameObject
	{
		private static List<float> asteroid = new List<float>();
		private static int texture = 0;
		private float x;
		private float y;
		private float z;
		private int frame = 0;
		private float asteroidWave = 0.0f;
		private float rotation = 0.0f;
		private int rotationDirection = 1;
		private float rotationX;
		private float rotationY;
		private float rotationZ;

		static Asteroid()
		{
			ModelUtility.LoadModel(asteroid, "asteroid");
		}

		#region IGameObject implementation

		public void Init()
		{
			Random random = new Random();
			float scale = (float)(random.Next(0, 120) + 20) / 255.0f;
			rotationX = (float)random.Next(0, 2);
			rotationY = (float)random.Next(0, 2);
			rotationZ = (float)random.Next(0, 2);
			Gl.glNewList(100, Gl.GL_COMPILE_AND_EXECUTE);
				Gl.glScalef(scale, scale, scale);
				ModelUtility.RenderModel(asteroid);
			Gl.glEndList();
		}

		public void Render (bool isColor)
		{
			Gl.glPushMatrix();
				Gl.glEnable(Gl.GL_BLEND);
					if (!isColor)
					{
						Gl.glColor3f(0.136f, 0.099f, 0.046f);
					}
					else
					{
						Gl.glColor4f(0.0f, 0.0f, 0.0f, 0.7f);
					}	
					Gl.glTranslatef(x, y - asteroidWave, z);
					Gl.glRotatef(rotation, rotationX, rotationY, rotationZ);
					Gl.glCallList(100);
				Gl.glDisable(Gl.GL_BLEND);
			Gl.glPopMatrix();

			UpdatePositions();
		}

		#endregion

		private void UpdatePositions()
		{
			frame++;
			asteroidWave += 0.05f;
			rotation += 5.0f * rotationDirection;

			if (rotation >= 360.0f || rotation <= 0)
			{
				rotationDirection = -rotationDirection;
			}

			if (y - asteroidWave <= -2.0f)
			{
				asteroidWave = -1.0f;
				frame = 0;
			}
		}

		public void SetPosition(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}

