/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 4/April/2014
 * Date Moddified :- 24/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.ParticleSystem
{
    public class OldParticleManager
    {
        OldParticle particle;

        public OldParticleManager()
        {
            particle = new OldParticle(new Vector2(100, 100), new Vector2(32, 32), 90f, 0.5f, 10000f, new Color(255, 255, 255, 255), new Color(0, 0, 0, 255));
        }

        public void LoadContent(ContentManager Content)
        {
            particle.LoadContent(Content);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            particle.UpdateOnce(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            particle.Update(gameTime);
        }

        public void Draw()
        {
            particle.Draw();
        }
    }
}