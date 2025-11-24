DebugManager
I created DebugManager as a way to store Debug.Log messages I wished to produce in the code. On build actual Debug.Log functionality is removed and
any messages send are stored within it to use for objects like output consoles and game notifications. I have created it as a singleton object if you 
do wish to have a event based notification provider in your Unity scenes. Typically you dont want Debug.Log in your code so this is my way of removing it.
If this is not for you feel free to remove it.

Extensions
Class to hold static extension methods used in this package

FlaggedEnum<T>
T == Your non-flagged enum
Custom flagged enum class, a standard flagged enum can only hold 32 unique options (long version is 64 which is the max, 128 available in NET9)
This takes in a standard enum (no flag attribute) and builts it into a flaggable structure and can be used as such, it is serializable and comes with
its own propery drawer so it can be used anywhere.
The field is collapsable, when collapsed the right side of the field will show the number of enums selected, hovering over the number/label will
render a tooltip listing all the selected enums. Opening the field presents all of the enum values found in a tick box list, ticking a value adds the value
to the class, provided at the top is a text field that searches the wording of the options and lists only options containing matches.

FlaggedEnum (non-generic)
Like the generic version above however it omits the additional data stored in Enum types and just stores the integer values.
It also works without an Enum required and can just be provided with an array of ints to use for flagging.
It does contain less information however to FlaggedEnum<T> and I would recommend that version of this one.

InternalSettingStore
This contains my code to parse settings to and from being saved within Unity, originally I used this with actual files saved in the system but this has
been adapted to use PlayerPrefs. To add a setting type to the enum SavedSettings. InputDataJSON will attempt to save Json data to PlayerPrefs as long as
the values are found under SavedSettings.

SingletonInstance
This is my implimentation of the Singleton model, objects that inherit this are still MonoBehaviour gameobjects. By default on creation on Start it will invoke
DontDestroyOnLoad to allow persistence between scenes, this can be overwritten. To ensure the singleton model EstablishSingleton function must be called
first thing within Awake of the class. This cements the first object and destroys and future duplicates. After creation, 2 static functions are used to get an
instance of the singleton, GetInstance and CreateOrGetInstance, both return bool with the instance itself defined in the out parameter in the function.
GetInstance is for if you want the instance to be optional, for example if it is not already in the scene to not create an instance and work around that.
CreateOrGetInstance is for dependency, this will create a new instance of the singleton to work or if it already exists it will get the instance.

SingletonDispatcher
Inherits from SingletonInstance, it adds ontop of SingletonInstance by adding a Action dispatcher to LateUpdate.
This is good for work that needs to be done on the main thread when many tasks are performed asynchronously, things like coroutines and image packing
need to be done on the main thread for example.