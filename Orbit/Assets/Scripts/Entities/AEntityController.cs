using System.Collections.Generic;
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
        public event DelegateUint BoostPowerChanged;
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
        private uint _maxHealthPoints;

        public uint Power
        {
            get { return _basePower + _boostPower; }
        }

        public uint BasePower
        {
            get { return _basePower; }
            protected set
            {
                _basePower = value;
                if ( PowerChanged != null )
                    PowerChanged( _basePower );
            }
        }
        [SerializeField]
        private uint _basePower;

        public uint BoostPower
        {
            get { return _boostPower; }
            private set
            {
                _boostPower = value;
                if ( BoostPowerChanged != null )
                    BoostPowerChanged( _boostPower );
            }
        }
        private uint _boostPower = 0;

        private List<KeyValuePair<IBoostingEntity, uint>> _listBoosters = new List<KeyValuePair<IBoostingEntity, uint>>();
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

        public void ReceiveBoost( IBoostingEntity boostingEntity )
        {
            AUnitController unitController = boostingEntity as AUnitController;

            if ( unitController != null && _listBoosters.Exists( x => x.Key == boostingEntity ) == false )
            {
                _listBoosters.Add( new KeyValuePair<IBoostingEntity, uint>(boostingEntity, unitController.Power ) );
                BoostPower += unitController.Power;
            }
        }

        public void CancelBoost( IBoostingEntity boostingEntity )
        {
            AUnitController unitController = boostingEntity as AUnitController;

            if ( unitController != null && _listBoosters.Exists( x => x.Key == boostingEntity ) == true )
            {
                KeyValuePair<IBoostingEntity, uint> pair = _listBoosters.Find( x => x.Key == boostingEntity );

                BoostPower -= pair.Value;
                _listBoosters.Remove( pair );
            }
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            TriggerDeath += OnDeath;

            Hp = (int)MaxHP;
        }

        protected virtual void OnDeath()
        {
            // Entity is dead
            Destroy( gameObject );
        }
        #endregion
    }
}