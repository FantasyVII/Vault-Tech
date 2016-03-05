/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 04/May/2015
 * Date Moddified :-05/May/2015
 * </Copyright>
 */

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI
{
    public class Container
    {
        SpriteBatch spriteBatch;

        List<Component> Components;

        public Container()
        {
            Components = new List<Component>();
        }

        public void Add(Component component)
        {
            Components.Add(component);
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].Initialize(Graphics);
        }

        public void LoadContent(string StyleFile)
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].LoadContent(StyleFile, null);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            for (int i = 0; i < Components.Count; i++)
                Components[i].UpdateOnce(spriteBatch);
        }

        void ComponentsZ_DepthOrder()
        {
            List<int> ComponentsToDisable = new List<int>();

            for (int i = 0; i < Components.Count; i++)
                Components[i].DisableMouseInteraction = false;

            for (int i = 0; i < Components.Count; i++)
            {
                for (int j = 0; j < Components.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (Components[i].Hovered && Components[j].Hovered)
                        if (!ComponentsToDisable.Contains(i) && i < j)
                            ComponentsToDisable.Add(i);
                }
            }

            for (int i = 0; i < ComponentsToDisable.Count; i++)
            {
                Components[ComponentsToDisable[i]].DisableMouseInteraction = true;
                Components[ComponentsToDisable[i]].CurrentComponentTexture = Components[ComponentsToDisable[i]].NormalComponentTexture;
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].Update(gameTime);

            ComponentsZ_DepthOrder();
        }

        public void Draw()
        {
            spriteBatch.Begin();

            for (int i = 0; i < Components.Count; i++)
                Components[i].Draw();

            spriteBatch.End();

            for (int i = 0; i < Components.Count; i++)
                Components[i].DrawText();
        }
    }
}