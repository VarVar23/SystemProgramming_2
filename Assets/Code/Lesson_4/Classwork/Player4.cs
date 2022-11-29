using UnityEngine;
using UnityEngine.Networking;

public class Player4 : NetworkBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    private GameObject _playerCharacter;

    private void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        if (!isServer) return;

        _playerCharacter = Instantiate(_playerPrefab);

        NetworkServer.SpawnWithClientAuthority(_playerCharacter, connectionToClient);
    }
}
