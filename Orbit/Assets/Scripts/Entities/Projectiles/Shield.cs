namespace Orbit.Entity
{
    public class Shield : ALivingEntity
    {
        #region Private functions
        private void FillHp()
        {
            MaxHP = ( uint )ShieldPower;
            Hp = ( int )MaxHP;
        }
        #endregion

        #region Members
        public event DelegateTrigger TriggerShieldDestroyed;
        private IShieldingEntity _selfGenerator;

        public int ShieldPower { get; set; }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            _selfGenerator = GetComponentInParent<IShieldingEntity>();
            TriggerShieldDestroyed = _selfGenerator.OnShieldDestroyed;

            GameManager.Instance.OnAttackMode.AddListener( FillHp );
        }

        protected override void Start()
        {
            base.Start();

            FillHp();
        }

        protected override void OnDeath()
        {
            if ( TriggerShieldDestroyed != null )
                TriggerShieldDestroyed();

            base.OnDeath();
        }

        protected override void OnDestroy()
        {
            if ( GameManager.Instance )
                GameManager.Instance.OnAttackMode.RemoveListener( FillHp );

            base.OnDestroy();
        }
        #endregion
    }
}