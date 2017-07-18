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
            BoostedUnit.Cell.OnPositionChange += OrientHead;
        }

        public void UnBoost()
        {
            BoostedUnit.CancelBoost( this );
            BoostedUnit.Cell.OnPositionChange -= OrientHead;
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

        private void OrientHead()
        {
            if (Head && BoostedUnit && FollowMouse == false)
                Head.transform.right = (BoostedUnit.Cell.TruePosition - Cell.TruePosition).normalized;
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
                OrientHead();
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

            Cell.OnPositionChange += OrientHead;

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
            if ( Cell )
                Cell.OnPositionChange -= OrientHead;

            if ( BoostedUnit )
                OnBoostedUnitDeath.Invoke();

            base.OnDestroy();
        }
        #endregion
    }
}