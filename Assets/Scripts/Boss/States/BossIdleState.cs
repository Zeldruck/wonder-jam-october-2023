using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossIdleState : BossState
    {
        public BossIdleState(Boss boss, BossStateMachine stateMachine, string animation) : base(boss, stateMachine, animation)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
