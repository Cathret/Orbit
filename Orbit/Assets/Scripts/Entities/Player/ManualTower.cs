using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ManualTower : ATower
    {
        public override void ExecuteOnClick( Vector3 target )
        {
            Shoot( target - transform.position );
        }
    }
}