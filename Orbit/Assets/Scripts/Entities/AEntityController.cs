using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AEntityController : ABaseEntity
    {
        #region Events
        public delegate void DelegateTrigger();

        public event DelegateTrigger TriggerDeath;

        public delegate void DelegateUint( uint value );

        public event DelegateUint HpChanged;
        public event DelegateUint MaxHpChanged;
        public event DelegateUint PowerChanged;
        #endregion

        #region Members

        public int Hp
        {
            get { return _healthPoints; }
            protected set
            {
                _healthPoints = value;

                if ( _healthPoints > MaxHP )
                {
                    _healthPoints = ( int )MaxHP;
                    return;
                }

                if ( _healthPoints < 0 )
                    _healthPoints = 0;
                if ( _healthPoints == 0 )
                {
                    if ( TriggerDeath != null )
                        TriggerDeath();
                    return;
                }

                if ( HpChanged != null )
                    HpChanged( ( uint )_healthPoints );
            }
        }

        [SerializeField]
        private int _healthPoints;

        public uint MaxHP
        {
            get { return _maxHealthPoints; }
            protected set
            {
                _maxHealthPoints = value;
                if ( MaxHpChanged != null )
                    MaxHpChanged( _maxHealthPoints );
            }
        }

        [SerializeField]
        private uint _maxHealthPoints;

        public uint Power
        {
            get { return _power; }
            protected set
            {
                _power = value;
                if ( PowerChanged != null )
                    PowerChanged( _power );
            }
        }

        [SerializeField]
        private uint _power;

        #endregion

        #region Protected functions

        protected override void Awake()
        {
            TriggerDeath += OnDeath;
        }

        protected virtual void OnDeath()
        {
            // Entity is dead
            Destroy( gameObject );
        }

        #endregion
    }
}
