using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "Snowfall Attack", menuName = "Boss/Attacks/Snowfall")]
    public class SO_Attack_Snowfall : SO_Attack
    {
        [Header("Snow Fall")]
        public float duration;
        public int zoneNumbers;
        public float zoneScale;
        public float minDistanceBetweenZones;
        
    }
}