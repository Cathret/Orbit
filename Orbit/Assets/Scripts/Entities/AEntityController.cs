using System.Collections.Generic;
using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AEntityController : ALivingEntity
    {
        #region Events
        public event DelegateUint PowerChanged;
        public event DelegateUint BoostPowerChanged;
        #endregion

        #region Members
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

        private List<KeyValuePair<IBoostingEntity, uint>> _listBoosters =
            new List<KeyValuePair<IBoostingEntity, uint>>();
        #endregion

        #region Public functions
        public void ReceiveBoost( IBoostingEntity boostingEntity )
        {
            AUnitController unitController = boostingEntity as AUnitController;

            if ( unitController != null && _listBoosters.Exists( x => x.Key == boostingEntity ) == false )
            {
                _listBoosters.Add( new KeyValuePair<IBoostingEntity, uint>( boostingEntity, unitController.Power ) );
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
    }
}