using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mattris
{
    class Board
    {
        // The game board is just an array of blocks 10 x 20
        const int UNITS_H = 20;
        const int UNITS_W = 10;
        public Block[,] BlockGrid;

        private MattrisGame game;

        // The x,y location of the top-left of the board
        public Vector2 Position;

        private Texture2D squareTextures;

        public GamePiece ActivePiece;
        public GamePiece NextActivePiece;

        public int Level;
        public int Score;
        private int fallTimer;
        private bool slamming;
        private int numClears;

        public Board(Vector2 position, int blockSize, Texture2D squares, MattrisGame game)
        {
            BlockGrid = new Block[UNITS_W, UNITS_H];
            squareTextures = squares;
            slamming = false;
            Position = position;
            numClears++;
            Level = 1;
            Score = 0;
            this.game = game;

            for (int a = 0; a < UNITS_W; a++)
            {
                for (int b = 0; b < UNITS_H; b++)
                {
                    BlockGrid[a, b] = new Block();
                    BlockGrid[a, b].Width = blockSize;
                    BlockGrid[a, b].Height = blockSize;
                    BlockGrid[a, b].Type = BlockTypes.Empty;
                    BlockGrid[a, b].Rectangle = new Rectangle((int)(a * blockSize + Position.X),
                        (int)(b * blockSize + Position.Y), blockSize, blockSize);
                    BlockGrid[a, b].Position = new Vector2(a * blockSize + Position.X, b * blockSize + Position.Y);
                    BlockGrid[a, b].SquareTextures = squares;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            fallTimer += gameTime.ElapsedGameTime.Milliseconds;
            int seconds = 0;
            if (slamming)
                seconds = 20;
            else
                seconds = 1000 - (Level * 100);
            if (fallTimer > seconds)
            {
                if (ActivePieceCanMove(Direction.Down))
                    ActivePiece.Move(Direction.Down);
                else
                    AddGamePieceToBoard();
                fallTimer = 0;
            }
            ActivePiece.Update(gameTime);
            CheckForCompleteLines();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int a = 0; a < UNITS_W; a++)
            {
                for (int b = 0; b < UNITS_H; b++)
                {
                    BlockGrid[a, b].Draw(spriteBatch);
                }
            }

            ActivePiece.Draw(spriteBatch);
            NextActivePiece.Draw(spriteBatch);
        }

        // Sets all the elements in the array to an empty block type
        public void Clear()
        {
            for (int a = 0; a < UNITS_W; a++)
            {
                for (int b = 0; b < UNITS_H; b++)
                {
                    BlockGrid[a, b].Type = BlockTypes.Empty;
                }
            }
        }

        public void NewActivePiece()
        {
            if (NextActivePiece == null)
            {
                Random rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Minute);
                int pieceType = rand.Next(0, 7);
                int color = rand.Next(0, 5);
                Vector2 startPosition = BlockGrid[3, 0].Position;
                ActivePiece = new GamePiece((PieceShape)pieceType, (BlockColors)color, startPosition, 25, squareTextures);

                pieceType = rand.Next(0, 7);
                color = rand.Next(0, 5);
                Vector2 displayPosition = new Vector2(275, -5);
                NextActivePiece = new GamePiece((PieceShape)pieceType, (BlockColors)color, displayPosition, 25, squareTextures);
            }
            else
            {
                ActivePiece = NextActivePiece;
                ActivePiece.Position = BlockGrid[3, 0].Position;
                ActivePiece.UpdateBlocks();
                Random rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Minute);
                int pieceType = rand.Next(0, 7);
                int color = rand.Next(0, 5);
                Vector2 displayPosition = new Vector2(275, -5);
                NextActivePiece = new GamePiece((PieceShape)pieceType, (BlockColors)color, displayPosition, 25, squareTextures);
            }

            // check if active piece has 1 or 2 spaces at the top rows - move up to compensate if necessary
            int highestBlock = 4;
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 5; b++)
                {
                    if (ActivePiece.PieceBlocks[a, b].Type != BlockTypes.Empty &&
                        b < highestBlock)
                        highestBlock = b;
                }
            }
            ActivePiece.Position -= new Vector2(0, (highestBlock) * 25);
            ActivePiece.UpdateBlocks();
        }

        public bool ActivePieceCanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    {
                        for (int a = 0; a < 5; a++)
                            for (int b = 0; b < 5; b++)
                            {
                                if (ActivePiece.PieceBlocks[a, b].Type != BlockTypes.Empty)
                                {
                                    if (IsBoardBlockFilled(ActivePiece.PieceBlocks[a, b].Position - new Vector2(25, 0)))
                                        return false;
                                }
                            }
                        break;
                    }
                case Direction.Right:
                    {
                        for (int a = 0; a < 5; a++)
                            for (int b = 0; b < 5; b++)
                            {
                                if (ActivePiece.PieceBlocks[a, b].Type != BlockTypes.Empty)
                                {
                                    if (IsBoardBlockFilled(ActivePiece.PieceBlocks[a, b].Position + new Vector2(25, 0)))
                                        return false;
                                }
                            }
                        break;
                    }
                case Direction.Down:
                    {
                        for (int a = 0; a < 5; a++)
                            for (int b = 0; b < 5; b++)
                            {
                                if (ActivePiece.PieceBlocks[a, b].Type != BlockTypes.Empty)
                                {
                                    if (IsBoardBlockFilled(ActivePiece.PieceBlocks[a, b].Position + new Vector2(0, 25)))
                                        return false;
                                }
                            }
                        break;
                    }
            }

            return true;
        }

        public bool ActivePieceCanRotate()
        {
            GamePiece newPiece = ActivePiece.Clone();
            newPiece.Rotate();

            for (int a = 0; a < 5; a++)
                for (int b = 0; b < 5; b++)
                {
                    if (newPiece.PieceBlocks[a, b].Type != BlockTypes.Empty)
                    {
                        if (IsBoardBlockFilled(newPiece.PieceBlocks[a, b].Position))
                            return false;
                    }
                }
            return true;
        }

        protected bool IsBoardBlockFilled(Vector2 position)
        {
            int xIndex, yIndex;

            xIndex = (int)((position.X - Position.X) / 25);
            yIndex = (int)((position.Y - Position.Y) / 25);

            if (xIndex < 0 || yIndex < 0 || xIndex > UNITS_W - 1 || yIndex > UNITS_H - 1 ||
                BlockGrid[xIndex, yIndex].Type != BlockTypes.Empty)
                return true;

            return false;
        }

        public void AddGamePieceToBoard()
        {
            for (int a = 0; a < 5; a++)
                for (int b = 0; b < 5; b++)
                {
                    if (ActivePiece.PieceBlocks[a, b].Type != BlockTypes.Empty)
                        SetBoardBlock(ActivePiece.PieceBlocks[a, b].Position, ActivePiece.PieceBlocks[a, b].BlockColor);
                }

            if (ActivePiece.Position.Y <= BlockGrid[3, 0].Position.Y)
            {
                game.GameOver();
            }
            else
            {
                NewActivePiece();
                slamming = false;
            }
        }

        protected void SetBoardBlock(Vector2 position, BlockColors color)
        {
            int xIndex, yIndex;

            xIndex = (int)((position.X - Position.X) / 25);
            yIndex = (int)((position.Y - Position.Y) / 25);

            if (xIndex < UNITS_W && yIndex < UNITS_H)
            {
                BlockGrid[xIndex, yIndex].BlockColor = color;
                BlockGrid[xIndex, yIndex].Type = BlockTypes.Static;
            }
        }

        public void SlamDown()
        {
            slamming = true;
        }

        public void ResetFallTimer()
        {
            fallTimer = 0;
        }

        protected void CheckForCompleteLines()
        {
            bool fullLine;
            int totalLines = 0;

            for (int a = 0; a < UNITS_H; a++)
            {
                fullLine = true;
                for (int b = 0; b < UNITS_W; b++)
                {
                    if (BlockGrid[b, a].Type == BlockTypes.Empty)
                    {
                        fullLine = false;
                        break;
                    }
                }
                if (fullLine)
                {
                    RemoveLine(a);
                    totalLines++;
                    numClears++;
                }
            }
            AddScore(totalLines);
        }

        protected void RemoveLine(int lineIndex)
        {
            for (int a = 0; a < UNITS_W; a++)
            {
                BlockGrid[a, lineIndex].Type = BlockTypes.Empty;
            }
            MoveLinesDown(lineIndex);
        }

        protected void MoveLinesDown(int lineIndex)
        {
            if(numClears % 5 == 0)
                Level++;
            for (int b = lineIndex; b >= 0; b--)
                for (int a = 0; a < UNITS_W; a++)
                {
                    if (b == 0)
                    {
                        BlockGrid[a, b].Type = BlockTypes.Empty;
                    }
                    else
                    {
                        BlockGrid[a, b].BlockColor = BlockGrid[a, b - 1].BlockColor;
                        BlockGrid[a, b].SquareTextures = BlockGrid[a, b - 1].SquareTextures;
                        BlockGrid[a, b].Type = BlockGrid[a, b - 1].Type;
                    }
                }
        }

        protected void AddScore(int totalLines)
        {
            int baseScore = 0;
            if(totalLines == 1)
                baseScore = 40;
            else if (totalLines ==2 )
                baseScore = 100;
            else if (totalLines == 3)
                baseScore = 300;
            else if(totalLines == 4)
                baseScore = 1200;

            Score += Level * baseScore;
        }

    }
}
