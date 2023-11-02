# LLAMASharp Unity Demo

This project serves as an example on how to integrate the [LLAMASharp library](https://github.com/SciSharp/LLamaSharp) into your Unity project.

## Demo project overview

### Prerequisites to run

To run this project you need to download the `GGUF` model into `Assets/StreamingAssets` folder.

e.g. [LLAMA-7B GGUF](https://huggingface.co/TheBloke/llama-2-7B-Guanaco-QLoRA-GGUF)

Note: You only need single `.gguf` file. Files there usually differ by their quantization level, see details in the model's readme.

### Overview

This project contains a single scene with a simple chat UI.  
Monobehaviour that powers is named `LLamaSharpTestScript` and it's already added and set up in `Example` GameObject.

It generally follows LLAMASharp readme example.

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
- Move the model to the StreamingAssets folder

At this point you should be able to copy example from this project and run it in yours.
