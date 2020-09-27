﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong_Game
{
    // Menu screen class
    public class MenuScreen
    {
        private Vector2 quitPosition;                           // Store position of quit button
        private Vector2 twoPlayersPosition;                     // Store position of two players button
        private Vector2 fourPlayersPosition;                    // Store position of four players button
        private Vector2 menuPongPosition;                       // Store position of title position

        private Point buttonSize = new Point(150, 60);          // Store button size
        private Point quitButtonSize = new Point(100, 40);      // Store quit button size
        private Point menuPongSize = new Point(500, 200);       // Store title size

        private MouseState mouseState;                          // Store mouse state
        private MouseState previousMouseState;                  // Store previous mouse state

        private Button[] buttons = new Button[3];               // Store all buttons in the menu

        // Constructor
        public MenuScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;

            // Calculate all button positions
            quitPosition = new Vector2(PongGame.pongGame.ScreenSize.X / 20, 7 * (PongGame.pongGame.ScreenSize.Y / 8));
            twoPlayersPosition = new Vector2(2 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            fourPlayersPosition = new Vector2(3 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            menuPongPosition = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (menuPongSize.X / 2), PongGame.pongGame.ScreenSize.Y / 10);

            // Create buttons
            buttons[0] = new Button(quitButtonSize, quitPosition, QuitButton, PongGame.pongGame.quitButtonTexture);
            buttons[1] = new Button(buttonSize, twoPlayersPosition, TwoPlayersButton, PongGame.pongGame.twoPlayersButtonTexture);
            buttons[2] = new Button(buttonSize, fourPlayersPosition, FourPlayersButton, PongGame.pongGame.fourPlayersButtonTexture);
        }

        // Update method
        public void Update()
        {
            // Get current mouse state
            mouseState = Mouse.GetState();

            // Handle input on buttons
            foreach (Button b in buttons)
                b.HandleInput(mouseState, previousMouseState);

            // Set previous mouse state to current
            previousMouseState = mouseState;
        }

        // Functionality for quit button
        private void QuitButton()
        {
            // Exit game
            PongGame.pongGame.Exit();
        }

        // Functionality for two players button
        private void TwoPlayersButton()
        {
            // Set to play with two players and start game
            PongGame.pongGame.fourPlayers = false;
            PongGame.pongGame.gameState = GameState.Playing;
        }

        // Functionality for four players button
        private void FourPlayersButton()
        {
            // Set to play with four players and start game
            PongGame.pongGame.fourPlayers = true;
            PongGame.pongGame.gameState = GameState.Playing;
        }

        // Draw method
        public void Draw()
        {
            // Draw title
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.menuPongTexture, new Rectangle(menuPongPosition.ToPoint(), menuPongSize), Color.White);

            // Draw buttons
            foreach (Button b in buttons)
                b.Draw();
        }

    }
}
