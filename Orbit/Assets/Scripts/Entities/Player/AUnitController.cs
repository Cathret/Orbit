using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AUnitController : AEntityController
    {
        #region Members
        public uint Price
        {
            get { return _price; }
        }
        [SerializeField]
        private uint _price;
        #endregion
    }
}
