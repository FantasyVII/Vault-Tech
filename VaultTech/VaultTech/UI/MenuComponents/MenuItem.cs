/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 10/August/2014
 * Date Moddified :- 8/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI.MenuComponents
{
    public class MenuItem : Component
    {
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

        public new Alignment Alignment { get; set; }

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

        public new bool Released
        {
            get { return base.Released; }
            set { base.Released = value; }
        }

        public MenuItem(string Text, Color TextColor)
        {
            base.Text = Text;
            this.TextColor = TextColor;
        }

        internal new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        internal new void LoadContent(string StyleFilePath, string MenuItemNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, MenuItemNodeNameInXml);
        }

        internal new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        internal new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal new void Draw()
        {
            base.Draw();
        }
    }
}