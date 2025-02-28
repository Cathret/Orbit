﻿using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class AutoTower
        : AUnitController
          , IShootingEntity
    {
        #region Private functions
        private void AutoShoot()
        {
            if ( !_canShoot )
                return;

            if ( Cell.Connected )
            {
                TowerAIManager.OpponentTarget target;

                if ( TowerAIManager.Instance.FindBestOpponent( Cell, out target ) )
                {
                    _lastOpponentTarget = target;
                    Vector3 enemyPosition = _lastOpponentTarget.OpponentController.transform.position;
                    Vector3 direction = enemyPosition - transform.position;

                    Shoot( direction );
                    PlaySound( _shootClip );
                    if ( Head )
                        Head.transform.right = direction.normalized;
                }
            }
        }
        #endregion

        #region Members
        [SerializeField]
        [Header( "Shoot Params" )]
        private float _shootCooldown = 0.5f;

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
        private float _shootTimer;

        protected bool _canShoot = true;

        // TODO: used to set "target" for one bullet, need to find a way to remove this
        private TowerAIManager.OpponentTarget _lastOpponentTarget;

        [SerializeField]
        private Projectile _projectileType;

        [SerializeField]
        private AudioClip _shootClip;

        [SerializeField]
        private Transform[] _projectileStartPositions;
        #endregion

        #region Public functions
        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        public void Shoot( Vector3 direction )
        {
            if ( !_canShoot )
                return;

            foreach ( Transform oneStartPosition in _projectileStartPositions )
            {
                direction.Normalize();
                Projectile bullet =
                    Instantiate( _projectileType, oneStartPosition.position, oneStartPosition.rotation );
                bullet.transform.up = direction;
                bullet.Power = Power;
                bullet.IsFriend = true;

                // TODO: this will have to be checked after cleaning and optimizing
                if ( _lastOpponentTarget != null )
                    _lastOpponentTarget.SetAsTargetFor( bullet );
            }

            _canShoot = false;
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _projectileType == null )
                Debug.LogError( "AutoTower.Awake() - Projectile Type is null, need to be set in Editor", this );

            if ( _projectileStartPositions == null || _projectileStartPositions.Length <= 0 )
            {
                Debug.LogWarning( "AutoTower.Awake() - Projectile Start Positions is null, using self GameObject Transform"
                                  , this );
                _projectileStartPositions = new Transform[1];
                _projectileStartPositions[0] = transform;
            }

            FollowMouse = false;
        }

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            if ( !_canShoot )
                ShootTimer += Time.deltaTime;

            if ( _canShoot )
                AutoShoot();
        }
        #endregion
    }
}