using UnityEngine;

namespace Orbit.Entity
{
    public class Asteroid
        : Projectile
          , IDropResources
    {
        #region Public functions
        public void DropResources()
        {
            GameManager.Instance.ResourcesCount += ResourcesToDrop;
        }
        #endregion

        #region Members
        public uint ResourcesToDrop
        {
            get { return _resourcesToDrop; }
            protected set { _resourcesToDrop = value; }
        }
        [SerializeField]
        private uint _resourcesToDrop;
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            IsFriend = false;
        }

        protected override void OnDestroy()
        {
            DropResources();

            base.OnDestroy();
        }
        #endregion
    }
}