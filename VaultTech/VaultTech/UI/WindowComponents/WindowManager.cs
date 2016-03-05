/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 7/June/2014
 * Date Moddified :- 12/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI.WindowComponents
{
    public class WindowManager
    {
        #region Private Variables
        List<Window> Windows;
        Window FocusedWindow;
        #endregion

        public WindowManager()
        {
            Windows = new List<Window>();
        }

        public void AddNewWindow(Window window)
        {
            Windows.Add(window);

            if (FocusedWindow == null)
                FocusedWindow = Windows[0];
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].Initialize(Graphics);
        }

        public void LoadContent()
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].LoadContent(Windows[i].StyleFilePath, Windows[i].WindowNodeNameInXml, Windows[i].WindowTitleBarNodeNameInXml);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].UpdateOnce(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                Windows[i].Update(gameTime);

                if (Windows[i].Focused)
                {
                    FocusedWindow = Windows[i];
                    FocusedWindow.WindowIndex = i;
                }
            }

            if (FocusedWindow != null)
            {
                FocusedWindow.Update(gameTime);

                if (!FocusedWindow.windowTitleBar.CloseButton.Hovered && FocusedWindow.windowTitleBar.CloseButton.Released)
                {
                    FocusedWindow.windowTitleBar.CloseButton.Pressed = false;
                    FocusedWindow.windowTitleBar.CloseButton.Released = false;
                }

                if (FocusedWindow.windowTitleBar.CloseButton.Hovered && FocusedWindow.windowTitleBar.CloseButton.Pressed && FocusedWindow.windowTitleBar.CloseButton.Released)
                {
                    Windows.RemoveAt(FocusedWindow.WindowIndex);
                    FocusedWindow = null;
                } 
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Windows.Count; i++)
                Windows[i].Draw();

            if (FocusedWindow != null)
                FocusedWindow.Draw();
        }
    }
}