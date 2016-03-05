/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 8/March/2015
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
    public class ScreenManager
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        static List<Screen> Screens;

        public ScreenManager()
        {
            Screens = new List<Screen>();
        }

        public void AddNewScreen(Screen scren)
        {
            Screens.Add(scren);
        }

        public static Screen GetScreen(int ScreenIndex)
        {
            return Screens[ScreenIndex];
        }

        public static Screen GetScreen(string ScreenName)
        {
            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Name == ScreenName)
                    return Screens[i];

            return null;
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;

            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && !Screens[i].Initialized)
                {
                    Screens[i].Initialize(Graphics);
                    Screens[i].Initialized = true;
                }
        }

        public void LoadContent()
        {
            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && !Screens[i].Loaded)
                {
                    Screens[i].LoadContent();
                    Screens[i].Loaded = true;
                }
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && !Screens[i].UpdatedOnce)
                {
                    Screens[i].UpdateOnce(spriteBatch);
                    Screens[i].UpdatedOnce = true;
                }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run)
                    Screens[i].UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && (!Screens[i].Initialized || !Screens[i].Loaded || !Screens[i].UpdatedOnce))
                {
                    Initialize(Graphics);
                    LoadContent();
                    UpdateOnce(spriteBatch);
                }

            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && Screens[i].Initialized && Screens[i].Loaded && Screens[i].UpdatedOnce)
                    Screens[i].Update(gameTime);
        }

        public void Draw()
        {
            for (int i = 0; i < Screens.Count; i++)
                if (Screens[i].Run && Screens[i].Initialized && Screens[i].Loaded && Screens[i].UpdatedOnce)
                    Screens[i].Draw();
        }
    }
}