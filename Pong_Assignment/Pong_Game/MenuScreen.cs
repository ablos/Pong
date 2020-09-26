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

        }

        // Draw method
        public void Draw()
        {

        }

    }
}
