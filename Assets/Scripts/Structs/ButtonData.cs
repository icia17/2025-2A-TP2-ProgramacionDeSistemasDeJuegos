[System.Serializable]
public struct ButtonData
{
    public InterfaceRef<ISpawnableFactory> spawnerFactory;
    public SpawnButton buttonPrefab;
    public string buttonText;
}
