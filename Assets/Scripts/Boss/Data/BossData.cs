using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "Boss Data", menuName = "Boss/Boss Data")]
    public class BossData : ScriptableObject
    {
        [SerializeField] private int _phaseNumbers;
        [SerializeField] private BossPhase[] _bossPhases;
    }

    [Serializable]
    public struct BossPhase
    {
        public float phaseHealth;
        public BossAttackProbability[] phaseAttacks;
    }

    [Serializable]
    public struct BossAttackProbability
    {
        public SO_Attack phaseAttack;
        [Range(0f, 1f)] public float probability;
    }

}