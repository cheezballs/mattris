using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Mattris
{
    class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        private float rotation;
        private Vector2 Velocity;
        public float RotationSpeed;
        public float Speed;
        private int screenHeight, screenWidth;

        public Sprite(GraphicsDeviceManager graphics, Texture2D texture, Vector2 position, Vector2 velocity, float rotationSpeed, float speed)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Speed = speed;

            rotation = 0.0f;
            RotationSpeed = rotationSpeed;

            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
        }

        public Sprite()
        {

        }

        public void Update(GameTime gameTime)
        {
            int screenEdge = 100;

            Velocity.Normalize();
            Position += Velocity * Speed;

            if (Position.X - screenEdge > screenWidth)
                Position = new Vector2(0 - Texture.Width - screenEdge, Position.Y);
            if (Position.X + screenEdge + Texture.Width < 0)
                Position = new Vector2(screenWidth + screenEdge, Position.Y);
            if (Position.Y - screenEdge > screenHeight)
                Position = new Vector2(Position.X, 0 - Texture.Height - screenEdge);
            if (Position.Y + screenEdge + Texture.Height < 0)
                Position = new Vector2(Position.X, screenHeight + screenEdge);

            rotation = rotation + RotationSpeed;

            if (rotation > MathHelper.ToRadians(360))
                rotation = rotation - MathHelper.ToRadians(360);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height),
                null,
                Color.White,
                rotation,                
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                SpriteEffects.None,
                1);
        }
    }
}
