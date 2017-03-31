# Thanks for helping the Microsoft Fizzyo Challenge
----

Our hope is the Fizzyo device this will motivate children to do their physio every day and potentially help other families with Cystic Fibrosis as well!

If you are a game developer it’s as easy as 1-2-3.

The Fizzyo device appears as a Joystick on the computer, so you simply need to have your game interpret joystick inputs.

## We allow for 2 types of inputs:

 - Breath – This appears as the Horizontal axis of the joystick, (float) returns breath strength from (-1 – 1) with 0 being not breathing, > 0.7 blowing or breathing out hard and < -0.5 breathing in hard
 
 - Button Press – We’ve added 1 button to the device to make game interactions a little more sophisticated. This button appears as Fire1 from a joystick control.

## Fizzyo Unity asset
In the Unity folder, you have all the Fizzyo scripts required to get started, including:

- FizzyoDevice - main Fizzyo device component exposing the Breath output and the device button.
- BreathRecogniser - helper class to examine the Fizzyo device output with details / analysis of each breath

These have also been packaged up in to a [.UnityAsset](https://github.com/fizzyo/games/blob/master/Fizzyo/Fizzyo-Unity/FizzyoDevice.unitypackage?raw=true) for easy inclusion in your project.

### [Fizzyo Unity Asset](https://github.com/fizzyo/games/blob/master/Fizzyo/Fizzyo-Unity/FizzyoDevice.unitypackage?raw=true)

----
## If you are developing in Unity, you can use the following commands:

```
//(bool) Will return if the Fizzyo button is pressed or not.
bool buttonPressed = Fizzyo.FizzyoDevice.Instance().ButtonDown();

//Alternatively, you can get the button state directly using:
bool buttonPresed = Input.GetButtonDown("Fire1");

//(float) returns breath strength from (-1 – 1) with 0 being not breathing,
          > 0.7 blowing or breathing out hard and < -0.5 breathing in hard
float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();

//Alternatively, you can get the axis data directly using:

float pressure = Input.GetAxis("Horizontal");
```

> ### The benefit of using the functions from the device is that it allows you to use the pre-recorded data for testing, which will switch over to using live data by changing the *useRecordedData* parameter to false.

----
## New BreathRecogniser control:
To help with detecting breath lengths / pressure and whether the player is blowing in to the Fizzyo device, a helper class has been provided.
Breath Analyser class decouples the logic of recognizing breaths from a stream of pressure samples from acting on the recognition.

To use:

1. Create an instance of BreathAnalyser, passing in the calibration values for MaxPressure and MaxBreathLength: 
```
    BreathAnalyser breathAnalyser = new BreathAnalyser(MaxPressure, MaxBreathLength);
```
2. Register for the ExhalationCompleteEvent: 
```
    breathAnalyser.ExhalationComplete += ExhalationCompleteHandler;
```
3. Add pressure samples in the update loop: 
```
    AddSample(Time.DeltaTime, pressure);
```
4. The event will fire at the end of an exhaled breath and provide information for:
    
       a) BreathLength
       b) BreathCount
       c) ExhaledVolume
       d) IsBreathGood
    
5. You can interrogate the breath analyser at any time to determine:
    
       a) BreathLength
       b) BreathCount
       c) ExhaledVolume
       d) IsExhaling
       e) MaxPressure
       f) MaxBreathLength
    
The algorithm for determining whether a breath is good or not is encapsulated in the method:
``` 
IsBreathGood()
```
This currently returns true if the average breath pressure and breath length is within 80% of the max.
