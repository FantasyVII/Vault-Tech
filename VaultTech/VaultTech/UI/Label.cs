/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 27/July/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI
{
    public class Label : Component
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

        internal override void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        internal override void LoadContent(string StyleFilePath, string LabelNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, "UIStyle/LabelsStyle");
        }

        internal override void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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