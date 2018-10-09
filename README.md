# Games

This repo is for test games and content, developed by the community  for the Fizzyo devices. 

All Games should based on the technical requirements as listed in https://github.com/Fizzyo/fizzyo-challenge and use the classes described and documented in the example games.

We offer two frameworks at present to build Fizzyo based games

- [Unity3d](https://github.com/Fizzyo/FizzyoFramework-Unity)

- [MonoGame](https://github.com/Fizzyo/Creating-Games-for-Fizzyo/tree/master/Fizzyo/Fizzyo-MonoGame)

> If you wish to add support for more engines / frameworks, then simply create / reuse the Fizzyo library, update it for that type and submit it back to the project.
> There are no restrictions on which engine you use, only that it is able to connect to the Fizzyo device using the Game Inputs

All Games should be opensourced to allow patient and phyisos access to the games. 

## Getting started

Getting started couldn't be simplier, simply grab the Framework components for the Fizzyo device from the [Fizzyo folder](https://github.com/fizzyo/games/tree/master/Fizzyo/) in this repository.
Full documentation for each Engine/Framework are included with the Fizzyo components.

> Note, the Fizzyo components are optional at this stage but highly recommended as they both include sample breath data for good / bad and short breathing cycles.
> You only need make your game work with a GamePad left thumbstick and A button for testing.

Once you have your game ready, just submit a Pull Request (PR) using the instructions below.

## How to design games for children
These principles create a common language that describes how to make great digital experiences for children. They talk about the tricky details that can sometimes be difficult to pinpoint or articulate when they’re missing.  http://www.bbc.co.uk/gel/guidelines/how-to-design-for-children you can also get a selection of audio assets from http://bbcsfx.acropolis.org.uk/

## Building Games for the Fizzyo project

### Step 1. Register on Github

You first needed to sign up for an account. Github has a range of accounts but all you need is a [free account](https://github.com/signup/free). This gives you a project space of your own and a user to tie all your activities to.

### Step 2. Create a new project (don't forget the gitIgnore)

Create yourself a new GIT project for your game on GitHub, full instructions can be found here ([Setting up a Repo](https://help.github.com/articles/create-a-repo/)) for getting started.
Remember to select the .gitIgnore definition for your project, ESPECIALLY if it is a Unity project.  
If in doubt, just select C#

### Step 3. Clone your project to your machine

Once you have your repository setup for your account, simply clone it to your local machine using your favorite GIT client.

if you are new to GIT, we would recommend the following:

* [GitHub for Windows https://desktop.github.com/](https://desktop.github.com/) - Supports both Windows and Mac

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

Then you need to register at http://fizzyo-ucl.co.uk 

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

## Contributing Games to this GitHub

Once you are ready and want to submit your game to the Fizzyo Game project, there are a few steps to do this.  To list your game on the Fizzyo Games site, you need to publish it as a SubModule (for more details about Submodules, [see here](https://github.com/blog/2104-working-with-submodules))

> We use submodules as there is a 1GB limit per project on GitHub, plus it means your own project remains in your own repository, we simply get a link/copy.

### Step 1. Fork the Project

Github uses "fork" to make your own personalised copy of a repository. Any changes to your fork will not affect the main project until you submit a "Pull" request (more on that in a bit)

Forking the repo is as simply as selecting Fork on the right of the repo name at github.com/fizzyo/games - again there is great [documentation](https://help.github.com/articles/fork-a-repo/) on the github site. In particular I recommend that you take the time to follow the bit about adding an alias for the "upstream" repository.
The forking instructions linked above also gave a description of how to actually use git, how to get my changes applied to your local repo, and how to push them to the remote repo on github itself.

### Step 2. Clone your fork to your machine

Once you have your copy of the Games repository for your account, simply clone it to your local machine using your favorite GIT client, as you did when creating your own repo.

> Don't click the "Update Submodules" option when cloning the repository unless you want a copy of all the other submitted games.

### Step 3. Add your project to the Fork

Here you have two options, either:

1. Create a Submodule which links to your own project repository.  Most Git Clients support this, so it is fairly painless.
It is recommended you name the SubModule using your **Game Name** and your **GIT logon** details to keep it unique.  E.G.

> For more details about Submodules, check the GitHub [Documentation Here]() or check the help for your Git client.


2. Edit the [Games Projects](https://github.com/Fizzyo/Creating-Games-for-Fizzyo/tree/master/Games_Projects.md) file in the Fizzyo Games projects root folder and add an entry for your game. It should include the Name and the Http link to your repository.  The Fizzyo team will then create the submodule link for you.  
It should look as follows (including the | characters):

My Game | ddreaper | https://GitHub/MyGame/Here

> Please only use this if you are struggling with option 1 as it may delay your project being added to the repo.

### Step 4. Make a Pull Request

When you have your games built and ready you can push the code back to the main project so that you can consider it for inclusion, please ensure you have everything is a single well named folder you can also include links to complied and published games which have been developed to the Fizzyo spec which you have published to one of the platform stores. i.e Windows, Android or iOS. To make a pull request from the main project page simply select pull request - you can add a comment about the changes you are supplying to help the maintainers to manage all the incoming content.

## Troubleshooting 

If you are having any trouble with the above, simply reach out to any of the Fizzyo team on GitHub.  For details on the team, simply click on the "Contributors" tab on the GitHub website.

## Games for Fizzyo

EggToss- A doddle jump inspired game where you need to take your egg on the hope up the cloudsusing your fizzyo device https://github.com/Fizzyo/EggTossGame

Qubi Game - Endless runner using your breadth to power up Qubi to give a super boost to compelete the level https://github.com/Fizzyo/QubiGame

Fishyo - Go fishing with your fizzyo device https://github.com/Fizzyo/Fishyo2Game

Ski Racer Get your skier down the Mountain through the salom gates watch out for the avalance https://github.com/Fizzyo/SkiRacerGame

AstroYoyage - Dodge the astroriods https://github.com/Fizzyo/AstroVoyageGame

FuelCell - Example of the game library https://github.com/Fizzyo/FuelCellGame

SolarExplorer - Fly your space craft through the solar system https://github.com/Fizzyo/SolarExplorerGame

Monster Hero Battle- Take on Monster use your breadth to defeat the monster and fizzyo device button to time your attack https://github.com/Fizzyo/MonsterHeroBattle

Fizzyo Book Reader - Read and book and use your beathe to turn the pages of short open source books https://github.com/Fizzyo/FizzyoBookReaderGame 

Fisyyo - Go fishing with your fizzyo device and see how many fish you can catch with a rod and line https://github.com/Fizzyo/FisyyoGame

Supershooter - Take on emeny and collect coins in combat with an AI https://github.com/Fizzyo/SuperShooterGame