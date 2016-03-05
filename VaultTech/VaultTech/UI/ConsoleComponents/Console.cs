/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 9/May/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.UI.TextBoxComponents;

namespace VaultTech.UI.ConsoleComponents
{
    public class Console
    {
        SpriteBatch spriteBatch;

        ConsolePanel panel;
        TextBox textbox;

        public Vector2 Position
        {
            get { return panel.Position; }
            set { panel.Position = value; }
        }

        public Vector2 Size
        {
            get { return panel.Size; }
            set { panel.Size = value; }
        }

        public Console()
        {
            panel = new ConsolePanel();
            textbox = new TextBox();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            panel.Initialize(Graphics);
            textbox.Initialize(Graphics);
        }

        public void LoadContent(string StyleFilePath, string PanelNodeNameInXml, string TextBoxNodeNameInXml)
        {
            panel.LoadContent(StyleFilePath, PanelNodeNameInXml);
            textbox.LoadContent(StyleFilePath, TextBoxNodeNameInXml);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            textbox.Position = new Vector2(panel.Position.X, panel.Position.Y + panel.Size.Y);
            textbox.Size = new Vector2(panel.Size.X, textbox.Size.Y);

            panel.UpdateOnce(spriteBatch);
            textbox.UpdateOnce(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            panel.Update(gameTime);
            textbox.Update(gameTime);
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            panel.Draw();
            textbox.Draw();
            spriteBatch.End();

          /*  if (textbox.EnterKeyPressed)
            {
                panel.EnterText(textbox.Text);

                textbox.Text = "";

                textbox.EnterKeyPressed = false;
            }*/
        }
        /*
        public void DrawDynamicText()
        {
            spriteBatch.Begin();
            textbox.DrawDynamicText(Color.White);
            spriteBatch.End();
        }*/
    }
}