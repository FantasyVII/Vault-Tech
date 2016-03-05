/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 11/April/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.ScreenManager
{
    public abstract class Screen
    {
        public bool Run, Initialized, Loaded, UpdatedOnce;
        public string Name;

        public Screen()
        {
            Run = false;
            Initialized = false;
            Loaded = false;
            UpdatedOnce = false;
        }

        public abstract void Initialize(GraphicsDeviceManager Graphics);
        public abstract void LoadContent();
        public abstract void UpdateOnce(SpriteBatch spriteBatch);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
    }
}