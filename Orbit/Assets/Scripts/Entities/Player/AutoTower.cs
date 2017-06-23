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

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();
            if ( _canShoot )
                AutoShoot();
        }

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        private void AutoShoot()
        {
            if ( Cell.Connected )
            {
                Vector3 target;
                if ( OpponentManager.Instance.FindClosestOpponent( Cell.transform, out target ) )
                {
                    Shoot( target - transform.position );
                    if ( Head )
                        Head.transform.right = ( target - transform.position ).normalized;
                }
            }
        }
    }
}