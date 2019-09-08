
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mattris
{
    class GamePiece
    {
        // Holds the w x h size of the array that makes up the GamePiece
        const int ARRAY_SIZE = 5;

        private int rotationCounter;

        // This is the array that holds the current moveable game piece
        public Block[,] PieceBlocks;

        public PieceShape Shape;

        // This will hold the color of the entire game piece - all blocks in a piece will be the same color
        public BlockColors Color;

        // The x,y pixel position of the top-left [0,0] element in the array
        public Vector2 Position;

        private Texture2D squaresTexture;

        public GamePiece(PieceShape shape, BlockColors color, Vector2 position, int blockSize, Texture2D squares)
        {
            Position = position;
            Shape = shape;
            rotationCounter = 0;
            squaresTexture = squares;

            PieceBlocks = new Block[ARRAY_SIZE, ARRAY_SIZE];

            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                {
                    PieceBlocks[a, b] = new Block();
                    PieceBlocks[a, b].Type = BlockTypes.Empty;
                    PieceBlocks[a, b].Width = blockSize;
                    PieceBlocks[a, b].Height = blockSize;
                    PieceBlocks[a, b].Position = new Vector2(a * blockSize + position.X, b * blockSize + Position.Y);
                    PieceBlocks[a, b].Rectangle = new Rectangle((int)(a * blockSize + Position.X),
                        (int)(b * blockSize + Position.Y), blockSize, blockSize);
                    PieceBlocks[a, b].SquareTextures = squares;
                    PieceBlocks[a, b].BlockColor = color;
                }

            switch (shape)
            {
                case PieceShape.I:
                    {
                        PieceBlocks[2, 1].Type = BlockTypes.Moving;
                        PieceBlocks[2, 1].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[2, 4].Type = BlockTypes.Moving;
                        PieceBlocks[2, 4].BlockColor = color;
                        break;
                    }
                case PieceShape.J:
                    {
                        PieceBlocks[2, 1].Type = BlockTypes.Moving;
                        PieceBlocks[2, 1].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[1, 3].Type = BlockTypes.Moving;
                        PieceBlocks[1, 3].BlockColor = color;
                        break;
                    }
                case PieceShape.L:
                    {
                        PieceBlocks[2, 1].Type = BlockTypes.Moving;
                        PieceBlocks[2, 1].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        PieceBlocks[3, 3].BlockColor = color;
                        break;
                    }
                case PieceShape.S:
                    {
                        PieceBlocks[1, 3].Type = BlockTypes.Moving;
                        PieceBlocks[1, 3].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        PieceBlocks[3, 2].BlockColor = color;
                        break;
                    }
                case PieceShape.Z:
                    {
                        PieceBlocks[1, 2].Type = BlockTypes.Moving;
                        PieceBlocks[1, 2].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        PieceBlocks[3, 3].BlockColor = color;
                        break;
                    }
                case PieceShape.Square:
                    {
                        PieceBlocks[1, 1].Type = BlockTypes.Moving;
                        PieceBlocks[1, 1].BlockColor = color;
                        PieceBlocks[1, 2].Type = BlockTypes.Moving;
                        PieceBlocks[1, 2].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 1].Type = BlockTypes.Moving;
                        PieceBlocks[2, 1].BlockColor = color;
                        break;
                    }
                case PieceShape.T:
                    {
                        PieceBlocks[1, 2].Type = BlockTypes.Moving;
                        PieceBlocks[1, 2].BlockColor = color;
                        PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        PieceBlocks[2, 2].BlockColor = color;
                        PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        PieceBlocks[2, 3].BlockColor = color;
                        PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        PieceBlocks[3, 2].BlockColor = color;
                        break;
                    }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                    PieceBlocks[a, b].Draw(spriteBatch);
        }

        public void ClearBlocks()
        {
            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                {
                    PieceBlocks[a, b].Type = BlockTypes.Empty;
                }
        }

        public void Rotate()
        {
            rotationCounter++;
            if(rotationCounter > 3)
                rotationCounter = 0;
            switch(Shape)
            {
                case PieceShape.I:
                    {
                        ClearBlocks();
                        if (rotationCounter % 2 == 0)
                        {
                            PieceBlocks[0, 3].Type = BlockTypes.Moving;
                            PieceBlocks[1, 3].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        }
                        else
                        {
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[2, 4].Type = BlockTypes.Moving;
                        }
                        break;
                    }
                case PieceShape.Square:
                    {
                        break;
                    }
                case PieceShape.L:
                    {
                        ClearBlocks();
                        if (rotationCounter == 0)
                        {
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 1)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 1].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 2)
                        {
                            PieceBlocks[1, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 3)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[1, 3].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        break;
                    }
                case PieceShape.J:
                    {
                        ClearBlocks();
                        if (rotationCounter == 0)
                        {
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[1, 3].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 1)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 2)
                        {
                            PieceBlocks[3, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 3)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[1, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        break;
                    }
                case PieceShape.S:
                    {
                        ClearBlocks();
                        if (rotationCounter % 2 == 0)
                        {
                            PieceBlocks[1, 3].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else
                        {
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        }
                        break;
                    }
                case PieceShape.Z:
                    {
                        ClearBlocks();
                        if (rotationCounter % 2 == 0)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 3].Type = BlockTypes.Moving;
                        }
                        else
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[1, 3].Type = BlockTypes.Moving;
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                        }
                        break;
                    }
                case PieceShape.T:
                    {
                        ClearBlocks();
                        if (rotationCounter == 0)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 1)
                        {
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 2)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[3, 2].Type = BlockTypes.Moving;
                        }
                        else if (rotationCounter == 3)
                        {
                            PieceBlocks[1, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 1].Type = BlockTypes.Moving;
                            PieceBlocks[2, 2].Type = BlockTypes.Moving;
                            PieceBlocks[2, 3].Type = BlockTypes.Moving;
                        }
                        break;
                    }
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    {
                        Position.Y += 25;
                        break;
                    }
                case Direction.Left:
                    {
                        Position.X -= 25;
                        break;
                    }
                case Direction.Right:
                    {
                        Position.X += 25;
                        break;
                    }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                {
                    PieceBlocks[a, b].Position = new Vector2(a * 25 + Position.X, b * 25 + Position.Y);
                    PieceBlocks[a, b].Rectangle = new Rectangle((int)(a * 25 + Position.X),
                        (int)(b * 25 + Position.Y), 25, 25);
                }
        }

        public void UpdateBlocks()
        {
            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                {
                    PieceBlocks[a, b].Position = new Vector2(a * 25 + Position.X, b * 25 + Position.Y);
                    PieceBlocks[a, b].Rectangle = new Rectangle((int)(a * 25 + Position.X),
                        (int)(b * 25 + Position.Y), 25, 25);
                }
        }

        public GamePiece Clone()
        {
            GamePiece newPiece = new GamePiece(this.Shape, this.Color, this.Position, 25, squaresTexture);
            newPiece.rotationCounter = this.rotationCounter;
            newPiece.PieceBlocks = new Block[ARRAY_SIZE, ARRAY_SIZE];
            for (int a = 0; a < ARRAY_SIZE; a++)
                for (int b = 0; b < ARRAY_SIZE; b++)
                    newPiece.PieceBlocks[a, b] = this.PieceBlocks[a, b].Clone();

            return newPiece;
        }
    }
}
