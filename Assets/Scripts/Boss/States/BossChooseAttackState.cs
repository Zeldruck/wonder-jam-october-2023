using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
    public class BossChooseAttackState : BossState
    {
        private int _currentPhase;
        private float[] _attacksProbabilities;
        private BossStateAttack[] _bossStateAttacks;
        
        public BossChooseAttackState(Boss boss, BossStateMachine stateMachine, string animation) : base(boss, stateMachine, animation)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (_currentPhase != _boss.CurrentPhase)
            {
                _currentPhase = _boss.CurrentPhase;
                float[] percentages;
                (_bossStateAttacks, percentages) = _boss.GetAllAttackStatesOfCurrentPhase();
                
                float allPercent = percentages.Sum(x => x);
            
                _attacksProbabilities = new float[percentages.Length];
                for (var i = 0; i < percentages.Length; i++)
                {
                    _attacksProbabilities[i] = (i - 1 >= 0 ? _attacksProbabilities[i - 1] : 0) + percentages[i] / allPercent;
                }
            }
            
            // Choose attack
            float random = Random.Range(0f, 1f);

            int selectedIndex = 0;

            for (var i = 0; i < _attacksProbabilities.Length; i++)
            {
                if (random <= _attacksProbabilities[i])
                {
                    selectedIndex = i;
                    break;
                }
            }

            BossStateAttack selectedAttack = _bossStateAttacks[selectedIndex];
            
            _stateMachine.ChangeState(selectedAttack);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}