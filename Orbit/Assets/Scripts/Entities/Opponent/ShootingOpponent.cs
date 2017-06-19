using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent : AOpponentController, IShootingEntity
    {
        #region Members

        [SerializeField]
        private readonly AProjectile _projectileType = null;

        #endregion

        #region Public functions

        public void Shoot()
        {
            AProjectile bullet = Instantiate( _projectileType, transform );
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