using UnityEngine;

namespace Orbit.Entity
{
    public class ABaseEntity : MonoBehaviour
    {
        #region Protected functions
        protected virtual void Awake()
        {
            // Awake
        }

        protected virtual void Start()
        {
            // Start
        }

        protected virtual void Update()
        {
            if ( GameManager.Instance.CurrentGameState != GameManager.GameState.Play )
                return;
            // Update
        }
        #endregion
    }
}