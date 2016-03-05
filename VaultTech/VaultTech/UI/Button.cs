/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 04/May/2015
 * </Copyright>
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI
{
    public class Button : Component
    {
        internal override void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        internal override void LoadContent(string StyleFilePath, string ButtonNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, "UIStyle/ButtonsStyle");
        }

        internal override void LoadContentFromArchive(string StyleFilePath, string ButtonNodeNameInXml)
        {
            base.LoadContentFromArchive(StyleFilePath, ButtonNodeNameInXml);
        }

        internal override void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!base.Hovered && base.Pressed)
                base.CurrentComponentTexture = base.HoveredComponentTexture;
        }

        internal override void Draw()
        {
            base.Draw();
        }

        internal override void DrawText()
        {
            base.DrawText();
        }
    }
}