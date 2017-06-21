using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ShieldGenerator : AUnitController
    {
        #region Members
        public float RechargeSpeed
        {
            get { return _rechargeSpeed; }
            protected set { _rechargeSpeed = value; }
        }
        [SerializeField]
        private float _rechargeSpeed;

        //[SerializeField]
        //public Shield _shieldPrefab;
        #endregion

        #region Protected functions
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        #endregion

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }
    }
}
