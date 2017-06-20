using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class Reparator : AUnitController, IRepairingEntity
    {
        #region Members
        protected Coroutine RepairCoroutine
        {
            get { return _repairCoroutine; }
            set { _repairCoroutine = value; }
        }
        private Coroutine _repairCoroutine = null;

        protected DelegateTrigger OnRepairedUnitDeath
        {
            get { return _onRepairedUnitDeath; }
            set { _onRepairedUnitDeath = value; }
        }
        private DelegateTrigger _onRepairedUnitDeath;

        protected AUnitController RepairedUnit
        {
            get { return _repairedUnit; }
            set { _repairedUnit = value; }
        }
        private AUnitController _repairedUnit;

        public float RepairSpeed
        {
            get { return _repairSpeed; }
            protected set { _repairSpeed = value; }
        }
        [SerializeField]
        private float _repairSpeed;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            RepairCoroutine = StartCoroutine( Repair() );
            OnRepairedUnitDeath = () =>
            {
                RepairedUnit.TriggerDeath -= OnRepairedUnitDeath;
                RepairedUnit = null;
            };
        }

        protected override void OnDestroy()
        {
            if ( RepairCoroutine != null )
                StopCoroutine( RepairCoroutine );

            base.OnDestroy();
        }

        protected override void ExecuteOnClick( Vector3 target )
        {
            GameCell targetCell = Cell.Grid.GetCellFromWorldPoint( target );

            if ( Cell.IsConnectedTo( targetCell ) )
            {
                if ( RepairedUnit == targetCell.Unit )
                    return;

                if ( RepairedUnit != null )
                    RepairedUnit.TriggerDeath -= OnRepairedUnitDeath;

                RepairedUnit = targetCell.Unit;
                RepairedUnit.TriggerDeath += OnRepairedUnitDeath;
            }
        }
        #endregion

        public IEnumerator Repair()
        {
            while ( true )
            {
                if ( RepairedUnit != null )
                    RepairedUnit.ReceiveHeal( (int)Power );

                yield return new WaitForSeconds( RepairSpeed );
            }

            yield break;
        }
    }
}