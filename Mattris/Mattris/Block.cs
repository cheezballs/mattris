
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mattris
{
    class Block
    {
        public int Height;
        public int Width;

        // Each block will have a block type assigned to it
        public BlockTypes Type;

        // Each block needs to keep track of its rectangle and position for drawing and collision
        public Rectangle Rectangle;
        public Vector2 Position;
        
        // Holds the texture that contains the 5 squares we'll be dividing up
        public Texture2D SquareTextures;

        public BlockColors BlockColor; 

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(0, 0, 25, 25);

            switch(BlockColor)
            {
                case BlockColors.Red:
                    {
                        sourceRect = new Rectangle(0, 0, 25, 25);
                        break;
                    }
                case BlockColors.Blue:
                    {
                        sourceRect = new Rectangle(25, 0, 25, 25);
                        break;
                    }
                case BlockColors.Green:
                    {
                        sourceRect = new Rectangle(50, 0, 25, 25);
                        break;
                    }
                case BlockColors.Orange:
                    {
                        sourceRect = new Rectangle(75, 0, 25, 25);
                        break;
                    }
                case BlockColors.Purple:
                    {
                        sourceRect = new Rectangle(100, 0, 25, 25);
                        break;
                    }
            }

            if(Type != BlockTypes.Empty)
                spriteBatch.Draw(SquareTextures, Rectangle, sourceRect, Color.White);
        }

        public Block Clone()
        {
            Block newBlock = new Block();
            newBlock.BlockColor = this.BlockColor;
            newBlock.Width = this.Width;
            newBlock.Height = this.Height;
            newBlock.Type = this.Type;
            newBlock.Rectangle = this.Rectangle;
            newBlock.Position = this.Position;
            newBlock.SquareTextures = this.SquareTextures;
            return newBlock;
        }

    }
}
