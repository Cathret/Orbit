using System;
using UnityEngine;

namespace Orbit.Entity
{
    public class Asteroid : Projectile,
                            IDropResources
    {
        #region Public functions
        public void DropResources()
        {
            throw new NotImplementedException();
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