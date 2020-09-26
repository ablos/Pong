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
    // Player class
    public class Player
    {
        public int lives = 3;                                           // Variable to store lives for player
        private int livesTextureOffset = 5;                             // Offset for the space between the textures of the lives
        private readonly Point lifeTextureSize = new Point(20, 20);     // Size of the life textures
        private const int speed = 10;                                   // Set speed of player (is for every player)
        public readonly Point size = new Point(20, 100);                // Set size of player (is for every player, readonly to prevent accedental edits)
        public Point location;                                          // Variable to store player location
        private Texture2D texture;                                      // Variable to store the texture of the player
        public Color color;                                             // Variable to store the color of the player
        public PlayField playField;                                     // Variable to store playfield for player -> where is he allowed to play

        // Constructor for the player class
        public Player(Color color, PlayField playField)
        {
            this.playField = playField;     // Copy the given value to the local playField variable
            this.color = color;             // Set color of player to the color given

            // Move the player to the start location
            MoveToStartLocation();

            // Create texture for player and set color data
            texture = new Texture2D(PongGame.pongGame.gDevice, size.X, size.Y);
            SetColor();
        }

        // Move the player to the start location
        public void MoveToStartLocation()
        {
            location = DetermineStartLocation();
        }

        // Function to determine start location of player (for multiplayer use)
        private Point DetermineStartLocation()
        {
            int rightSideX = (int)PongGame.pongGame.ScreenSize.X - size.X;                          // Calculate the X coördinate for players on the right side of the screen
            int halfScreenY = (int)(PongGame.pongGame.ScreenSize.Y / 2) - (size.Y / 2);             // Calculate the Y coördinate if player is allowed to use the whole side of the screen
            int topHalfScreenY = (int)(PongGame.pongGame.ScreenSize.Y / 4) - (size.Y / 2);          // Calculate the Y coördinate if player is allowed to use only the top of the screen
            int bottomHalfScreenY = (int)(PongGame.pongGame.ScreenSize.Y * 0.75) - (size.Y / 2);    // Calculate the Y coördinate if player is allowed to use only the bottom of the screen

            // Set location of player depending on the PlayField the player is allowed to play on
            switch (playField)
            {
                // If playfield is left side of the screen, place player in the middle of the left side of the screen
                case PlayField.Left:
                    return new Point(0, halfScreenY);
                // If playfield is right side of the screen, place player in the middle of the right side of the screen
                case PlayField.Right:
                    return new Point(rightSideX, halfScreenY);
                // If playfield is top left of the screen, place player in the middle of the top half of the screen on the left side
                case PlayField.TopLeft:
                    return new Point(0, topHalfScreenY);
                // If playfield is bottom left of the screen, place player in the middle of the bottom half of the screen on the left side
                case PlayField.BottomLeft:
                    return new Point(0, bottomHalfScreenY);
                // If playfield is top right of the screen, place player in the middle of the top half of the screen on the right side
                case PlayField.TopRight:
                    return new Point(rightSideX, topHalfScreenY);
                // If playfield is bottom right of the screen, place player in the middle of the bottom half of the screen on the right side
                case PlayField.BottomRight:
                    return new Point(rightSideX, bottomHalfScreenY);
                // If playfield isn't set somehow, throw an exception killing the program.
                default:
                    throw new Exception("No player field given! Please contact developers.");
            }
        }

        // Set the color data of the texture of the player
        private void SetColor()
        {
            // Make color array same size as there are pixels in the texture
            Color[] data = new Color[size.X * size.Y];
            // Set each color to the desired player color
            for (int i = 0; i < data.Length; i++)
                data[i] = color;

            // Apply the color array to the texture pixels
            texture.SetData(data);
        }

        // Draw player and lives on the screen
        public void Draw()
        {
            // Draw the player
            PongGame.pongGame.spriteBatch.Draw(texture, new Rectangle(location, size), color);

            // Draw the lives
            // Variable to store the position of the lives
            Point pos = new Point(0, livesTextureOffset);

            // Determine the X position of the lives for players on the right side of the screen
            if (playField == PlayField.Right || playField == PlayField.TopRight || playField == PlayField.BottomRight)
                pos.X = (int)PongGame.pongGame.ScreenSize.X - (lives * (livesTextureOffset + lifeTextureSize.X)) - livesTextureOffset;

            // Determine the Y position of the lives for the players on the bottom side of the screen
            if (playField == PlayField.BottomRight || playField == PlayField.BottomLeft)
                pos.Y = (int)PongGame.pongGame.ScreenSize.Y - lifeTextureSize.Y - livesTextureOffset;

            // Draw all the lives
            for (int i = 0; i < lives; i++)
            {
                pos.X += livesTextureOffset;
                PongGame.pongGame.spriteBatch.Draw(PongGame.pongGame.lifeTexture, new Rectangle(pos, lifeTextureSize), Color.White);
                pos.X += lifeTextureSize.X;
            }
        }

        // Action when player fails to hit ball
        public void Die()
        {
            // Remove life
            lives--;

            // When player doesn't have any lives left, show game over screen, or in case of four players, let the remaining player play the whole half of the screen
            if (lives <= 0)
            {
                if (PongGame.pongGame.fourPlayers)
                {
                    // Remove one player from the field, unless all players from one half are dead
                    bool hasTeammate = false;
                    foreach (Player p in PongGame.pongGame.players)
                    {
                        // Try to find a teammate, if one is found, edit the playfield for this player
                        switch (playField)
                        {
                            case PlayField.TopLeft:
                                if (p.playField == PlayField.BottomLeft && p.lives > 0)
                                {
                                    p.playField = PlayField.Left;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.BottomLeft:
                                if (p.playField == PlayField.TopLeft && p.lives > 0)
                                {
                                    p.playField = PlayField.Left;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.TopRight:
                                if (p.playField == PlayField.BottomRight && p.lives > 0)
                                {
                                    p.playField = PlayField.Right;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.BottomRight:
                                if (p.playField == PlayField.TopRight && p.lives > 0)
                                {
                                    p.playField = PlayField.Right;
                                    hasTeammate = true;
                                }
                                break;
                        }

                        // If a teammate was found, break out of the loop
                        if (hasTeammate)
                            break;
                    }

                    // If there is no teammate: game over
                    if (!hasTeammate)
                    {
                        PongGame.pongGame.gameState = GameState.GameOver;
                        return;
                    }

                    // If there is a teammate, remove this player from game
                    List<Player> players = PongGame.pongGame.players.ToList();
                    players.Remove(this);
                    PongGame.pongGame.players = players.ToArray();
                }
                else
                {
                    // Game over
                    PongGame.pongGame.gameState = GameState.GameOver;
                }
            }
        }

        // Move the player - Arg. moveUp to determine if player should move up or down
        public void Move(bool moveUp)
        {
            int _speed = moveUp ? speed : (speed * -1); // Set speed positive or negative depending on the moveUp arg., then move player
            int _locationY = location.Y;                // Create temporary Y location variable (to prevent player from going out of bound temporarily)
            _locationY -= _speed;                       // Change the location depending on the speed

            // If player can reach the bottom of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.Left || playField == PlayField.Right || playField == PlayField.BottomLeft || playField == PlayField.BottomRight)
            {
                // Prevent the player from going out of the screen on the bottom
                if ((_locationY + size.Y) > PongGame.pongGame.ScreenSize.Y)
                    _locationY = (int)PongGame.pongGame.ScreenSize.Y - size.Y;
            }

            // If player can reach the top of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.Left || playField == PlayField.Right || playField == PlayField.TopLeft || playField == PlayField.TopRight)
            {
                // Prevent the player from going out of the screen on the top
                if (_locationY < 0)
                    _locationY = 0;
            }
            // If the player is allowed to use only the top half of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.TopLeft || playField == PlayField.TopRight)
            {
                // Prevent player from going under half of the screen.
                if ((_locationY + size.Y) > (PongGame.pongGame.ScreenSize.Y / 2))
                    _locationY = (int)(PongGame.pongGame.ScreenSize.Y / 2) - size.Y;
            }
            // If the player is allowed to use only the bottom half of the screen (depending of playfield), prevent it from going out of bounds
            else if (playField == PlayField.BottomRight || playField == PlayField.BottomLeft)
            {
                // Prevent player from going above half of the screen.
                if (_locationY < (PongGame.pongGame.ScreenSize.Y / 2))
                    _locationY = (int)PongGame.pongGame.ScreenSize.Y / 2;
            }

            // Set the actual location value to the temporary location value
            location.Y = _locationY;
        }
    }
}
