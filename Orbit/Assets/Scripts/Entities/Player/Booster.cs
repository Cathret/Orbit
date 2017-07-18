using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class Booster
        : AUnitController
          , IBoostingEntity
    {
        public void Boost()
        {
            BoostedUnit.ReceiveBoost( this );
        }

        public void UnBoost()
        {
            BoostedUnit.CancelBoost( this );
        }

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

                Cell.Selected = false;
            }
        }

        protected override void ModifySelected( bool selected )
        {
            base.ModifySelected( selected );

            if ( selected )
            {
                FollowMouse = true;
            }
            else if ( BoostedUnit != null )
            {
                FollowMouse = false;
                if ( Head )
                    Head.transform.right = ( BoostedUnit.transform.position - transform.position ).normalized;
            }
        }

        #region Members
        protected DelegateTrigger OnBoostedUnitDeath { get; set; }

        protected AUnitController BoostedUnit { get; set; }
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
    }
}