using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossSnowballAsyncAttackState : BossStateAttack
    {
        private So_Attack_Snowball_Async attackSnowball;
        
        private List<Snowball> _snowBalls;
        
        private struct Snowball
        {
            public GameObject snowball;
            public float timer;

            public Snowball(GameObject snowball, float timer)
            {
                this.snowball = snowball;
                this.timer = timer;
            }
        }

        private float spawnNextTimer;
        private int spawnCount;
        
        public BossSnowballAsyncAttackState(Boss boss, BossStateMachine stateMachine, SO_Attack attack, GameObject bossHead, string animation) : base(boss, stateMachine, attack, bossHead, animation)
        {
            attackSnowball = attack as So_Attack_Snowball_Async;
        }

        public override void Enter()
        {
            base.Enter();

            spawnCount = 0;
            spawnNextTimer = Random.Range(attackSnowball.rangeTimeBetweenSpawn.x, attackSnowball.rangeTimeBetweenSpawn.y);
            _snowBalls = new List<Snowball>();
            
            Casting();
        }
        
        public override void Exit()
        {
            base.Exit();

            foreach (var snowBall in _snowBalls)
            {
                _boss.DestroyGameObject(snowBall.snowball);
            }

            _snowBalls = null;
        }
        
        public override void Casting()
        {
            base.Casting();
            
            spawnNextTimer = Random.Range(attackSnowball.rangeTimeBetweenSpawn.x, attackSnowball.rangeTimeBetweenSpawn.y);
            spawnCount++;
            
            float randomRadius = Random.Range(attackSnowball.minDistanceFromBoss, attackSnowball.maxDistanceFromBoss);

            float theta = randomRadius * Mathf.Sqrt(Random.Range(0f, 1f));
                
            Vector3 position;
            position.x = randomRadius * Mathf.Cos(theta);
            position.y = attackSnowball.ySpawnPosition;
            position.z = randomRadius * Mathf.Sin(theta);

            GameObject snowBall = _boss.InstantiateGameObject(attackSnowball.prefab, position, Quaternion.identity);
            snowBall.GetComponent<SnowfallCallbacks>().onPlayerHit += OnPlayerHit;

            _snowBalls.Add(new Snowball(snowBall, attackSnowball.duration));
            
            DangerZonesManager.instance.AddDangerZone(DangerZonesManager.DangerZone.EShape.Circle, position, Vector3.zero, new Vector2(attackSnowball.zoneScale, attackSnowball.zoneScale), attackSnowball.duration / 2, attackSnowball.duration);
        }

        public override void Update()
        {
            base.Update();

            spawnNextTimer -= Time.deltaTime;
            
            if (spawnNextTimer <= 0 && spawnCount < attackSnowball.zoneNumbers)
                Casting();
            
            if (spawnCount >= attackSnowball.zoneNumbers && _snowBalls.Count <= 0)
            {
                // Finished
                Debug.Log("Attack Finished");
                _stateMachine.ChangeState(_boss.ReloadingState);
                return;
            }
            
            for (int i = 0; i < _snowBalls.Count; i++)
            {
                var snowball = _snowBalls[i];
                
                Vector3 nPosition = snowball.snowball.transform.position;
                nPosition.y -= Time.deltaTime * attackSnowball.ySpawnPosition / attackSnowball.duration;
                snowball.snowball.transform.position = nPosition;

                snowball.timer -= Time.deltaTime;

                _snowBalls[i] = snowball;

                if (snowball.timer <= 0f)
                {
                    _boss.DestroyGameObject(snowball.snowball);
                    _snowBalls.RemoveAt(i);
                    i--;
                    
                    // Particles & sound effect
                }
            }
        }

        private void OnPlayerHit(Player player, GameObject snowfall)
        {
            Debug.Log("Player hit");
            player.ReduceLife();
        }
    }
}
