using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AOpponentController : AEntityController,
                                                IMovingEntity
    {
        #region Members
        public uint Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        [SerializeField]
        private uint _speed;
        #endregion

        #region Protected functions
        protected override void Update()
        {
            base.Update();

            // TODO: create a path and follow it
        }
        #endregion
    }
}