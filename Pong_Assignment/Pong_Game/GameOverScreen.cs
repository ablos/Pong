using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong_Game
{
    // Game over screen class
    public class GameOverScreen
    {
        private Vector2 newGamePosition;                                    // Store position of new game button
        private Vector2 menuPosition;                                       // Store position of menu button
        private Vector2 gameOverPosition;                                   // Store position of title

        private Point buttonSize = new Point(150, 60);                      // Store buttons size
        private Point gameOverSize = new Point(400, 160);                   // Store title size

        private MouseState mouseState;                                      // Store current mousestate
        private MouseState previousMouseState;                              // Store previous mousestate

        private Button[] buttons = new Button[2];                           // Store buttons

        private string winningPlayerOne;                                    // Store name of the first winning player
        private string winningPlayerTwo;                                    // Store name of the second winning player
        private string victoryTextTwoPlayers = "THE WINNER IS ";            // Store text to be displayed on the screen for two player mode
        private string victoryTextFourPlayers = "THE WINNERS ARE ";         // Store text to be displayed on the screen for four player mode

        private Color winnerColorOne;                                       // Store color of the first winning player
        private Color winnerColorTwo;                                       // Store color of the second winning player

        private Vector2 victoryTextSize;                                    // Store the size of the victory text (calculated by font)
        private Vector2 winningPlayerTextSizeOne;                           // Store the size of the first winning player name (calculated by font)
        private Vector2 winningPlayerTextSizeTwo;                           // Store the size of the second winning player name (calculated by font)
        private Vector2 andTextSize;                                        // Store the size of the "AND" text (calculated by font)

        // Constructor
        public GameOverScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;

            // Calculate button and title positions
            newGamePosition = new Vector2(2 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            menuPosition = new Vector2(3 * (PongGame.pongGame.ScreenSize.X / 5) - (buttonSize.X / 2), 2 * (PongGame.pongGame.ScreenSize.Y / 3) - (buttonSize.Y / 2));
            gameOverPosition = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (gameOverSize.X / 2), PongGame.pongGame.ScreenSize.Y / 10);

            // Create buttons
            buttons[0] = new Button(buttonSize, newGamePosition, NewGameButton, PongGame.pongGame.newGameButtonTexture);
            buttons[1] = new Button(buttonSize, menuPosition, MenuButton, PongGame.pongGame.menuButtonTexture);

            // Check which team is still alive and set winning values accordingly
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

            // Calculate victory text size by font
            if (PongGame.pongGame.fourPlayers)
                victoryTextSize = PongGame.pongGame.pixelFont.MeasureString(victoryTextFourPlayers);
            else
                victoryTextSize = PongGame.pongGame.pixelFont.MeasureString(victoryTextTwoPlayers);

            // Calculate "AND" text size by font
            andTextSize = PongGame.pongGame.pixelFont.MeasureString(" AND ");
        }

        // Set winning values method
        private void SetWinningValues(bool teamRedGreenWon)
        {
            // Set values according to what team won
            if (teamRedGreenWon)
            {
                winningPlayerOne = "RED";
                winningPlayerTwo = "GREEN";
                winnerColorOne = Color.Red;
                winnerColorTwo = Color.LimeGreen;
            }
            else
            {
                winningPlayerOne = "YELLOW";
                winningPlayerTwo = "BLUE";
                winnerColorOne = Color.Yellow;
                winnerColorTwo = Color.CornflowerBlue;
            }

            // Calculate player name sizes by font
            winningPlayerTextSizeOne = PongGame.pongGame.pixelFont.MeasureString(winningPlayerOne);
            winningPlayerTextSizeTwo = PongGame.pongGame.pixelFont.MeasureString(winningPlayerTwo);
        }

        // Update Method
        public void Update()
        {
            // Get the current mousestate
            mouseState = Mouse.GetState();

            // Handle input on all buttons
            foreach (Button b in buttons)
                b.HandleInput(mouseState, previousMouseState);

            // Set the previous mousestate to the current
            previousMouseState = mouseState;
        }

        // Functionality for new game button
        private void NewGameButton()
        {
            // Create new game handler and set gamestate to playing
            PongGame.pongGame.gameState = GameState.Playing;
            PongGame.pongGame.gameHandler = new GameHandler();
        }

        // Functionality for menu button
        private void MenuButton()
        {
            // Create new screen menu and set gamestate to in menu
            PongGame.pongGame.menuScreen = new MenuScreen();
            PongGame.pongGame.gameState = GameState.InMenu;
        }


        // Draw method
        public void Draw()
        {
            // Draw the players and ball on their last position
            PongGame.pongGame.gameHandler.Draw();

            // Draw the title
            PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.gameOverTexture, new Rectangle(gameOverPosition.ToPoint(), gameOverSize), Color.White);

            // Draw the buttons
            foreach (Button b in buttons)
                b.Draw();

            // Draw victory text (calculate position of strings depending on sizes of the strings)
            if (PongGame.pongGame.fourPlayers)
            {
                // victoryTextFourPlayers + winningPlayerOne (winnerColorOne) + "AND" + winningPlayerTwo (winnerColorTwo)
                float stringStartPosX = PongGame.pongGame.ScreenSize.X / 2 - (victoryTextSize.X + winningPlayerTextSizeOne.X + andTextSize.X + winningPlayerTextSizeTwo.X) / 2;
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, victoryTextFourPlayers,   new Vector2(stringStartPosX, gameOverPosition.Y + gameOverSize.Y), Color.White);
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, winningPlayerOne,         new Vector2(stringStartPosX + victoryTextSize.X, gameOverPosition.Y + gameOverSize.Y), winnerColorOne);
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, " AND ",                  new Vector2(stringStartPosX + victoryTextSize.X + winningPlayerTextSizeOne.X, gameOverPosition.Y + gameOverSize.Y), Color.White);
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, winningPlayerTwo,         new Vector2(stringStartPosX + victoryTextSize.X + winningPlayerTextSizeOne.X + andTextSize.X, gameOverPosition.Y + gameOverSize.Y), winnerColorTwo);

            }
            else
            {
                // victoryTextTwoPlayers + winningPlayerOne (winnerColorOne)
                float stringStartPosX = PongGame.pongGame.ScreenSize.X / 2 - (victoryTextSize.X + winningPlayerTextSizeOne.X) / 2;
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, victoryTextTwoPlayers,    new Vector2(stringStartPosX, gameOverPosition.Y + gameOverSize.Y), Color.White);
                PongGame.pongGame.spriteBatch.DrawString(PongGame.pongGame.pixelFont, winningPlayerOne,         new Vector2(stringStartPosX + victoryTextSize.X, gameOverPosition.Y + gameOverSize.Y), winnerColorOne);
            }
        }
    }
}
