using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{        
    public class WaypointContainer : MonoBehaviour
    {
        [SerializeField] GameObject[] waypoints;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDrawGizmos() {
            Transform firstPoint = transform.GetChild(0);
            Transform previousPoint = firstPoint;
            foreach (Transform point in transform)
            {
                Gizmos.color = new Color(66f, 134f, 244f, 1f);
                Gizmos.DrawSphere(point.position, 0.2f);
                Gizmos.DrawLine(previousPoint.position, point.position);
                previousPoint = point;
            }
            Gizmos.DrawLine(previousPoint.position, firstPoint.position);
        }
    }
}