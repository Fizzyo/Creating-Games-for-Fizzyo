using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Fizzyo
{
    public class FizzyoDevice : MonoBehaviour
    {
        //public 
        public bool useRecordedData = true;
        public bool loop = true;
        public string recordedDataPath = "Data/FizzyoData_3min.fiz";
        private string[] recordedData;
        private int recordedIndex = 0;

        //Singleton
        private static FizzyoDevice instance;
        private static object threadLock = new System.Object();

        //protected
        protected string text = " "; // assigned to allow first line to be read below
        float pollTimer = 0;
        float pollTimerInterval = 0.3f;
        float pressure = 0;

        //Singleton instance of the device - There can be only one!
        public static FizzyoDevice Instance()
        {
            if (instance == null)
            {
                lock (threadLock)
                {
                    if (instance == null)
                    {
                        instance = GameObject.FindObjectOfType<FizzyoDevice>();
                    }

                    if (instance == null)
                    {
                        instance = (new GameObject("FizzyoDevice")).AddComponent<FizzyoDevice>();
                    }

                }
            }
            return instance;
        }


        // Load the recorded data on start if used
        void Start()
        {
            if (useRecordedData)
            {
                //Read the recorded data, if available
                try
                {
                    using (StreamReader fileReader = new StreamReader(Application.dataPath + "/" + recordedDataPath))
                    {
                        List<String> inputArray = new List<string>();
                        while (fileReader.Peek() >= 0)
                        {
                            inputArray.Add(fileReader.ReadLine());
                        }
                        recordedData = inputArray.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("could not load file " + recordedDataPath + " " + ex.ToString());
                }
                finally
                {
                    Debug.Log("file loaded " + recordedDataPath);
                }
            }
        }

        //Update the recorded poll interval if using recorded data
        void Update()
        {
            if (useRecordedData)
            {
                pollTimer += Time.deltaTime;
                if (pollTimer > pollTimerInterval)
                {
                    pollTimer = 0;
                    PollLoggedData();
                }
            }
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
                return Input.GetAxisRaw("Horizontal");
            }
        }

        /// <summary>
        /// Is the Fizzyo device button down?
        /// </summary>
        /// <returns></returns>
        public bool ButtonDown()
        {
            return Input.GetButtonDown("Fire1");
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
    }
}