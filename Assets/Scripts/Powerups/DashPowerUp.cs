using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowerUp : PowerUp
{
    protected override void GivePlayerPowerUp(Player player)
    {
        player.SetCanDash(true);
    }
}
