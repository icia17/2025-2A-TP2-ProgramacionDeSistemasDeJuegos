using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayAnimationCommand : ICommand
{
    public string Name => "playanimation";
    public string Description => "Plays an animation in all active characters.";
    public List<string> Aliases => new List<string> { "playanim", "anim", "pa" };

    private readonly CommandConsole console;

    public PlayAnimationCommand(CommandConsole console)
    {
        this.console = console;
    }

    public void Execute(string[] parameters)
    {
        if (parameters.Length == 0)
        {
            console.LogToConsole("Use: playanimation <nameofanimation> <duration (optional)>");
            console.LogToConsole("Example: playanimation Jump 5");
            return;
        }

        string animationName = parameters[0];
        float duration = 5f; // Default duration

        // Try parse optional duration
        if (parameters.Length > 1)
        {
            if (!float.TryParse(parameters[1], out duration))
            {
                console.LogToConsole($"Invalid duration '{parameters[1]}'. Using default 5 seconds.");
                duration = 5f;
            }
        }

        var characterAnimators = GameObject.FindObjectsOfType<CharacterAnimator>();

        if (characterAnimators.Length == 0)
        {
            console.LogToConsole("No characters with CharacterAnimator were found in the scene");
            return;
        }

        int successCount = 0;
        foreach (var characterAnimator in characterAnimators)
        {
            var animator = characterAnimator.GetComponentInParent<Animator>();
            if (animator != null)
            {
                if (HasAnimation(animator, animationName))
                {
                    characterAnimator.PlayManualAnimation(animationName, duration);
                    successCount++;
                }
                else
                {
                    console.LogToConsole($"Animation '{animationName}' not found in {characterAnimator.name}");
                }
            }
        }

        if (successCount > 0)
        {
            console.LogToConsole($"Animation '{animationName}' played in {successCount} character/s for {duration} second(s).");
        }
        else
        {
            console.LogToConsole($"Could not play animation '{animationName}' in any character.");
        }
    }


    private bool HasAnimation(Animator animator, string animationName)
    {
        if (animator.runtimeAnimatorController == null) return false;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return true;
        }
        return false;
    }
}