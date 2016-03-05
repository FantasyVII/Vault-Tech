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
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.UI.PanelComponents.Scrollbar;
using VaultTech.Contents;

namespace VaultTech.UI.PanelComponents
{
    public class Panel : Component
    {
        #region Private Variables
        HorizontalScrollbar horizontalScrollbar;
        VerticalScrollbar verticalScrollbar;

        RasterizerState rasterizerState;
        Rectangle CurrentScissorRectangle;

        Vector2 InvisibleCotentSize;
        #endregion
        #region Protected properties
        protected new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        protected new Vector2 Size
        {
            get { return base.Size; }
            set { base.Size = new Vector2(value.X - horizontalScrollbar.Size.Y, value.Y - verticalScrollbar.Size.X); }
        }

        protected bool HasHorizontalScrollbar { get; set; }
        protected bool HasVerticalScrollbar { get; set; }

        protected Rectangle ScissorRectangle { get; set; }

        protected Vector2 ContentPosition { get; set; }
        protected Vector2 ContentSize { get; set; }

        // This is the total amound of pixels that the content has moved.
        Vector2 ContentOffsetPosition;
        float DesiredPixelToScrollContent;
        #endregion

        public Panel()
        {
            rasterizerState = new RasterizerState() { ScissorTestEnable = true };

            horizontalScrollbar = new HorizontalScrollbar();
            verticalScrollbar = new VerticalScrollbar();
        }

        protected virtual new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            if (HasHorizontalScrollbar)
                horizontalScrollbar.Initialize(Graphics);

            if (HasVerticalScrollbar)
                verticalScrollbar.Initialize(Graphics);
        }

        void LoadScrollingSpeed(string StyleFilePath, string PanelNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileManager.ContentFolder + StyleFilePath);

            if (xmlDoc.SelectSingleNode(PanelNodeNameInXml + "/ScrollingSpeed") != null)
            {
                DesiredPixelToScrollContent = float.Parse(xmlDoc.SelectSingleNode(PanelNodeNameInXml + "/ScrollingSpeed").Attributes.GetNamedItem("OnePress").Value);

                horizontalScrollbar.horizontalSlider.PixelsToScrollSliderEveryFrame = float.Parse(xmlDoc.SelectSingleNode(PanelNodeNameInXml + "/ScrollingSpeed").Attributes.GetNamedItem("ContinuousPress").Value);
                verticalScrollbar.verticalSlider.PixelsToScrollSliderEveryFrame = float.Parse(xmlDoc.SelectSingleNode(PanelNodeNameInXml + "/ScrollingSpeed").Attributes.GetNamedItem("ContinuousPress").Value);
            }
        }

        protected virtual new void LoadContent(string StyleFilePath, string PanelNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, PanelNodeNameInXml);
            
            if (HasHorizontalScrollbar)
                horizontalScrollbar.LoadContent(StyleFilePath, PanelNodeNameInXml + "/HorizontalScrollbarStyle");

            if (HasVerticalScrollbar)
                verticalScrollbar.LoadContent(StyleFilePath, PanelNodeNameInXml + "/VerticalScrollbarStyle");

            LoadScrollingSpeed(StyleFilePath, PanelNodeNameInXml);
        }

        protected virtual new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);

            if (HasHorizontalScrollbar)
                horizontalScrollbar.UpdateOnce(spriteBatch);

            if (HasVerticalScrollbar)
                verticalScrollbar.UpdateOnce(spriteBatch);
        }

        void UpdateDimantions()
        {
            ScissorRectangle = new Rectangle((int)base.Position.X, (int)base.Position.Y, (int)base.Size.X, (int)base.Size.Y);

            if (HasHorizontalScrollbar)
            {
                horizontalScrollbar.Position = new Vector2(base.Position.X, base.Position.Y + base.Size.Y);
                horizontalScrollbar.Size = new Vector2(base.Size.X, horizontalScrollbar.Size.Y);
            }

            if (HasVerticalScrollbar)
            {
                verticalScrollbar.Position = new Vector2(base.Position.X + base.Size.X, base.Position.Y);
                verticalScrollbar.Size = new Vector2(verticalScrollbar.Size.X, base.Size.Y);
            }
        }

        void UpdateSildersPosition(GameTime gameTime)
        {
            if (HasHorizontalScrollbar)
            {
                if (horizontalScrollbar.horizontalSlider.CalculatePixelsToScroll && ContentSize.X > base.Size.X)
                {
                    horizontalScrollbar.horizontalSlider.Show = true;
                    horizontalScrollbar.horizontalSlider.Size = new Vector2(50, 7);
                    InvisibleCotentSize = ContentSize - base.Size;
                    horizontalScrollbar.horizontalSlider.PixelsToScrollContent = InvisibleCotentSize.X / (horizontalScrollbar.ScrollableLength - horizontalScrollbar.horizontalSlider.Size.X);
                    horizontalScrollbar.horizontalSlider.PixelsToScrollSlider = ((horizontalScrollbar.ScrollableLength - horizontalScrollbar.horizontalSlider.Size.X) / InvisibleCotentSize.X) * DesiredPixelToScrollContent;
                    horizontalScrollbar.horizontalSlider.CalculatePixelsToScroll = false;
                }

                ContentOffsetPosition.X = (float)((horizontalScrollbar.horizontalSlider.DefaultPosition.X - horizontalScrollbar.horizontalSlider.Position.X) * horizontalScrollbar.horizontalSlider.PixelsToScrollContent);
                horizontalScrollbar.Update(gameTime);
            }

            if (HasVerticalScrollbar)
            {
                if (verticalScrollbar.verticalSlider.CalculatePixelsToScroll && ContentSize.Y > base.Size.Y)
                {
                    verticalScrollbar.verticalSlider.Show = true;
                    verticalScrollbar.verticalSlider.Size = new Vector2(7, 50);
                    InvisibleCotentSize = ContentSize - base.Size;
                    verticalScrollbar.verticalSlider.PixelsToScrollContent = InvisibleCotentSize.Y / (verticalScrollbar.ScrollableLength - verticalScrollbar.verticalSlider.Size.Y);
                    verticalScrollbar.verticalSlider.PixelsToScrollSlider = ((verticalScrollbar.ScrollableLength - verticalScrollbar.verticalSlider.Size.Y) / InvisibleCotentSize.Y) * DesiredPixelToScrollContent;
                    verticalScrollbar.verticalSlider.CalculatePixelsToScroll = false;
                }

                ContentOffsetPosition.Y = (float)((verticalScrollbar.verticalSlider.DefaultPosition.Y - verticalScrollbar.verticalSlider.Position.Y) * verticalScrollbar.verticalSlider.PixelsToScrollContent);
                verticalScrollbar.Update(gameTime);
            }
        }

        protected virtual new void Update(GameTime gameTime)
        {
            UpdateDimantions();
            UpdateSildersPosition(gameTime);

            ContentPosition = base.Position + ContentOffsetPosition;
            base.Update(gameTime);
        }

        protected virtual new void Draw()
        {
            spriteBatch.Begin();

            if (HasHorizontalScrollbar)
                horizontalScrollbar.Draw();

            if (HasVerticalScrollbar)
                verticalScrollbar.Draw();

            base.Draw();
            spriteBatch.End();
        }

        protected void BeginDrawScissorRectangle()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizerState);
            CurrentScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = ScissorRectangle;
        }

        protected void EndDrawScissorRectangle()
        {
            spriteBatch.GraphicsDevice.ScissorRectangle = CurrentScissorRectangle;
            spriteBatch.End();
        }
    }
}