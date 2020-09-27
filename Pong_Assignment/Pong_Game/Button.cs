using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Pong_Game
{
    class Button
    {
        private delegate void ButtonClicked();

        private Point baseSize;
        private Point size;
        private Vector2 position;
        private Vector2 basePosition;
        private ButtonClicked buttonClicked;
        private Texture2D texture;
        private bool isLarger = false;
        private float sizeMultiplier = 1.1f;

        public Button(Point size, Vector2 position, Action buttonClickedCallBack, Texture2D texture)
        {
            this.size = baseSize = size;
            this.position = basePosition = position;
            this.texture = texture;

            buttonClicked = new ButtonClicked(buttonClickedCallBack);
        }

        public void HandleInput(MouseState mouseState, MouseState previousMouseState)
        {
            if (mouseState.X >= position.X && mouseState.X <= position.X + size.X && mouseState.Y >= position.Y && mouseState.Y <= position.Y + size.Y)
            {
                Mouse.PlatformSetCursor(MouseCursor.Hand);
                if (!isLarger)
                {
                    size = new Point((int)(baseSize.X * sizeMultiplier), (int)(baseSize.Y * sizeMultiplier));
                    position = new Vector2(basePosition.X - ((size.X - baseSize.X) / 2), basePosition.Y - ((size.Y - baseSize.Y) / 2));
                    isLarger = true;
                }

                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                    buttonClicked();
            }
            else if (isLarger)
            {
                size = baseSize;
                position = basePosition;
                isLarger = false;
            }
        }

        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size), Color.White);
        }

    }
}
