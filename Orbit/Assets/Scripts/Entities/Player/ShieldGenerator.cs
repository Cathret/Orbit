using System.Collections;
using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ShieldGenerator : AUnitController,
                                   IShieldingEntity
    {
        #region Members
        protected Coroutine CooldownCoroutine
        {
            get { return _cooldownCoroutine; }
            set { _cooldownCoroutine = value; }
        }
        private Coroutine _cooldownCoroutine;

        public float RechargeSpeed
        {
            get { return _rechargeSpeed; }
            protected set { _rechargeSpeed = value; }
        }
        [SerializeField]
        private float _rechargeSpeed;

        protected Shield ShieldInstance
        {
            get { return _shieldInstance; }
            set { _shieldInstance = value; }
        }
        private Shield _shieldInstance = null;

        [SerializeField]
        private Shield _shieldPrefab = null;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _shieldPrefab == null )
                Debug.LogError( "ShieldGenerator.Awake() - Shield Prefab is null, need to be set in Editor" );
        }

        protected override void Start()
        {
            base.Start();

            CreateShield();
        }

        protected override void OnDestroy()
        {
            if ( CooldownCoroutine != null )
                StopCoroutine( CooldownCoroutine );

            base.OnDestroy();
        }
        #endregion

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        public void CreateShield()
        {
            if ( ShieldInstance != null )
                return;

            ShieldInstance = Instantiate( _shieldPrefab, transform );
            ShieldInstance.ShieldPower = (int)Power;
            ShieldInstance.gameObject.layer = gameObject.layer;
        }

        public void OnShieldDestroyed()
        {
            ShieldInstance = null;
            CooldownCoroutine = StartCoroutine( GenerationCooldown() );
        }

        private IEnumerator GenerationCooldown()
        {
            yield return new WaitForSeconds( RechargeSpeed );

            CreateShield();
        }
    }
}