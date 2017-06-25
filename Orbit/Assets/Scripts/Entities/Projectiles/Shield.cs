using Orbit.Entity.Unit;
using UnityEngine;

namespace Orbit.Entity
{
    public class Shield : ALivingEntity
    {
        #region Members
        public event DelegateTrigger TriggerShieldDestroyed;
        private IShieldingEntity _selfGenerator = null;

        public int ShieldPower
        {
            get { return _shieldPower; }
            set { _shieldPower = value; }
        }
        private int _shieldPower;
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

        #region Private functions
        private void FillHp()
        {
            MaxHP = ( uint )ShieldPower;
            Hp = ( int )MaxHP;
        }
        #endregion
    }
}