/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 28/April/2014
 * Date Moddified :- 12/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VaultTech.Graphics.FontRenderer;

class KeyIdentifier
{

    Keys key;
    
}

namespace VaultTech.UI.TextBoxComponents
{
    public class TextBoxHelper
    {
        FontRenderer fontRenderer;

        KeyboardState newState, oldState;

        public string Text;
        public string PasswordString;

        public bool IsPasswordProtected;
        public bool IsMultiLine;

        public Vector2 TextPosition, TextSize;
        public Vector2 TextBoxPosition, TextBoxSize;
        bool CapsLock;

        public bool EnterKeyPressed;

        public Rectangle CursorRectangle;
        public bool DrawCursor;
        Stopwatch CursorTimer;
        int CurrentTextIndex;

        int StartingIndexOfNewLine;

        public TextBoxHelper(FontRenderer fontRenderer)
        {
            this.fontRenderer = fontRenderer;
            Text = "";
            PasswordString = "";

            CursorTimer = new Stopwatch();
            CursorTimer.Start();
        }

        void CheckCapsLock()
        {
            if (newState.IsKeyDown(Keys.CapsLock) && oldState.IsKeyUp(Keys.CapsLock) && !CapsLock)
                CapsLock = true;

            else if (newState.IsKeyDown(Keys.CapsLock) && oldState.IsKeyUp(Keys.CapsLock) && !CapsLock)
                CapsLock = false;
        }

        void PrintNewLine()
        {
            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                Text += "\n";
                StartingIndexOfNewLine = Text.Length;
            }
        }

        void PrintSpace()
        {
            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
                PrintCharacter(" ");
        }

        void DeleteCharacter()
        {
            if (newState.IsKeyDown(Keys.Back) && oldState.IsKeyUp(Keys.Back) && Text.Length > 0)
            {
                Text = Text.Remove(Text.Length - 1, 1);
                CurrentTextIndex -= 1;

                if (Text.Length > 0)
                {
                    MoveTextForward(Text[Text.Length - 1].ToString());
                    TextSize.X -= (int)fontRenderer.MeasureString(Text[Text.Length - 1].ToString()).X;
                }
            }
        }

        void PrintCharacter(string Character)
        {
            Text = Text.Insert(CurrentTextIndex, Character);
            TextSize.X += (int)fontRenderer.MeasureString(Character).X;
            CurrentTextIndex += 1;
            MoveTextBackwards(Character);
        }

        void PrintNumbers()
        {
            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D0) && oldState.IsKeyUp(Keys.D0))
                PrintCharacter(")");
            else if ((newState.IsKeyDown(Keys.D0) && oldState.IsKeyUp(Keys.D0)) || (newState.IsKeyDown(Keys.NumPad0) && oldState.IsKeyUp(Keys.NumPad0)))
                PrintCharacter("0");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1))
                PrintCharacter("!");
            else if (newState.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1) || (newState.IsKeyDown(Keys.NumPad1) && oldState.IsKeyUp(Keys.NumPad1)))
                PrintCharacter("1");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D2) && oldState.IsKeyUp(Keys.D2))
                PrintCharacter("@");
            else if (newState.IsKeyDown(Keys.D2) && oldState.IsKeyUp(Keys.D2) || (newState.IsKeyDown(Keys.NumPad2) && oldState.IsKeyUp(Keys.NumPad2)))
                PrintCharacter("2");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D3) && oldState.IsKeyUp(Keys.D3))
                PrintCharacter("#");
            else if (newState.IsKeyDown(Keys.D3) && oldState.IsKeyUp(Keys.D3) || (newState.IsKeyDown(Keys.NumPad3) && oldState.IsKeyUp(Keys.NumPad3)))
                PrintCharacter("3");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D4) && oldState.IsKeyUp(Keys.D4))
                PrintCharacter("$");
            else if (newState.IsKeyDown(Keys.D4) && oldState.IsKeyUp(Keys.D4) || (newState.IsKeyDown(Keys.NumPad4) && oldState.IsKeyUp(Keys.NumPad4)))
                PrintCharacter("4");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D5) && oldState.IsKeyUp(Keys.D5))
                PrintCharacter("%");
            else if (newState.IsKeyDown(Keys.D5) && oldState.IsKeyUp(Keys.D5) || (newState.IsKeyDown(Keys.NumPad5) && oldState.IsKeyUp(Keys.NumPad5)))
                PrintCharacter("5");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D6) && oldState.IsKeyUp(Keys.D6))
                PrintCharacter("^");

            else if (newState.IsKeyDown(Keys.D6) && oldState.IsKeyUp(Keys.D6) || (newState.IsKeyDown(Keys.NumPad6) && oldState.IsKeyUp(Keys.NumPad6)))
                PrintCharacter("6");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D7) && oldState.IsKeyUp(Keys.D7))
                PrintCharacter("&");
            else if (newState.IsKeyDown(Keys.D7) && oldState.IsKeyUp(Keys.D7) || (newState.IsKeyDown(Keys.NumPad7) && oldState.IsKeyUp(Keys.NumPad7)))
                PrintCharacter("7");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D8) && oldState.IsKeyUp(Keys.D8))
                PrintCharacter("*");
            else if (newState.IsKeyDown(Keys.D8) && oldState.IsKeyUp(Keys.D8) || (newState.IsKeyDown(Keys.NumPad8) && oldState.IsKeyUp(Keys.NumPad8)))
                PrintCharacter("8");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.D9) && oldState.IsKeyUp(Keys.D9))
                PrintCharacter("(");
            else if (newState.IsKeyDown(Keys.D9) && oldState.IsKeyUp(Keys.D9) || (newState.IsKeyDown(Keys.NumPad9) && oldState.IsKeyUp(Keys.NumPad9)))
                PrintCharacter("9");
        }

        int SmallLetters = 32;
        int TotalNumbersOfLetter = 26;

        void PrintLetters(bool IsCapital)
        {
            for (int i = 0; i < TotalNumbersOfLetter; i++)
            {
                if (newState.IsKeyDown(Keys.A + i) && oldState.IsKeyUp(Keys.A + i))
                {
                    if (IsPasswordProtected)
                        Text = Text.Insert(CurrentTextIndex, "*");
                    else if (!IsCapital)
                        PrintCharacter(((char)(Keys.A + SmallLetters + i)).ToString());
                    else
                        PrintCharacter(((char)(Keys.A + i)).ToString());

                    break;
                }
            }
        }

        void PrintSymboles()
        {
            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemComma) && oldState.IsKeyUp(Keys.OemComma))
                PrintCharacter("<");
            else if ((newState.IsKeyDown(Keys.OemComma) && oldState.IsKeyUp(Keys.OemComma)))
                PrintCharacter(",");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemPeriod) && oldState.IsKeyUp(Keys.OemPeriod))
                PrintCharacter(">");
            else if ((newState.IsKeyDown(Keys.OemPeriod) && oldState.IsKeyUp(Keys.OemPeriod)) || (newState.IsKeyDown(Keys.Decimal) && oldState.IsKeyUp(Keys.Decimal)))
                PrintCharacter(".");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemSemicolon) && oldState.IsKeyUp(Keys.OemSemicolon))
                PrintCharacter(":");
            else if (newState.IsKeyDown(Keys.OemSemicolon) && oldState.IsKeyUp(Keys.OemSemicolon))
                PrintCharacter(";");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemQuotes) && oldState.IsKeyUp(Keys.OemQuotes))
                PrintCharacter("\"");
            else if (newState.IsKeyDown(Keys.OemQuotes) && oldState.IsKeyUp(Keys.OemQuotes))
                PrintCharacter("'");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemPipe) && oldState.IsKeyUp(Keys.OemPipe))
                PrintCharacter("|");
            else if (newState.IsKeyDown(Keys.OemPipe) && oldState.IsKeyUp(Keys.OemPipe))
                PrintCharacter("\\");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemQuestion) && oldState.IsKeyUp(Keys.OemQuestion))
                PrintCharacter("?");
            else if (newState.IsKeyDown(Keys.OemQuestion) && oldState.IsKeyUp(Keys.OemQuestion))
                PrintCharacter("/");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemOpenBrackets) && oldState.IsKeyUp(Keys.OemOpenBrackets))
                PrintCharacter("{");
            else if (newState.IsKeyDown(Keys.OemOpenBrackets) && oldState.IsKeyUp(Keys.OemOpenBrackets))
                PrintCharacter("[");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemCloseBrackets) && oldState.IsKeyUp(Keys.OemCloseBrackets))
                PrintCharacter("}");
            else if (newState.IsKeyDown(Keys.OemCloseBrackets) && oldState.IsKeyUp(Keys.OemCloseBrackets))
                PrintCharacter("]");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemPlus) && oldState.IsKeyUp(Keys.OemPlus))
                PrintCharacter("+");
            else if (newState.IsKeyDown(Keys.OemPlus) && oldState.IsKeyUp(Keys.OemPlus))
                PrintCharacter("=");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemMinus) && oldState.IsKeyUp(Keys.OemMinus))
                PrintCharacter("_");
            else if (newState.IsKeyDown(Keys.OemMinus) && oldState.IsKeyUp(Keys.OemMinus))
                PrintCharacter("-");

            if ((newState.IsKeyDown(Keys.LeftShift) || newState.IsKeyDown(Keys.RightShift)) && newState.IsKeyDown(Keys.OemTilde) && oldState.IsKeyUp(Keys.OemTilde))
                PrintCharacter("~");
            else if (newState.IsKeyDown(Keys.OemTilde) && oldState.IsKeyUp(Keys.OemTilde))
                PrintCharacter("`");
        }

        void MoveTextBackwards(string Character)
        {
            if ((fontRenderer.MeasureString(Text).X) >= TextBoxSize.X)
            {
                TextPosition.X -= fontRenderer.MeasureString(Character).X;
            }
        }

        void MoveTextForward(string Character)
        {
            if (TextPosition.X <= TextBoxPosition.X)
            {
                TextPosition.X += fontRenderer.MeasureString(Character).X;
            }
        }

        void FindClosestNumberInList(float TargetNumber, List<float> ListOfNumbers)
        {
            int ClosestIndex = 0;
            float Number1 = 0;
            float Number2 = 0;

            for (int i = 0; i < ListOfNumbers.Count; i++)
            {
                Number1 = Math.Abs(TargetNumber - ListOfNumbers[i]);

                if (i + 1 < ListOfNumbers.Count)
                    Number2 = Math.Abs(TargetNumber - ListOfNumbers[i + 1]);
                else
                    Number2 = Number1;

                if (Number1 < Number2)
                {
                    if (Number1 < Math.Abs(TargetNumber - ListOfNumbers[ClosestIndex]))
                        ClosestIndex = i;
                }
                else
                    if (Number2 < Math.Abs(TargetNumber - ListOfNumbers[ClosestIndex]))
                    ClosestIndex = i + 1;
            }

            CurrentTextIndex = ClosestIndex;
        }

        void UpdateCursor(GameTime gameTime)
        {
            if (CursorTimer.ElapsedMilliseconds <= 500)
                DrawCursor = true;

            if (CursorTimer.ElapsedMilliseconds >= 500)
                DrawCursor = false;

            if (CursorTimer.ElapsedMilliseconds >= 1000)
                CursorTimer.Restart();

            if (MouseCursor.CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                List<float> LettersPosition = new List<float>();

                for (int i = 0; i < fontRenderer.Characters.Count; i++)
                    LettersPosition.Add(fontRenderer.Characters[i].Position.X);

                LettersPosition.Add(fontRenderer.Characters[fontRenderer.Characters.Count - 1].Position.X + fontRenderer.Characters[fontRenderer.Characters.Count - 1].characterData.Spacing);
                FindClosestNumberInList(MouseCursor.Position.X, LettersPosition);
            }
        }

        public void UpdateOnce()
        {
            if (Text.Length > 0)
                CurrentTextIndex = Text.Length;

            //+ BorderThikness.Left
            //this.TextPosition = new Vector2(TextBoxPosition.X + 1, TextBoxPosition.Y);

            CursorRectangle = new Rectangle((int)TextBoxPosition.X + 2, (int)TextBoxPosition.Y + 1, 1, (int)TextBoxSize.Y - 2);
        }

        public void Update(GameTime gameTime)
        {
            if (fontRenderer.Characters.Count > 0)
                if (CurrentTextIndex > 0)
                    CursorRectangle = new Rectangle((int)fontRenderer.Characters[CurrentTextIndex - 1].Position.X +
                                                    (int)fontRenderer.Characters[CurrentTextIndex - 1].characterData.Spacing,
                                                    (int)TextBoxPosition.Y + 1, 1, (int)TextBoxSize.Y - 2);
                else
                    CursorRectangle = new Rectangle((int)TextBoxPosition.X + 1, (int)TextBoxPosition.Y + 1, 1, (int)TextBoxSize.Y - 2);

            PasswordString = Text;

            newState = Keyboard.GetState();

            CheckCapsLock();

            PrintNumbers();

            PrintSymboles();

            PrintSpace();

            //PrintNewLine();

            DeleteCharacter();

            UpdateCursor(gameTime);

            if (oldState.IsKeyUp(Keys.LeftShift) && oldState.IsKeyUp(Keys.RightShift) && !CapsLock)
                PrintLetters(false);

            if ((oldState.IsKeyDown(Keys.LeftShift) || oldState.IsKeyDown(Keys.RightShift) || CapsLock))
                PrintLetters(true);

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                TextPosition.X = TextBoxPosition.X;
                CursorRectangle = new Rectangle((int)TextBoxPosition.X + 2, (int)TextBoxPosition.Y + 1, 1, (int)TextBoxSize.Y - 2);
                EnterKeyPressed = true;
            }
            else
                EnterKeyPressed = false;

            oldState = newState;
        }

        public void DrawString(Color TextColor)
        {


            if (IsMultiLine)
            {
                /*fontRenderer.DrawDynamicText(Text, TextPosition, TextColor);

                if (fontRenderer.MeasureString(Text, StartingIndexOfNewLine, Text.Length).X >= TextBoxSize.X)
                {
                    Text += "\n";
                    StartingIndexOfNewLine = Text.Length;
                }*/
            }
            else
            {
                if (IsPasswordProtected)
                {
                    //fontRenderer.DrawDynamicText(PasswordString, TextPosition, new Rectangle((int)TextBoxPosition.X, 0, (int)(TextBoxSize.X), 0), TextColor);
                }
                else
                {
                    /*BeginDrawScissorRectangle();
                    fontRenderer.Draw(Text, TextPosition, TextColor);
                    EndDrawScissorRectangle();*/
                    //fontRenderer.DrawDynamicText(Text, TextPosition, new Rectangle((int)TextBoxPosition.X, 0, (int)(TextBoxSize.X), 0), TextColor);
                }
            }
        }
    }
}