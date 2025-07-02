using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner Instance { get; private set; }
    
    [SerializeField] private List<InterfaceRef<ICharacterButtonFactory>> buttonFactories;
    [SerializeField] private Transform buttonsParent;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CreateButtons();
    }
    
    private void CreateButtons()
    {
        foreach (var factory in buttonFactories)
        {
            factory.Ref.CreateConfiguredButton(buttonsParent);
        }
    }
    
    public void Spawn(CharacterData data)
    {
        if (data.characterPrefab == null)
        {
            Debug.LogError("Character prefab is null!");
            return;
        }

        var characterInstance = Instantiate(data.characterPrefab, transform.position, transform.rotation);
        ConfigureCharacter(characterInstance, data);
    }
    
    private void ConfigureCharacter(Character character, CharacterData data)
    {
        character.Setup(data.characterModel);

        if (!character.TryGetComponent(out PlayerController controller))
            controller = character.gameObject.AddComponent<PlayerController>();
        controller.Setup(data.controllerModel);

        var animator = character.GetComponentInChildren<Animator>();
        if (!animator)
            animator = character.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = data.animatorController;
    }
}