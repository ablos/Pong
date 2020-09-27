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
        delegate void ButtonClicked();

        private Point baseSize;
        private Point size;
        private Vector2 position;
        private ButtonClicked buttonClicked;
        private Texture2D texture;


        public Button(Point size, Vector2 position, Action buttonClickedCallBack, Texture2D texture)
        {
            this.size = baseSize = size;
            this.position = position;
            this.texture = texture;

            buttonClicked = new ButtonClicked(buttonClickedCallBack);
        }

        public void HandleInput(MouseState mouseState, MouseState previousMouseState)
        {
            if (mouseState.X >= position.X && mouseState.X <= position.X + size.X && mouseState.Y >= position.Y && mouseState.Y <= position.Y + size.Y)
            {
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                    buttonClicked();               
            }
        }

        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size), Color.White);
        }

    }
}
