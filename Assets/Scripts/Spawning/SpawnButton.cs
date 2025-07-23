using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour, ISetup<ButtonData>
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI tmp;
    private ISpawnableFactory spawnerFactory;
    private ISpawnableFactoryCreator spawnerFactoryCreator;
    
    private void Reset()
        => button = GetComponent<Button>();

    private void Awake()
    {
        if (!button)
            button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (!button)
        {
            Debug.LogError($"{name} <color=grey>({GetType().Name})</color>: {nameof(button)} is null!");
            enabled = false;
            return;
        }
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button?.onClick?.RemoveListener(HandleClick);
    }

    public void SetupFactoryCreator(ISpawnableFactoryCreator factory)
    {
        if (factory == null)
            Debug.LogError("The factory creator being set is null!");
        else
            spawnerFactoryCreator = factory;
    }
    
    public void Setup(ButtonData buttonData)
    {
        if (tmp) tmp.text = buttonData.buttonText;

        spawnerFactory = buttonData.spawnerFactory.Ref;
    }
    
    public void HandleClick()
    {
        spawnerFactoryCreator.Spawn(spawnerFactory);
    }
}