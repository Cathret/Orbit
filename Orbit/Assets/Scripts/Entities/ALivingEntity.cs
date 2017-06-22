using UnityEngine;

namespace Orbit.Entity
{
    public abstract class ALivingEntity : ABaseEntity
    {
        #region Events
        public delegate void DelegateTrigger();

        public event DelegateTrigger TriggerDeath;
        public event DelegateTrigger TriggerDestroy;

        public delegate void DelegateUint( uint value );

        public event DelegateUint HpChanged;
        public event DelegateUint MaxHpChanged;
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
                    _healthPoints = (int)MaxHP;
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
                    HpChanged( (uint)_healthPoints );
            }
        }
        private int _healthPoints = 0;

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
        private uint _maxHealthPoints = 1;

        [SerializeField]
        private ParticleSystem _deathParSysPrefab;
        #endregion

        #region Public functions
        public void ReceiveHeal( int power )
        {
            Hp += power;
        }

        public void ReceiveDamages( int power )
        {
            Hp -= power;
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            TriggerDeath += OnDeath;

            Hp = (int)MaxHP;
        }

        protected virtual void OnDeath()
        {
            // Entity is dead
            float timer = 0.0f;
            if ( _deathParSysPrefab )
            {
                ParticleSystem particle = Instantiate( _deathParSysPrefab, transform );
                timer = particle.main.duration;
                particle.Play();
                SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
                foreach ( SpriteRenderer t in children )
                {
                    if ( t )
                        Destroy( t );
                }
            }
            Destroy( gameObject, timer );
        }

        protected virtual void OnDestroy()
        {
            if ( TriggerDestroy != null )
                TriggerDestroy();
        }
        #endregion
    }
}