using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePowerUp : PowerUp
{
    protected override void GivePlayerPowerUp(Player player)
    {
        player.SetCanSlide(true);
    }
}
