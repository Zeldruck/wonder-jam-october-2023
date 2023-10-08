using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected abstract void GivePlayerPowerUp(Player player);
    private void OnTriggerEnter(Collider other)
    {
        GivePlayerPowerUp(other.GetComponent<Player>());
        gameObject.SetActive(false);
    }
}
