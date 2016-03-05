/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 26/January/2015
 * Date Moddified :- 11/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.MapContent
{
    /// <summary>
    /// This class is responsible for storing all the tile sheets.
    /// </summary>
    class TileSheet
    {
        /// <summary>
        /// Tile sheet texture.
        /// </summary>
        internal Texture2D Texture;
        /// <summary>
        /// Tile sheet path or location on the hard drive.
        /// </summary>
        internal string Path;

        /// <summary>
        /// TileSheet constructor that stors the texture and path.
        /// </summary>
        /// <param name="Texture">Tile sheet texture.</param>
        /// <param name="Path">Tile sheet path in the hard drive.</param>
        internal TileSheet(Texture2D Texture, string Path)
        {
            this.Texture = Texture;
            this.Path = Path;
        }
    }
}