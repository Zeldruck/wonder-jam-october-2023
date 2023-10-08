using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossStateAttack : BossState
    {
        private SO_Attack _attack;
        private GameObject _head;

        private float _rSpeed = 1.5f;
        
        public BossStateAttack(Boss boss, BossStateMachine stateMachine, SO_Attack attack, GameObject bossHead, string animation) : base(boss, stateMachine, animation)
        {
            _attack = attack;
            _head = bossHead;
        }

        public virtual void Casting()
        {
            
        }

        public override void Update()
        {
            base.Update();
            
            // Follow the player
            Vector3 playerPosition = _boss.PlayerP.transform.position;
            playerPosition.y = _boss.transform.position.y;
            
            var targetRotation = Quaternion.LookRotation(playerPosition - _boss.transform.position);
            _boss.transform.rotation = Quaternion.Slerp(_boss.transform.rotation, targetRotation, _rSpeed * Time.deltaTime);
            
            
            // Head
            targetRotation = Quaternion.LookRotation(_boss.PlayerP.transform.position - _head.transform.position);
            _head.transform.rotation = Quaternion.Slerp(_head.transform.rotation, targetRotation, 5f * Time.deltaTime);
            
            
            //_head.transform.LookAt(_boss.PlayerP.transform.position);
        }
    }
}