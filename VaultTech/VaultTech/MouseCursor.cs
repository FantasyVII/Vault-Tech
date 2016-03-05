/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 11/March/2015
 * </Copyright>
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VaultTech.Contents;

namespace VaultTech
{
    /// <summary>
    /// MouseCursor class is responsible for mouse movment.
    /// </summary>
    public static class MouseCursor
    {
        #region MonoGame variables
        static GraphicsDeviceManager Graphics;
        static SpriteBatch spriteBatch;
        #endregion
        #region Mouse class variables
        
        /// <summary>
        /// Mouse cursor icon types.
        /// </summary>
        public enum MouseType
        {
            /// <summary>
            /// Normal mouse cursor icon.
            /// </summary>
            Normal, 
            /// <summary>
            /// ColumnResizer mouse cursor icon.
            /// </summary>
            ColumnResizer
        };

        static Texture2D NormalMouseTexture, ColumnResizerTexture;

        /// <summary>
        /// Mouse position.
        /// </summary>
        public static Vector2 Position;

        /// <summary>
        /// Mouse collision rectangle size.
        /// </summary>
        public static Vector2 Size;

        /// <summary>
        /// Mouse position taking into account camera position.
        /// </summary>
        public static Vector2 WorldPosition;

        /// <summary>
        /// Mouse collision rectangle.
        /// </summary>
        public static Rectangle rectangle;

        /// <summary>
        /// Previous mouse button action state.
        /// </summary>
        public static MouseState LastMouseState;

        /// <summary>
        /// Cureent mouse button action state.
        /// </summary>
        public static MouseState CurrentMouseState;

        /// <summary>
        /// Current mouse icon type to draw.
        /// </summary>
        public static MouseType MousePointer;
        #endregion

        public static void Initialize(GraphicsDeviceManager Graphics)
        {
            MouseCursor.Graphics = Graphics;
        }

        /// <summary>
        /// Load mouse cursor icon from disk drive.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(SpriteBatch spriteBatch)
        {
            MouseCursor.spriteBatch = spriteBatch;
            NormalMouseTexture = StreamTexture.LoadTextureFromStream(Graphics, "Content/MouseCursor.png");
            ColumnResizerTexture = StreamTexture.LoadTextureFromStream(Graphics, "Content/ColumnCursor.png");
        }

        /// <summary>
        /// Calculate the mouse position inside the world. 
        /// When calculating the world mouse position, we take into account the camrea position.
        /// So mouse world position is = mouse position + camera position.
        /// </summary>
        /// <param name="CameraPosition">Position of the camera from the Camera class.</param>
        public static void CalculateWorldPosition(Vector2 CameraPosition)
        {
            WorldPosition = new Vector2((int)(MouseCursor.Position.X + CameraPosition.X), (int)(MouseCursor.Position.Y + CameraPosition.Y));
        }

        /// <summary>
        /// Update the mouse cursor position.
        /// </summary>
        public static void Update()
        {
            LastMouseState = CurrentMouseState;

            CurrentMouseState = Mouse.GetState();

            Position = new Vector2((int)CurrentMouseState.X, (int)CurrentMouseState.Y);

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        /// <summary>
        /// Draw the mouse cursor icon.
        /// </summary>
        public static void Draw()
        {
            if (MousePointer == MouseType.Normal)
                spriteBatch.Draw(NormalMouseTexture, Position, Color.White);

            if (MousePointer == MouseType.ColumnResizer)
                spriteBatch.Draw(ColumnResizerTexture, (Position - new Vector2(ColumnResizerTexture.Width, ColumnResizerTexture.Height) / 2), Color.White);
        }
    }
}