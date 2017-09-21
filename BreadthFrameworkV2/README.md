# CSE Hack Week Challenge

# Test and Refine Fizzyo Breadth Framework V2 

## System Overview

	The goal of the system was to provide a breath framework that could be incorporated into any 
	Unity games that are created for the Fizzyo project. Development of the framework included:
		o	A breath recogniser system that interprets Fizzyo device data and translates this into 
		variables that can used to customise gameplay
		o	A calibration system which allows the user of a Fizzyo game to set the difficulty of 
		the game based on their individual capacity for breath exhalation
		o	A system for uploading analytical session data that contains information about the 
		user’s performance in game
		o	An achievement system which allows the developer of the game to create achievements 
		and have these unlock for the specific user playing the game
		o	An achievement display which shows all of the achievements within the game and which 
		that have been unlocked by the current user
		o	A leader board system which allows a user’s high score to be uploaded to the database 
		and for the top scores for that game to be displayed within the game itself

## System Implementation
	
	The system is designed as a set of files Breadth Framework Components within Unity. These files can be packaged and inserted into any other Unity game for the system, giving the game access to the frameworks features. These Unity games are to be built into UWP applications using the .Net framework for use on Windows devices, and more specifically the target device, a Linx 1020 10” Tablet Running Windows 10.

	The front end of the system takes the form of Unity scenes which are collections of game objects that provide a user interface. The back end of the system takes the form of various C# scripts containing classes and methods which can be utilised by the games developer to integrate the system with their game. The system also utilises access to a web API which is used for uploading and loading user data. This API was created alongside the system as a separate project.

	The framework was created within the game “Qubi”, which was made in Unity for the Fizzyo system prior to the projects initiation. Table 3 contains an overview of all of the system component assets that were created, modified or required for the system to function. Further details of each asset can be found in section 4.2 of this report.

	Please Refer to Table 1 and Figure 1 on page 2 of the System Description Manual for an overview of the systems components and a visualisation of the system.

	Key Features of the system include (For more details please refer to the Systems Description
	and Manual PDF file):
		- Database Integration 
		- Loading of User Data
		- Achievements
		- Leader boards
		- Session Data
		- Calibration
		- Breath Recognition
		
## Database Integration

	Interactions between the API and framework are fully encapsulated into the Fizzyo.Data namespace found in the Fizzyo.cs script of this project; they are further separated into the Load and Upload classes. As these methods are static they can be called by any script that includes the Fizzyo.Data namespace. The points where data is loaded and uploaded are detailed within the key system component descriptions below.
	
	Details of the Fizzyo API can be found at: https://api.fizzyo-ucl.co.uk/#api-v1_Authv1_auth_token.
	
	Currently the system uses a built-in username and password (hardcoded into the Fizzyo.cs class) for a test user to load and upload data. In the future the data should be transferred using an
	account associated with the current users Windows Live account. An endpoint exists for receiving an access token using Windows Live login details but this system currently doesn’t utilise it. This could be solved in two ways: the Windows Live access token could be sent to the game by the hub application using app to app technology or the game could incorporate a Windows Live login screen of its own. The former solution being preferable as it means the user only has to log in once.
	
## Loading of User Data
	
	Please Refer to Figure 3 on page 6 of the System Description Manual for the class diagram that
	describes the initial data loading system.
	
	Loading of user data within the system occurs when the user starts the game. The functionality for this user data load is encapsulated in the “Load” Class. This is a public class and its methods can be accessed by using the namespace Fizzyo.Data. To perform the initial load the game calls the LoadUserData method in the InitialDataLoad class,passing it the games ID string and the game secret. These parameters are given to developers when their game is added to the system, they are required to input these into the InitialDataLoad class when implementing the framework. The various user data, if the user is online, is then loaded from the API into the games player preferences. Player	preferences in Unity are used to store and access player details between game sessions.

	They can be accessed or set at any time in a script.
	
	InitialDataLoad.cs: This script is used in the scene of the same name, it calls the LoadUserData method within the Fizzyo.Data.Load class using the parameter strings gameId and gameSecret.	The gameID and gameSecret strings are procured when the game developer registers their game with the system. The LoadUserData method then precedes to do the following:
	- Using the ResetPlayerPrefs() method, the relevant exiting player preferences are cleared to make way for new data
	- The username and password are set. These values are used by the PostAuthentication method to procure an accessToken from the API, to allow the framework to make further API accesses. Currently the system uses a test patient to upload data to and load all data from. The access token is saved into playerpreferences
	- If an access token is successfully procured, the player preference value for userLoaded is set to 1. The method then precedes to use other methods	(GetCalibrationData GetUnlockedAchievements and GetUserTag) to load in the calibration, achievement unlock and tag data for the user. These values are saved in various player preferences, which are described within the code in Fizzyo.cs. How achievements are loaded is discussed further in the Achievements section of this file.
	- When loading of these parameters concludes, the user’s online status is set to 1,which allows them to view the games leader board and upload further data using their access token
	- If the framework fails to procure an access token or load the calibration, achievement or tag data, then the user’s online status is set to 0. This signifies that they are playing offline and will not be able to upload data or load the games leaderboard. The front end then offers the user an attempt to retry the connection
		
	In the InitialDataLoad class, if the data load is successful but shows that, through values	set in the player preferences, that the user has not uploaded a tag, then the user is prompted to do so, or can choose to play offline. If a tag is chosen, using the arrow buttons to change the three-letter tag then pressing continue, it is uploaded using the UserTag method found in the upload class. The option to change your tag also appears in the calibration screen if the user is online. A three-letter tag was chosen to represent a player as both a way of securing anonymity and to mirror the system which is commonly used in arcade games. Tag uploads are filtered, so if the tag contains profanity, it will not beuploaded and a message is displayed asking the user to input a different tag.
		
	The loading of user data is then complete. If the user is playing offline or has not yet uploaded calibration data then they are automatically taken to the calibration screen. If calibration data was loaded in then the user goes straight to the main menu, where they can start playing immediately.
	
## Achievements
	
	Please Refer to Figures 8 and 10 on pages 11 and 13 of the System Description Manual for the class diagrams that describe the achievement system.
	
	For the frameworks achievement system to function in a specific game, the developer must perform two actions:
	- Submit achievement details to the Fizzyo database developer. These can then be added to the database and each achievement can be given a unique ID. Details to add to the database are the achievement title, description, points and gameID
	- Include full details of each achievement, including the ID given above, in JSON format within the Achievements.json file within the Streaming Assets folder. A detailed description and example of the data that must be included can be found in the System	Description and Manual PDF file.
	
	The Achievements.cs JSON file is used as a template for the loading of user achievement	data. During the initial data load, the file is read and loaded as a string into the player	preferences under the name “achievements”. The GetUnlockedAchievements method then modifies the template in player preferences based on which achievements have been unlocked (these are loaded into the system from the database) and how much achievements have been progressed (this information is held within the player preferences). The “achievements” string is then used as a record of the user’s achievements within the rest of the game.
	
	Achievements are handled in game by the FizzyoOverlay.cs script. This script is attached to a set of game objects that must be added to the games gameplay scenes. These game objects include:
		- FizzyoOverlay – Used to hold the FizzyoOverlay Script and contain the other two objects
		- LeadUnlock – Canvas and Vertical Layout Group used to display upload status notifications
		- AchUnlock – Canvas and Vertical Layout Group used to display achievement unlock notifications
	
	The FizzyoOverlay.cs script contains the UnlockAchievement method which can be used in the gameplay scripts (via accessing the objects script component) to add to the progress of a specific achievement. If an achievements unlockProgress meets its unlockRequirement value then the  ahievement is unlocked. If an achievement is unlocked a notification is shown onscreen alerting the player. This notification shows the achievements title and point value. The script, after a number of seconds, then removes	the notification.
	
	IDs of achievements that have been unlocked during a session are held under player preferences, so they can be uploaded to the database at the end of a session. IDs of achievements that are only progressed and not unlocked yet are added to a separate player preference, so the progress made can be saved within the game.
	
	The dependency system allows achievements to be connected in a manner similar to a linked list. An example of this would be an achievement that required one jump to be performed which is linked to an achievement that requires 10 jumps. The algorithm that handles unlocking and progressing achievements is designed so that only the first achievement needs to be added to via the UnlockAchievement method. If this achievement is already unlocked the progress will be transferred to its dependency achievement and if this is unlocked then if will transfer to the dependents dependency and so on. For the full code listing of this system please see the code listing section of the appendix.
	
	Achievement unlocks and progress can be viewed in the Achievements scene. They are displayed by the AchievementManager.cs script. The AchievementButton class is used to create buttons that switch between different achievement categories.
	
	The layout of this scene can be seen in Figure 11 of the System Description and Manual, in this view the First Set Completed achievement has been unlocked as well as other achievements in different categories. Progress is shown to the left of an achievements point value. The total amount of points a player has attained through unlocks is displayed below the heading. This value gives the player an idea of how much they have achieved within the game.
	
## Leaderboards

	Please Refer to Figures 12 and 13 on pages 15 and 16 of the System Description Manual for 
	the class diagrams that describe the leader board system.
	
	The score that a user gets when they have finished a session is uploaded via the FizzyoOverlay.cs script. At the point where a session is finished in the game script, the LeaderBoard method is called using the score that was attained as a parameter. This score is then uploaded via the Score method within the Upload class contained in Fizzyo.cs in the Fizzyo.Upload namespace. A notification is shown onscreen which states if the upload was complete or if it failed.
	
	The top 20 scores that have been uploaded by players can be viewed in the Leader boards scene. The LeaderboardManager class utilises the GetHighScores method in the Load class of the Fizzyo.Data namespace which returns the high scores as a JSON formatted string. These high scores are then displayed with their position, name and score in the scene.
	
## SessionData

	Please Refer to Figure 15 on page 16 of the System Description Manual for the class diagram that describes the session data upload system.

	The framework is designed to gather and hold session data as the game is played in an instance of the Session class. The Session class is held within the Fizzyo namespace and contains methods for constructing and uploading a session. When a session is constructed the breath count and set count for the session are set, the start time is also saved. When a session is uploaded the amount of good breaths, bad breaths and score	are set, the end time is also found by the class. The class then uses the Session method in the Upload class to submit this information to the database. The Session method also uploads unlocked achievements and saves achievement progress, so a session must be	completed for achievement data to be saved. Once the session has finished uploading, a string is returned containing the status of the upload. This can then be used by the ShowSession method in the FizzyoOverlay to display a notification in game.
		
## Calibration

	Please Refer to Figure 17 on page 18 of the System Description Manual for the class diagram
	that describes the calibration system.

	A class is used to represent a calibration session. This class is held in the Fizzyo namespace.
	It holds all of the data for a given calibration and the Calibrate method which is called
	within the Update function of the Recalibration class.
	
	The framework is designed so that a user cannot play the game without first calibrating.
	Calibrating the system involves exhaling as much as possible though the device for as long
	as possible three times. The average breath length and pressure for each of these
	calibration steps is then calculated by the framework and calibration is complete. The
	user is then presented with the results. The results are uploaded to the database and held
	within the games player preferences, for use in the breath recognition system. The
	breaths that are completed must be longer than one second each and have a pressure of
	over 10% of the devices maximum output, if these parameters are not met then the
	system asks for the user to repeat the breaths.
	
## Breath Recognition

	Please Refer to Figure 19 on page 20 of the System Description Manual for the class diagram
	that describes the calibration system.

	The breath recogniser for the system has been included in the Fizzyo namespace and
	modified to include the following features:
	- Calibration data is now used to define a target pressure and breath length that the
	system uses to judge the quality of a completed breath
	- A breathPercentage variable has been added to the BreathRecogniser class which can be used to communicate to the user how far though a target breath they are	or how good the breath they have completed was. An example of this variable being used can be observed in Qubi. The outer bar that fills up on the maincharacter uses this percentage value to show the user their breath progression
	- A breathQuality variable has been added to the event that triggers when a breath is completed. This integer value of this variable signifies one of the following:
			o 0 : Breath was 0 - 25% of the calibrated breath length
			o 1 : Breath was 25% - 50% of the calibrated breath length
			o 2 : Breath was 50 - 75% of the calibrated breath length
			o 3 : Breath was 75% - 100% of the calibrated breath length
			o 4 : Breath was 100% of the calibrated breath length
			
## Building the Application Into UWP

	The following steps are required when building the system into a UWP Build application:
		- The publishing capabilities in Unity should include joystick, Bluetooth, Network and
		Internet for the Fizzyo device and data transfer to occur correctly
		- When the solution is built a protocol should be added, this protocol, when shared
		with the hub application will allow the hub application to launch the game. This
		also requires the inclusion of the following piece of code in the solution within the
		App.xaml.cs file:
		
			protected override void OnActivated(IActivatedEventArgs args) 
			{ 
				if (args.Kind == ActivationKind.Protocol) 
				{ 
					Frame rootFrame = Window.Current.Content as Frame; 
			  
					if (rootFrame == null) 
					{ 
						rootFrame = new Frame(); 
						Window.Current.Content = rootFrame; 
						rootFrame.NavigationFailed += OnNavigationFailed; 
					} 
			  
					var protocolEventArgs = args as ProtocolActivatedEventArgs; 
					switch (protocolEventArgs.Uri.Scheme) 
					{ 
						case "myapplication": 
							rootFrame.Navigate(typeof(MainPage), protocolEventArgs.Uri); 
							break; 
					} 
					Window.Current.Activate(); 
				} 
			}
		- The protocol name needs to be sent to the developer of the hub application. This will allow the hub to launch the game

## Testing the System in Qubi

	The system has been made to work with a sample game. This game, called Qubi, was created by Microsoft before the system was built. The files included with this project still contain this game and this can be used to view and test the system. 
	
	The following steps can be taken to open and run the game:
		- Download and install Unity
		- Save the Qubi project folder within the Unity folder
		- Open Unity
		- Press Open and find the project file
		- Open this project file in Unity
		- You should then be presented with a view of one of the scenes from the game
		- Open the project tab locate the Scenes file 
		- Open the InitialDataLoad scene
		- Press the play button at the top of the screen
		- This will begin the game
		
	The flow of the game, once it is opened, is described on page 26 of the System Description and Manual in the game flow section. 
	
	The details for a test user have been used in the code so any data that is loaded or uploaded will be related to that user in the database. You will notice that some Qubi achievements have already been unlocked, the calibration has been completed and a tag has been entered for this user;Therefore the system doesn't prompt the user for a calibration session or tag input when the user data is initially loaded.

## Adding the Framework to a New Game

	The files that need to be added to a game to incorporate the system are included in the Required Fizzyo Files folder. An overview of these components can be found in Table 1 on page 2 of the System Description.
	
	All scenes are self contained and complete except for MapTest. The game objects in MapTest (FizzyoOverlay,	AchUnlock and LeadUnlock) are intended to be included in the main game scene of any game that is created for the system. These objects and their associated scripts, as described in the earlier sections of this readme file, are used to progress/unlock achievements, gather session data and upload data at the end of a session.
	
	For the frameworks achievement system to function in a specific game, the developer
	must perform two actions:
	- Submit achievement details to the Fizzyo database developer. These can then be added to the	database and each achievement can be given a unique ID. Details to add to the database are the achievement title, description, points and gameID
	- Include full details of each achievement, including the ID given above, in JSON format within the Achievements.json file within the Streaming Assets folder. A detailed description and example of the data that must be included can be found in the System Description and Manual PDF file.
	
	Perform the actions described in the "Building the Application Into UWP" section of this readme file
	
	The developer will need to be added to the system, by the Fizzyo database administrator, to allow them to get an access code for the API. This access code can then be used to register their game with the Fizzyo database though the API to get a gameID and gameSecret. The following API endpoint is used for	this: https://api.fizzyo-ucl.co.uk/api/v1/games 
	
	The gameID and gameSecret should then be added within the InitialDataLoad.cs script at lines 11 and 12.These values are used when accessing the API.

	The test users username and password are included in the Fizzyo.cs file at line 62 and 63. A new test user can be requested from the Fizzyo database administator. If a new test user is to be used then the aforementioned username and password variables need to be changed to reflect the new details. 
	
## Future Work Related to the Project

	The following system attributes must be addressed:
		
	- Currently there is no endpoint in the Fizzyo API that can be used for developer achievement
	data upload and ID assignment. The database administrator has to be contacted directly for this to occur. 
	- The calibration process focuses on a high breath pressure where it should focus on the best breath length a user can accomplish
	- A test user is currently used. Data is not associated with a users Windows Live account details. This could be solved in two ways: the Windows Live access token could be sent to the game by the hub application using app to app technology or the game could incorporate a Windows Live login screen of its own. The former solution being preferable as it means the user only has to sign in once.User authentication is crucial, the use of the Xbox Live creators program is recommended.
	- Program analytics such as program launched and closed reports are not currently sent to the
	database. This could be stored by the game and uploaded with a session.