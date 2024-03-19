# LLAMASharp Unity Demo

This project serves as an example on how to integrate the [LLAMASharp library](https://github.com/SciSharp/LLamaSharp) into your Unity project.

## Demo project overview

### Prerequisites to run

To run this project you need to download the `GGUF` model into `Assets/StreamingAssets` folder.

e.g. [LLAMA-7B GGUF](https://huggingface.co/TheBloke/llama-2-7B-Guanaco-QLoRA-GGUF)

Note: You only need single `.gguf` file. Files there usually differ by their quantization level, see details in the model's readme.

### Overview

This project contains a single scene at `Assets/Scenes/SampleScene.unity` with a simple chat UI.  
Monobehaviour that has all the logic is named `LLamaSharpTestScript` and it's already added and set up in `Example` GameObject.

It generally follows LLAMASharp readme example and shows how to switch between different chat sessions.

Before running the project you should point `LLamaSharpTestScript.ModelPath` from the inspector to your model path in `StreamingAssets`.

Additionally this project uses the following packages:
- [UniTask](https://github.com/Cysharp/UniTask)  
  For integrating async tasks with Unity and offloading blocking tasks to thread pool.
- [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)  
  For fetching LLAMASharp and all it's dependencies from NuGet.

## Setting up a project from scratch


- Install [UniTask](https://github.com/Cysharp/UniTask) and [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) via PackageManager.
- Install LLAMASharp via NuGetForUnity.
- Manually download one of the LLAMASharp.Backend.xx NuGet packages  
  - [CPU](https://www.nuget.org/packages/LLamaSharp.Backend.Cpu)
  - [CUDA 11](https://www.nuget.org/packages/LLamaSharp.Backend.Cuda11)
  - [CUDA 12](https://www.nuget.org/packages/LLamaSharp.Backend.Cuda12)
- Unpack using ZIP, move `runtimes/<your runtime>/libllama.dll` to your Unity project Assets.
  (Note: dll must be called `libllama.dll` to be found. If it's named `llama.dll` - rename it when adding to the Unity project.)
- Move the model to the StreamingAssets folder
- Move `LLamaSharpBuildPostprocessor` into your project, or write your own for targets other than windows (see `Build and distribution`).
- Download CUDA Runtime dlls and add them to your project to be able to run the build on systems w/o CUDA installed. 
For Windows-x64 target you can download them from llama.cpp releases [here](https://github.com/ggerganov/llama.cpp/releases), you need files named cudart-llama-bin-win-cu#.#.#-x64.zip where #.#.# is your LLamaSharp backend's CUDA version.

At this point you should be able to copy example from this project and run it in yours.

## Build and distribution

There is some issue with LLamaSharp finding `libllama.dll` in build's plugin directory.
As a quick workaround there is `LLamaSharpBuildPostprocessor` that copies libllama.dll into the same directory as the `.exe`.
It only supports windows build target but you can adapt it to work with other targets, or just resolve this manually each build.