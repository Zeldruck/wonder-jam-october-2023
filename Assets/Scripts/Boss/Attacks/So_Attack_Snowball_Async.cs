using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "Snowball Attack Async", menuName = "Boss/Attacks/Snowball Async")]
    public class So_Attack_Snowball_Async : SO_Attack
    {
        [Header("Snow Ball")]
        public float duration;
        public int zoneNumbers;
        public float zoneScale;
        public float ySpawnPosition;
        public float minDistanceFromBoss;
        public float maxDistanceFromBoss;
        public float minDistanceBetweenZones;

        [Header("Async")]
        public Vector2 rangeTimeBetweenSpawn;
    }
}