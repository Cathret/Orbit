using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ManualTower : ATower
    {
        #region Protected functions
        protected override void ExecuteOnClick( Vector3 target )
        {
            Shoot( target - transform.position );
        }
        #endregion
    }
}
