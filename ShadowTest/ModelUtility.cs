
using System;
using System.IO;
using System.Collections.Generic;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Ode;

namespace Voodoo.Game
{
	public static class ModelUtility
	{
		
		public static void LoadModel(List<float> points, String modelName)
		{
			using(StreamReader reader = new StreamReader(new FileStream(String.Format("models/{0}.raw", modelName), FileMode.Open)))
			{
				while(!reader.EndOfStream)
				{
					String[] lines = reader.ReadLine().Split(new char[]{' '});
					if(lines.Length > 0)
					{
						for(int i = 0 ; i < lines.Length; i++)
						{
							if (!String.IsNullOrEmpty(lines[i]))
							{
								points.Add((float)Convert.ToDouble(lines[i]));
							}
						}
					}
				}
			}
		}
		
		public static void RenderModel(List<float> model)
		{
			Gl.glBegin(Gl.GL_TRIANGLES);
				for(int i = 0; i < model.Count; i += 9)
				{
					float x1 = model[i];
					float y1 = model[i + 1];
					float z1 = model[i + 2];
				
					float x2 = model[i + 3];
					float y2 = model[i + 4];
					float z2 = model[i + 5];
				
					float x3 = model[i + 6];
					float y3 = model[i + 7];
					float z3 = model[i + 8];
				
					Gl.glNormal3f(
						y1 * (z2 - z3) + y2 * (z3 - z1) + y3 * (z1 - z2),
						z1 * (x2 - x3) + z2 * (x3 - x1) + z3 * (x1 - x2),
						x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)
					);
				
					Gl.glVertex3f(x1, y1, z1);
					Gl.glVertex3f(x2, y2, z2);
					Gl.glVertex3f(x3, y3, z3);
				}
			Gl.glEnd();
		}

	}
}
