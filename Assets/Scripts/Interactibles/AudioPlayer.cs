using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AudioPlayer : NetworkBehaviour {
    [SerializeField] private AudioSource audioSource;

    public void playAudio() {
        if (IsServer) {
            PlayAudioClientRpc();
        } else {
            PlayAudioServerRpc();
        }
    }

    [ServerRpc]
    public void PlayAudioServerRpc() {
        PlayAudioClientRpc();
    }

    [ClientRpc]
    public void PlayAudioClientRpc() {
        audioSource.PlayOneShot(audioSource.clip);
    }
}