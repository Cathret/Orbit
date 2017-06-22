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

        protected ParticleSystem ReparatorParticles
        {
            get { return _reparatorParticles; }
            set { _reparatorParticles = value; }
        }
        private ParticleSystem _reparatorParticles;

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
            base.Awake();

            RepairCoroutine = StartCoroutine( Repair() );
            OnRepairedUnitDeath = () =>
            {
                RepairedUnit.TriggerDeath -= OnRepairedUnitDeath;
                RepairedUnit = null;
                FollowMouse = true;
            };

            ReparatorParticles = GetComponentInChildren<ParticleSystem>();
            if ( ReparatorParticles == null )
                Debug.LogWarning( "Reparator.Awake() - could not find ParticleSystem in children" );
        }

        protected override void OnDestroy()
        {
            if ( RepairCoroutine != null )
                StopCoroutine( RepairCoroutine );

            if ( RepairedUnit )
                OnRepairedUnitDeath.Invoke();

            base.OnDestroy();
        }
        #endregion

        public override void ExecuteOnClick( Vector3 target )
        {
            GameCell targetCell = GameGrid.Instance.GetCellFromWorldPoint( target );

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

        protected override void ModifySelected( bool selected )
        {
            base.ModifySelected( selected );

            if ( selected )
                FollowMouse = true;
            else if ( RepairedUnit != null )
            {
                FollowMouse = false;
                if (Head)
                    Head.transform.right = (RepairedUnit.transform.position - transform.position).normalized;
            }
        }

        public IEnumerator Repair()
        {
            while ( true )
            {
                if ( RepairedUnit != null )
                {
                    RepairedUnit.ReceiveHeal( (int)Power );
                    if ( ReparatorParticles != null )
                        ReparatorParticles.Play();
                }

                yield return new WaitForSeconds( RepairSpeed );
            }
        }
    }
}