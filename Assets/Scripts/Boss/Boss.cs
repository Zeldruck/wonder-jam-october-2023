using System;
using UnityEngine;

namespace Boss
{
    public class Boss : MonoBehaviour
    {
        public BossStateMachine StateMachine { get; private set; }
        public Animator Anim { get; private set; }
        
        private int _currentPhase;
        private int _currentHealth;
        private bool isInvulnerable;
        
        [SerializeField] private BossData _bossData;

        #region States
 
        public BossIdleState IdleState { get; private set; }
        public BossReloadingState ReloadingState { get; private set; }
        public BossRefillingState RefillingState { get; private set; }
        public BossDieState DieState { get; private set; }

        #endregion

        private void Awake()
        {
            Anim = GetComponent<Animator>();
            
            StateMachine = new BossStateMachine();

            IdleState = new BossIdleState(this, StateMachine, null, "Idle");
            ReloadingState = new BossReloadingState(this, StateMachine, null, "Reloading");
            
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.Update();
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