using UnityEngine;

public interface ICharacterButtonFactory
{
    GameObject CreateConfiguredButton(Transform parent);
}