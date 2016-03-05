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
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.UI.PanelComponents.Scrollbar.Sliders;

namespace VaultTech.UI.PanelComponents.Scrollbar
{
    class HorizontalScrollbar : Component
    {
        #region Private Variables
        Button LeftButton, RightButton;
        internal HorizontalSlider horizontalSlider;
        Vector2 DefaultPosition { get; set; }
        #endregion
        #region internal properties
        internal new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        internal new Vector2 Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        internal float ScrollableLength
        {
            get { return base.Size.X - (LeftButton.Size.X + RightButton.Size.X); }
            private set { }
        }
        #endregion

        internal HorizontalScrollbar()
        {
            LeftButton = new Button();
            RightButton = new Button();

            horizontalSlider = new HorizontalSlider(LeftButton, RightButton);
        }

        internal new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            LeftButton.Initialize(Graphics);
            RightButton.Initialize(Graphics);

            horizontalSlider.Initialize(Graphics);
        }

        internal new void LoadContent(string StyleFilePath, string HorizontalScrollbarNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, HorizontalScrollbarNodeNameInXml);

            LeftButton.LoadContent(StyleFilePath, HorizontalScrollbarNodeNameInXml + "/LeftButton");
            RightButton.LoadContent(StyleFilePath, HorizontalScrollbarNodeNameInXml + "/RightButton");

            horizontalSlider.LoadContent(StyleFilePath, HorizontalScrollbarNodeNameInXml + "/SliderStyle");
        }

        internal new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);

            horizontalSlider.CalculatePixelsToScroll = true;

            LeftButton.UpdateOnce(spriteBatch);
            RightButton.UpdateOnce(spriteBatch);
            horizontalSlider.UpdateOnce(spriteBatch);
        }

        void UpdateDimantions()
        {
            LeftButton.Position = new Vector2(base.Position.X, base.Position.Y + ((base.Size.Y / 2) - (LeftButton.Size.Y / 2)));
            RightButton.Position = new Vector2(base.Position.X + (base.Size.X - RightButton.Size.X), base.Position.Y + ((base.Size.Y / 2) - (LeftButton.Size.Y / 2)));

            if (base.Position != DefaultPosition)
            {
                horizontalSlider.Position = new Vector2(base.Position.X + 15, base.Position.Y + 5);
                DefaultPosition = base.Position;
            }
        }

        internal new void Update(GameTime gameTime)
        {
            UpdateDimantions();

            LeftButton.Update(gameTime);
            RightButton.Update(gameTime);

            horizontalSlider.Update(gameTime);

            base.Update(gameTime);
        }

        internal new void Draw()
        {
            base.Draw();

            LeftButton.Draw();
            RightButton.Draw();

            horizontalSlider.Draw();
        }
    }
}