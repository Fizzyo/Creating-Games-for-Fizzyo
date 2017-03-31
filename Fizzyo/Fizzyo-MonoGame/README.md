# Thanks for helping the Microsoft Fizzyo Challenge
----

Our hope is the Fizzyo device this will motivate children to do their physio every day and potentially help other families with Cystic Fibrosis as well!

If you are a game developer it’s as easy as 1-2-3.

The Fizzyo device appears as a Joystick on the computer, so you simply need to have your game interpret joystick inputs.

----
## We allow for 2 types of inputs:

 - Breath – This appears as the Horizontal axis of the joystick, (float) returns breath strength from (-1 – 1) with 0 being not breathing, > 0.7 blowing or breathing out hard and < -0.5 breathing in hard
 
 - Button Press – We’ve added 1 button to the device to make game interactions a little more sophisticated. This button appears as Fire1 from a joystick control.

----
## Fizzyo library project
Contains all the Fizzyo components, scripts and project/game helpers, including:

- FizzyoDevice - main Fizzyo device component exposing the Breath output and the device button.
- BreathRecogniser - helper class to examine the Fizzyo device output with details / analysis of each breath
- InputState - A generic input helper used by the Fizzyo classes but also available for games as well.

Simply add to your project and create a reference to it. Simples.

----
## Fizzyo device use with MonoGame

Grab the Fizzyo library from the sample and add it to your project as follows:

1.  Initialise the InputState class:

```
var inputState = new InputState(this);
Services.AddService(typeof(InputState), inputState);
```

> Note, the InputState object needs updating in the main game loop each update.
> ```
> inputstate.Update(gameTime);
> ```

2. Initialise the FizzyoDevice class:

```
var fizzyoDevice = new FizzyoDevice(this);
fizzyo.useRecordedData = true; // Change this value to use actual values instead of recorded data
Services.AddService(typeof(FizzyoDevice), fizzyoDevice);
```
> Note, the FizzyoDevice object needs updating in the main game loop each update.
> ```
> fizzyoDevice.Update(gameTime);
> ```

3. To consume the Fizzyo device from other classes / objects, simply use the Game Service retrieval logic.

```
var fizzyoDevice = (FizzyoDevice)game.Services.GetService(typeof(FizzyoDevice));
```

4. poll the fizzyo device for breath / button data each update

```
//(bool) Will return if the Fizzyo Device button is pressed or not.
bool buttonPresed = FizzyoDevice.ButtonDown(); 
//(float) returns breath strength from (-1 – 1) with 0 being not breathing, **> 0.7** blowing or breathing out hard and **< -0.5** breathing in hard
float pressure = FizzyoDevice.Pressure();
```

> Alternatively you can just bypass the Fizzyo logic and call the device inputs directly using:
> ```
> //Get the button input direct from the gamePad:
> bool buttonPresed = Keyboard.GetState().IsKeyDown(Keys.A);
> //Get the pressure input direct from the gamePad:
> float pressure = GamePad.GetState(0).ThumbSticks.Left.X;
> ```

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