/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 5/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics
{
    /// <summary>
    /// This class is responsible for cropping a texture from a specific position to specific position.
    /// </summary>
    static class CropTexture
    {
        /// <summary>
        /// Crops a texture and then returns the cropped texture.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager.</param>
        /// <param name="Texture">Texture you want to crop.</param>
        /// <param name="Area">The area you want to be cropped.</param>
        /// <returns>The cropped texture.</returns>
        public static Texture2D Crop(GraphicsDeviceManager Graphics, Texture2D Texture, Rectangle Area)
        {
            try
            {
                Texture2D cropped = new Texture2D(Graphics.GraphicsDevice, Area.Width, Area.Height);
                Color[] data = new Color[Texture.Width * Texture.Height];
                Color[] cropData = new Color[cropped.Width * cropped.Height];

                Texture.GetData<Color>(data);

                int index = 0;

                for (int y = Area.Y; y < Area.Y + Area.Height; y++)
                {
                    for (int x = Area.X; x < Area.X + Area.Width; x++)
                    {
                        cropData[index] = data[x + (y * Texture.Width)];
                        index++;
                    }
                }

                cropped.SetData<Color>(cropData);

                return cropped;
            }
            catch (Exception ex)
            {
                if (Graphics == null)
                    throw new Exception("Graphics argument is null", ex);
                else if (Texture == null)
                    throw new Exception("Texture argument is null", ex);
                else
                    throw ex;
            }
        }
    }
}