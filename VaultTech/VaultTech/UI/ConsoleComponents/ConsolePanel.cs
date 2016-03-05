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

namespace VaultTech.UI.ConsoleComponents
{
    class ConsolePanel : PanelComponents.Panel
    {
        public ConsolePanel()
        {
            base.DisableVerticalAlighment = true;
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        public new void LoadContent(string StyleFilePath, string ComponentNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, ComponentNodeNameInXml);
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
            base.Text = "6";
        }

        public new void Update(GameTime gameTime)
        {
            /*  if (base.fontRenderer.MeasureString(base.Text).Y > base.Size.Y)
                  base.TextOffsetPosition.Y += fontRenderer.NewLineHeight;*/

            base.Update(gameTime);
        }

        public void EnterText(string Text)
        {
            base.Text += Text + "\n";
            base.TextPosition = new Vector2(0, 500);

        }

        public void ClearText()
        {
            base.Text = " ";
           // base.fontRenderer.ConvertStaticTextToTexture = true;

        }

        public new void Draw()
        {
            base.Draw();
        }

        public void DrawStaticText()
        {
            //base.BeginDrawScissorRectangle();
            //   base.DrawStaticText();
            //   base.EndDrawScissorRectangle();
        }
    }
}