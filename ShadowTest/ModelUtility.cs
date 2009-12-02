
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
								points.Add((float)Convert.ToDouble(lines[i].Replace(".",",")));
							}
						}
					}
				}
			}
		}
		
		public static void RenderModel(List<float> model)
		{
			Gl.glBegin(Gl.GL_TRIANGLES);
				for(int i = 0; i < model.Count; i += 3)
				{
					Gl.glVertex3f(model[i], model[i + 1], model[i + 2]);
				}
			Gl.glEnd();
		}

	}
}
