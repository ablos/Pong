using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong_Game
{
    // Ball class
    public class Ball
    {
        private float speed = 400;                                  // Variable to store speed of the ball
        public Vector2 location;                                    // Variable to store location of the ball
        private static readonly Point size = new Point(18, 18);     // Variable to store size of the ball
        public Color color = Color.White;                           // Variable to store color of the ball
        private Texture2D texture;                                  // Variable to store the texture of the ball
        private Vector2 direction;                                  // Variable to store the direction of the ball
        private static float minimumAngle = 0.4f;                   // Variable to store minimum angle of the start direction of the ball
        private bool allowBounceRight = false;                      // Variable to store if ball is allowed to bounce against players on the right
        private bool allowBounceLeft = false;                       // Variable to store if ball is allowed to bounce against players on the left
        private bool allowBounceTop = true;                         // Variable to store if ball is allowed to bounce against the ceiling
        private bool allowBounceBottom = true;                      // Variable to store if ball is allowed to bounce against the bottom
        private const float speedMultiplier = 1.02f;                // Variable to multiply speed by

        // Constructor of the ball class
        public Ball(Texture2D texture)
        {
            this.texture = texture;     // Copy the given texture value to the local texture variable

            // Position the ball in the middle of the screen
            location = new Vector2((PongGame.pongGame.ScreenSize.X / 2) - (size.X / 2), (PongGame.pongGame.ScreenSize.Y / 2) - (size.Y / 2));

            // Get a random angle and make sure it is above the minimum angle
            float r = (float)PongGame.pongGame.random.NextDouble();
            if (r < minimumAngle)
                r = minimumAngle;

            // Set the direction according to the angle
            direction = new Vector2(PongGame.pongGame.random.Next(1, 3) == 1 ? 1 : -1, (PongGame.pongGame.random.Next(1, 3) == 1 ? 1 : -1) * r);

            // Set restrictions for bouncing
            if (direction.X < 0)
                allowBounceLeft = true;
            else
                allowBounceRight = true;
        }

        // Draw the ball
        public void Draw()
        {
            PongGame.pongGame.spriteBatch.Draw(texture, new Rectangle(location.ToPoint(), size), color);
        }

        public void DetectOutOfBounds()
        {
            // Make sure the ball doesn't go out of bounds, when it hits the left or right side of the screen, take a live.
            if (location.X > (PongGame.pongGame.ScreenSize.X - size.X) || location.X < 0)
            {
                // Is the ball on the left side of the screen?
                if (location.X < PongGame.pongGame.ScreenSize.X / 2)
                {
                    // If there is a player that plays on the left side, kill it
                    if (!PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.Left))
                    {
                        // If there is a player on the bottom left, and the ball is there as well, kill it
                        if (location.Y >= PongGame.pongGame.ScreenSize.Y / 2)
                            PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.BottomLeft);
                        // If there is a player on the top left, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.TopLeft);
                    }
                }
                // The ball is on the right side of the screen
                else
                {
                    // If there is a player that plays the right side, kill it
                    if (!PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.Right))
                    {
                        // If there is a player on the bottom right, and the ball is there as well, kill it
                        if (location.Y >= PongGame.pongGame.ScreenSize.Y / 2)
                            PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.BottomRight);
                        // If there is a player on the top right, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.gameHandler.FindPlayerToKill(PlayField.TopRight);
                    }
                }
            }

            // Make sure the ball doesn't go out of bounds, when it hits the top or the bottom, invert the Y direction when allowed
            if (location.Y > (PongGame.pongGame.ScreenSize.Y - size.Y) && allowBounceBottom)
            {
                direction.Y *= -1;
                allowBounceBottom = false;
                allowBounceTop = true;

                // Play wall hit sound when sfx on
                if (PongGame.pongGame.sfxOn)
                    PongGame.pongGame.wallHitSound.Play();
            }
            else if (location.Y < 0 && allowBounceTop)
            {
                direction.Y *= -1;
                allowBounceBottom = true;
                allowBounceTop = false;

                // Play wall hit sound when sfx on
                if (PongGame.pongGame.sfxOn)
                    PongGame.pongGame.wallHitSound.Play();
            }
        }

        public void DetectCollision()
        {
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
                    if (location.X < PongGame.pongGame.ScreenSize.X / 2 && !allowBounceLeft)
                        return;

                    // If the ball is on the right side, but it isn't allowed to bounce against the player (to prevent the ball getting stuck in the player), stop here
                    if (location.X > PongGame.pongGame.ScreenSize.X / 2 && !allowBounceRight)
                        return;

                    // Invert the restrictions
                    allowBounceRight = !allowBounceRight;
                    allowBounceLeft = !allowBounceLeft;

                    // Ball bounced, allow to bounce on top and bottom again
                    allowBounceTop = true;
                    allowBounceBottom = true;

                    // Take player color and apply on the ball
                    color = p.color;

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

                    // Increase speed of ball
                    speed *= speedMultiplier;
                    
                    // Play paddle hit sound effect when sfx on
                    if (PongGame.pongGame.sfxOn)
                        PongGame.pongGame.paddleHitSound.Play();
                }
            }
        }

        // Move the ball
        public void Move(float elapsedTime)
        {
            // Move the ball according to the direction, speed and time
            location += direction * speed * elapsedTime;
        }
    }
}
