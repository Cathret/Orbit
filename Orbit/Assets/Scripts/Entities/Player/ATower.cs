using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public abstract class ATower : AUnitController, IShootingEntity
    {
        #region Members
        [SerializeField]
        private float _shootCooldown = 0.5f;

        private float _shootTimer = 0.0f;

        public float ShootTimer
        {
            get { return _shootTimer; }
            private set
            {
                _shootTimer = value;
                if ( _shootTimer > _shootCooldown )
                {
                    _canShoot = true;
                    _shootTimer = 0.0f;
                }
            }
        }

        [SerializeField]
        private Projectile _projectileType = null;

        private bool _canShoot = true;
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
            if ( !_canShoot )
                return;

            direction.Normalize();
            Projectile bullet = Instantiate( _projectileType, transform.position, transform.rotation );
            bullet.transform.up = direction;
            bullet.Power = Power;
            bullet.IsFriend = true;

            _canShoot = false;
        }

        protected override void Update()
        {
            base.Update();

            ShootTimer += Time.deltaTime;
        }
    }
}