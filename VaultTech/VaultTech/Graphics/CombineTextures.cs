/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 30/April/2014
 * Date Moddified :- 23/February/2015
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
    /// CombineTextures class is responsible for drawing everything on a render target and then converting it to a texture.
    /// </summary>
    public static class CombineTextures
    {
        #region MonoGame variables
        static GraphicsDeviceManager Graphics;
        #endregion
        #region CombineTextures class variables
        static Texture2D Texture;
        static RenderTarget2D RenderTarget;
        #endregion

        /// <summary>
        /// The begining of the drawing on the render target.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager.</param>
        public static void Begin(GraphicsDeviceManager Graphics)
        {
            CombineTextures.Graphics = Graphics;

            RenderTarget = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);

            Graphics.GraphicsDevice.Clear(Color.Transparent);
        }

        /// <summary>
        /// Creates the render targer backbuffer to draw on.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager.</param>
        /// <param name="BackBufferSize">The size of the render target backbuffer (texture).</param>
        public static void Begin(GraphicsDeviceManager Graphics, Vector2 BackBufferSize)
        {
            CombineTextures.Graphics = Graphics;

            RenderTarget = new RenderTarget2D(Graphics.GraphicsDevice, (int)BackBufferSize.X, (int)BackBufferSize.Y);

            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);

            Graphics.GraphicsDevice.Clear(Color.White * 0f);
        }

        /// <summary>
        /// End of the drawing on the render target.
        /// </summary>
        public static void End()
        {
            Graphics.GraphicsDevice.SetRenderTarget(null);

            Color[] data = new Color[RenderTarget.Width * RenderTarget.Height];

            RenderTarget.GetData<Color>(data);

            Texture = new Texture2D(Graphics.GraphicsDevice, RenderTarget.Width, RenderTarget.Height);

            Texture.SetData<Color>(data);

            RenderTarget.Dispose();
        }

        /// <summary>
        /// Gets the final texture. This method must be called before calling Dispose function.
        /// </summary>
        /// <returns></returns>
        public static Texture2D GetFinalTexture()
        {
            return Texture;
        }

        /// <summary>
        /// Gets the final croped Texture. This method must be called before calling Dispose function.
        /// </summary>
        /// <param name="TextureRectangle">The position and size of where the texture should be croped</param>
        /// <returns></returns>
        public static Texture2D GetFinalTexture(Rectangle TextureRectangle)
        {
            return CropTexture.Crop(Graphics, Texture, TextureRectangle);
        }
    }
}