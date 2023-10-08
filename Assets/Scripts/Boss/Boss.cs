using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class Boss : MonoBehaviour
    {
        public BossStateMachine StateMachine { get; private set; }
        public Animator Anim { get; private set; }
        public BossAudio BossAudio { get; private set; }
        
        private int _currentPhase;
        public int CurrentPhase => _currentPhase;
        private float _currentHealth;
        public float CurrentHealth => _currentHealth;
        private bool _isInvulnerable;
        public bool IsInvulnerable => _isInvulnerable;
        
        [SerializeField] private BossData _bossData;
        public BossData BossData => _bossData;

        [SerializeField] private Player _player;
        public Player PlayerP => _player;

        [SerializeField] private GameObject _bossHead;

        #region States
 
        public BossIdleState IdleState { get; private set; }
        public BossReloadingState ReloadingState { get; private set; }
        public BossRefillingState RefillingState { get; private set; }
        public BossDieState DieState { get; private set; }
        
        public BossChooseAttackState ChooseAttackState { get; private set; }
        
        public List<List<BossStateAttack>> BossPhaseAttackStates { get; private set; }

        #endregion

        private void Awake()
        {
            Anim = GetComponent<Animator>();
            BossAudio = GetComponent<BossAudio>();
            
            StateMachine = new BossStateMachine();

            IdleState = new BossIdleState(this, StateMachine, "Idle");
            ReloadingState = new BossReloadingState(this, StateMachine, "Reloading");
            RefillingState = new BossRefillingState(this, StateMachine, "Refilling");
            DieState = new BossDieState(this, StateMachine, "Die");

            ChooseAttackState = new BossChooseAttackState(this, StateMachine, "");
            
            BossPhaseAttackStates = new List<List<BossStateAttack>>();
            
            for (int i = 0; i < _bossData._bossPhases.Length; i++)
            {
                List<BossStateAttack> stateAttacks = new List<BossStateAttack>();

                for (int j = 0; j < _bossData._bossPhases[i].phaseAttacks.Length; j++)
                {
                    switch (_bossData._bossPhases[i].phaseAttacks[j].phaseAttack)
                    {
                        case SO_Attack_Snowfall snowfall:
                            stateAttacks.Add(new BossSnowFallAttackState(this, StateMachine, snowfall, _bossHead, snowfall.animation));
                            break;
                        
                        case SO_Attack_Snowball snowball:
                            stateAttacks.Add(new BossSnowballAttackState(this, StateMachine, snowball, _bossHead, snowball.animation));
                            break;
                        
                        case So_Attack_Snowball_Async snowballAsync:
                            stateAttacks.Add(new BossSnowballAsyncAttackState(this, StateMachine, snowballAsync, _bossHead, snowballAsync.animation));
                            break;
                    }
                }
                
                if (stateAttacks.Count > 0)
                    BossPhaseAttackStates.Add(stateAttacks);
            }
        }

        private void Start()
        {
            //StateMachine.Initialize(IdleState);
            StateMachine.Initialize(ChooseAttackState);
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
            if (_currentHealth > 0f)
                BossAudio.PlaySound("damage");
            
            _currentHealth -= damages;

            if (_currentHealth <= 0f) return;

            PhaseChange();
        }

        public void ReceiveHeal(float heal)
        {
            if (_currentHealth < _bossData._bossPhases[_currentPhase].phaseHealth)
                BossAudio.PlaySound("heal");
            
            _currentHealth += heal;
            
            if (_currentHealth < _bossData._bossPhases[_currentPhase].phaseHealth) return;

            _currentHealth = _bossData._bossPhases[_currentPhase].phaseHealth;
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

        public void DestroyGameObject(GameObject toDestroy) => Destroy(toDestroy);
        
        public GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instantiate(prefab, position, rotation);
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