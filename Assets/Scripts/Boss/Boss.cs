using System;
using UnityEngine;

namespace Boss
{
    public class Boss : MonoBehaviour
    {
        private int _currentPhase;
        private int _currentHealth;

        [SerializeField] private int _phaseNumbers;
        [SerializeField] private BossPhase[] _bossPhases;

        private void Update()
        {
            BossLoop();
        }

        private void BossLoop()
        {
            
        }
    }

    #region BossStructs
    [System.Serializable]
    public struct BossPhase
    {
        public float phaseHealth;
        public BossAttackProbability[] phaseAttacks;
    }

    [System.Serializable]
    public struct BossAttackProbability
    {
        public SO_Attack phaseAttack;
        [Range(0f, 1f)] public float probability;
    }
    #endregion
}