using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour, ISpawnableFactoryCreator
{
    [SerializeField] private List<CharacterButtonFactorySO> characterButtonFactories;
    [SerializeField] private Transform buttonsParent;
    
    private void Start()
    {
        SpawnButtons();
    }
    
    // Method used for spawning characters
    public void Spawn(ISpawnableFactory characterFactory)
    {
        characterFactory.Spawn(transform);
    }
    
    // Method used for spawning the buttons that spawn the characters
    private void SpawnButtons()
    {
        foreach (var factory in characterButtonFactories)
        {
            factory.Spawn(buttonsParent, this);
        }
    }

}