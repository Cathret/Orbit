using System;
using UnityEngine;

namespace Orbit.Entity
{
    public class Asteroid : Projectile,
                            IDropResources
    {
        #region Members
        public uint ResourcesToDrop
        {
            get { return _resourcesToDrop; }
            protected set { _resourcesToDrop = value; }
        }
        [SerializeField]
        private uint _resourcesToDrop;
        #endregion

        #region Public functions
        public void DropResources()
        {
            // TODO: Drop ressources
            // ResourcesToDrop;
        }
        #endregion

        #region Protected functions
        protected override void Awake()
        {
            base.Awake();

            IsFriend = false;
        }

        protected void OnDestroy()
        {
            DropResources();
        }
        #endregion
    }
}