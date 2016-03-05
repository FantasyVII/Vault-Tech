/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 8/May/2014
 * Date Moddified :- 12/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI.PanelComponents.Scrollbar.Sliders
{
    class HorizontalSlider : Component
    {
        #region Private Variables
        Button LeftButton, RightButton;
        Stopwatch Timer;

        float PreviousMousePositionXOnSilder;
        bool SavePreviousMousePositionXOnSilder;
        #endregion
        #region internal properties
        internal new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        internal Vector2 DefaultPosition
        {
            get { return (LeftButton.Position + LeftButton.Size); }
            private set { }
        }

        internal new Vector2 Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        internal float PixelsToScrollContent { get; set; }
        internal float PixelsToScrollSlider { get; set; }
        internal float PixelsToScrollSliderEveryFrame { get; set; }
        internal bool CalculatePixelsToScroll;

        internal bool Show;
        #endregion

        internal HorizontalSlider(Button LeftButton, Button RightButton)
        {
            this.LeftButton = LeftButton;
            this.RightButton = RightButton;

            Timer = new Stopwatch();

            SavePreviousMousePositionXOnSilder = true;
        }

        internal new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        internal new void LoadContent(string StyleFilePath, string HorizontalSliderNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, HorizontalSliderNodeNameInXml);
        }

        internal new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        void UpdateButtons()
        {
            if (LeftButton.Pressed)
            {
                Timer.Start();

                if (base.Position.X > LeftButton.Position.X + LeftButton.Size.X)
                    base.Position -= new Vector2(PixelsToScrollSlider, 0);

                LeftButton.Pressed = false;
            }

            if (LeftButton.Hovered && !LeftButton.Released && Timer.ElapsedMilliseconds >= 500)
                if (base.Position.X > LeftButton.Position.X + LeftButton.Size.X)
                    base.Position -= new Vector2(PixelsToScrollSliderEveryFrame, 0);

            if (LeftButton.Released)
            {
                LeftButton.Released = false;
                Timer.Reset();
            }


            if (RightButton.Pressed)
            {
                Timer.Start();

                if (base.Position.X + base.Size.X < RightButton.Position.X)
                    base.Position += new Vector2(PixelsToScrollSlider, 0);

                RightButton.Pressed = false;
            }

            if (RightButton.Hovered && !RightButton.Released && Timer.ElapsedMilliseconds >= 500)
                if (base.Position.X + base.Size.X < RightButton.Position.X)
                    base.Position += new Vector2(PixelsToScrollSliderEveryFrame, 0);

            if (RightButton.Released)
            {
                RightButton.Released = false;
                Timer.Reset();
            }
        }

        void MoveSlider()
        {
            if (base.Pressed)
            {
                if (SavePreviousMousePositionXOnSilder)
                {
                    PreviousMousePositionXOnSilder = MouseCursor.Position.X - base.Position.X;
                    SavePreviousMousePositionXOnSilder = false;
                }

                base.Position = new Vector2(MouseCursor.Position.X - PreviousMousePositionXOnSilder, base.Position.Y);

                if (base.Position.X <= LeftButton.Position.X + LeftButton.Size.X)
                    base.Position = new Vector2(LeftButton.Position.X + LeftButton.Size.X, base.Position.Y);

                if (base.Position.X >= RightButton.Position.X - base.Size.X)
                    base.Position = new Vector2(RightButton.Position.X - base.Size.X, base.Position.Y);
            }

            if (base.Released)
            {
                SavePreviousMousePositionXOnSilder = true;
                base.Released = false;
                base.Pressed = false;
            }
        }

        internal new void Update(GameTime gameTime)
        {
            if (Show)
            {
                base.Update(gameTime);

                MoveSlider();
                UpdateButtons();
            }
        }

        internal new void Draw()
        {
            if (Show)
                base.Draw();
        }
    }
}