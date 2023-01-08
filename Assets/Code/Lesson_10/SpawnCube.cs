using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    public void Create()
    {
        Instantiate(_prefab);
    }
}
