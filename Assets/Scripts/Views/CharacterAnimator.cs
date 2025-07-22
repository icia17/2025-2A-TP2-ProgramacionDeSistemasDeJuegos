using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string isJumpingParameter = "IsJumping";
    [SerializeField] private string isFallingParameter = "IsFalling";
    [SerializeField] private string isTransitionAllowedParameter = "IsTransitionAllowed";
    private float overrideTimer = 0f;
    
    private void Reset()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Awake()
    {
        if (!character)
            character = GetComponentInParent<Character>();
        if (!animator)
            animator = GetComponentInParent<Animator>();
        if (!spriteRenderer)
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (!character || !animator || !spriteRenderer)
        {
            Debug.LogError($"{name} <color=grey>({GetType().Name})</color>: At least one reference is null!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (overrideTimer > 0f)
        {
            overrideTimer -= Time.deltaTime;

            if (overrideTimer <= 0f)
                animator.SetBool(isTransitionAllowedParameter, true);
            
            return; // Skip auto animation update while overriding
        }
        
        var speed = character.Velocity;
        animator.SetFloat(speedParameter, Mathf.Abs(speed.x));
        animator.SetBool(isJumpingParameter, character.Velocity.y > 0);
        animator.SetBool(isFallingParameter, character.Velocity.y < 0);
        spriteRenderer.flipX = speed.x < 0;
    }
    
    public void PlayManualAnimation(string animationName, float duration)
    {
        animator.SetBool(isTransitionAllowedParameter, false);
        animator.Play(animationName);
        overrideTimer = duration;
    }
}
