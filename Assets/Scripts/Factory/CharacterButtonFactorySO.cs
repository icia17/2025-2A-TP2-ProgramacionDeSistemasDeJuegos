using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptables/CharacterButtonFactory")]
public class CharacterButtonFactorySO : ScriptableObject, ISpawnableFactory
{
    [SerializeField] private ButtonData buttonData;
    
    public void Spawn(Transform parent, ISpawnableFactoryCreator creator)
    {
        var buttonObject = Instantiate(buttonData.buttonPrefab, parent);
        
        ConfigureButton(buttonObject, creator);
    }
    
    private void ConfigureButton(SpawnButton button, ISpawnableFactoryCreator creator)
    {
        button.SetupFactoryCreator(creator);
        
        if (button.TryGetComponent<ISetup<ButtonData>>(out var setupComponent))
        {
            setupComponent.Setup(buttonData);
        }
        else
        {
            Debug.LogWarning($"{button.name} does have ISetup<ButtonData>!");
        }
    }
}
