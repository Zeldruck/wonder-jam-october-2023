using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class DangerZonesManager : MonoBehaviour
    {
        public static DangerZonesManager instance;

        private List<DangerZone> _dangerZones;

        private void Awake()
        {
            if (instance != null && instance == this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                _dangerZones = new List<DangerZone>();
            }
        }

        public int AddDangerZone(DangerZone.EShape shape, Vector3 position, float size, float loadTime)
        {
            int id = _dangerZones.Count > 0 ? _dangerZones[^1].id : 0;
            
            _dangerZones.Add(new DangerZone(id, shape, position, size, loadTime));

            return id;
        }

        #region Structs

        [Serializable]
        public struct DangerZone
        {
            private bool loaded;
            
            public int id;
            public EShape shape;
            public Vector3 position;
            public float size;
            public float loadTime;

            public DangerZone(int id, EShape shape, Vector3 position, float size, float loadTime)
            {
                this.id = id;
                this.shape = shape;
                this.position = position;
                this.size = size;
                this.loadTime = loadTime;
                
                loaded = false;
            }

            public void SetLoaded() => loaded = true;

            public enum EShape
            {
                Square,
                Circle
            }
        }

        #endregion
    }
}