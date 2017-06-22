using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class Booster : AUnitController, IBoostingEntity
    {
        #region Members
        protected DelegateTrigger OnBoostedUnitDeath
        {
            get { return _onBoostedUnitDeath; }
            set { _onBoostedUnitDeath = value; }
        }
        private DelegateTrigger _onBoostedUnitDeath;

        protected AUnitController BoostedUnit
        {
            get { return _boostedUnit; }
            set { _boostedUnit = value; }
        }
        private AUnitController _boostedUnit;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            OnBoostedUnitDeath = () =>
            {
                BoostedUnit.CancelBoost( this );
                BoostedUnit.TriggerDeath -= OnBoostedUnitDeath;
                BoostedUnit = null;
            };

            BoostPowerChanged += UpdateBoost;
        }

        protected override void OnDestroy()
        {
            if ( BoostedUnit )
                OnBoostedUnitDeath.Invoke();

            base.OnDestroy();
        }
        #endregion

        #region Private functions
        private void UpdateBoost( uint boostPower )
        {
            if ( BoostedUnit )
            {
                UnBoost();
                Boost();
            }
        }
        #endregion

        public override void ExecuteOnClick( Vector3 target )
        {
            GameCell targetCell = GameGrid.Instance.GetCellFromWorldPoint( target );

            if ( Cell.IsConnectedTo( targetCell ) )
            {
                if ( BoostedUnit == targetCell.Unit )
                    return;

                if ( BoostedUnit != null )
                {
                    UnBoost();
                    BoostedUnit.TriggerDeath -= OnBoostedUnitDeath;
                }

                BoostedUnit = targetCell.Unit;
                BoostedUnit.TriggerDeath += OnBoostedUnitDeath;
                Boost();
            }
        }

        protected override void ModifySelected(bool selected)
        {
            base.ModifySelected(selected);

            if (selected)
                FollowMouse = true;
            else if (BoostedUnit != null)
            {
                FollowMouse = false;
                if (Head)
                    Head.transform.right = (BoostedUnit.transform.position - transform.position).normalized;
            }
        }

        public void Boost()
        {
            BoostedUnit.ReceiveBoost( this );
        }

        public void UnBoost()
        {
            BoostedUnit.CancelBoost( this );
        }
    }
}
