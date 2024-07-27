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
- Inside RESOURCES folder. Left mouse click -> Create -> Quicorax -> AudioUtil -> HardAudioDefinitions.

Fill the HardAudioDefinitions with your audio clips and their associated keys.

## Usage
All the package calls are accessed by calling the "IAudioService" interface.

## Example
``` 
_audioService.TransitionMusic("CaveMusic", "CaveBossMusic", 2);
```

## Dependencies
DOTween: (https://dotween.demigiant.com/).

# by: @quicorax
Under [MIT copyright license](https://github.com/Quicorax/jamUtils-pkg/blob/main/LICENSE.txt).

--- @quicorax ---
