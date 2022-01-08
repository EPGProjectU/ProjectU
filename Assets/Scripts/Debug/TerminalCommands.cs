using System.Linq;
using CommandTerminal;
using UnityEngine.SceneManagement;

namespace Debug
{
    public static class TerminalCommands
    {
        [RegisterCommand(Name = "ReloadScene", Help = "Reloads current scene restarting its state", MaxArgCount = 0)]
        private static void ReloadLevel(CommandArg[] args)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            UnityEngine.Debug.Log($"Scene {SceneManager.GetActiveScene().name} reloaded");
        }


        [RegisterCommand(Name = "ShowProgressionTags", Help = "Displays list of progression tags categorized by: inactive, active and collected", MaxArgCount = 0)]
        private static void ShowProgressionTags(CommandArg[] args)
        {
            var result = ProgressionManager.GetInactiveTags().Aggregate("", (current, pTag) => current + "- " + pTag.Name + "\n");

            result = ProgressionManager.GetActiveTags().Aggregate(result, (current, pTag) => current + "+ " + pTag.Name + "\n");

            result = ProgressionManager.GetCollectedTags().Aggregate(result, (current, pTag) => current + "* " + pTag.Name + "\n");

            UnityEngine.Debug.Log(result);
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