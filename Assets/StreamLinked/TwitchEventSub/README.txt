TwitchEventSub
With Unity C#, Eventsub is implemented with WebSockets NOT Webhooks for API reference as there are specifics for both, and Webhooks are ASP.NET specific
so they are not included or supported with Unity.

UnityEvents are provided for all subscriptions, be warned as UnityEvents they are not restricted by 'event' keywording for delegates.

The EventSub works with a WebSocket async connection with a task reading its response and building notification bodies.

All received notifications are in JSON format, all object on Twitchs documentation have been converted into classes which are built on construction of the notification.
These classes are found in the ../WebSocket/ and ../Events/ subfolders.

For identifying which Event you wish to subscribe to, they are all listed as enums under TwitchEventSubSubscriptionsEnum.
Each enum has an attribute attached to it pertaining to its string equivolent and the current version number of the Event.
Event version values are currently either 1, 2 or beta this can change in the future.

For subscriptions, for the majority of them, the broadcaster ID required is the ID of the OAuth tokens User ID, you can get the broadcaster ID of the Auth
be making a Twitch API call to GetUsers with no requested user specified.

An example scene is provided showing a basic setup of EventSub implementation, this is found in scene /Scenes/EventSub Example.unity
It demonstrates the Eventsub by subscribing to the Auth holders channel for Polls, polls are a good example as they can be triggered even in offline chats.