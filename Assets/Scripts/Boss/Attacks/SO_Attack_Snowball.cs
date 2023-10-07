using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "Snowball Attack", menuName = "Boss/Attacks/Snowball")]
    public class SO_Attack_Snowball : SO_Attack
    {
        [Header("Snow Ball")]
        public float duration;
        public int zoneNumbers;
        public float zoneScale;
        public float ySpawnPosition;
        public float minDistanceFromBoss;
        public float maxDistanceFromBoss;
        public float minDistanceBetweenZones;
    }
}
