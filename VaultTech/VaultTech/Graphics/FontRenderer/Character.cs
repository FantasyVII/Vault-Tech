/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 30/April/2014
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
    public class Character
    {
        public CharacterData characterData;
        internal Vector2 PositionInString;
        public Vector2 Position, Size;
        public Color color;

        internal Character(CharacterData characterData, Vector2 Position, Color color)
        {
            this.characterData = characterData;
            this.Position = Position;
            this.Size = new Vector2(characterData.SourceRectangle.Width, characterData.SourceRectangle.Height);
            this.color = color;
        }
    }
}