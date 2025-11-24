Twitch API Client
The Twitch API client is the centre of the functionality of this integration and must be handled with due care and attention when making edits.
The API client is responsible for making OAuth requests, storing details and providing information for other clients to make requests.

Included is all authentication methods each with their own request patterns.
When using Twitchs API inside Unity, it is recommended to use Implicit Client Flow or Device Code Grant Flow. Authorization code grant flow is
recommended for server to Twitch authentication coupled with server to client authentication and requires your Twitch Secret to work, you must
never distribute your Twitch Secret.
Lastly Client credentials grant flow is a scopeless App Access Token and is not suitable for the majority of Twitch API requests.

To make a request to Twitch API, you can call the static task MakeTwitchAPIRequest.
This is provided in multiple formats, the first is a plain JSON in a string. Second is as a LightJson.JsonValue for easy access to provided values.
Lastly is as a TwitchAPIDataContainer<T> which has already processed your request into the requested data container from Twitch.
Type T is one of the API endpoints provided as classes in the code found under /APIEndpoints/..

Multiple formats have been provided to make the call to Twitch be it as a coroutine or a task.
Coroutines have parameters which provide a success callback (Action), which is called before the coroutine completes, meaning the Action is performed
before the continuation of the request if the request was performed with a yield return. The routine itself will also return the data inside the Current
value of IEnumerator once it is complete.
Tasks are the best to use, it uses more resources and performs the work on a seperate thread but an awaited task will return the data directly where you
left off in your method. As it doesnt depend on Unity to perform the request, as long as your platform has spare resources to make the request it will
perform faster than coroutine too.
Lastly is to perform the request synchronously, like a task it will return the requested data to point the method was called and async and await are not
required. However this method of calling Twitch is not recommended as the method will hang the frame until the request is complete, freezing the application.
This method is best used for testing solutions to making calls to twitch until proper structuring of code is performed.

TwitchWords is a static class I've used to store all Website specific works on the platform, if adjustments need to be made to working, this can be
handled globally from this class.
The same goes the enums and links found in other classes, an attempt was make to make sure these words and links and be changes in a singular place
in case of a large Twitch edit to the code base.

Twitch OAuth Tokens
StreamLinked now includes seperate token management of Twitch OAuth tokens.
Tokens are managed on a single credential instance (1 Client ID + 1 (optional) Secret) to multiple tokens.
Tokens are stored and created in Unity as ScriptableObjects under the type TokenInstance. Each instance stores a GUID as its identity then
stores the downloaded token inside PlayerPrefs appended with its GUID.
All stored tokens are found in the InternalSettingsStore under the value SavedSettings.TwitchAuthenticationTokens, the value returned is a string
but is processed as a JsonArray.
To create more tokens, a right click option is available and also a menu option under Tools/StreamLinked at the top.
Included is 3 pre created ones, a default one for the API, one for the IRC and one for the EventSub.
A space is included on each instance to switch out the token and can be done at will.