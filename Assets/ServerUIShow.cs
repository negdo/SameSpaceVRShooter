using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerUIShow : NetworkBehaviour
{
    [SerializeField] private GameObject ServerUI;
    // Start is called before the first frame update
    
    // on network spawn if is server show server ui and is not host
    public override void OnNetworkSpawn()
    {
        if (IsServer && !NetworkManager.Singleton.IsHost)
        {
            ServerUI.SetActive(true);
        } else {
            ServerUI.SetActive(false);
        }
    }


}
