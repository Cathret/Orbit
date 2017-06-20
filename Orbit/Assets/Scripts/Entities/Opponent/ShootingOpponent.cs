using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent : AOpponentController,
                                    IShootingEntity
    {
        #region Members
        [SerializeField]
        private AProjectile _projectileType = null;
        #endregion

        #region Public functions
        public void Shoot( Vector3 direction )
        {
            direction.Normalize();
            AProjectile bullet = Instantiate( _projectileType, transform );
            bullet.transform.up = direction;
            bullet.Power = Power;
            bullet.IsFriend = false;
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _projectileType == null )
                Debug.Log( "ShootingOpponent.Awake() - Projectile Type is null", this );
        }
        #endregion
    }
}