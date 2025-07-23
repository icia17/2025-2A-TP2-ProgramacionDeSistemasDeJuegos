using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptables/CharacterFactory")]
public class CharacterFactorySO : ScriptableObject, ISpawnableFactory
{
    [SerializeField] private PlayerData playerData;

    public void Spawn(Transform characterTransform, ISpawnableFactoryCreator creator = null)
    {
        var characterInstance = Instantiate(playerData.characterPrefab, characterTransform.position, characterTransform.rotation);
        
        ConfigureCharacter(characterInstance);
    }
    
    private void ConfigureCharacter(Character character)
    {
        character.Setup(playerData.characterModel);

        if (!character.TryGetComponent(out PlayerController controller))
            controller = character.gameObject.AddComponent<PlayerController>();
        controller.Setup(playerData.controllerModel);

        var animator = character.GetComponentInChildren<Animator>();
        if (!animator)
            animator = character.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = playerData.animatorController;
    }
}
