TwitchIRC
Built to include all functionality from the Twitch Specific commands as well [https://dev.twitch.tv/docs/irc/capabilities/]
UnityEvents have been provided for all message types for you to respond from when using the IRC.
Be warned as UnityEvents they are not restricted by 'event' keywording for delegates.

To connect, ensure the IRC is turned on in the inspector or enabled via code with the value IRCEnabled.
If no Target is provided, it will default to the Username for the channel to connect to.

The IRC works by maintaining a TCP Client with methods reading responses recieved and building objects.
A toggle is provided to handle responce reading either asyncronously or via a Unity coroutine.

All messages received from Twitch are built into a TwitchMessage class.
All of the information provided in a message is extracted and available in the format and then send out via the events to requesters.
As messages are not JSON and just a string of information, TwitchMessage has been rewitten to attempt to extract the information,
errors can be difficult to establish as well so please take caution when editing this for updates.
During processing if the message is compatible with Twitch Chat for example where emotes are used,
the Twitch message extracts this information and send it to the Emote and Badge processors.

If you wish to change channel while it is running, simply change the TwitchTarget Value and it will disconnect and reconnect to Twitch automatically.

Folder /TwitchTagClasses/ contains all of the objects and bodies which read information for each type of response to be populated into TwitchMessage.

The scenes /Scenes/Message Test Scene.unity and /Scenes/Chatbox Example.unity demonstrate the IRC working with incorperating Twitch badges and emotes into
Textmeshpro elements to display chat.