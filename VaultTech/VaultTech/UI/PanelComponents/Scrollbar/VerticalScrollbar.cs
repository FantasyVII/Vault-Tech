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
    class VerticalScrollbar : Component
    {
        #region Private Variables
        Button TopButton, BottomButton;
        internal VerticalSlider verticalSlider;
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
            get { return base.Size.Y - (TopButton.Size.Y + BottomButton.Size.Y); }
            private set { }
        }
        #endregion

        internal VerticalScrollbar()
        {
            TopButton = new Button();
            BottomButton = new Button();

            verticalSlider = new VerticalSlider(TopButton, BottomButton);
        }

        internal new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            TopButton.Initialize(Graphics);
            BottomButton.Initialize(Graphics);

            verticalSlider.Initialize(Graphics);
        }

        internal new void LoadContent(string StyleFilePath, string VerticalScrollbarNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, VerticalScrollbarNodeNameInXml);

            TopButton.LoadContent(StyleFilePath, VerticalScrollbarNodeNameInXml + "/TopButton");
            BottomButton.LoadContent(StyleFilePath, VerticalScrollbarNodeNameInXml + "/BottomButton");

            verticalSlider.LoadContent(StyleFilePath, VerticalScrollbarNodeNameInXml + "/SliderStyle");
        }

        internal new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);

            verticalSlider.CalculatePixelsToScroll = true;

            TopButton.UpdateOnce(spriteBatch);
            BottomButton.UpdateOnce(spriteBatch);
            verticalSlider.UpdateOnce(spriteBatch);
        }

        void UpdateDimantions()
        {
            TopButton.Position = new Vector2(base.Position.X + ((base.Size.X / 2) - (TopButton.Size.X / 2)), base.Position.Y);
            BottomButton.Position = new Vector2(base.Position.X + ((base.Size.X / 2) - (BottomButton.Size.X / 2)), base.Position.Y + (base.Size.Y - BottomButton.Size.X));

            if (base.Position != DefaultPosition)
            {
                verticalSlider.Position = new Vector2(base.Position.X + ((base.Size.X / 2) - (verticalSlider.Size.X / 2)), base.Position.Y + TopButton.Size.Y);
                DefaultPosition = base.Position;
            }
        }

        internal new void Update(GameTime gameTime)
        {
            UpdateDimantions();

            TopButton.Update(gameTime);
            BottomButton.Update(gameTime);
            verticalSlider.Update(gameTime);

            base.Update(gameTime);
        }

        internal new void Draw()
        {
            base.Draw();

            TopButton.Draw();
            BottomButton.Draw();

            verticalSlider.Draw();
        }
    }
}