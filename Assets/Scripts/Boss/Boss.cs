using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class Boss : MonoBehaviour
    {
        public BossStateMachine StateMachine { get; private set; }
        public Animator Anim { get; private set; }
        
        private int _currentPhase;
        public int CurrentPhase => _currentPhase;
        private float _currentHealth;
        public float CurrentHealth => _currentHealth;
        private bool _isInvulnerable;
        public bool IsInvulnerable => _isInvulnerable;
        
        [SerializeField] private BossData _bossData;
        public BossData BossData => _bossData;

        #region States
 
        public BossIdleState IdleState { get; private set; }
        public BossReloadingState ReloadingState { get; private set; }
        public BossRefillingState RefillingState { get; private set; }
        public BossDieState DieState { get; private set; }
        
        public List<List<BossStateAttack>> BossPhaseAttackStates { get; private set; }

        #endregion

        private void Awake()
        {
            Anim = GetComponent<Animator>();
            
            StateMachine = new BossStateMachine();

            IdleState = new BossIdleState(this, StateMachine, "Idle");
            ReloadingState = new BossReloadingState(this, StateMachine, "Reloading");
            RefillingState = new BossRefillingState(this, StateMachine, "Refilling");
            DieState = new BossDieState(this, StateMachine, "Die");

            BossPhaseAttackStates = new List<List<BossStateAttack>>();
            
            for (int i = 0; i < _bossData._bossPhases.Length; i++)
            {
                List<BossStateAttack> stateAttacks = new List<BossStateAttack>();

                for (int j = 0; j < _bossData._bossPhases[i].phaseAttacks.Length; j++)
                {
                    switch (_bossData._bossPhases[i].phaseAttacks[j].phaseAttack)
                    {
                        case SO_Attack_Snowfall snowfall:
                            stateAttacks.Add(new BossSnowFallAttackState(this, StateMachine, snowfall, snowfall.animation));
                            break;
                    }
                }
                
                if (stateAttacks.Count > 0)
                    BossPhaseAttackStates.Add(stateAttacks);
            }
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.Update();
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            _isInvulnerable = isInvulnerable;
        }
        
        public void SetHealth(float newHealth)
        {
            _currentHealth = newHealth;
        }

        public void ReceiveDamages(float damages)
        {
            _currentHealth -= damages;

            if (_currentHealth <= 0f) return;

            PhaseChange();
        }

        private void PhaseChange()
        {
            _currentPhase++;

            if (_currentPhase >= _bossData._phaseNumbers)
            {
                // Victory
                Debug.Log("Victory");
                
                StateMachine.ChangeState(DieState);
                return;
            }
            
            StateMachine.ChangeState(RefillingState);
        }

        public (BossStateAttack[], float[]) GetAllAttackStatesOfCurrentPhase()
        {
            float[] probs = new float[_bossData._bossPhases[_currentPhase].phaseAttacks.Length];

            for (int i = 0; i < probs.Length; i++)
                probs[i] = _bossData._bossPhases[_currentPhase].phaseAttacks[i].probability;

            return (BossPhaseAttackStates[_currentPhase].ToArray(), probs);
        }
    }

    #region BossStructs
    
    public enum EBossStatus
    {
        None = 0,
        Casting = 1,
        Attacking = 2,
        Reloading = 3,
        Waiting = 4,
        Refilling = 5
    }
    #endregion
}