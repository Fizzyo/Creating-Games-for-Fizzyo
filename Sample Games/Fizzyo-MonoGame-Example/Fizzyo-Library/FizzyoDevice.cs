using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Fizzyo_Library
{
    public class FizzyoDevice : GameComponent
    {
        #region Fields

        //public 
        public bool useRecordedData = true;
        public bool loop = true;

        //protected
        protected string[] recordedData;
        private int recordedIndex = 0;
        protected StreamReader fileReader = null;
        protected string text = " ";

        InputState Input;
        float pollTimer = 0;
        int pollTimerInterval = 300;
        float pressure = 0;
        #endregion

        #region constructor/initialisation
        public FizzyoDevice (Game game): base (game)
        {
            // retrieve the Input manager
            Input = (InputState)Game.Services.GetService(
                typeof(InputState));
        }

#endregion

        #region Update
        /// <summary>
        /// Update the Fizzio Device system.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (useRecordedData)
            {
                pollTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (pollTimer > pollTimerInterval)
                {
                    pollTimer = 0;
                    PollLoggedData();
                }
            }
            base.Update(gameTime);
        }
        #endregion

        #region Disposing
        /// <summary>
        /// Clean up the component when it is disposing.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion

        #region public Functions

        /// <summary>
        /// Use recorded data from file, for example using the following code:
        ///
        /// public string recordedDataPath = @"Data/FizzyoData_3min.fiz";
        /// using (FileStream fs = new FileStream(recordedDataPath, FileMode.Open))
        /// {
        ///    using (StreamReader sr = new StreamReader(fs))
        ///    {
        ///        List<String> inputArray = new List<string>();
        ///        while (sr.Peek() >= 0)
        ///        {
        ///            inputArray.Add(sr.ReadLine());
        ///        }
        ///        fizzyo.LoadRecordedData(inputArray.ToArray());
        ///    }
        /// }
        /// </summary>
        /// <param name="input"></param>
        public void LoadRecordedData(string[] input)
        {
            recordedData = input;
        }

        /// <summary>
        /// If useRecordedData is set to true, data is supplied from the RecordedArray, else pressure data is streamed direct from the device.
        /// </summary>
        /// <returns>pressure data reported from device or log file with a range of -1 - 1.</returns>
        public float Pressure()
        {
            if (useRecordedData)
            {
                return pressure;
            }
            else
            {
                float p;
                //Check Gamepad stick
                p = Input.GetThumbStickLeft(0).X;
                //Check left arrow - decrease if pressed
                if (Input.IsKeyPressed(Keys.Left)) p--;
                //check right arrow - increase if pressed
                if (Input.IsKeyPressed(Keys.Right)) p++;

                return p;
            }
        }

        /// <summary>
        /// Is the Fizzyo device button down?
        /// </summary>
        /// <returns>bool</returns>
        public bool ButtonDown()
        {
            return Input.PlayerPressedFire(0);
        }

        /// <summary>
        /// Pull the next recorded value from the RecordedArray
        /// </summary>
        void PollLoggedData()
        {
            if (recordedData != null && recordedData.Length > 0)
            {
                text = recordedData[recordedIndex];
                recordedIndex++;
                string[] parts = text.Split(' ');
                if (parts.Length == 2 && parts[0] == "v")
                {
                    float pressure = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat) / 100.0f;
                    this.pressure = pressure;
                }
                if (loop && recordedIndex >= recordedData.Length)
                {
                    recordedIndex = 0;
                }
            }
        }
        #endregion

    }
}