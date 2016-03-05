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
    class VerticalSlider : Component
    {
        #region Private Variables
        Button TopButton, BottomButton;
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
            get { return (TopButton.Position + TopButton.Size); }
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

        internal VerticalSlider(Button TopButton, Button BottomButton)
        {
            this.TopButton = TopButton;
            this.BottomButton = BottomButton;

            Timer = new Stopwatch();

            SavePreviousMousePositionXOnSilder = true;
        }

        internal new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        internal new void LoadContent(string StyleFilePath, string VerticalSliderNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, VerticalSliderNodeNameInXml);
        }

        internal new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        void UpdateButtons()
        {
            if (TopButton.Pressed)
            {
                Timer.Start();

                if (base.Position.Y > TopButton.Position.Y + TopButton.Size.Y)
                    base.Position -= new Vector2(0, PixelsToScrollSlider);

                TopButton.Pressed = false;
            }

            if (TopButton.Hovered && !TopButton.Released && Timer.ElapsedMilliseconds >= 500)
                if (base.Position.Y > TopButton.Position.Y + TopButton.Size.Y)
                    base.Position -= new Vector2(0, PixelsToScrollSliderEveryFrame);

            if (TopButton.Released)
            {
                TopButton.Released = false;
                Timer.Reset();
            }


            if (BottomButton.Pressed)
            {
                Timer.Start();

                if (base.Position.Y + base.Size.Y < BottomButton.Position.Y)
                    base.Position += new Vector2(0, PixelsToScrollSlider);

                BottomButton.Pressed = false;
            }

            if (BottomButton.Hovered && !BottomButton.Released && Timer.ElapsedMilliseconds >= 500)
                if (base.Position.Y + base.Size.Y < BottomButton.Position.Y)
                    base.Position += new Vector2(0, PixelsToScrollSliderEveryFrame);

            if (BottomButton.Released)
            {
                BottomButton.Released = false;
                Timer.Reset();
            }
        }

        void MoveSlider()
        {
            if (base.Pressed)
            {
                if (SavePreviousMousePositionXOnSilder)
                {
                    PreviousMousePositionXOnSilder = MouseCursor.Position.Y - base.Position.Y;
                    SavePreviousMousePositionXOnSilder = false;
                }

                base.Position = new Vector2(base.Position.X, MouseCursor.Position.Y - PreviousMousePositionXOnSilder);

                if (base.Position.Y <= TopButton.Position.Y + TopButton.Size.Y)
                    base.Position = new Vector2(base.Position.X, TopButton.Position.Y + TopButton.Size.Y);

                if (base.Position.Y >= BottomButton.Position.Y - base.Size.Y)
                    base.Position = new Vector2(base.Position.X, BottomButton.Position.Y - base.Size.Y);
            }

            if (base.Released)
            {
                SavePreviousMousePositionXOnSilder = true;
                base.Pressed = false;
                base.Released = false;
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