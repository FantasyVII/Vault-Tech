/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 20/December/2014
 * Date Moddified :- 11/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.FontRenderer
{
    public class CharacterData
    {
        public int ASCII_ID;
        public char Character;
        public Texture2D Texture;
        public Vector2 OffsetPosition;
        public Rectangle SourceRectangle;
        public int Spacing;

        public CharacterData(int ASCII_ID, Texture2D Texture, Rectangle SourceRectangle, Vector2 OffsetPosition, int Spacing)
        {
            this.ASCII_ID = ASCII_ID;
            this.Character = (char)ASCII_ID;
            this.Texture = Texture;
            this.SourceRectangle = SourceRectangle;
            this.OffsetPosition = OffsetPosition;
            this.Spacing = Spacing;
        }
    }
}