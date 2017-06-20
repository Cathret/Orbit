using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class AutoTower : AUnitController, IShootingEntity
    {
        protected override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        public void Shoot( Vector3 direction )
        {
            // Shoot
        }
    }
}