using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterFactory")]
public class CharacterFactorySO : ScriptableObject, ICharacterButtonFactory
{
    [SerializeField] private CharacterData characterData;
    
    public CharacterData Data => characterData;
    
    public GameObject CreateConfiguredButton(Transform parent)
    {
        var buttonObject = Instantiate(characterData.buttonPrefab, parent);
        
        if (buttonObject.TryGetComponent<ISetup<CharacterData>>(out var setupComponent))
        {
            setupComponent.Setup(characterData);
        }
        
        return buttonObject.gameObject;
    }
}