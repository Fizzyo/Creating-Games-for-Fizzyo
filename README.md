# Games

This repo is for test games and content, developed by the community  for the Fizzyo devices. 

All Games should based on the technical requirements as listed in https://github.com/Fizzyo/fizzyo-challenge and use the classes described and documented in the example games.

We offer two frameworks at present to build Fizzyo based games

- [Unity3d](https://github.com/Fizzyo/fizzyo-challenge/tree/master/Fizzyo-Unity-Example)

- [MonoGame](https://github.com/Fizzyo/fizzyo-challenge/tree/master/Fizzyo-MonoGame-Example)

All Games should be opensourced to allow patient and phyisos access to the games. 

## Getting started

Getting started couldn't be simplier, simply grab the Framework components for the Fizzyo device from the [Fizzyo folder](https://github.com/fizzyo/games/tree/master/Fizzyo/) in this repository.
Full documentation for each Engine/Framework are included with the Fizzyo components.

> Note, the Fizzyo components are optional at this stage but highly recommended as they both include sample breath data for good / bad and short breathing cycles.
> You only need make your game work with a GamePad left thumbstick and A button for testing.

Once you have your game ready, just submit a Pull Request (PR) using the instructions below.

## Contributing Games to this GitHub

### Step 1. Register on Github

You first needed to sign up for an account. Github has a range of accounts but all you need is a [free account](https://github.com/signup/free). This gives you a project space of your own and a user to tie all your activities to.

### Step 2. Fork the Project

Github uses "fork" to allow you to "checkout" and begin modifying repos
But in this case you're just making your own copy of the project repository. This is where you will commit your changes to and it retains its link with the original repository making it easy for anyone with commit access to that to pull in your changes.  Forking the repo is as simply as selecting Fork on the right of the repo name at github.com/fizzyo/games - again there was great [documentation](https://help.github.com/articles/fork-a-repo/) on the github site. In particular I recommend that you take the time to follow the bit about adding an alias for the "upstream" repository.
The forking instructions linked above also gave a description of how to actually use git, how to get my changes applied to your local repo, and how to push them to the remote repo on github itself.

### Step 3. Clone your fork to your machine

Once you have your copy of the repository for your account, simply clone it to your local machine using your favorite GIT client.

if you are new to GIT, we would recommend the following:

* [GitHub for Windows https://desktop.github.com/](https://desktop.github.com/) - Supports both Windows and Mac

With this client, you can use the "Open in Desktop" option from GitHub

> See the [main GIT site](https://git-scm.com/downloads) for more details on alternate clients
[https://git-scm.com/downloads](https://git-scm.com/downloads)

### Step 4. Create a new folder for your Game

With a copy of the Games Git repositiory on your local machine, create a new folder to put your game in.

> DO NOT create your game at the top level of the repository or it cannot be merged.

It is recommended you name the folder using your **Game Name** and your **GIT logon** details to keep it unique.

### Step 5. Make a Pull Request

When you have your games built and ready you can push the code back to the main project so that you can consider it for inclusion Please ensure you have everything is a single well named folder you can also include links to complied and published games which have been developed to the Fizzyo spec which you have published to one of the platform stores. i.e Windows, Android or iOS. To make a pull request from the main project page simply select pull request - you can add a comment about the changes you are supplying to help the maintainers to manage all the incoming content.
