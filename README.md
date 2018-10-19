# Games

This repo is for test games and content, developed by the community  for the Fizzyo devices.

We offer two frameworks at present to build Fizzyo based games

- [Unity3d](https://github.com/Fizzyo/FizzyoFramework-Unity) Updated 19/10/2018

- [MonoGame](https://github.com/Fizzyo/Creating-Games-for-Fizzyo/tree/master/Fizzyo/Fizzyo-MonoGame) not all features fully supported

At present we recommed the use of the Unity3d Framwork.

> If you wish to add support for more engines / frameworks, then simply create / reuse the Fizzyo library, update it for that type and submit it back to the project.
> There are no restrictions on which engine you use, only that it is able to connect to the Fizzyo device using the Game Inputs

All Games should be opensourced to allow patient and phyisos access to the games.

## Getting started

Getting started couldn't be simplier, simply grab the Framework components for the Fizzyo device from the [Fizzyo folder](https://github.com/Fizzyo/FizzyoFramework-Unity) in this repository.
Full documentation for each Engine/Framework are included with the Fizzyo components.

> Note, the Fizzyo components are optional at this stage but highly recommended as they both include sample breath data for good / bad and short breathing cycles.
> You only need make your game work with a GamePad left thumbstick and A button for testing.

Once you have your game ready, just submit a Pull Request (PR) using the instructions below.

## How to design games for children

These principles create a common language that describes how to make great digital experiences for children. They talk about the tricky details that can sometimes be difficult to pinpoint or articulate when they’re missing.  <http://www.bbc.co.uk/gel/guidelines/how-to-design-for-children> you can also get a selection of audio assets from <http://bbcsfx.acropolis.org.uk/>

## Building Games for the Fizzyo project

### Step 1. Register on Github

You first needed to sign up for an account. Github has a range of accounts but all you need is a [free account](https://github.com/signup/free). This gives you a project space of your own and a user to tie all your activities to.

### Step 2. Create a new project (don't forget the gitIgnore)

Create yourself a new GIT project for your game on GitHub, full instructions can be found here ([Setting up a Repo](https://help.github.com/articles/create-a-repo/)) for getting started.
Remember to select the Unity .gitIgnore definition for your project, ESPECIALLY if it is a Unity project.  

### Step 3. Clone your project to your machine

Once you have your repository setup for your account, simply clone it to your local machine using your favorite GIT client.

if you are new to GIT, we would recommend the following:

- [GitHub for Windows https://desktop.github.com/](https://desktop.github.com/) - Supports both Windows and Mac

With this client, you can use the "Open in Desktop" option from GitHub

> See the [main GIT site](https://git-scm.com/downloads) for more details on alternate clients
[https://git-scm.com/downloads](https://git-scm.com/downloads)

### Step 4. Start Building

You have your folder on your local machine, complete with a default ReadMe file, so you have all you need to get going. Just create your new game in your folder and off you go.

Once you are ready to submit your game to the Main Fizzyo Games Repo, be sure to update your ReadMe to tell people about your game (include screenshots / gifs if you can).  
The ReadMe is written in Markdown which is a very easy to use format, for tips / tricks and formatting just see [this guide](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) (or check out the other ReadMe's on the site)

## Creating a Game for Fizzyo

### Registering your game

You first need to recieve and invitaion code from the Project Fizzyo team - please contact Tim Kuzhagaliyev at tim.kuzhagaliyev.15 (at) ucl.ac.uk

Then you need to register at <http://fizzyo-ucl.co.uk>

Input your code here and register with tour Microsoft Services Account

### Logging in

Once you have registered click login and you will be asked for Microsoft Service Account which you used at the registration

### The Games Dashboard

You will see your games dashboard (which will be initally emppty)
You can see, add, edit and delete games or log out

### Adding Games

Clicking NEW prompts for a new windows to pop up, with the option of adding your new games details
Fields cannot be left Empty

- Game Name

- Game Version

- Unity Version

### Editing Games

Double click a editable field this will and edit windows to pop up

### Adding Achievements or Hight Scores

Go to Game Achievements or High Scores, Click the relevant button
To go back to the game dashboard or to the high scores click on the relevant button
The Achievement Dashboard - All actions performed on games can be preformed on achievements
The High Score Dashboard - At the moment you can only view the top 20 scores in your game there

### Deleting Games

Select the games you want to delete and press delete
Be suire you have selected the right game and confirm

### Testing your game

This example <https://github.com/Fizzyo/Creating-Games-for-Fizzyo/tree/master/Sample%20Games/Fizzyo-Unity-Example> includes a test harness and test data that allows you to load and playback breath data saved from a fizzyo device. There is a selection of good and bad breadths available at

<https://github.com/Fizzyo/Creating-Games-for-Fizzyo/tree/master/Sample%20Games/Fizzyo-Unity-Example/Assets/Data>

To use this a singleton class is provided FizzyoDevice.Instance() that can be used at any point in your code if FizzyoDevice.cs is present in your project.

By default FizzyoDevice plays back pre-recorded data but can also be used to gather data directly from the device if the bool useRecordedData is set to false.
This can be done through the editor or programmatically in your code.

This allows you to program your game completely against pre-recoreded pressure values if desired and switched over to live values at a later stage.

```C#
FizzyoDevice.cs

/* (float) Return the current pressure value, either from the device or streamed from a log file.
*   range: -1.0f - 1.0f
*   comment: if useRecordedData is set pressure data is streamed from the specified data file instead of the device.
*/
Fizzyo.FizzyoDevice.Instance().Pressure();


/* (bool) Return if the fizzyo device button is pressed */
Fizzyo.FizzyoDevice.Instance().ButtonDown();

## Building your game for Windows 10

At present Unity doesnt allows you to specific VID & PID's so to you need to have to manuually add the following to Package.appxmanifest after exporting to ensure the game will support the Fizzyo Device. See <https://docs.microsoft.com/en-gb/windows/uwp/packaging/packaging-uwp-apps> and <https://docs.microsoft.com/en-us/windows/uwp/packaging/app-capability-declarations>

### Adding Capabilities to your app manifest

To Edit the package.manifest you simply have to right-click on the "Package.appxmanifest" and click "View Code" to see the xml

Then manually update the capabilities section if your interested in available capabilities see https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/how-to-specify-device-capabilities-in-a-package-manifest


### Capabilities which need to be added to support Fizzyo Device

```C#

<Capabilities>
<Capability Name="internetClient" />
<uap:Capability Name="userAccountInformation" />
<DeviceCapability Name="bluetooth" />
<DeviceCapability Name="humaninterfacedevice">
<Device Id="any">
<Function Type="usage:0001 0004" />
<Function Type="usage:0001 0005" />
</Device>
</DeviceCapability>
</Capabilities>

```

## Contributing Games back to the Fizzyo GitHub Repo

Once you are ready and want to submit your game to the Fizzyo Game project, there are a few steps to do this.  To list your game on the Fizzyo Games site, you need to contact us to have your source code published at <http://github.com/fizzyo> When you develop a game for Fizzyo, you first need to recieve and invitaion code from the Project Fizzyo team. When your ready to share your game please contact Tim Kuzhagaliyev at tim.kuzhagaliyev.15 (at) ucl.ac.uk and share the github link for your source control

## Unity Source Code & Github

Enable External option in Unity → Preferences → Packages → Repository
Switch to Hidden Meta Files in Editor → Project Settings → Editor → Version Control Mode
Switch to Force Text in Editor → Project Settings → Editor → Asset Serialization Mode
Save scene and project from File menu

Note that the only folders you need to keep under source control are Assets and Project Settings.

More information about keeping Unity Project under source control you can find in this post.

## Troubleshooting

If you are having any trouble with the above, simply reach out to any of the Fizzyo team on GitHub.  For details on the team, simply click on the "Contributors" tab on the GitHub website.<https://www.studica.com/blog/how-to-setup-github-with-unity-step-by-step-instructions>

## Games for Fizzyo

EggToss- A doddle jump inspired game where you need to take your egg on the hope up the cloudsusing your fizzyo device <https://github.com/Fizzyo/EggTossGame>

Qubi Game - Endless runner using your breadth to power up Qubi to give a super boost to compelete the level <https://github.com/Fizzyo/QubiGame>

Fishyo - Go fishing with your fizzyo device <https://github.com/Fizzyo/Fishyo2Game>

Ski Racer Get your skier down the Mountain through the salom gates watch out for the avalance <https://github.com/Fizzyo/SkiRacerGame>

AstroYoyage - Dodge the astroriods <https://github.com/Fizzyo/AstroVoyageGame>

FuelCell - Example of the game library <https://github.com/Fizzyo/FuelCellGame>

SolarExplorer - Fly your space craft through the solar system <https://github.com/Fizzyo/SolarExplorerGame>

Monster Hero Battle- Take on Monster use your breadth to defeat the monster and fizzyo device button to time your attack <https://github.com/Fizzyo/MonsterHeroBattle>

Fizzyo Book Reader - Read and book and use your beathe to turn the pages of short open source books <https://github.com/Fizzyo/FizzyoBookReaderGame>

Fisyyo - Go fishing with your fizzyo device and see how many fish you can catch with a rod and line <https://github.com/Fizzyo/FisyyoGame>

Supershooter - Take on emeny and collect coins in combat with an AI <https://github.com/Fizzyo/SuperShooterGame>