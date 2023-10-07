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

        public void AddDangerZone(DangerZone.EShape shape, Vector3 position, float size, float loadTime)
        {
            _dangerZones.Add(new DangerZone(shape, position, size, loadTime));
        }

        private void Update()
        {
            foreach (var dangerZone in _dangerZones)
            {
                float newLoadTime = dangerZone.loadTime - Time.deltaTime;
                
                dangerZone.ChangeLoadTime(newLoadTime);

                if (dangerZone.IsLoaded)
                {
                    _dangerZones.Remove(dangerZone);
                }
            }
        }

        #region Structs

        [Serializable]
        public struct DangerZone
        {
            public bool IsLoaded { get; private set; }
            
            public EShape shape;
            public Vector3 position;
            public float size;
            public float loadTime;

            public DangerZone(EShape shape, Vector3 position, float size, float loadTime)
            {
                this.shape = shape;
                this.position = position;
                this.size = size;
                this.loadTime = loadTime;
                
                IsLoaded = false;
            }

            public void ChangeLoadTime(float newLoadTime)
            {
                loadTime = newLoadTime;

                if (loadTime < 0f)
                {
                    IsLoaded = true;
                }
            }

            public enum EShape
            {
                Square,
                Circle
            }
        }

        #endregion
    }
}