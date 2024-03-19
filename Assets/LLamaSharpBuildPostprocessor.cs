# if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class LLamaSharpBuildPostprocessor {

    /// <summary>
    ///  Due to how LLamaSharp looks for the .dll files, we need to copy them to the project directory where exe is from plugins directory
    /// </summary>
    /// <param name="target"></param>
    /// <param name="pathToBuiltProject"></param>
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        string pathToLibllama;
        pathToBuiltProject = Path.GetDirectoryName(pathToBuiltProject);
        if (target == BuildTarget.StandaloneWindows64)
        {
            pathToLibllama = Path.Join(
                Path.Join(
                    pathToBuiltProject, $"{PlayerSettings.productName}_Data", "Plugins"
                ), 
                "x86_64",
                "libllama.dll"
            );
        }
        else if (target == BuildTarget.StandaloneWindows)
        {
            pathToLibllama = Path.Join(
                Path.Join(
                    pathToBuiltProject, $"{PlayerSettings.productName}_Data", "Plugins"
                ), 
                "x86",
                "libllama.dll"
            );
        }
        else
        {
            Debug.LogError("Unsupported build target");
            return;
        }
        Debug.Log($"Copying libllama.dll from {pathToLibllama}");
        if (!File.Exists(pathToLibllama)) {
            Debug.LogError("libllama.dll not found in the built project");
            return;
        }
        // copy the libllama.dll to the project directory
        File.Copy(pathToLibllama, Path.Join(pathToBuiltProject, "libllama.dll"), true);
    }
}
#endif
