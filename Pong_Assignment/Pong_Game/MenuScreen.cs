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
    public class MenuScreen
    {

        private Vector2 quitPosition;
        private Vector2 twoPlayersPosition;
        private Vector2 fourPlayersPosition;
        private Vector2 menuPongPosition;

        private Point buttonSize = new Point(200, 80);
        private Point menuPongSize = new Point(600, 240);

        MouseState mouseState;
        MouseState previousMouseState;

        // COnstructor
        public MenuScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;

            quitPosition = new Vector2(PongGame.pongGame.ScreenSize.X / 8, 4 * (PongGame.pongGame.ScreenSize.Y / 5));
            twoPlayersPosition = new Vector2(2 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 3 * (PongGame.pongGame.ScreenSize.Y / 4) - (buttonSize.Y / 2));
            fourPlayersPosition = new Vector2(3 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 3 * (PongGame.pongGame.ScreenSize.Y / 4) - (buttonSize.Y / 2));
            menuPongPosition = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (menuPongSize.X / 2), PongGame.pongGame.ScreenSize.Y / 10);

        }

        // Deconstructor
        ~MenuScreen()
        {
            // Make the mouse pointer invisible
            PongGame.pongGame.IsMouseVisible = false;
        }

        // Update method
        public void Update()
        {
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            if(quitPosition.X <= mouseState.X && mouseState.X <= quitPosition.X + buttonSize.X && quitPosition.Y <= mouseState.Y && mouseState.Y <= quitPosition.Y + buttonSize.Y)
            {
                if(previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {

                }
            }

            if(twoPlayersPosition.X <= mouseState.X && mouseState.X <= twoPlayersPosition.X + buttonSize.X && twoPlayersPosition.Y <= mouseState.Y && mouseState.Y <= twoPlayersPosition.Y + buttonSize.Y)
            {
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {

                }
            }

            if(fourPlayersPosition.X <= mouseState.X && mouseState.X <= fourPlayersPosition.X + buttonSize.X && fourPlayersPosition.Y <= mouseState.Y && mouseState.Y <= fourPlayersPosition.Y + buttonSize.Y)
            {
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {

                }
            }
        }

        // Draw method
        public void Draw()
        {
            PongGame.pongGame.GraphicsDevice.Clear(Color.Black);

            PongGame.pongGame.spriteBatch.Begin();

            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.quitButtonTexture, quitPosition, Color.White);
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.twoPlayersButtonTexture, twoPlayersPosition, Color.White);
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.fourPlayersButtonTexture, fourPlayersPosition, Color.White);
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.menuPongTexture, menuPongPosition, Color.White);

            PongGame.pongGame.spriteBatch.End();
        }

    }
}
