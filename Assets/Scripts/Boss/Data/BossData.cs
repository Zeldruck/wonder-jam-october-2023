using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "Boss Data", menuName = "Boss/Boss Data")]
    public class BossData : ScriptableObject
    {
        public int _phaseNumbers;
        public BossPhase[] _bossPhases;
    }

    [Serializable]
    public struct BossPhase
    {
        public float phaseHealth;
        public BossAttackProbability[] phaseAttacks;
        public float refillingTime;
        public float waitAfterAttack;
    }

    [Serializable]
    public struct BossAttackProbability
    {
        public SO_Attack phaseAttack;
        [Range(0f, 1f)] public float probability;
    }

}