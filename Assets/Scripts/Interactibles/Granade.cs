using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;

public abstract class Granade : PhysicsGrabable, IPooledObject {

    private float maxLifeTime = 3f;
    private float lifeTime = 3f;
    protected NetworkVariable<bool> isTriggered = new NetworkVariable<bool>(false);
    protected bool isExploded = false;
    protected NetworkPooledObject networkPooledObject;

    public void Start() {
        networkPooledObject = gameObject.GetComponent<NetworkPooledObject>();
    }

    public void ResetState() {
        lifeTime = maxLifeTime;
        isTriggered.Value = false;
        isExploded = false;
    }

    public void UpdateState() {
        // called on server
        if (IsServer) {
            if (isTriggered.Value) {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0 && !isExploded) {
                    isExploded = true;
                    Explode();
                    networkPooledObject.ReturnToPool();
                }
            }
        }
    }

    public override void OnRelease() {
        base.OnRelease();

        if (PlayerLocalState.Singleton.GetPlayerState() != PlayerState.Alive) {
            return; // throwing only allowed when player is alive
        }
        TriggerGranadeServerRpc();
    }




    public override void OnPrimaryAction() {
        if (!grabEnabled.Value || !isGrabbed.Value || !isGrabbedLocal || ownerClientId.Value != NetworkManager.Singleton.LocalClientId) {
            return; // check if isEnabled in parent class
        }

        if (PlayerLocalState.Singleton.GetPlayerState() != PlayerState.Alive) {
            return; // shooting only allowed when player is alive
        }
        TriggerGranadeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void TriggerGranadeServerRpc() {
        isTriggered.Value = true;
    }


    protected abstract void Explode();
}
