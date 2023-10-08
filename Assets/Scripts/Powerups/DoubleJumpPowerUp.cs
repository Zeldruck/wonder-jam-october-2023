using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerUp : PowerUp
{
    protected override void GivePlayerPowerUp(Player player)
    {
        player.SetAirJumpCount(1);
    }
}
