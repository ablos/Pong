﻿using Microsoft.Xna.Framework;
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

        private string winningPlayerOne;
        private string winningPlayerTwo;
        private string victoryTextTwoPlayers = "THE WINNER IS";
        private string victoryTextFourPlayers = "THE WINNERS ARE";

        private Color winnerColorOne;
        private Color winnerColorTwo;

        private Vector2 victoryTextSize;
        private Vector2 winningColorTextSize;

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

            foreach (Player p in PongGame.pongGame.players)
            {
                if (p.lives > 0)
                {
                    if (p.color == Color.Red || p.color == Color.LimeGreen)
                        SetWinningValues(true);
                    else
                        SetWinningValues(false);
                }
            }
        }

        private void SetWinningValues(bool teamRedGreenWon)
        {
            if (teamRedGreenWon)
            {
                winningPlayerOne = "RED";
                winningPlayerTwo = "GREEN";
                winnerColorOne = Color.Red;
                winnerColorTwo = Color.LimeGreen;
            }else
            {
                winningPlayerOne = "YELLOW";
                winningPlayerTwo = "BLUE";
                winnerColorOne = Color.Yellow;
                winnerColorTwo = Color.CornflowerBlue;
            }

            victoryTextSize = PongGame.pongGame.pixelFont.MeasureString(victoryText);
            winningColorTextSize = PongGame.pongGame.pixelFont.MeasureString(winningColor);
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
            PongGame.pongGame.gameHandler = null;
            PongGame.pongGame.gameState = GameState.Playing;
        }

        private void MenuButton()
        {
            PongGame.pongGame.gameState = GameState.InMenu;
        }


        // Draw method
        public void Draw()
        {
            PongGame.pongGame.gameHandler.Draw();

            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.gameOverTexture, new Rectangle(gameOverPosition.ToPoint(), gameOverSize), Color.White);

            foreach (Button b in buttons)
                b.Draw();

            PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, victoryText, new Vector2((PongGame.pongGame.ScreenSize.X - victoryTextSize.X) / 2 - winningColorTextSize.X / 2, 
                                                                                                            gameOverPosition.Y + gameOverSize.Y), Color.White);
            PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, winningColor, new Vector2((PongGame.pongGame.ScreenSize.X - winningColorTextSize.X) / 2 + victoryTextSize.X / 2, 
                                                                                                            gameOverPosition.Y + gameOverSize.Y), winnerColor);
        }
    }
}
