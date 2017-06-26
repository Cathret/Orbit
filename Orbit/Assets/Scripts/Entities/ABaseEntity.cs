using UnityEngine;

namespace Orbit.Entity
{
    public class ABaseEntity : MonoBehaviour
    {
        #region Member
        protected delegate void DelegateUpdate();
        protected event DelegateUpdate OnUpdate = () => { };
        #endregion

        #region Protected functions
        protected virtual void Awake()
        {
            GameManager.Instance.OnAttackMode.AddListener( OnAttackMode );
            GameManager.Instance.OnBuildMode.AddListener( OnBuildMode );
            GameManager.Instance.OnPause.AddListener( OnPause );
            GameManager.Instance.OnPlay.AddListener( OnPlay );
        }

        protected virtual void Start()
        {
            if ( GameManager.Instance.CurrentGameState == GameManager.GameState.Play )
                OnPlay();
        }

        protected virtual void OnDestroy()
        {
			if (GameManager.Instance) 
			{
				GameManager.Instance.OnAttackMode.RemoveListener (OnAttackMode);
				GameManager.Instance.OnBuildMode.RemoveListener (OnBuildMode);
				GameManager.Instance.OnPause.RemoveListener (OnPause);
				GameManager.Instance.OnPlay.RemoveListener (OnPlay);
			}
        }

        protected void Update()
        {
            OnUpdate();
        }

        protected virtual void UpdateAttackMode()
        {
            // Update Attack Mode
        }

        protected virtual void UpdateBuildMode()
        {
            // Update Build Mode
        }

        #region Private functions
        private void OnAttackMode()
        {
            OnUpdate = UpdateAttackMode;
        }

        private void OnBuildMode()
        {
            OnUpdate = UpdateBuildMode;
        }

        private void OnPause()
        {
            OnUpdate = () => { };
        }

        private void OnPlay()
        {
            if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
                OnAttackMode();
            else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
                OnBuildMode();
            else
                Debug.LogWarning( "ABaseEntity.OnPlay() - CurrentGameMode is None." );
        }
        #endregion

        #endregion
    }
}