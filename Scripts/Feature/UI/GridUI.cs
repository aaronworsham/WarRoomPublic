using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sazboom.WarRoom
{
    public class GridUI : MonoBehaviour
    {
        [SerializeField] private float _gridUnit = 2.0f;
        [SerializeField] private int _gridSize = 5;
        [SerializeField] private GameObject _gridPoint;

        List<GameObject> gridPoints = new List<GameObject>();

        public Vector3 GetNearestPointOnGrid(Vector3 origin, Vector3 dest)
        {
            dest -= origin;

            int xCount = Mathf.RoundToInt(dest.x / _gridUnit);
            int yCount = Mathf.RoundToInt(dest.y / _gridUnit);
            int zCount = Mathf.RoundToInt(dest.z / _gridUnit);

            Vector3 result = new Vector3(
                Mathf.RoundToInt((float)xCount * _gridUnit),
                Mathf.RoundToInt((float)yCount),
                Mathf.RoundToInt((float)zCount * _gridUnit));

            result += origin;

            return result;

        }


        public void DrawGrid(Vector3 origin)
        {
            if(gridPoints.Count > 0)
            {
                DestroyGrid();
            }

        
            for (float x = _gridSize * -1; x < _gridSize; x += _gridUnit)
            {
                for (float z = _gridSize * -1; z < _gridSize; z += _gridUnit)
                {
                    Vector3 pos = new Vector3(
                        (float)origin.x + x,
                        (float)origin.y + 1,
                        (float)origin.z + z);

                    var point = GetNearestPointOnGrid(origin, pos);
                    gridPoints.Add(Instantiate(_gridPoint, point, Quaternion.identity));
                }
            }
        }

        public void DestroyGrid()
        {
            foreach ( GameObject obj in gridPoints)
            {
                Destroy(obj);
            }
        }
    }
}


