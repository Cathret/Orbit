using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class Tower : AUnitController, IShootingEntity
    {
        #region Members
        [SerializeField]
        private AProjectile _projectileType = null;
        #endregion

        protected override void ExecuteOnClick( Vector3 target )
        {
            Shoot( target - transform.position );
        }

        public void Shoot( Vector3 direction )
        {
            direction.Normalize();
            AProjectile bullet = Instantiate( _projectileType, transform );
            bullet.transform.up = direction;
            bullet.Power = Power;
            bullet.IsFriend = true;
        }
    }
}
