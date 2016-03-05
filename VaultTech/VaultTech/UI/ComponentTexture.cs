/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 24/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Graphics;

namespace VaultTech.UI
{
    internal class BorderThickness
    {
        internal int Top, Right, Bottom, Left;

        internal BorderThickness() { }
        internal BorderThickness(int Top, int Right, int Bottom, int Left)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
        }
    }

    internal class ComponentTexture
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        internal enum RenderMode { NoneScalable, Scalable, ScalableRealTime };
        internal RenderMode RendererMode;

        internal Texture2D OriginalTexture;
        internal Color Color;

        Texture2D ScaledTexture;
        internal Vector2 Position, Size, CornersSize;
        BorderThickness BorderThickness;

        Texture2D BackgraoundTexture;
        Texture2D TopLineTexture, RightLineTexture, BottomLineTexture, LeftLineTexture;
        Texture2D TopLeftCornerTexture, TopRightCornerTexture, BottomRightCornerTexture, BottomLeftCornerTexture;

        Rectangle BackgraoundRec, BackgraoundSourceRec;

        Rectangle TopLineRec, RightLineRec, BottomLineRec, LeftLineRec,
                  TopLineSourceRec, RightSourceLineRec, BottomSourceLineRec, LeftSourceLineRec;

        Rectangle TopLeftCorenerRec, BottomLeftCorener, TopRightCorener, BottomRightCorener,
                  TopLeftCorenerSourceRec, BottomLeftCorenerSourceRec, TopRightCorenerSourceRec, BottomRightCorenerSourceRec;

        internal ComponentTexture()
        {
            RendererMode = new RenderMode();
        }

        internal void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        internal void CreateTexture(SpriteBatch spriteBatch, Vector2 Position, Vector2 Size, Vector2 CornersSize, BorderThickness BorderThickness)
        {
            this.spriteBatch = spriteBatch;
            this.Position = Position;
            this.Size = Size;
            this.CornersSize = CornersSize;
            this.BorderThickness = BorderThickness;

            if (RendererMode == RenderMode.Scalable || RendererMode == RenderMode.ScalableRealTime)
            {
                SetCropRectangle();
                MoveAndResizeUI();
                CropTextures(Graphics);
            }

            if (RendererMode == RenderMode.Scalable)
            {
                CombineTextures.Begin(Graphics, Size);
                DrawUIParts();
                CombineTextures.End();

                ScaledTexture = CombineTextures.GetFinalTexture(new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y));
            }
        }

        internal void Update()
        {
            MoveAndResizeUI();
        }

        internal void Draw()
        {
            if (RendererMode == RenderMode.NoneScalable)
                spriteBatch.Draw(OriginalTexture, Position, Color);

            else if (RendererMode == RenderMode.Scalable)
                spriteBatch.Draw(ScaledTexture, Position, Color);

            else if (RendererMode == RenderMode.ScalableRealTime)
                DrawUIParts();
        }

        void SetCropRectangle()
        {
            BackgraoundSourceRec = new Rectangle((int)BorderThickness.Left, (int)BorderThickness.Top, (int)OriginalTexture.Width - (BorderThickness.Right * 2), (int)(OriginalTexture.Height - (BorderThickness.Bottom * 2)));

            TopLineSourceRec = new Rectangle((int)CornersSize.X, 0, (int)(OriginalTexture.Width - CornersSize.X * 2), BorderThickness.Top);
            RightSourceLineRec = new Rectangle((int)(OriginalTexture.Width - BorderThickness.Right), (int)CornersSize.Y, BorderThickness.Right, (int)(OriginalTexture.Height - CornersSize.Y * 2));
            BottomSourceLineRec = new Rectangle((int)CornersSize.X, (int)(OriginalTexture.Height - BorderThickness.Bottom), (int)(OriginalTexture.Width - CornersSize.X * 2), BorderThickness.Bottom);
            LeftSourceLineRec = new Rectangle(0, (int)CornersSize.Y, BorderThickness.Left, (int)(OriginalTexture.Height - CornersSize.Y * 2));

            TopLeftCorenerSourceRec = new Rectangle(0, 0, (int)CornersSize.X, (int)CornersSize.Y);
            TopRightCorenerSourceRec = new Rectangle((int)(OriginalTexture.Width - CornersSize.X), 0, (int)CornersSize.X, (int)CornersSize.Y);
            BottomRightCorenerSourceRec = new Rectangle((int)(OriginalTexture.Width - CornersSize.X), (int)(OriginalTexture.Height - CornersSize.Y), (int)CornersSize.X, (int)CornersSize.Y);
            BottomLeftCorenerSourceRec = new Rectangle(0, (int)(OriginalTexture.Height - CornersSize.Y), (int)CornersSize.X, (int)CornersSize.Y);
        }

        void MoveAndResizeUI()
        {
            BackgraoundRec = new Rectangle((int)(Position.X + BorderThickness.Left), (int)(Position.Y + BorderThickness.Top), (int)(Size.X - (BorderThickness.Right * 2)), (int)(Size.Y - (BorderThickness.Bottom * 2)));

            TopLineRec = new Rectangle((int)(CornersSize.X + Position.X), (int)Position.Y, (int)(Size.X - CornersSize.X * 2), BorderThickness.Top);
            RightLineRec = new Rectangle((int)((Size.X - BorderThickness.Right) + Position.X), (int)(CornersSize.Y + Position.Y), BorderThickness.Right, (int)(Size.Y - CornersSize.Y * 2));
            BottomLineRec = new Rectangle((int)(CornersSize.X + Position.X), (int)((Size.Y - BorderThickness.Bottom) + Position.Y), (int)(Size.X - CornersSize.X * 2), BorderThickness.Bottom);
            LeftLineRec = new Rectangle((int)Position.X, (int)(CornersSize.Y + Position.Y), BorderThickness.Left, (int)(Size.Y - CornersSize.Y * 2));

            TopLeftCorenerRec = new Rectangle((int)Position.X, (int)Position.Y, (int)CornersSize.X, (int)CornersSize.Y);
            BottomLeftCorener = new Rectangle((int)Position.X, (int)((Size.Y - CornersSize.Y) + Position.Y), (int)CornersSize.X, (int)CornersSize.Y);
            TopRightCorener = new Rectangle((int)((Size.X - CornersSize.X) + Position.X), (int)Position.Y, (int)CornersSize.X, (int)CornersSize.Y);
            BottomRightCorener = new Rectangle((int)((Size.X - CornersSize.X) + Position.X), (int)((Size.Y - CornersSize.Y) + Position.Y), (int)CornersSize.X, (int)CornersSize.Y);
        }

        void CropTextures(GraphicsDeviceManager Graphics)
        {
            BackgraoundTexture = CropTexture.Crop(Graphics, OriginalTexture, BackgraoundSourceRec);

            TopLineTexture = CropTexture.Crop(Graphics, OriginalTexture, TopLineSourceRec);
            RightLineTexture = CropTexture.Crop(Graphics, OriginalTexture, RightSourceLineRec);
            BottomLineTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomSourceLineRec);
            LeftLineTexture = CropTexture.Crop(Graphics, OriginalTexture, LeftSourceLineRec);

            TopLeftCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, TopLeftCorenerSourceRec);
            TopRightCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, TopRightCorenerSourceRec);
            BottomRightCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomRightCorenerSourceRec);
            BottomLeftCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomLeftCorenerSourceRec);
        }

        void DrawUIParts()
        {
            if (RendererMode == RenderMode.Scalable)
                spriteBatch.Begin();

            spriteBatch.Draw(BackgraoundTexture, BackgraoundRec, Color);

            spriteBatch.Draw(TopLineTexture, TopLineRec, Color);
            spriteBatch.Draw(TopLineTexture, TopLineRec, Color);
            spriteBatch.Draw(RightLineTexture, RightLineRec, Color);
            spriteBatch.Draw(BottomLineTexture, BottomLineRec, Color);
            spriteBatch.Draw(LeftLineTexture, LeftLineRec, Color);

            spriteBatch.Draw(TopLeftCornerTexture, TopLeftCorenerRec, Color);
            spriteBatch.Draw(TopRightCornerTexture, TopRightCorener, Color);
            spriteBatch.Draw(BottomRightCornerTexture, BottomRightCorener, Color);
            spriteBatch.Draw(BottomLeftCornerTexture, BottomLeftCorener, Color);

            if (RendererMode == RenderMode.Scalable)
                spriteBatch.End();
        }
    }
}