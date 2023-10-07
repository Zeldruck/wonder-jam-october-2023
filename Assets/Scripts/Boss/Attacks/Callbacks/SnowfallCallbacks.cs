using System;
using UnityEngine;

public class SnowfallCallbacks : MonoBehaviour
{
    public Action<Player, GameObject> onPlayerHit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Player player = other.gameObject.GetComponent<Player>();
        
        if (player == null) return;
        
        onPlayerHit?.Invoke(player, gameObject);
    }
}
