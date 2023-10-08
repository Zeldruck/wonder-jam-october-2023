using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boss
{
    public class BossSnowFallAttackState : BossStateAttack
    {
        private SO_Attack_Snowfall attackSnowfall;
        
        private List<SnowFall> _snowFalls;

        private float durationTimer;

        private bool hasLoaded;
        
        private struct SnowFall
        {
            public GameObject snowfall;
            public float timer;

            public SnowFall(GameObject snowfall, float timer)
            {
                this.snowfall = snowfall;
                this.timer = timer;
            }
        }

        public BossSnowFallAttackState(Boss boss, BossStateMachine stateMachine, SO_Attack attack, GameObject bossHead, string animation) : base(boss, stateMachine, attack, bossHead, animation)
        {
            attackSnowfall = attack as SO_Attack_Snowfall;
        }

        public override void Enter()
        {
            base.Enter();

            hasLoaded = false;
            durationTimer = attackSnowfall.duration;
            _snowFalls = new List<SnowFall>();

            Casting();
        }
        
        public override void Exit()
        {
            base.Exit();

            foreach (var snowFall in _snowFalls)
            {
                _boss.DestroyGameObject(snowFall.snowfall);
            }

            _snowFalls = null;
        }
        
        public override void Casting()
        {
            base.Casting();
            
            // Select the zones
            for (int i = 0; i < attackSnowfall.zoneNumbers; i++)
            {
                float randomRadius = Random.Range(attackSnowfall.minDistanceFromBoss, attackSnowfall.maxDistanceFromBoss);
                
                Vector3 position;

                float theta = randomRadius * Mathf.Sqrt(Random.Range(0f, 1f));
                
                position.x = randomRadius * Mathf.Cos(theta);
                position.y = attackSnowfall.zoneYScale * 2;
                position.z = randomRadius * Mathf.Sin(theta);

                GameObject snowFall = _boss.InstantiateGameObject(attackSnowfall.prefab, position, Quaternion.identity);
                snowFall.GetComponent<SnowfallCallbacks>().onPlayerHit += OnPlayerHit;

                _snowFalls.Add(new SnowFall(snowFall, attackSnowfall.zoneLoadTime));
                
                DangerZonesManager.instance.AddDangerZone(DangerZonesManager.DangerZone.EShape.Circle, position, Vector3.zero, new Vector2(attackSnowfall.zoneScale, attackSnowfall.zoneScale), attackSnowfall.zoneLoadTime / 2, attackSnowfall.duration);
            }
        }

        public override void Update()
        {
            base.Update();

            durationTimer -= Time.deltaTime;

            if (durationTimer <= 0f)
            {
                // Finished
                _stateMachine.ChangeState(_boss.ReloadingState);
                return;
            }

            if (hasLoaded) return;
            
            for (int i = 0; i < _snowFalls.Count; i++)
            {
                var snowFall = _snowFalls[i];
                
                snowFall.timer -= Time.deltaTime;

                if (snowFall.timer <= 0f)
                    hasLoaded = true;

                float newAddY = attackSnowfall.zoneYScale * Time.deltaTime / 2f;
                
                Vector3 scale = snowFall.snowfall.transform.GetChild(0).localScale;
                scale.y += attackSnowfall.zoneYScale * Time.deltaTime / attackSnowfall.zoneLoadTime;
                snowFall.snowfall.transform.GetChild(0).localScale = scale;
                
                snowFall.snowfall.transform.GetChild(0).position -= Vector3.up * newAddY / 2f;

                _snowFalls[i] = snowFall;
            }
        }

        private void OnPlayerHit(Player player, GameObject snowfall)
        {
            Debug.Log("Player hit");
            player.ReduceLife();
        }
    }
}
