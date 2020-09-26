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
    // Ball class
    public class Ball
    {
        private const float speed = 400;                        // Variable to store speed of the ball
        public Vector2 location;                                // Variable to store location of the ball
        private readonly Point size = new Point(18, 18);        // Variable to store size of the ball
        public Color color = Color.White;                       // Variable to store color of the ball
        private GraphicsDevice gDevice;                         // Variable to store graphics device from monogame (used for boundaries)
        private SpriteBatch spriteBatch;                        // Variable to store sprite batch from monogame (used for drawing)
        private Texture2D texture;                              // Variable to store the texture of the ball
        private Vector2 direction;                              // Variable to store the direction of the ball
        private Random random = new Random();                   // Variable used to create random numbers
        private float minimumAngle = 0.4f;                      // Variable to store minimum angle of the start direction of the ball
        private bool allowBounceRight = false;                  // Variable to store if ball is allowed to bounce against players on the right
        private bool allowBounceLeft = false;                   // Variable to store if ball is allowed to bounce against players on the left

        /*
         * !!! IMPORTANT !!!
         * Ball can get stuck on ceiling or bottom, prevent this from allowing it to only bounce once, until it has bounced onto something else.
         */

        // Constructor of the ball class
        public Ball(Texture2D texture, GraphicsDevice gDevice, SpriteBatch spriteBatch)
        {
            this.texture = texture;             // Copy the given texture value to the local texture variable
            this.gDevice = gDevice;             // Save the GraphicsDevice in a local variable
            this.spriteBatch = spriteBatch;     // Save the Sprite Batch in a local variable

            // Position the ball in the middle of the screen
            location = new Vector2((gDevice.Viewport.Bounds.Width / 2) - (size.X / 2), (gDevice.Viewport.Bounds.Height / 2) - (size.Y / 2));

            // Get a random angle and make sure it is above the minimum angle
            float r = (float)random.NextDouble();
            if (r < minimumAngle)
                r = minimumAngle;

            // Set the direction according to the angle
            direction = new Vector2((random.Next(1, 3) == 1 ? 1 : -1), (random.Next(1, 3) == 1 ? 1 : -1) * r);

            // Set restrictions for bouncing
            if (direction.X < 0)
                allowBounceLeft = true;
            else
                allowBounceRight = true;
        }

        // Draw the ball
        public void Draw()
        {
            spriteBatch.Draw(texture, new Rectangle(location.ToPoint(), size), color);
        }

        // Move the ball
        public void Move(float elapsedTime)
        {
            // Move the ball according to the direction, speed and time
            location += direction * speed * elapsedTime;

            // Make sure the ball doesn't go out of bounds, when it hits the left or right side of the screen, take a live.
            if (location.X > (gDevice.Viewport.Width - size.X) || location.X < 0)
            {
                // Is the ball on the left side of the screen?
                if (location.X < gDevice.Viewport.Width / 2)
                {
                    // If there is a player that plays on the left side, kill it
                    if (!PongGame.pongGame.FindPlayerToKill(PlayField.Left))
                    {
                        // If there is a player on the bottom left, and the ball is there as well, kill it
                        if (location.Y >= gDevice.Viewport.Height / 2)
                            PongGame.pongGame.FindPlayerToKill(PlayField.BottomLeft);
                        // If there is a player on the top left, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.FindPlayerToKill(PlayField.TopLeft);
                    }
                }
                // The ball is on the right side of the screen
                else
                {
                    // If there is a player that plays the right side, kill it
                    if (!PongGame.pongGame.FindPlayerToKill(PlayField.Right))
                    {
                        // If there is a player on the bottom right, and the ball is there as well, kill it
                        if (location.Y >= gDevice.Viewport.Height / 2)
                            PongGame.pongGame.FindPlayerToKill(PlayField.BottomRight);
                        // If there is a player on the top right, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.FindPlayerToKill(PlayField.TopRight);
                    }
                }
            }

            // Make sure the ball doesn't go out of bounds, when it hits the top or the bottom, invert the Y direction
            if (location.Y > (gDevice.Viewport.Height - size.Y) || location.Y < 0)
                direction.Y *= -1;

            // Create a rectangle around the ball to detect collision
            Rectangle ballRect = new Rectangle(location.ToPoint(), size);
            foreach (Player p in PongGame.pongGame.players)
            {
                // Create a rectangle around the player to detect collision
                Rectangle playerRect = new Rectangle(p.location, p.size);

                // If both rectangles intersect, decide what to do with the ball
                if (playerRect.Intersects(ballRect))
                {
                    // If the ball is on the left side, but it isn't allowed to bounce against the player (to prevent the ball getting stuck in the player), stop here
                    if (location.X < gDevice.Viewport.Bounds.Width / 2 && !allowBounceLeft)
                        return;

                    // If the ball is on the right side, but it isn't allowed to bounce against the player (to prevent the ball getting stuck in the player), stop here
                    if (location.X > gDevice.Viewport.Bounds.Width / 2 && !allowBounceRight)
                        return;

                    // Invert the restrictions
                    allowBounceRight = !allowBounceRight;
                    allowBounceLeft = !allowBounceLeft;

                    // Invert the X direction
                    direction.X *= -1;

                    // If the ball hits the player on the top 1/5th of the player, bounce up
                    if (((int)location.Y + size.Y / 2) <= (p.location.Y + p.size.Y / 5))
                    {
                        direction.Y = -1;
                    }
                    // If the ball hits the player on the second to top 1/5th of the player, bounce slightly up
                    else if (((int)location.Y + size.Y / 2) > (p.location.Y + p.size.Y / 5)
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 2 * p.size.Y / 5)
                    {
                        direction.Y = -0.5f;
                    }
                    // If the ball hits the player in the middle, bounce straight
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 2 * p.size.Y / 5
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 3 * p.size.Y / 5)
                    {
                        direction.Y = 0;
                    }
                    // If the ball hits the player on the second to bottom 1/5th of the player, bounce slightly down
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 3 * p.size.Y / 5
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 4 * p.size.Y / 5)
                    {
                        direction.Y = 0.5f;
                    }
                    // If the ball hits the player on the bottom 1/5th of the player, bounce down
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 4 * p.size.Y / 5)
                    {
                        direction.Y = 1;
                    }
                }
            }
        }
    }
}
