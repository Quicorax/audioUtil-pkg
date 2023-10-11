# audioUtil-pkg
Audio management utility for Unity.

## Installation using the Unity Package Manager
1. Open the Package Manager Window. 
2. Click on the + icon on the top left.
3. Select "Add package from git URL...".
4. In the input field enter the URL of the git repository ("https://github.com/Quicorax/audioUtil-pkg.git")
5. Click on "Add" and wait until the package is downloaded and imported.

## SetUp
Generate a new "HardAudioDefinitions":
- Inside any Assets folder. Left mouse click -> Create -> Quicorax -> AudioUtil -> HardAudioDefinitions.

Set the reference of the newly created HardAudioDefinitions to the previously added AudioInitializer.

Fill the HardAudioDefinitions with your audio clips and their associated keys.

## Usage
All the package calls are accessed by calling the "IAudioPlayer" interface.

There are the following calls:
```
void PlaySFX(string sfxKey);
void PlayMusic(string musicKey);
void StopMusic(string musicKey, float fadeTime = 0, Action onComplete = null);
void TransitionMusic(string fromMusicKey, string toMusicKey, float fadeTime = 0);
void StopAllMusics(float fadeTime);
void AddMasterVolume(float additiveValue);
void AddMusicVolume(float additiveValue);
void AddSFXVolume(float additiveValue);

bool MuteMaster();
bool MuteMusic();
bool MuteSFX();
void ClearAudio();
```
## Example
``` 
_audioService.TransitionMusic("CaveMusic", "CaveBossMusic", 2);
```

## Dependencies
DOTween: (https://dotween.demigiant.com/).

# by: @quicorax
Under [MIT copyright license](https://github.com/Quicorax/jamUtils-pkg/blob/main/LICENSE.txt).

--- @quicorax ---
