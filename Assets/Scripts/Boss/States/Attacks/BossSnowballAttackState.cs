using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossSnowballAttackState : BossStateAttack
    {
        private SO_Attack_Snowball attackSnowball;
        
        private List<GameObject> _snowBalls;

        private float durationTimer;
        
        public BossSnowballAttackState(Boss boss, BossStateMachine stateMachine, SO_Attack attack, string animation) : base(boss, stateMachine, attack, animation)
        {
            attackSnowball = attack as SO_Attack_Snowball;
        }

        public override void Enter()
        {
            base.Enter();

            durationTimer = attackSnowball.duration;
            _snowBalls = new List<GameObject>();
            
            Casting();
        }
        
        public override void Exit()
        {
            base.Exit();

            foreach (var snowBall in _snowBalls)
            {
                _boss.DestroyGameObject(snowBall);
            }

            _snowBalls = null;
        }
        
        public override void Casting()
        {
            base.Casting();
            
            // Select the zones
            for (int i = 0; i < attackSnowball.zoneNumbers; i++)
            {
                float randomRadius = Random.Range(attackSnowball.minDistanceFromBoss, attackSnowball.maxDistanceFromBoss);
                
                Vector3 position;

                float theta = randomRadius * Mathf.Sqrt(Random.Range(0f, 1f));
                
                position.x = randomRadius * Mathf.Cos(theta);
                position.y = attackSnowball.ySpawnPosition;
                position.z = randomRadius * Mathf.Sin(theta);

                GameObject snowBall = _boss.InstantiateGameObject(attackSnowball.prefab, position, Quaternion.identity);
                snowBall.GetComponent<SnowfallCallbacks>().onPlayerHit += OnPlayerHit;

                _snowBalls.Add(snowBall);
            }
        }

        public override void Update()
        {
            base.Update();

            durationTimer -= Time.deltaTime;

            if (durationTimer <= 0f)
            {
                // Finished
                //_stateMachine.ChangeState();
                
                // DEBUG
                Debug.Log("Attack Finished");
                _stateMachine.ChangeState(_boss.ChooseAttackState);
                return;
            }
            
            foreach (var snowball in _snowBalls)
            {
                Vector3 nPosition = snowball.transform.position;

                nPosition.y -= Time.deltaTime * attackSnowball.ySpawnPosition / attackSnowball.duration;
                
                snowball.transform.position = nPosition;
            }
        }

        private void OnPlayerHit(Player player, GameObject snowfall)
        {
            Debug.Log("Player hit");
        }
    }
}
