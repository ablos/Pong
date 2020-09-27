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

        private Point buttonSize = new Point(150, 60);
        private Point quitButtonSize = new Point(100, 40);
        private Point menuPongSize = new Point(500, 200);

        private MouseState mouseState;
        private MouseState previousMouseState;

        private Button[] buttons = new Button[3];

        // COnstructor
        public MenuScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;

            quitPosition = new Vector2(PongGame.pongGame.ScreenSize.X / 20, 7 * (PongGame.pongGame.ScreenSize.Y / 8));
            twoPlayersPosition = new Vector2(2 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            fourPlayersPosition = new Vector2(3 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            menuPongPosition = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (menuPongSize.X / 2), PongGame.pongGame.ScreenSize.Y / 10);

            CreateButtons();
        }

        private void CreateButtons()
        {
            buttons[0] = new Button(quitButtonSize, quitPosition, QuitButton, PongGame.pongGame.quitButtonTexture);
            buttons[1] = new Button(buttonSize, twoPlayersPosition, TwoPlayersButton, PongGame.pongGame.twoPlayersButtonTexture);
            buttons[2] = new Button(buttonSize, fourPlayersPosition, FourPlayersButton, PongGame.pongGame.fourPlayersButtonTexture);
        }

        // Update method
        public void Update()
        {
            mouseState = Mouse.GetState();

            foreach (Button b in buttons)
                b.HandleInput(mouseState, previousMouseState);

            previousMouseState = mouseState;
        }

        private void QuitButton()
        {
            PongGame.pongGame.Exit();
        }
        private void TwoPlayersButton()
        {
            PongGame.pongGame.fourPlayers = false;
            PongGame.pongGame.gameState = GameState.Playing;
        }
        private void FourPlayersButton()
        {
            PongGame.pongGame.fourPlayers = true;
            PongGame.pongGame.gameState = GameState.Playing;
        }

        // Draw method
        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.menuPongTexture, new Rectangle(menuPongPosition.ToPoint(), menuPongSize), Color.White);

            foreach (Button b in buttons)
                b.Draw();
        }

    }
}
