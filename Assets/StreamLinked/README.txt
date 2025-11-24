StreamLinked

Created by Duncan 'ScoredOne' Mellor
Project start date 01/03/2023

This project contains modified versions of LightJson and UniGif. 
Licensing and links are provided in their respective folders and are covered under the MIT License.
https://opensource.org/license/mit/

Project developed with C#9 in Unity 2022.2.5f1 Api Compatibility Level NetStandard 2.1
Currently maintained and published under version 2021.3.33f1, Compatibility Level NetStandard 2.1
Due to inclusion of 'System.Net' this project is NOT compatible with Unity WebGL.

For operation of Twitchs API and OAuth generation, please refer to the Twitch Dev console and instructions in the API [https://dev.twitch.tv/console]
All authentication methods require your applications ClientID.

If you have found an aspect of this project has not been kept up to date, you can find all of the latest changes on Twitchs website [https://dev.twitch.tv/docs/change-log/]
References updated as of change log post 2025-05‑23

This project is divided into seperate sections related to their respective sections of Twitchs API and build sections.
Twitch classes and references are found under the folders:
	- TwitchAPI 		[https://dev.twitch.tv/docs/api/]
	- TwitchEventSub 	[https://dev.twitch.tv/docs/eventsub/]
	- TwitchIRC 		[https://dev.twitch.tv/docs/irc/]
Twitch CLI is not included due to it packaged and download format style, I may look into a way to incorperate this package but as of now it is not included.
https://dev.twitch.tv/docs/cli/

Unity specific classes and builds can be found under:
	- ManagersAndBuilders 	[TwitchIRC chat badge and emote downloading and processing]
	- Other					[Debug, PlayerPrefs, Singletons and Extensions]
	- Prefabs				[Example scene prefabs]
	- Scenes				[Example scene files]
	- TwitchSceneFiles		[Example scene code]
	- TwitchStatics			[Static code and data]

Archived code due for removal is found under this folder. It follows the same folder structure and namespaces as the active files for ease of access.
It is not recommended to use this code as it will no longer be maintained and commented out/removed in a future update.
Use of any code found in this area will prompt you a [depreciated] warning.

Online documentation can be found at my Website: https://scoredone.github.io/StreamLinked/docs
Unity Asset Store Page: https://assetstore.unity.com/packages/slug/273978

JSON
This project uses JSON to process the information provided by Twitch, all objects have specifically implemented reading from LightJson.JsonValue objects.
This is due to NetStandard compatibility and also due to its compact nature.
Switching out LightJson should see no issues for compatbility (JSON format)
However due care is needed due to its heavy inclusion.

Contact Email: scoredproductionsonline@gmail.com

