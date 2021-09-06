using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoboGun_V1
{
    class Player
    {
        private Texture2D hoboWalk;
        private Texture2D hoboIdle; 
        private Vector2 hoboPosition = new Vector2(64, 470);
        private Vector2 velocity;
        private Rectangle hoboRectangle;

        private bool hasJumped = false;
        private bool idle = true;

        int frame;
        int row;
        int totalFrames;
        float totalElapsed;
        float timePerFrame;
        int framePerSec;

        KeyboardState keyboardState;
        public Vector2 Position
        {
            get { return hoboPosition; }
        }

        public Player() { }

        public void Load(ContentManager Content)
        {
            hoboWalk = Content.Load<Texture2D>("hoboWalk");
            hoboIdle = Content.Load<Texture2D>("hoboIdle");
            
            frame = 0;
            row = 0;
            totalFrames = 4;
            framePerSec = 6;
            timePerFrame = (float)1 / framePerSec;
            totalElapsed = 0;
        }


        public void Update(GameTime gameTime)
        {
            hoboPosition += velocity;
            hoboRectangle = new Rectangle((int)hoboPosition.X, (int)hoboPosition.Y, 70, 80);

            Input(gameTime);

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

        }
        private void Input(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D))
            {
                idle = false;
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 5;
                row = 0;

                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                idle = false;
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 5;
                row = 1;

                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                velocity.X = 0f;
                idle = true;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && hasJumped == false)
            {
                totalFrames = 3;
                hoboPosition.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
            }
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (hoboRectangle.TouchTopOf(newRectangle))
            {
                hoboRectangle.Y = newRectangle.Y - hoboRectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }
            if (hoboRectangle.TouchLeftOf(newRectangle))
            {
                hoboPosition.X = newRectangle.X - hoboRectangle.Width - 2;
            }
            if (hoboRectangle.TouchRightOf(newRectangle))
            {
                hoboPosition.X = newRectangle.X + newRectangle.Width + 2;
            }
            if (hoboRectangle.TouchBottomOf(newRectangle))
            {
                velocity.Y = 1f;
            }

            if (hoboPosition.X < 0) hoboPosition.X = 0;
            if (hoboPosition.X > xOffset - hoboRectangle.Width) hoboPosition.X = xOffset - hoboRectangle.Width;
            if (hoboPosition.Y < 0) velocity.Y = 1f;
            if (hoboPosition.Y > yOffset - hoboRectangle.Height) hoboPosition.Y = yOffset - hoboRectangle.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (idle == false)
                spriteBatch.Draw(hoboWalk, hoboRectangle, new Rectangle(90 * frame, 100 * row, 90, 100), Color.White);
            else if (idle == true)
                spriteBatch.Draw(hoboIdle, hoboRectangle, new Rectangle(90 * 0, 100 * row, 90, 100), Color.White);
           
        }

        void UpdateFrame(float elapsed)
        {
            totalElapsed += elapsed;
            if (totalElapsed > timePerFrame)
            {
                frame = (frame + 1) % totalFrames;
                totalElapsed -= timePerFrame;
            }
        }
    }
}

