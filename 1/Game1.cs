using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboGun_V1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D background;
        Map map;
        Player player;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 832;
            graphics.PreferredBackBufferHeight = 640;
        }

        protected override void Initialize()
        {
            map = new Map();
            player = new Player();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Background");
            Tiles.Content = Content;
            map.Generate(new int[,]
            {
                //1                      //13
                {0,0,0,0,0,0,0,0,0,0,0,0,0,}, //1
                {0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,1,1,1,0,0,1,1,1,},
                {1,1,1,0,0,0,0,0,0,0,0,0,0,},
                {2,2,2,1,0,0,0,0,0,0,0,0,0,},
                {2,0,0,0,0,1,1,1,1,0,0,0,1,},
                {0,0,0,0,0,0,0,0,0,0,0,1,2,},
                {0,0,0,0,0,0,0,0,0,0,1,2,2,},
                {0,0,0,0,0,0,0,0,0,1,2,2,2,},
                {1,1,1,1,1,1,1,1,1,2,2,2,2,}, //10
            }, 64);
            player.Load(Content);
        
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update(gameTime);
            foreach (CollisionTiles tile in map.CollisionTiles)
                player.Collision(tile.Rectangle, map.Width, map.Height);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
          
            base.Draw(gameTime);
        }
    }
}
