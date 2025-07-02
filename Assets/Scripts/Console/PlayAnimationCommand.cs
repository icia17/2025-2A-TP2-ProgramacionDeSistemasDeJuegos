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
            console.LogToConsole("Use: playanimation <nameofanimation>");
            console.LogToConsole("Example: playanimation Jump");
            return;
        }

        string animationName = parameters[0];
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
                // Verify if the animation exists
                if (HasAnimation(animator, animationName))
                {
                    animator.Play(animationName, 1);
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
            console.LogToConsole($"Animation '{animationName}' played in {successCount} character/s.");
        }
        else
        {
            console.LogToConsole($"Couldnt play animation '{animationName}' in any character.");
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