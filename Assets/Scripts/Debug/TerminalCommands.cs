using CommandTerminal;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


public static class TerminalCommands
{
    [RegisterCommand(Name = "ReloadScene", Help = "Reloads current scene restarting its state", MaxArgCount = 0)]
    private static void ReloadLevel(CommandArg[] args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log($"Scene {SceneManager.GetActiveScene().name} reloaded");
    }
}