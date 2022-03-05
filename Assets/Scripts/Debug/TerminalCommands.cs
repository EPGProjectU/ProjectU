using System.Linq;
using CommandTerminal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DebugU
{
    /// <summary>
    /// Custom terminal commands
    /// </summary>
    public static class TerminalCommands
    {
        [RegisterCommand(Name = "ReloadScene", Help = "Reloads current scene restarting its state", MaxArgCount = 0)]
        private static void ReloadLevel(CommandArg[] args)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log($"Scene {SceneManager.GetActiveScene().name} reloaded");
        }


        [RegisterCommand(Name = "ShowProgressionTags", Help = "Displays list of progression tags categorized by: - unavailable, + available, @ active and * collected", MaxArgCount = 0)]
        private static void ShowProgressionTags(CommandArg[] args)
        {
            var tags = ProgressionManager.GetAllTags();

            var result = tags.Where(tag => tag.State == ProgressionTag.TagState.Unavailable).Aggregate("", (current, tag) => current + "- " + tag.Name + "\n");

            result = tags.Where(tag => tag.State == ProgressionTag.TagState.Available).Aggregate(result, (current, tag) => current + "+ " + tag.Name + "\n");

            result = tags.Where(tag => tag.State == ProgressionTag.TagState.Active).Aggregate(result, (current, tag) => current + "@ " + tag.Name + "\n");

            result = tags.Where(tag => tag.State == ProgressionTag.TagState.Collected).Aggregate(result, (current, tag) => current + "* " + tag.Name + "\n");

            Debug.Log(result);
        }

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
}