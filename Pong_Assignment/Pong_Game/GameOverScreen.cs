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
    public class GameOverScreen
    {
        private Vector2 newGamePosition;
        private Vector2 menuPosition;
        private Vector2 gameOverPosition;

        private Point buttonSize = new Point(150, 60);
        private Point gameOverSize = new Point(400, 160);

        private MouseState mouseState;
        private MouseState previousMouseState;

        private Button[] buttons = new Button[2];

        private string playerWonText = "nigga";

        // Constructor
        public GameOverScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;

            newGamePosition = new Vector2(2 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            menuPosition = new Vector2(3 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            gameOverPosition = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (gameOverSize.X / 2), PongGame.pongGame.ScreenSize.Y / 10);

            buttons[0] = new Button(buttonSize, newGamePosition, NewGameButton, PongGame.pongGame.newGameButtonTexture);
            buttons[1] = new Button(buttonSize, menuPosition, MenuButton, PongGame.pongGame.menuButtonTexture);
        }

        // Update Method
        public void Update()
        {
            mouseState = Mouse.GetState();

            foreach (Button b in buttons)
                b.HandleInput(mouseState, previousMouseState);

            previousMouseState = mouseState;
        }

        private void NewGameButton()
        {
            PongGame.pongGame.gameState = GameState.Playing;
        }

        private void MenuButton()
        {
            PongGame.pongGame.gameState = GameState.InMenu;
        }


        // Draw method
        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.gameOverTexture, new Rectangle(gameOverPosition.ToPoint(), gameOverSize), Color.White);

            foreach (Button b in buttons)
                b.Draw();

            PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.customFont, playerWonText, new Vector2(), Color.White);
        }
    }
}
