using System;
namespace Fizzyo
{
    #region Exhalation Event
    /// <summary>
    /// Provides data about the current breath to the receiver when the ExhalationComplete event fires.
    /// </summary>
    public class ExhalationCompleteEventArgs : EventArgs
    {
        private float breathLength = 0;
        private int breathCount = 0;
        private float exhaledVolume = 0;
        private bool isBreathGood = false;

        public ExhalationCompleteEventArgs(float breathLength, int breathCount, float exhaledVolume, bool isBreathGood)
        {
            this.breathLength = breathLength;
            this.breathCount = breathCount;
            this.exhaledVolume = exhaledVolume;
            this.isBreathGood = isBreathGood;
        }

        /// The length of the exhaled breath in seconds
        public float Breathlength
        {
            get
            {
                return breathLength;
            }
        }

        /// The total number of exhaled breaths this session
        public int BreathCount
        {
            get
            {
                return breathCount;
            }
        }

        /// The total exhaled volume of this breath
        public float ExhaledVolume
        {
            get
            {
                return exhaledVolume;
            }
        }

        /// Returns true if the breath was within the toterance of a 'good breath'
        public bool IsBreathGood
        {
            get
            {
                return isBreathGood;
            }
        }
    }

    public delegate void ExhalationCompleteEventHandler(object sender, ExhalationCompleteEventArgs e);
    #endregion

    /// <summary>
    /// Breath Analyser class decouples the logic of recognizing breaths from a stream of pressure samples
    /// from acting on the recognition.  To use:
    /// 
    /// 1. Create an instance of BreathAnalyser: BreathAnalyser breathAnalyser = new BreathAnalyser()
    /// 2. Set the calibration properties: MaxPressure and MaxBreathLength
    /// 3. Register for the ExhalationCompleteEvent: breathAnalyser.ExhalationComplete += ExhalationCompleteHandler
    /// 4. Add pressure samples in the update loop: AddSample(Time.DeltaTime, pressure)
    /// 5. The event will fire at the end of an exhaled breath and provide information for:
    /// 
    ///    a) BreathLength
    ///    b) BreathCount
    ///    c) ExhaledVolume
    ///    d) IsBreathGood
    /// 
    /// 6. You can interrogate the breath analyser at any time to determine:
    /// 
    ///    a) BreathLength
    ///    b) BreathCount
    ///    c) ExhaledVolume
    ///    d) IsExhaling
    ///    e) MaxPressure
    ///    f) MaxBreathLength
    /// 
    /// The algorithm for determining whether a breath is good or not is encapsulated in the method IsBreathGood()
    /// and currently returns true if the average breath pressure and breath length is within 80% of the max.
    /// </summary>
    public class BreathRecogniser
    {
        private float breathLength = 0;
        private int breathCount = 0;
        private float exhaledVolume = 0;
        private bool isExhaling = false;
        private float maxPressure = 0;
        private float maxBreathLength = 0;
        private const float kTollerance = 0.80f;
        private float minBreathThreshold = .05f;

        public event ExhalationCompleteEventHandler ExhalationComplete;

        public BreathRecogniser(float MaxPressure, float MaxBreathLength)
        {
            maxPressure = MaxPressure;
            maxBreathLength = MaxBreathLength;
        }

        /// The length of the current exhaled breath in seconds
        public float Breathlength
        {
            get
            {
                return this.breathLength;
            }
        }

        /// The total number of exhaled breaths this session
        public int BreathCount
        {
            get
            {
                return this.breathCount;
            }
        }

        /// The total exhaled volume for this breath
        public float ExhaledVolume
        {
            get
            {
                return this.exhaledVolume;
            }
        }

        /// True if the user is exhaling
        public bool IsExhaling
        {
            get
            {
                return this.isExhaling;
            }
        }

        /// The maximum pressure recorded during calibration
        public float MaxPressure
        {
            get
            {
                return this.maxPressure;
            }
            set
            {
                this.maxPressure = value;
            }
        }

        /// The maximum breath length recorded during calibration
        public float MaxBreathLength
        {
            get
            {
                return this.maxBreathLength;
            }
            set
            {
                this.maxBreathLength = value;
            }
        }

        /// Adds a sample to the BreathAnalyser
        public void AddSample(float dt, float value)
        {
            if (this.isExhaling && value < this.minBreathThreshold)
            {
                // Notify the delegate that the exhaled breath is complete
                bool isBreathGood = this.IsBreathGood(this.breathLength, this.maxBreathLength, this.exhaledVolume, this.maxPressure);
                ExhalationCompleteEventArgs eventArgs = new ExhalationCompleteEventArgs(
                    this.breathLength,
                    this.breathCount,
                    this.exhaledVolume,
                    isBreathGood);
                this.OnExhalationComplete(this, eventArgs);

                // Reset the state
                this.breathLength = 0;
                this.exhaledVolume = 0;
                this.isExhaling = false;
                this.breathCount++;
            }
            else if (value >= this.minBreathThreshold)
            {
                this.isExhaling = true;
                this.exhaledVolume += dt * value;
                this.breathLength += dt;
            }
        }

        /// Returns true if the breath was within the toterance of a 'good breath'
        public bool IsBreathGood(float breathLength, float maxBreathLength, float exhaledVolume, float maxPressure)
        {
            bool isBreathGood = false;

            // Is the breath the right within 80% of the correct length
            isBreathGood = breathLength > BreathRecogniser.kTollerance * maxBreathLength;

            // Is the average pressure within 80% of the max pressure
            if (this.breathLength > 0)
            {
                isBreathGood = isBreathGood && ((exhaledVolume / breathLength) > BreathRecogniser.kTollerance * maxPressure);
            }

            return isBreathGood;
        }

        /// Resest the BreathAnalyser
        public void ResetSession()
        {
            this.breathLength = 0;
            this.breathCount = 0;
            this.exhaledVolume = 0;
            this.isExhaling = false;
        }

        /// Invoke the event - called whenever exhalation finishes
        protected virtual void OnExhalationComplete(object sender, ExhalationCompleteEventArgs e)
        {
            if (ExhalationComplete != null)
            {
                ExhalationComplete(this, e);
            }
        }
    }
}