using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent
        : AOpponentController
          , IShootingEntity
    {
        #region Public functions
        public void Shoot( Vector3 direction )
        {
            if ( currentWayPoint > 0 )
                return;
            direction.Normalize();
            Projectile bullet = Instantiate( _projectileType, transform.position, transform.rotation );
            bullet.transform.up = direction;
            bullet.Power = Power;
            bullet.IsFriend = false;
        }
        #endregion

        #region Private functions
        private void SelfUpdate()
        {
            CooldownTimer += Time.deltaTime;
        }
        #endregion

        #region Members
        [SerializeField]
        private float _shootCooldown = 2.0f;
        [SerializeField]
        private float _timeBeforeFirstShot = 2.0f;

        public float CooldownTimer
        {
            get { return _cooldownTimer; }
            private set
            {
                _cooldownTimer = value;
                if ( _cooldownTimer > _shootCooldown )
                {
                    _cooldownTimer = 0.0f;
                    Shoot( transform.up );
                }
            }
        }
        private float _cooldownTimer;

        [SerializeField]
        private Projectile _projectileType;

        private event DelegateUpdate OnSelfUpdate = () => { };
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _projectileType == null )
                Debug.LogError( "ShootingOpponent.Awake() - Projectile Type is null, need to be set in Editor", this );
        }

        protected override void Start()
        {
            base.Start();

            Vector3 center = GameGrid.Instance.RealCenter;
            float distance = ( transform.position - center ).magnitude - GameGrid.Instance.RealEfficientSide;

            Vector2 point = Random.insideUnitCircle * GameGrid.Instance.RealEfficientSide / 2;
            Vector3 target = new Vector3( point.x, point.y );
            target += center;

            WayPoints.Add( ( target - transform.position ).normalized * distance + transform.position );
            WayPoints.Add( transform.position );
        }

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            OnSelfUpdate();
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();

            CooldownTimer = _shootCooldown - _timeBeforeFirstShot;

            OnSelfUpdate = SelfUpdate;
        }
        #endregion
    }
}