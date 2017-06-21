using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent : AOpponentController,
                                    IShootingEntity
    {
        
        protected override void Start()
        {
            base.Start();

            Vector3 center = GameGrid.Instance.RealCenter;
            Vector3 distance = ( center - transform.position ).normalized
                               * GameGrid.Instance.RealEfficientSide;
            WayPoints.Add(center + distance);
            WayPoints.Add( transform.position );
        }

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