Cubemap processor aimed at converting textures to optimized .ktx2 formats for Kitten Space Agency.

## Note - You MUST have installed the Khronos KTX Tools AND have added it to Path, or this will not work: https://github.com/KhronosGroup/KTX-Software/releases

## Import Options

### Open (Equirectangular)
The tool can process equirectangular (2:1) textures and convert them to cubemaps. Importing an equirectangular automatically converts it to a cubemap in OpenGL's standard cubemap orientation.
Uses the Equirect. Import Options when importing equirectangular textures.
Unaffected by the source orientation option.

### Open (Cubemap)
The tool can open existing cubemaps by selecting all six cubemap faces in the file dialogue. The faces can either be in the OpenGL standard cubemap orientation or KSA's CCF coordinate frame. The Source Orientation option must be set according to the cubemap orientation stored on disk.

### Equirect. Import Options - Nearest Neighbour Filtering
When importing an equirectangular texture, toggles whether to use nearest neighbour filtering when automatically converting to a cubemap. Generally, this should only be used if importing a Biome ID texture.

### Source Orientation
The orientation of the cubemap currently being imported. Options are OpenGL or KSA CCF.

## Export Options

### Export (Height Map)
Exports as uncompressed R16UNorm, mipmaps, linear.

### Export (Normal Map)
Exports as BC5, mipmaps, linear.

### Export (Color Map)
Exports as BC7, mipmaps, sRGB.

### Export (Biome ID Map)
Exports as uncompressed R8G8B8A8, no mipmaps, linear.

### Export (Biome Control Map)
Exports as uncompressed R8G8B8A8, mipmaps, linear.

### Destination Orientation
The orientation to transform the cubemap into when exporting. Options are OpenGL or KSA CCF.

### Destination Resolution (Currently unused)
The resolution for each exported cubemap face. Currently unused, and will export based on whatever the import resolution was. For equirectangular textures, this will be 1/4 the height.

## Credit
This project depends on ImageSharp (Six Labors)
licensed under the Apache License 2.0.

Copyright (c) Six Labors
https://github.com/SixLabors/ImageSharp
