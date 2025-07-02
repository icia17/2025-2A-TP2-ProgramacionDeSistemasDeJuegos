using UnityEngine;

[System.Serializable]
public struct CharacterData
{
    public Character characterPrefab;
    public CharacterModel characterModel;
    public PlayerControllerModel controllerModel;
    public RuntimeAnimatorController animatorController;
    
    public SpawnButton buttonPrefab;
    public string buttonText;
}
