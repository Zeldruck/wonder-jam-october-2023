using UnityEngine;

namespace Boss
{
    public class BossState
    {
        protected Boss _boss;
        protected BossStateMachine _stateMachine;

        protected float _startTime;
        
        private string _animBoolName;

        public BossState(Boss boss, BossStateMachine stateMachine, string animation)
        {
            _boss = boss;
            _stateMachine = stateMachine;

            _animBoolName = animation;
        }

        public virtual void Enter()
        {
            _startTime = Time.time;
            
            if (!string.IsNullOrEmpty(_animBoolName) && _boss.Anim != null)
                _boss.Anim.SetBool(_animBoolName, true);
        }

        public virtual void Exit()
        {
            if (!string.IsNullOrEmpty(_animBoolName) && _boss.Anim != null)
                _boss.Anim.SetBool(_animBoolName, false);
        }

        public virtual void Update()
        {

        }
    }
}

