using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CharacterData
{
    public PlayerData playerData;
    public ButtonData buttonData;
}

[System.Serializable]
public struct PlayerData
{
    public Character characterPrefab;
    public CharacterModel characterModel;
    public PlayerControllerModel controllerModel;
    public RuntimeAnimatorController animatorController;
}

[System.Serializable]
public struct ButtonData
{
    public SpawnButton buttonPrefab;
    public string buttonText;
}
