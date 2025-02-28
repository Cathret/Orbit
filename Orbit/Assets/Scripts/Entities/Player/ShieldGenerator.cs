﻿using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ShieldGenerator
        : AUnitController
          , IShieldingEntity
    {
        public void CreateShield()
        {
            if ( ShieldInstance != null )
                return;

            ShieldInstance = Instantiate( _shieldPrefab, transform );
            ShieldInstance.ShieldPower = ( int )Power;
            ShieldInstance.gameObject.layer = gameObject.layer;

            CanCreateShield = false;
        }

        public void OnShieldDestroyed()
        {
            // Is automaticallty called when Shield is created with a parent
            ShieldInstance = null;
        }

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        private void ChangeMode()
        {
            CreateShield();
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            CanCreateShield = false;
            ShieldTimer = 0.0f;
        }

        #region Members
        public float RechargeSpeed
        {
            get { return _rechargeSpeed; }
            protected set { _rechargeSpeed = value; }
        }
        [SerializeField]
        private float _rechargeSpeed;

        public float ShieldTimer
        {
            get { return _shieldTimer; }
            protected set
            {
                _shieldTimer = value;

                if ( _shieldTimer >= _rechargeSpeed )
                {
                    _shieldTimer = 0.0f;
                    CanCreateShield = true;
                }
            }
        }
        private float _shieldTimer;

        public bool CanCreateShield { get; protected set; }

        protected Shield ShieldInstance { get; set; }

        [SerializeField]
        private Shield _shieldPrefab;

        public ShieldGenerator()
        {
            ShieldInstance = null;
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _shieldPrefab == null )
                Debug.LogError( "ShieldGenerator.Awake() - Shield Prefab is null, need to be set in Editor" );

            GameManager.Instance.OnAttackMode.AddListener( ChangeMode );
            GameManager.Instance.OnBuildMode.AddListener( ChangeMode );
        }

        protected override void Start()
        {
            base.Start();

            CreateShield();
        }

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            if ( ShieldInstance == null )
                if ( CanCreateShield )
                    CreateShield();
                else
                    ShieldTimer += Time.deltaTime;
        }

        protected override void OnDestroy()
        {
            if ( GameManager.Instance )
            {
                GameManager.Instance.OnAttackMode.RemoveListener( ChangeMode );
                GameManager.Instance.OnBuildMode.RemoveListener( ChangeMode );
            }
            base.OnDestroy();
        }
        #endregion
    }
}