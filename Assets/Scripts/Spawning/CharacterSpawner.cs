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
    
    public void Spawn(ISpawnableFactory characterFactory)
    {
        characterFactory.Spawn(transform);
    }
    
    private void SpawnButtons()
    {
        foreach (var factory in characterButtonFactories)
        {
            factory.Spawn(buttonsParent, this);
        }
    }

}