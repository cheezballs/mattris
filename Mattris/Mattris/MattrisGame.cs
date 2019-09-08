using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;

/***********************
 * TO DO
 * 
 * sounds
 * animate line removing
 * 
 * *********************/

namespace Mattris
{
    public class MattrisGame : Microsoft.Xna.Framework.Game
    {
        const int BLOCK_SIZE = 25;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Board gameBoard;
        GameState gameState;

        Texture2D[] menuScreenGraphics;
        Texture2D squares, interfaceGraphics, pauseBackground, pauseText;
        Sprite[] menuSprites;
        SpriteFont menuFont, scoreFont;
        KeyboardState oldKeyState;

        int fontFlashTimer;

        public MattrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Mattris";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 500;

            graphics.ApplyChanges();

            gameState = GameState.FirstLoaded;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuScreenGraphics = new Texture2D[5];
            menuScreenGraphics[0] = Content.Load<Texture2D>("Images/GiantIBlock");
            menuScreenGraphics[1] = Content.Load<Texture2D>("Images/GiantJBlock");
            menuScreenGraphics[2] = Content.Load<Texture2D>("Images/GiantLBlock");
            menuScreenGraphics[3] = Content.Load<Texture2D>("Images/GiantSBlock");
            menuScreenGraphics[4] = Content.Load<Texture2D>("Images/GiantSquareBlock");

            menuFont = Content.Load<SpriteFont>("MenuFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            squares = Content.Load<Texture2D>("Images/Squares");
            interfaceGraphics = Content.Load<Texture2D>("Images/UI");

            pauseBackground = Content.Load<Texture2D>("Images/PauseBox");
            pauseText = Content.Load<Texture2D>("Images/PausedText");

            SetUpMenu();
        }

        protected override void UnloadContent()
        {
            menuScreenGraphics[0].Dispose();
            menuScreenGraphics[1].Dispose();
            menuScreenGraphics[2].Dispose();
            menuScreenGraphics[3].Dispose();
            menuScreenGraphics[4].Dispose();
            squares.Dispose();
            interfaceGraphics.Dispose();
            pauseBackground.Dispose();
            pauseText.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameState == GameState.FirstLoaded)
                ProcessMenuUpdate(gameTime);
            else if (gameState == GameState.InProgress)
                ProcessGameUpdate(gameTime);
            else if (gameState == GameState.Paused)
                ProcessPause(gameTime);
            else if (gameState == GameState.GameOver)
                ProcessGameOver(gameTime);
            GetKeyInput(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameState == GameState.FirstLoaded)
                DrawMenuScreen(gameTime);
            else if (gameState == GameState.InProgress)
                DrawGameFrame(gameTime);
            else if (gameState == GameState.Paused)
                DrawPauseScreen(gameTime);
            else if (gameState == GameState.GameOver)
                DrawGameOverScreen(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void SetUpMenu()
        {
            menuSprites = new Sprite[9];
            menuSprites[0] = new Sprite(this.graphics, menuScreenGraphics[0], new Vector2(100, 100), new Vector2(.5f, .5f), .01f, 1.5f);
            menuSprites[1] = new Sprite(this.graphics, menuScreenGraphics[1], new Vector2(75, 120), new Vector2(.3f, -.5f), .05f, 1.5f);
            menuSprites[2] = new Sprite(this.graphics, menuScreenGraphics[2], new Vector2(300, 19), new Vector2(-.5f, .2f), .09f, 2f);
            menuSprites[3] = new Sprite(this.graphics, menuScreenGraphics[3], new Vector2(200, 300), new Vector2(-.7f, -.5f), .12f, 2.5f);
            menuSprites[4] = new Sprite(this.graphics, menuScreenGraphics[4], new Vector2(5, 450), new Vector2(-.5f, -.2f), .10f, 3f);
            menuSprites[5] = new Sprite(this.graphics, menuScreenGraphics[3], new Vector2(100, 450), new Vector2(-.9f, -.5f), .03f, 3f);
            menuSprites[6] = new Sprite(this.graphics, menuScreenGraphics[4], new Vector2(200, 200), new Vector2(.5f, -.5f), .05f, 1.5f);
            menuSprites[7] = new Sprite(this.graphics, menuScreenGraphics[2], new Vector2(10, 200), new Vector2(-.9f, -.5f), .07f, 1.8f);
            menuSprites[8] = new Sprite(this.graphics, menuScreenGraphics[1], new Vector2(5, 300), new Vector2(.3f, -.7f), .09f, 2.2f);

            fontFlashTimer = 0;
        }

        protected void NewGame()
        {
            gameState = GameState.InProgress;
            gameBoard = new Board(new Vector2(15, 15), BLOCK_SIZE, squares, this);
            gameBoard.NewActivePiece();
        }

        protected void TogglePause()
        {
            if (gameState == GameState.InProgress)
                gameState = GameState.Paused;
            else
                gameState = GameState.InProgress;
        }

        public void GameOver()
        {
            gameState = GameState.GameOver;
        }

        #region Drawing Methods

        protected void DrawMenuScreen(GameTime gameTime)
        {
            Color fontFlashColor = Color.White;
            for (int a = 0; a < 9; a++)
                menuSprites[a].Draw(spriteBatch);

            if (fontFlashTimer < 50)
                fontFlashColor = Color.White;
            else if (fontFlashTimer < 100)
                fontFlashColor = Color.Yellow;
            else
                fontFlashTimer = 0;

            spriteBatch.DrawString(menuFont,
                "Press Enter!!!",
                new Vector2(graphics.PreferredBackBufferWidth / 2 - 120, graphics.PreferredBackBufferHeight / 2 - 30),
                fontFlashColor);
        }

        protected void DrawGameFrame(GameTime gameTime)
        {
            spriteBatch.Draw(interfaceGraphics, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(menuFont, gameBoard.Score.ToString(), new Vector2(300, 197), Color.White);
            gameBoard.Draw(spriteBatch);
        }

        protected void DrawPauseScreen(GameTime gameTime)
        {
            DrawGameFrame(gameTime);
            spriteBatch.Draw(pauseBackground,
                new Rectangle(50, 150, pauseBackground.Width, pauseBackground.Height),
                Color.White);
            spriteBatch.Draw(pauseText,
                new Rectangle(50 + ((pauseBackground.Width - pauseText.Width) / 2),
                    150 + ((pauseBackground.Height - pauseText.Height) / 2),
                    pauseText.Width, pauseText.Height),
                Color.White);
        }

        protected void DrawGameOverScreen(GameTime gameTime)
        {
            DrawGameFrame(gameTime);
            spriteBatch.Draw(pauseBackground,
                new Rectangle(50, 150, pauseBackground.Width, pauseBackground.Height),
                Color.White);
            spriteBatch.DrawString(menuFont,
                "Game Over",
                new Vector2(50 + ((pauseBackground.Width - menuFont.MeasureString("Game Over").X) / 2),
                    120 + ((pauseBackground.Height - menuFont.MeasureString("Game Over").Y) / 2)), Color.White);
            spriteBatch.DrawString(scoreFont,
                "Score: " + gameBoard.Score,
                new Vector2(50 + ((pauseBackground.Width - scoreFont.MeasureString("Score: " + gameBoard.Score).X) / 2),
                    170 + ((pauseBackground.Height - scoreFont.MeasureString("Score: " + gameBoard.Score).Y) / 2)),
                    Color.White);
        }

        #endregion

        #region Update Methods

        protected void ProcessMenuUpdate(GameTime gameTime)
        {
            fontFlashTimer += gameTime.ElapsedGameTime.Milliseconds;

            for (int a = 0; a < 9; a++)
                menuSprites[a].Update(gameTime);
        }

        protected void ProcessGameUpdate(GameTime gameTime)
        {
            gameBoard.Update(gameTime);
        }

        protected void ProcessPause(GameTime gameTime)
        {

        }
        
        protected void ProcessGameOver(GameTime gameTime)
        {

        }

        #endregion

        #region Input methods

        protected void GetKeyInput(GameTime gameTime)
        {
            KeyboardState currKeyState = Keyboard.GetState();

            if (gameState == GameState.FirstLoaded)
            {
                if (currKeyState.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.InProgress;
                    NewGame();
                }
            }
            else if (gameState == GameState.InProgress)
            {
                if (currKeyState.IsKeyDown(Keys.Down) && !oldKeyState.IsKeyDown(Keys.Down))
                    if (gameBoard.ActivePieceCanMove(Direction.Down))
                    {
                        gameBoard.ActivePiece.Move(Direction.Down);
                        gameBoard.ResetFallTimer();
                    }
                    else
                        gameBoard.AddGamePieceToBoard();

                else if (currKeyState.IsKeyDown(Keys.Left) && !oldKeyState.IsKeyDown(Keys.Left))
                {
                    if (gameBoard.ActivePieceCanMove(Direction.Left))
                        gameBoard.ActivePiece.Move(Direction.Left);
                }
                else if (currKeyState.IsKeyDown(Keys.Right) && !oldKeyState.IsKeyDown(Keys.Right))
                {
                    if (gameBoard.ActivePieceCanMove(Direction.Right))
                        gameBoard.ActivePiece.Move(Direction.Right);
                }
                else if (currKeyState.IsKeyDown(Keys.Up) && !oldKeyState.IsKeyDown(Keys.Up))
                {
                    if (gameBoard.ActivePieceCanRotate())
                        gameBoard.ActivePiece.Rotate();
                }
                else if (currKeyState.IsKeyDown(Keys.N) && !oldKeyState.IsKeyDown(Keys.N))
                    NewGame();
                else if (currKeyState.IsKeyDown(Keys.P) && !oldKeyState.IsKeyDown(Keys.P))
                    TogglePause();
                else if (currKeyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space))
                    gameBoard.SlamDown();
            }
            else if (gameState == GameState.GameOver)
            {
                if (currKeyState.IsKeyDown(Keys.N) && !oldKeyState.IsKeyDown(Keys.N))
                    NewGame();
            }
            else if (gameState == GameState.Paused)
            {
                if (currKeyState.IsKeyDown(Keys.P) && !oldKeyState.IsKeyDown(Keys.P))
                    TogglePause();
            }
            oldKeyState = currKeyState;
        }

        #endregion
    }
}
