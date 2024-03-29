using System.Linq;
using CommandTerminal;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Custom terminal commands
/// </summary>
public static class TerminalCommands
{
    [UsedImplicitly]
    [RegisterCommand(Name = "ReloadScene", Help = "Reloads current scene restarting its state", MaxArgCount = 0)]
    private static void ReloadLevel(CommandArg[] args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log($"Scene {SceneManager.GetActiveScene().name} reloaded");
    }

    [UsedImplicitly]
    [RegisterCommand(Name = "ShowProgressionTags", Help = "Displays list of progression tags categorized by: - unavailable, + available, @ active and * collected", MaxArgCount = 0)]
    private static void ShowProgressionTags(CommandArg[] args)
    {
        var tags = ProgressionManager.GetAllTags();

        var result = tags.Where(tag => tag.State == TagNode.TagState.Unavailable).Aggregate("", (current, tag) => current + "- " + tag.Name + "\n");

        result = tags.Where(tag => tag.State == TagNode.TagState.Available).Aggregate(result, (current, tag) => current + "+ " + tag.Name + "\n");

        result = tags.Where(tag => tag.State == TagNode.TagState.Active).Aggregate(result, (current, tag) => current + "@ " + tag.Name + "\n");

        result = tags.Where(tag => tag.State == TagNode.TagState.Collected).Aggregate(result, (current, tag) => current + "* " + tag.Name + "\n");

        Debug.Log(result);
    }

    [UsedImplicitly]
    [RegisterCommand(Name = "GiveTag", Help = "Collects given tag/s", MinArgCount = 1)]
    private static void GiveTag(CommandArg[] args)
    {
        var force = false;

        foreach (var tag in args)
        {
            if (tag.ToString() == "-f")
            {
                force = true;

                continue;
            }

            ProgressionManager.CollectTag(tag.String, force);
        }
    }
}