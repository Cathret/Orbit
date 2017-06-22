using UnityEngine;

namespace Orbit.Entity.Opponent
{
    public class ShootingOpponent : AOpponentController,
                                    IShootingEntity
    {
        #region Members
        [SerializeField]
        protected float ShootCooldown = 0.7f;

        private float _cooldownTimer = 0.0f;

        public float CooldownTimer
        {
            get { return _cooldownTimer; }
            private set
            {
                _cooldownTimer = value;
                if ( _cooldownTimer > ShootCooldown )
                {
                    _cooldownTimer = 0.0f;
                    Shoot( transform.up );
                }
            }
        }

        [SerializeField]
        private Projectile _projectileType = null;
        #endregion

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
            ;
            Vector3 center = GameGrid.Instance.RealCenter;
            float distance = ( transform.position - center ).magnitude - GameGrid.Instance.RealEfficientSide;

            Vector2 point = Random.insideUnitCircle * GameGrid.Instance.RealEfficientSide / 2;
            Vector3 target = new Vector3( point.x, point.y );
            target += center;

            WayPoints.Add( ( target - transform.position ).normalized * distance + transform.position );
            WayPoints.Add( transform.position );
        }

        protected override void Update()
        {
            base.Update();
            if ( GameManager.Instance.CurrentGameState == GameManager.GameState.Play )
            {
                CooldownTimer += Time.deltaTime;
            }
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();

            CooldownTimer = ShootCooldown - 1.0f;
        }
        #endregion
    }
}