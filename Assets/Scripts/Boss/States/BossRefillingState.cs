using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossRefillingState : BossState
    {
        private int _currentPhase;
        private float _refillingTime;
        private float _maxHealth;
        
        public BossRefillingState(Boss boss, BossStateMachine stateMachine, string animation) : base(boss, stateMachine, animation)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _boss.SetInvulnerable(true);
            
            _boss.BossAudio.PlaySound("refilling");

            _currentPhase = _boss.CurrentPhase;

            _refillingTime = _boss.BossData._bossPhases[_currentPhase].refillingTime;
            _maxHealth = _boss.BossData._bossPhases[_currentPhase].phaseHealth;
        }

        public override void Exit()
        {
            base.Exit();
            
            _boss.BossAudio.StopSounds();
            _boss.BossAudio.PlaySound("roar");
            
            _boss.SetInvulnerable(false);
        }

        public override void Update()
        {
            base.Update();

            _boss.SetHealth(_boss.CurrentHealth + _maxHealth * Time.deltaTime / _refillingTime);

            if (_boss.CurrentHealth >= _maxHealth)
            {
                _boss.SetHealth(_maxHealth);
                // Exit state to roar state, then choose attack state
                _stateMachine.ChangeState(_boss.ReloadingState);
            }
        }
    }
}
