using System;
using UnityEngine;

namespace Orbit.Entity.Projectile
{
    public class Asteroid : AProjectile, IDropResources
    {
        #region Public functions

        public void DropResources()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected functions

        protected void OnDestroy()
        {
            DropResources();
        }

        #endregion
    }
}