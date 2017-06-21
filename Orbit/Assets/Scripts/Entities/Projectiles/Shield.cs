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
            set
            {
                _shieldPower = value;

                if ( _shieldPower < 0 )
                    _shieldPower = 0;
                if ( _shieldPower == 0 )
                    Hp = 0;
            }
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

        protected override void Update()
        {
            base.Update();
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