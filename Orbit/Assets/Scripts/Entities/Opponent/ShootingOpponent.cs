using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent : AOpponentController,
                                    IShootingEntity
    {
        #region Members
        [SerializeField]
        private Projectile _projectileType = null;
        #endregion

        #region Public functions
        public void Shoot( Vector3 direction )
        {
            direction.Normalize();
            Projectile bullet = Instantiate( _projectileType, transform.position, transform.rotation );
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
                Debug.LogError( "ShootingOpponent.Awake() - Projectile Type is null, need to be set in Editor", this );
        }
        #endregion
    }
}