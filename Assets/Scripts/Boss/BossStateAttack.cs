using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossStateAttack : BossState
    {
        private SO_Attack _attack;
        
        public BossStateAttack(Boss boss, BossStateMachine stateMachine, SO_Attack attack, string animation) : base(boss, stateMachine, animation)
        {
            _attack = attack;
        }

        public virtual void Casting()
        {
            
        }
    }
}