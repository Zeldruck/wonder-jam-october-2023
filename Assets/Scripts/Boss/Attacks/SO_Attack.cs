using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public abstract class SO_Attack : ScriptableObject
    {
        [Header("Basics")]
        public int damage;
        public float cooldown;
        public float waitAfterAttack;
        public bool canUseInARow;
    }
}