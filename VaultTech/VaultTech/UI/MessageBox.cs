/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 12/June/2014
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VaultTech.UI.WindowComponents;

namespace VaultTech.UI
{
    public class MessageBox : Window
    {
        /* public enum Buttons { YesNo, YesNoCancel, YesNoHelp, Ok, OKCancel, OKCancelHelp };
         Button[] buttons;

         public MessageBox()
         {
             buttons = new Button[3];

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i] = new Button();
         }

         public override void Initialize(GraphicsDeviceManager Graphics)
         {
             base.Initialize(Graphics);

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i].Initialize(Graphics);
         }

         public override void LoadContent(string StyleFilePath, string MessageBoxNodeNameInXml, string ButtonNodeNameInXml)
         {
             base.LoadContent(StyleFilePath, MessageBoxNodeNameInXml);

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i].LoadContent(StyleFilePath, ButtonNodeNameInXml);
         }

         public override void UpdateOnce()
         {
             base.Position = new Vector2((Graphics.PreferredBackBufferWidth / 2) - (base.Size.X / 2),
                                         (Graphics.PreferredBackBufferHeight / 2) - (base.Size.Y / 2));
             base.UpdateOnce();

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i].UpdateOnce();
         }

         public override void Update(GameTime gameTime)
         {
             base.Update(gameTime);

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i].Update(gameTime);
         }

         public override void Draw(SpriteBatch spriteBatch)
         {
             base.Draw(spriteBatch);

             for (int i = 0; i < buttons.Length; i++)
                 buttons[i].Draw(spriteBatch);
         }*/
    }
}