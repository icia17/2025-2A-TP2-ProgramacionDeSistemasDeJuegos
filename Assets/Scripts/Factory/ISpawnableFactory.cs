using UnityEngine;

public interface ISpawnableFactory
{
    void Spawn(Transform transform, ISpawnableFactoryCreator creator = null);
}