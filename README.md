Allows using AnimationCurves in a Unity ShaderGraph. This script generates `Texture2D` assets at import time based on an `AnimationCurve` specified in the importer settings. Modifying the AnimationCurve will re-bake the texture, allowing you to quickly iterate even while the game is still running. 

![GIF demonstrating the functionality](curve_changing_for_shaders.gif)

# To install:
Place this folder in your Assets/Editor folder, or under a folder with an Assembly Definition with "Editor" checked.

# To use:
1. Create an empty file with the 'curve_texture' extension. 
2. In Unity, select the file, and tweak the AnimationCurve in the inspector. Hit 'apply' when done.
3. The empty file will now act like a Texture2D in Unity. It contains the curve, baked into an R8 texture.

To use the baked curve:
1. In shader graph, create a new "SampleCurveTexture" node. This is a custom node, included in this repo.
2. Select the previously generated curve texture. 

