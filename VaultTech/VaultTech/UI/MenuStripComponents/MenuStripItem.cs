/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/June/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI.MenuStripComponents
{
    public class MenuStripItem : Component
    {
        public MenuStripPanel menuStripPanel;

        public bool Show { get; set; }

        public new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        public new Vector2 Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        public new bool Hovered
        {
            get { return base.Hovered; }
            set { base.Hovered = value; }
        }

        public new bool Pressed
        {
            get { return base.Pressed; }
            set { base.Pressed = value; }
        }

        new string Text;
        new Color TextColor;

        public MenuStripItem(string Text, Color TextColor)
        {
            this.Text = Text;
            this.TextColor = TextColor;

            menuStripPanel = new MenuStripPanel();
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
            menuStripPanel.Initialize(Graphics);
        }

        public new void LoadContent(string StyleFilePath, string MenuStripItemNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, MenuStripItemNodeNameInXml);
            menuStripPanel.LoadContent(StyleFilePath, "UIStyle/PanelStyle");
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            menuStripPanel.Position = new Vector2(base.Position.X, base.Position.Y + base.Size.Y);
            menuStripPanel.Size = new Vector2(base.Size.X, 100);
            menuStripPanel.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //base.CreateTextures();

            menuStripPanel.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            if (Show)
                menuStripPanel.Draw();
        }

        public void DrawStaticText()
        {
            if (Show)
                menuStripPanel.DrawStaticText();
        }
    }
}