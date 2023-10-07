using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public abstract class SO_Attack : ScriptableObject
    {
        [Header("Basics")]
        public GameObject prefab;
        public int damage;
        public float cooldown;
        public float waitAfterAttack;
        public bool canUseInARow;
        public string animation;
    }
}