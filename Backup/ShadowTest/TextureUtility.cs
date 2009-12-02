
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.FreeGlut;
using Tao.OpenGl;
using System.IO;

namespace Voodoo.Game
{


	public static class TextureUtility
	{

		public static Bitmap LoadBMP(string fileName) {
            if(fileName == null || fileName == string.Empty) {                  // Make Sure A Filename Was Given
                return null;                                                    // If Not Return Null
            }

            string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
                Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
                "..", Path.DirectorySeparatorChar, fileName);

            // Make Sure The File Exists In One Of The Usual Directories
            if(!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2)) {
                return null;                                                    // If Not Return Null
            }

            if(File.Exists(fileName)) {                                         // Does The File Exist Here?
                return new Bitmap(fileName);                                    // Load The Bitmap
            }
            else if(File.Exists(fileName1)) {                                   // Does The File Exist Here?
                return new Bitmap(fileName1);                                   // Load The Bitmap
            }
            else if(File.Exists(fileName2)) {                                   // Does The File Exist Here?
                return new Bitmap(fileName2);                                   // Load The Bitmap
            }

            return null;                                                        // If Load Failed Return Null
        }

		public static int[] LoadGLTextures(String[] names)
	    {
	        Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture
			int[] textures = new int[names.Length];
			Gl.glGenTextures(names.Length, textures);                                       // Create The Texture

            for(int i = 0; i < names.Length; i++)
            {
	            textureImage[0] = TextureUtility.LoadBMP(names[i]);                // Load The Bitmap
	            // Check For Errors, If Bitmap's Not Found, Quit
	            if (textureImage[0] != null)
	            {
	                textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
	                // Rectangle For Locking The Bitmap In Memory
	                Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width,
	                    textureImage[0].Height);
	                // Get The Bitmap's Pixel Data From The Locked Bitmap
	                BitmapData bitmapData = textureImage[0].LockBits(rectangle,
	                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

	                // Typical Texture Generation Using Data From The Bitmap
	                Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[i]);
	                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width,
	                    textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
	                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
	                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

	                if (textureImage[0] != null)
	                {                                   // If Texture Exists
	                    textureImage[0].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
	                    textureImage[0].Dispose();                                  // Dispose The Bitmap
	                }
	            }
            }
	        return textures;                                                      // Return The Status
		}
	}
}

