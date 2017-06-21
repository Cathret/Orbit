using UnityEngine;

namespace Orbit.Entity.Unit
{
    public abstract class ATower : AUnitController, IShootingEntity
    {
        #region Members
        [SerializeField]
        private Projectile _projectileType = null;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _projectileType == null )
                Debug.LogError( "ATower.Awake() - Projectile Type is null, need to be set in Editor", this );
        }
        #endregion

        public void Shoot( Vector3 direction )
        {
            direction.Normalize();
            Projectile bullet = Instantiate( _projectileType, transform.position, transform.rotation );
            bullet.transform.up = direction;
            bullet.Power = Power;
            bullet.IsFriend = true;
        }
    }
}