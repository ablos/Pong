using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong_Game
{
    // Button class
    class Button
    {
        private delegate void ButtonClicked();      // Create delegate to call when button is clicked

        private Point baseSize;                     // Store the size of the button when it isn't hovered over
        private Point size;                         // Current size of the button
        private Vector2 position;                   // Current position of the button
        private Vector2 basePosition;               // Store the position of the button when it isn't hovered over
        private ButtonClicked buttonClicked;        // Local delegate to call when button is clicked
        private Texture2D texture;                  // Store texture of button
        private bool isLarger = false;              // Store if button is already larger from hover over
        private float sizeMultiplier = 1.1f;        // Size multiplier when button is hovered over

        // Constructor
        public Button(Point size, Vector2 position, Action buttonClickedCallBack, Texture2D texture)
        {
            this.size = baseSize = size;                // Copy given size
            this.position = basePosition = position;    // Copy given position
            this.texture = texture;                     // Copy given texture

            // Subscribe the given method to the delegate
            buttonClicked = new ButtonClicked(buttonClickedCallBack);
        }

        // Method for input handling (is called in update)
        public void HandleInput(MouseState mouseState, MouseState previousMouseState)
        {
            // Check if mouse is on top of the button
            if (mouseState.X >= position.X && mouseState.X <= position.X + size.X && mouseState.Y >= position.Y && mouseState.Y <= position.Y + size.Y)
            {
                // Change cursor to be a hand
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                // If button isn't already larger, make it larger (also update position of button to expand from middle)
                if (!isLarger)
                {
                    size = new Point((int)(baseSize.X * sizeMultiplier), (int)(baseSize.Y * sizeMultiplier));
                    position = new Vector2(basePosition.X - ((size.X - baseSize.X) / 2), basePosition.Y - ((size.Y - baseSize.Y) / 2));
                    isLarger = true;
                }

                // If there was a click, call the delegate
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                    buttonClicked();
            }
            // If the button is already larger and the mouse is not on the button anymore, set the size and position to the original size and position
            else if (isLarger)
            {
                size = baseSize;
                position = basePosition;
                isLarger = false;
            }
        }

        // Draw  method: draws the button
        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size), Color.White);
        }

    }
}
