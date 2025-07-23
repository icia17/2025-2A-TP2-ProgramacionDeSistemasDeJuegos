using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public Character characterPrefab;
    public CharacterModel characterModel;
    public PlayerControllerModel controllerModel;
    public RuntimeAnimatorController animatorController;
}