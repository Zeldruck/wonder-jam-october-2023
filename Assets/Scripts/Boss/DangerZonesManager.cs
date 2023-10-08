using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Boss
{
    public class DangerZonesManager : MonoBehaviour
    {
        public static DangerZonesManager instance;

        private List<DangerZone> _dangerZones;

        [SerializeField] private GameObject circularShapePrefab;
        [SerializeField] private GameObject squareShapePrefab;

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

        public void AddDangerZone(DangerZone.EShape shape, Vector3 position, Vector3 orientation, Vector2 size, float loadTime, float duration)
        {
            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit))
            {
                GameObject shapeObject = null;
                Vector3 finalPosition = hit.point + Vector3.up * 0.05f;

                switch (shape)
                {
                    case DangerZone.EShape.Circle:
                        shapeObject = Instantiate(circularShapePrefab, finalPosition, circularShapePrefab.transform.rotation);
                        break;
                    
                    case DangerZone.EShape.Square:
                        shapeObject = Instantiate(squareShapePrefab, finalPosition, Quaternion.Euler(orientation));
                        break;
                }
                
                _dangerZones.Add(new DangerZone(shape, shapeObject, size, loadTime, duration));
            }
        }

        public void RemoveAllZones()
        {
            for (int i = 0; i < _dangerZones.Count; i++)
            {
                Destroy(_dangerZones[i].shapeObject);
                _dangerZones.RemoveAt(i);
                i--;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _dangerZones.Count; i++)
            {
                if (!_dangerZones[i].isLoaded)
                {
                    _dangerZones[i].loadTime -= Time.deltaTime;
                    _dangerZones[i].isLoaded = _dangerZones[i].loadTime <= 0f;
                    Vector3 fScale = _dangerZones[i].shapeObject.transform.localScale;
                    fScale += (Vector3)(_dangerZones[i].size * Time.deltaTime / _dangerZones[i].FinalLoadTime);
                    _dangerZones[i].shapeObject.transform.localScale = fScale;
                }

                _dangerZones[i].duration -= Time.deltaTime;

                if (_dangerZones[i].duration <= 0f)
                {
                    Destroy(_dangerZones[i].shapeObject);
                    _dangerZones.RemoveAt(i);
                    i--;
                }
            }
        }

        #region Structs
        
        public class DangerZone
        {
            public float FinalLoadTime { get; private set; }
            
            public bool isLoaded;
            
            public EShape shape;
            public GameObject shapeObject;
            public Vector2 size;
            public float loadTime;
            public float duration;

            public DangerZone(EShape shape, GameObject shapeObject, Vector2 size, float loadTime, float duration)
            {
                this.shape = shape;
                this.shapeObject = shapeObject;
                this.size = size;
                this.loadTime = loadTime;
                this.duration = duration;

                FinalLoadTime = loadTime;
                
                isLoaded = false;
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