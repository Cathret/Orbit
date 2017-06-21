using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class AutoTower : ATower
    {
        #region Members
        protected Coroutine ShootCoroutine
        {
            get { return _shootCoroutine; }
            set { _shootCoroutine = value; }
        }
        private Coroutine _shootCoroutine = null;

        public float ShootingSpeed
        {
            get { return _shootingSpeed; }
            protected set { _shootingSpeed = value; }
        }
        [SerializeField]
        private float _shootingSpeed;
        #endregion

        #region Protected functions
        protected override void Start()
        {
            base.Start();

            ShootCoroutine = StartCoroutine( AutoShoot() );
        }

        protected override void OnDestroy()
        {
            if ( ShootCoroutine != null )
                StopCoroutine( ShootCoroutine );

            base.OnDestroy();
        }
        #endregion

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        public IEnumerator AutoShoot()
        {
            while ( true )
            {
                if ( Cell.Connected )
                {
                    //Vector3 target;
                    //if ( FindClosestOpponent( Cell, out target ) )
                    //{
                    //    Shoot( target - transform.position );
                    //}
                }

                yield return new WaitForSeconds( ShootingSpeed );
            }
        }
    }
}