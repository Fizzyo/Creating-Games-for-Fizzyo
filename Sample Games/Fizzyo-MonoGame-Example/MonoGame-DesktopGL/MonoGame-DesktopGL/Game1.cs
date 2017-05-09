using Fizzyo_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonoGame_DesktopGL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState { Loading, Running, Won, Lost }


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //fizzyo Properties
        public string recordedDataPath = @"Data/FizzyoData_3min.fiz"; // Simply Empty string to not load data, or set "useRecordedData" to false below (Files MUST be set yo "Copy Always" in Build Action properties)
        InputState inputState;
        FizzyoDevice fizzyo;
        float pressurevalue;
        float maxpressurevalue = 1;
        bool fizzyoButtonPressed;
        BreathRecogniser breathRecogniser;
        public float maxPressure = 0.4f;
        public float maxBreathLength = 3f;

        //Game Properties
        int retrievedFuelCells;
        float currentPower;
        TimeSpan startTime, roundTimer, roundTime;
        Random random;
        SpriteFont statsFont;
        GameState currentGameState = GameState.Loading;

        GameObject ground;
        Camera gameCamera;

        GameObject boundingSphere;

        FuelCarrier fuelCarrier;
        FuelCell[] fuelCells;
        Barrier[] barriers;

        Texture2D blankTexture;

        public float CurrentPower
        {
            get { return currentPower; }
            set
            {
                if (value > 0)
                {
                    currentPower = value;
                }
            }
        }
        //Don't let the player move, if they are breathing in to the device
        public bool CanMove;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 853;
            graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";

            //Initialise Input Service
            inputState = new InputState(this);
            Services.AddService(typeof(InputState), inputState);

            //Initialise fizzyo Service
            fizzyo = new FizzyoDevice(this);
            fizzyo.useRecordedData = true; // Change this value to use actual values instead of recorded data
            Services.AddService(typeof(FizzyoDevice), fizzyo);

            breathRecogniser = new BreathRecogniser(maxPressure, maxBreathLength);
            
            roundTime = GameConstants.RoundTime;
            random = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Load fizzyo recorded data (optional)
            if(recordedDataPath != string.Empty) LoadfizzyoRecordedData();

            // TODO: Add your initialization logic here
            ground = new GameObject();
            gameCamera = new Camera();
            boundingSphere = new GameObject();

            base.Initialize();
        }

        private void LoadfizzyoRecordedData()
        {
            try
            {
                using (FileStream fs = new FileStream(recordedDataPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        List<String> inputArray = new List<string>();
                        while (sr.Peek() >= 0)
                        {
                            inputArray.Add(sr.ReadLine());
                        }
                        fizzyo.LoadRecordedData(inputArray.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("could not load file " + recordedDataPath + " " + ex.ToString());
            }
            finally
            {
                Debug.WriteLine("file loaded " + recordedDataPath);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blankTexture = new Texture2D(GraphicsDevice,1,1);
            blankTexture.SetData<Color>(new Color[1] { Color.White });

            ground.Model = Content.Load<Model>("Models/ground");
            boundingSphere.Model = Content.Load<Model>("Models/sphere1uR");

            statsFont = Content.Load<SpriteFont>("Fonts/StatsFont");

            //Initialize fuel cells
            fuelCells = new FuelCell[GameConstants.NumFuelCells];
            for (int index = 0; index < fuelCells.Length; index++)
            {
                fuelCells[index] = new FuelCell();
                fuelCells[index].LoadContent(Content, "Models/fuelcell");
            }

            //Initialize barriers
            barriers = new Barrier[GameConstants.NumBarriers];
            int randomBarrier = random.Next(3);
            string barrierName = null;

            for (int index = 0; index < barriers.Length; index++)
            {

                switch (randomBarrier)
                {
                    case 0:
                        barrierName = "Models/cube10uR";
                        break;
                    case 1:
                        barrierName = "Models/cylinder10uR";
                        break;
                    case 2:
                        barrierName = "Models/pyramid10uR";
                        break;
                }
                barriers[index] = new Barrier();
                barriers[index].LoadContent(Content, barrierName);
                randomBarrier = random.Next(3);
            }
            PlaceFuelCellsAndBarriers();

            //Initialize fuel carrier
            fuelCarrier = new FuelCarrier(this);
            fuelCarrier.LoadContent(Content, "Models/fuelcarrier");
        }

        private void PlaceFuelCellsAndBarriers()
        {
            int min = GameConstants.MinDistance;
            int max = GameConstants.MaxDistance;
            Vector3 tempCenter;

            //place fuel cells
            foreach (FuelCell cell in fuelCells)
            {
                cell.Position = GenerateRandomPosition(min, max);
                tempCenter = cell.BoundingSphere.Center;
                tempCenter.X = cell.Position.X;
                tempCenter.Y = 0;
                tempCenter.Z = cell.Position.Z;
                cell.BoundingSphere =
                    new BoundingSphere(tempCenter, cell.BoundingSphere.Radius);
                cell.Retrieved = false;
            }

            //place barriers
            foreach (Barrier barrier in barriers)
            {
                barrier.Position = GenerateRandomPosition(min, max);
                tempCenter = barrier.BoundingSphere.Center;
                tempCenter.X = barrier.Position.X;
                tempCenter.Y = 0;
                tempCenter.Z = barrier.Position.Z;
                barrier.BoundingSphere = new BoundingSphere(tempCenter,
                    barrier.BoundingSphere.Radius);
            }
        }

        private Vector3 GenerateRandomPosition(int min, int max)
        {
            int xValue, zValue;
            do
            {
                xValue = random.Next(min, max);
                zValue = random.Next(min, max);
                if (random.Next(100) % 2 == 0)
                    xValue *= -1;
                if (random.Next(100) % 2 == 0)
                    zValue *= -1;

            } while (IsOccupied(xValue, zValue));

            return new Vector3(xValue, 0, zValue);
        }

        private bool IsOccupied(int xValue, int zValue)
        {
            foreach (GameObject currentObj in fuelCells)
            {
                if (((int)(MathHelper.Distance(
                    xValue, currentObj.Position.X)) < 15) &&
                    ((int)(MathHelper.Distance(
                    zValue, currentObj.Position.Z)) < 15))
                    return true;
            }

            foreach (GameObject currentObj in barriers)
            {
                if (((int)(MathHelper.Distance(
                    xValue, currentObj.Position.X)) < 15) &&
                    ((int)(MathHelper.Distance(
                    zValue, currentObj.Position.Z)) < 15))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            fizzyo.Update(gameTime);
            inputState.Update(gameTime);
            PlayerIndex playerIndex = PlayerIndex.One;
            // Allows the game to exit
            if (inputState.IsNewButtonPress(Buttons.Back, PlayerIndex.One, out playerIndex) || inputState.IsNewKeyPress(Keys.Escape))
                Exit();

            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            if (currentGameState == GameState.Loading)
            {
                if (inputState.IsNewKeyPress(Keys.Enter) ||
                    inputState.IsNewButtonPress(Buttons.Start, PlayerIndex.One, out playerIndex))
                {
                    roundTimer = roundTime;
                    currentGameState = GameState.Running;
                }
            }

            if ((currentGameState == GameState.Running))
            {
                fuelCarrier.Update(barriers);
                gameCamera.Update(fuelCarrier.ForwardDirection,
                    fuelCarrier.Position, aspectRatio);
                retrievedFuelCells = 0;
                foreach (FuelCell fuelCell in fuelCells)
                {
                    fuelCell.Update(fuelCarrier.BoundingSphere);
                    if (fuelCell.Retrieved)
                    {
                        retrievedFuelCells++;
                    }
                }
                if (retrievedFuelCells == GameConstants.NumFuelCells)
                {
                    currentGameState = GameState.Won;
                }
                CanMove = !breathRecogniser.IsExhaling;
                //Timer only counts down when the player has finished a breath
                if (CanMove)
                {
                    roundTimer -= gameTime.ElapsedGameTime;
                }

                if ((roundTimer < TimeSpan.Zero) &&
                    (retrievedFuelCells != GameConstants.NumFuelCells))
                {
                    currentGameState = GameState.Lost;
                }
            }

            if ((currentGameState == GameState.Won) ||
                (currentGameState == GameState.Lost))
            {
                // Reset the world for a new game
                if (inputState.IsNewKeyPress(Keys.Enter) ||
                    inputState.IsNewButtonPress(Buttons.Start, PlayerIndex.One, out playerIndex))
                    ResetGame(gameTime, aspectRatio);
            }

            HandleInput(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void ResetGame(GameTime gameTime, float aspectRatio)
        {
            fuelCarrier.Reset();
            gameCamera.Update(fuelCarrier.ForwardDirection,
                fuelCarrier.Position, aspectRatio);
            InitializeGameField();

            retrievedFuelCells = 0;
            startTime = gameTime.TotalGameTime;
            roundTimer = roundTime;
            currentGameState = GameState.Running;
        }

        private void InitializeGameField()
        {
            //Initialize barriers
            barriers = new Barrier[GameConstants.NumBarriers];
            int randomBarrier = random.Next(3);
            string barrierName = null;

            for (int index = 0; index < GameConstants.NumBarriers; index++)
            {
                switch (randomBarrier)
                {
                    case 0:
                        barrierName = "Models/cube10uR";
                        break;
                    case 1:
                        barrierName = "Models/cylinder10uR";
                        break;
                    case 2:
                        barrierName = "Models/pyramid10uR";
                        break;
                }
                barriers[index] = new Barrier();
                barriers[index].LoadContent(Content, barrierName);
                randomBarrier = random.Next(3);
            }
            PlaceFuelCellsAndBarriers();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            switch (currentGameState)
            {
                case GameState.Loading:
                    DrawSplashScreen();
                    break;
                case GameState.Running:
                    DrawGameplayScreen();
                    break;
                case GameState.Won:
                    DrawWinOrLossScreen(GameConstants.StrGameWon);
                    break;
                case GameState.Lost:
                    DrawWinOrLossScreen(GameConstants.StrGameLost);
                    break;
            };

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the game terrain, a simple blue grid.
        /// </summary>
        /// <param name="model">Model representing the game playing field.</param>
        private void DrawTerrain(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.Identity;

                    // Use the matrices provided by the game camera
                    effect.View = gameCamera.ViewMatrix;
                    effect.Projection = gameCamera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        private void DrawSplashScreen()
        {
            float xOffsetText, yOffsetText;
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            Vector2 strCenter;

            graphics.GraphicsDevice.Clear(Color.SteelBlue);

            xOffsetText = yOffsetText = 0;
            Vector2 strInstructionsSize =
                statsFont.MeasureString(GameConstants.StrInstructions1);
            Vector2 strPosition;
            strCenter = new Vector2(strInstructionsSize.X / 2,
                strInstructionsSize.Y / 2);

            yOffsetText = (viewportSize.Y / 2 - strCenter.Y);
            xOffsetText = (viewportSize.X / 2 - strCenter.X);
            strPosition = new Vector2((int)xOffsetText, (int)yOffsetText);

            spriteBatch.Begin();
            spriteBatch.DrawString(statsFont, GameConstants.StrInstructions1,
                strPosition, Color.White);

            strInstructionsSize = statsFont.MeasureString(GameConstants.StrInstructions2);
            strCenter = new Vector2(strInstructionsSize.X / 2,
                strInstructionsSize.Y / 2);
            yOffsetText = 30 +
                (viewportSize.Y / 2 - strCenter.Y) + statsFont.LineSpacing;
            xOffsetText = (viewportSize.X / 2 - strCenter.X);
            strPosition = new Vector2((int)xOffsetText, (int)yOffsetText);

            spriteBatch.DrawString(statsFont, GameConstants.StrInstructions2,
                strPosition, Color.LightGray);

            strInstructionsSize = statsFont.MeasureString(GameConstants.StrInstructions3);
            strCenter = new Vector2(strInstructionsSize.X / 2,
                strInstructionsSize.Y / 2);
            yOffsetText = 70 +
                (viewportSize.Y / 2 - strCenter.Y) + statsFont.LineSpacing;
            xOffsetText = (viewportSize.X / 2 - strCenter.X);
            strPosition = new Vector2((int)xOffsetText, (int)yOffsetText);

            spriteBatch.DrawString(statsFont, GameConstants.StrInstructions3,
                strPosition, Color.LightGray);

            spriteBatch.End();

            //re-enable depth buffer after sprite batch disablement

            //GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;

            //GraphicsDevice.RenderState.AlphaBlendEnable = false;
            //GraphicsDevice.RenderState.AlphaTestEnable = false;

            //GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }

        private void DrawWinOrLossScreen(string gameResult)
        {
            float xOffsetText, yOffsetText;
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            Vector2 strCenter;

            xOffsetText = yOffsetText = 0;
            Vector2 strResult = statsFont.MeasureString(gameResult);
            Vector2 strPlayAgainSize =
                statsFont.MeasureString(GameConstants.StrPlayAgain);
            Vector2 strPosition;
            strCenter = new Vector2(strResult.X / 2, strResult.Y / 2);

            yOffsetText = (viewportSize.Y / 2 - strCenter.Y);
            xOffsetText = (viewportSize.X / 2 - strCenter.X);
            strPosition = new Vector2((int)xOffsetText, (int)yOffsetText);

            spriteBatch.Begin();
            spriteBatch.DrawString(statsFont, gameResult,
                strPosition, Color.Red);

            strCenter =
                new Vector2(strPlayAgainSize.X / 2, strPlayAgainSize.Y / 2);
            yOffsetText = (viewportSize.Y / 2 - strCenter.Y) +
                (float)statsFont.LineSpacing;
            xOffsetText = (viewportSize.X / 2 - strCenter.X);
            strPosition = new Vector2((int)xOffsetText, (int)yOffsetText);
            spriteBatch.DrawString(statsFont, GameConstants.StrPlayAgain,
                strPosition, Color.AntiqueWhite);

            spriteBatch.End();

            //re-enable depth buffer after sprite batch disablement

            //GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;

            //GraphicsDevice.RenderState.AlphaBlendEnable = false;
            //GraphicsDevice.RenderState.AlphaTestEnable = false;

            //GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }

        private void DrawGameplayScreen()
        {
            DrawTerrain(ground.Model);
            foreach (FuelCell fuelCell in fuelCells)
            {
                if (!fuelCell.Retrieved)
                {
                    fuelCell.Draw(gameCamera.ViewMatrix,
                        gameCamera.ProjectionMatrix);
                }
            }
            foreach (Barrier barrier in barriers)
            {
                barrier.Draw(gameCamera.ViewMatrix,
                    gameCamera.ProjectionMatrix);
            }

            fuelCarrier.Draw(gameCamera.ViewMatrix,
                gameCamera.ProjectionMatrix);
            DrawStats();
        }

        private void DrawStats()
        {
            float xOffsetText, yOffsetText;
            string str1 = GameConstants.StrTimeRemaining;
            string str2 =
                GameConstants.StrCellsFound + retrievedFuelCells.ToString() +
                " of " + GameConstants.NumFuelCells.ToString();
            Rectangle rectSafeArea;
            Rectangle pressureRectBar;
            Rectangle pressureBar;

            float destHeight = GameConstants.MaxPower * Math.Min((pressurevalue / maxpressurevalue), 1);

            var newPower = CurrentPower + ((destHeight - currentPower) * GameConstants.Smoothing);

            if (newPower > CurrentPower)
            {
                CurrentPower = newPower;
            }

            str1 += (roundTimer.Seconds).ToString();

            //Calculate str1 position
            rectSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            //Calculate pressure rect
            pressureRectBar = new Rectangle(rectSafeArea.Width - 50,10,40,110);
            pressureBar = new Rectangle(pressureRectBar.X + 5, 110 - (int)(currentPower), 30, (int)(currentPower));

            xOffsetText = rectSafeArea.X;
            yOffsetText = rectSafeArea.Y;

            Vector2 strSize = statsFont.MeasureString(str1);
            Vector2 strPosition =
                new Vector2((int)xOffsetText + 10, (int)yOffsetText);

            spriteBatch.Begin();
            spriteBatch.DrawString(statsFont, str1, strPosition, Color.White);
            strPosition.Y += strSize.Y;
            spriteBatch.DrawString(statsFont, str2, strPosition, Color.White);
            strPosition.Y += strSize.Y;
            spriteBatch.DrawString(statsFont, "Output Pressure is: [" + pressurevalue + "]", strPosition, Color.White);
            strPosition.Y += strSize.Y;
            spriteBatch.DrawString(statsFont, "CanMove [" + CanMove + "]", strPosition, Color.White);

            spriteBatch.Draw(blankTexture, pressureRectBar, Color.White);
            spriteBatch.Draw(blankTexture, pressureBar, Color.Green);

            spriteBatch.End();

            //re-enable depth buffer after sprite batch disablement

            //GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;

            //GraphicsDevice.RenderState.AlphaBlendEnable = false;
            //GraphicsDevice.RenderState.AlphaTestEnable = false;

            //GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }

        void HandleInput(GameTime gameTime)
        {
            fizzyoButtonPressed = false;
            if (fizzyo.ButtonDown())
            {
                //Do Something with the Button
                fizzyoButtonPressed = true;
            }
            var pressureReading = fizzyo.Pressure(); // Get the current Pressure value
            breathRecogniser.AddSample(gameTime.ElapsedGameTime.Seconds, pressureReading);
            pressurevalue = pressureReading;
        }

        #region Disposing
        /// <summary>
        /// Clean up the component when it is disposing.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}
