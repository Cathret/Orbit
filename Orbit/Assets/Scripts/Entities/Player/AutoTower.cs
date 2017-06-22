using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class AutoTower : ATower
    {
        protected override void Awake()
        {
            base.Awake();
            FollowMouse = false;
        }

        protected override void Update()
        {
            base.Update();
            if ( _canShoot )
                AutoShoot();
        }

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        private bool FindClosestOpponent( Transform cell, out Vector3 target )
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = cell.position;
            foreach (AOpponentController t in AOpponentController.OpponentList)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
            if ( tMin )
            {
                target = tMin.position;
                return true;
            }
            target = new Vector3();
            return false;
        }

        private void AutoShoot()
        {
            if (Cell.Connected)
            {
                Vector3 target;
                if (FindClosestOpponent(Cell.transform, out target))
                {
                    Shoot(target - transform.position);
                    if (Head)
                        Head.transform.right = (target - transform.position).normalized;
                }
            }
        }
    }
}