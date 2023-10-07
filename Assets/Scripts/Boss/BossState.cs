using System.Collections;
using System.Collections.Generic;
using Boss;
using UnityEditor;
using UnityEngine;

namespace Boss
{
    public class BossState
    {
        protected Boss _boss;
        protected BossStateMachine _stateMachine;

        protected float _startTime;

        private SO_Attack _attack;
        private string _animBoolName;

        public BossState(Boss boss, BossStateMachine stateMachine, SO_Attack attack, string animation)
        {
            _boss = boss;
            _stateMachine = stateMachine;
            _attack = attack;
            _animBoolName = animation;
        }

        public virtual void Enter()
        {
            _startTime = Time.time;
            
            _boss.Anim.SetBool(_animBoolName, true);
        }

        public virtual void Exit()
        {
            _boss.Anim.SetBool(_animBoolName, false);
        }

        public virtual void Update()
        {

        }
    }
}

