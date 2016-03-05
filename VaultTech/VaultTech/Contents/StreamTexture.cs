/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 25/February/2015
 * Date Moddified :- 26/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Contents
{
    /// <summary>
    /// This class is responsible for loading a PNG, JPG or BMP file format.
    /// </summary>
    public static class StreamTexture
    {
        static Texture2D Texture;

        /// <summary>
        /// Set the Alpha channel and make the Texture transparent.
        /// </summary>
        /// <param name="Texture">Texture that will make the alpha value transparent.</param>
        static void PremultiplyYourAlpha(Texture2D Texture)
        {
            Color[] pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++)
            {
                Color p = pixels[i];
                pixels[i] = new Color(p.R, p.G, p.B) * (p.A / 255f);
            }
            Texture.SetData(pixels);
        }

        /// <summary>
        /// Load textures that are not xnb format. (PNG, JPG, BMP, etc..) and set Alpha channel.
        /// </summary>
        /// <param name="Graphics">XNA GraphicsDeviceManager.</param>
        /// <param name="TexturePath">Texture Path.</param>
        /// <returns></returns>
        public static Texture2D LoadTextureFromStream(GraphicsDeviceManager Graphics, string TexturePath)
        {
            using (FileStream TextureFileStream = new FileStream(FileManager.ContentFolder + TexturePath, FileMode.Open))
            {
                Texture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureFileStream);
                PremultiplyYourAlpha(Texture);
            }

            return Texture;
        }
    }
}