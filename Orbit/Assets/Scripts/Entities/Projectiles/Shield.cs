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
        }

        protected override void Start()
        {
            base.Start();

            MaxHP = (uint)ShieldPower;
            Hp = (int)MaxHP;
        }

        protected override void OnDeath()
        {
            if ( TriggerShieldDestroyed != null )
                TriggerShieldDestroyed();

            base.OnDeath();
        }
        #endregion
    }
}