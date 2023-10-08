using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossReloadingState : BossState
    {
        private float _timeToWait;

        public BossReloadingState(Boss boss, BossStateMachine stateMachine, string animation) : base(boss, stateMachine, animation)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _timeToWait = _boss.BossData._bossPhases[_boss.CurrentPhase].waitAfterAttack;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            _timeToWait -= Time.deltaTime;

            if (_timeToWait <= 0f)
            {
                // Finished
                _stateMachine.ChangeState(_boss.ChooseAttackState);
                return;
            }
        }
    }

}