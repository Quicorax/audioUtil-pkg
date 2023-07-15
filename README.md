# audioUtil-pkg
Audio utilities for Unity.

## Installation using the Unity Package Manager
1. Open the Package Manager Window. 
2. Click on the + icon on the top left.
3. Select "Add package from git URL...".
4. In the input field enter the URL of the git repository ("https://github.com/Quicorax/XXXXXXX.git")
5. Click on "Add" and wait until the package is downloaded and imported.

## Features


## SetUp
The utility needs to be initialized, to do so: 
- Go to Packages -> AudioUtilityPackage -> Sample. Then drop the "AudioInitializer" prefab to the scene.
Generate a new "HardAudioDefinitions" by:
- Inside any Assets folder. Left mouse click -> Create -> Quicorax -> AudioUtil -> HardAudioDefinitions.

Set the reference of the newly created HardAudioDefinitions to the previously added AudioInitializer.

Fill the HardAudioDefinitions with your audio clips and their associated keys.

## Use
All the package calls are accessed by calling the static "AudioPlayer" and accessing its "Actions": ``` AudioPlayer.Do. ```

There are the following calls:
```
PlaySFX(string clipKey);
PlayMusic(string clipKey);
StopMusic(string clipKey, float fadeTime = 0, Action onComplete = null);
StopAllMusics(float fadeTime);
Clear();

AddMasterVolume(float additiveValue);
AddMusicVolume(float additiveValue);
AddSFXVolume(float additiveValue);
MuteMaster();
MuteMusic();
MuteSFX();
```
## Example
``` AudioPlayer.Do.StopMusic("CaveMusic", 2, ()=> AudioPlayer.Do.PlayMusic("CaveBossMusic")) ```

## Dependencies
None.

# by: @quicorax
Under [MIT copyright license](https://github.com/Quicorax/audioUtil-pkg/blob/main/LICENSE.txt).
