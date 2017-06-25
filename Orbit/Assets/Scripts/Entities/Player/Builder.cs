using UnityEngine;

namespace Orbit.Entity
{
    public class Builder : AUnitController,
                           IDropResources
    {
        #region Members
        public uint ResourcesToDrop
        {
            get { return Power; }
        }

        public float RechargeSpeed
        {
            get { return _rechargeSpeed; }
            protected set { _rechargeSpeed = value; }
        }
        [SerializeField]
        private float _rechargeSpeed;

        public float BuildTimer
        {
            get { return _buildTimer; }
            protected set
            {
                _buildTimer = value;

                if ( _buildTimer >= _rechargeSpeed )
                {
                    _buildTimer = 0.0f;
                    _canBuildResource = true;
                }
            }
        }
        private float _buildTimer = 0;

        public bool CanBuildResource
        {
            get { return _canBuildResource; }
            protected set { _canBuildResource = value; }
        }
        private bool _canBuildResource;

        [SerializeField]
        private ParticleSystem _particlesOnBuild = null;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            if ( _particlesOnBuild == null )
                Debug.LogWarning( "Builder.Awake() - particles on build not set" );

            GameManager.Instance.OnAttackMode.AddListener( ChangeMode );
            GameManager.Instance.OnBuildMode.AddListener( ChangeMode );
        }

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            if ( CanBuildResource )
                DropResources();
            else
                BuildTimer += Time.deltaTime;
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

        public override void ExecuteOnClick( Vector3 target )
        {
            // Automatic, so do nothing
        }

        private void ChangeMode()
        {
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            CanBuildResource = false;
            BuildTimer = 0.0f;
        }

        public void DropResources()
        {
            if ( _particlesOnBuild != null )
                _particlesOnBuild.Play();
            
            GameManager.Instance.ResourcesCount += ResourcesToDrop;
            CanBuildResource = false;
        }
    }
}