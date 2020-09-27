using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pong_Game
{
    public class GameHandler
    {
        private bool startDelayDone = false;        // Variable to store if ball is allowed to move, this allows for a delay at the start of each round
        private int startDelay = 1500;              // Variable to store how long the start delay should be in milliseconds

        // Constructor for PlayerHandler
        public GameHandler()
        {
            // Set mouse to be invisible
            PongGame.pongGame.IsMouseVisible = false;
            // Create all players
            CreatePlayers(PongGame.pongGame.fourPlayers);
            // Call reset method to start new round
            Reset();
        }

        // Deconstructor for PlayerHandler
        ~GameHandler()
        {
            // Destroy all players
            PongGame.pongGame.players = null;

            // Destoy ball
            PongGame.pongGame.ball = null;
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            // Handle player input
            HandleInput();

            // If the start delay hasn't been finished, don't move the ball
            if (!startDelayDone)
                return;

            // Move ball, detect if out of bounds and detect collision
            PongGame.pongGame.ball.Move((float)gameTime.ElapsedGameTime.TotalSeconds);
            PongGame.pongGame.ball.DetectCollision();
            PongGame.pongGame.ball.DetectOutOfBounds();
        }

        // Draw method
        public void Draw()
        {
            // Draw all players
            foreach (Player p in PongGame.pongGame.players)
                p.Draw();

            // Draw the ball
            PongGame.pongGame.ball.Draw();
        }

        // Create players
        private void CreatePlayers(bool _fourPlayers)
        {
            // Create player list to store all created players (using list for easy adding without setting a size first)
            List<Player> _players = new List<Player>();

            // Create players one and two
            Player playerOne = new Player(Color.Red, PlayField.Left);
            Player playerTwo = new Player(Color.Yellow, PlayField.Right);

            // Add all players to the player list
            _players.Add(playerOne);
            _players.Add(playerTwo);

            // When four player mode is activated, create more players and update old players update field and start locations
            if (PongGame.pongGame.fourPlayers)
            {
                // Edit playfields of players one and two and update start locations
                playerOne.playField = PlayField.TopLeft;
                playerOne.MoveToStartLocation();
                playerTwo.playField = PlayField.TopRight;
                playerTwo.MoveToStartLocation();

                // Create players three and four and add to the list
                _players.Add(new Player(Color.LimeGreen, PlayField.BottomLeft));
                _players.Add(new Player(Color.CornflowerBlue, PlayField.BottomRight));
            }

            // Convert the player list to an array and store it (arrays are faster and use less memory)
            PongGame.pongGame.players = _players.ToArray();
        }

        // Find a player to kill with the same playfield the ball is in, when found, kill the player and reset the ball
        public bool FindPlayerToKill(PlayField field)
        {
            foreach (Player p in PongGame.pongGame.players)
            {
                if (p.playField == field)
                {
                    p.Die();
                    Reset();
                    return true;
                }
            }

            return false;
        }

        // Reset the game for a new round
        private async void Reset()
        {
            // If the gamestate is equal to gameover, stop method (this is for drawing the players and ball on their last location at game over screen)
            if (PongGame.pongGame.gameState == GameState.GameOver)
                return;

            // Create a new ball in the middle of the screen
            PongGame.pongGame.ball = new Ball(PongGame.pongGame.ballTexture);

            // Reset all players to their start location
            foreach (Player p in PongGame.pongGame.players)
                p.MoveToStartLocation();

            // Delay the moving of the ball at the start of the round
            startDelayDone = false;
            await Task.Delay(startDelay);
            startDelayDone = true;
        }

        // Handle input
        private void HandleInput()
        {
            // Get current keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // Go back to menu when escape is pressed
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                PongGame.pongGame.menuScreen = new MenuScreen();
                PongGame.pongGame.gameState = GameState.InMenu;
                return;
            }


            // Check if playing with four or two players
            if (!PongGame.pongGame.fourPlayers)
            {
                // When playing with two players

                // When key S is pressed
                if (keyboardState.IsKeyDown(Keys.S))
                    PongGame.pongGame.players[0].Move(false);
                // When key W is pressed
                else if (keyboardState.IsKeyDown(Keys.W))
                    PongGame.pongGame.players[0].Move(true);

                // When arrow down is pressed
                if (keyboardState.IsKeyDown(Keys.Down))
                    PongGame.pongGame.players[1].Move(false);
                // When arrow up is pressed
                else if (keyboardState.IsKeyDown(Keys.Up))
                    PongGame.pongGame.players[1].Move(true);
            }
            else
            {
                // When playing with four players

                // Loop trough all players
                foreach (Player p in PongGame.pongGame.players)
                {
                    // Key checks for top left player
                    if (p.playField == PlayField.TopLeft || p.playField == PlayField.Left)
                    {
                        // When S key is pressed
                        if (keyboardState.IsKeyDown(Keys.S))
                            p.Move(false);
                        // When W key is pressed
                        else if (keyboardState.IsKeyDown(Keys.W))
                            p.Move(true);
                    }

                    // Key checks for bottom left player
                    if (p.playField == PlayField.BottomLeft || p.playField == PlayField.Left)
                    {
                        // When C key is pressed
                        if (keyboardState.IsKeyDown(Keys.C))
                            p.Move(false);
                        // When F key is pressed
                        else if (keyboardState.IsKeyDown(Keys.F))
                            p.Move(true);
                    }

                    // Key checks for top right player
                    if (p.playField == PlayField.TopRight || p.playField == PlayField.Right)
                    {
                        // When arrow down key is pressed
                        if (keyboardState.IsKeyDown(Keys.Down))
                            p.Move(false);
                        // When arrow up key is pressed
                        else if (keyboardState.IsKeyDown(Keys.Up))
                            p.Move(true);
                    }

                    // Key checks for bottom right player
                    if (p.playField == PlayField.BottomRight || p.playField == PlayField.Right)
                    {
                        // When ? key is pressed
                        if (keyboardState.IsKeyDown(Keys.OemQuestion))
                            p.Move(false);
                        // When ' key is pressed
                        else if (keyboardState.IsKeyDown(Keys.OemQuotes))
                            p.Move(true);
                    }
                }
            }
        }
    }
}
