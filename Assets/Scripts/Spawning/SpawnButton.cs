using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour, ISetup<CharacterData>
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI tmp;
    private CharacterData characterData;
    
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

    public void Setup(CharacterData data)
    {
        characterData = data;
        if (tmp) tmp.text = data.buttonText;
    }
    
    public void HandleClick()
    {
        CharacterSpawner.Instance?.Spawn(characterData);
    }
}