TwitchMessageAtlasManager and the IRC Twitch Chat scenes depend on TextMeshPro.
If its not already included in your project you will be prompted to download it when you open a scene that requires it.
This can be found in the Unity Registry of the Package Manager if it is not included.

TwitchBadgeManager and TwitchEmoteManager download images via coroutines in a singleton instance.
Information for these managers is provided via the IRC.
Badges are aquired via Twitch API Client requests, first for the global badges then when the room is joined.
Emotes are aquired first via a Twitch API Client request for the Global emotes, afterwards individaul emotes are aquired from tag information in IRC chat messages.

Once populated with data, the managers will provide the Textmeshpro specific text for a badge or emote.
If it cannot be found then nothing will be returned and must be requested specifically by the provided getters.

TwitchMessageAtlasManager is the singleton that takes the downloaded badges and emotes from the other two managers to produce the Textmeshpro Atlas.
The process of producing this Atlas was made with reliance of Unity specific functionality.
In the future I will explore other methods of not only producing atlas's but an alternative to Textmeshpro, documentation into sprites and animated sprites
is sparce at best and hard to personally expand uppon.
This is hampered further by Unitys only image packaging method Texture2D.PackTextures, for optimal operation this also needs deconstructing and adapting to working with a Twitch chat and the managers.

TwitchBadgeManager and TwitchEmoteManager both have methods to set a badge or emote to be ignored and clear image date is space is becoming an issue.

Two example scenes have been provided to demonstrate there use.
The first a single textbox output of the latest chat message. ../Scenes/Message Test Scene.Unity
The second is outputting all received messages into a UI Scroll view. ../Scenes/Chatbox Example.Unity
These scenes demonstrate the IRC, Managers and Textmeshpro working to provide the messages and highlights the limitations of Textmeshpro (especially the chatbox)

Prefabs of the Atlas manager and text boxes are provided under ../Prefabs/

NOTES:
Badges
Badges are particularly annoying to work with, they can only be aquired via the API and names can overlap in the IRC.
I probably rewrote the manager for this half a dozen times due to storing and reading issues.
Downloading is simple due to all of them being static images, however with name overlapping as global badges for example 'subscribers' also matches the name of
a channels badges 'subscribers' its makes searching and providing the correct one painful, so do be careful when editing this manager in particular.

Emotes
Animated emotes are processed using UniGif, UniGif methods processes the information and provides an array of all of the images.
Due care is needed when storing and building animated emotes into the atlas, Textmeshpro has very little documentation of animated sprites and can often
throw during runtime (does not stop or break the object, keeps running normally)
The way it currently works for Textmeshpro is the average frametimings are used to calculate how many image references should be places in the atlas to read.
For example if a frame timing is twice as long as the average per frame, 2 references to the frame are placed in the list to put into the Atlas.
