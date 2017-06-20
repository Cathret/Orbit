using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AUnitController : AEntityController
    {
        #region Members
        // TODO: create a shop that will instanciate a prefab
        //public uint Price
        //{
        //    get { return _price; }
        //}
        //[SerializeField]
        //private uint _price;
        //

        public uint Level
        {
            get { return _level; }
            protected set { _level = value; }
        }
        [SerializeField]
        private uint _level;

        public bool IsSelected
        {
            get { return _bIsSelected; }
            protected set { _bIsSelected = value; }
        }
        [SerializeField] // TODO remove
        private bool _bIsSelected = false;
        #endregion

        #region Public functions
        protected abstract void ExecuteOnClick( Vector3 target );
        #endregion

        #region Protected functions
        protected override void Update()
        {
            base.Update();

            if ( IsSelected )
            {
                if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
                {
                    ExecuteOnClick( Camera.main.ScreenToWorldPoint(
                        new Vector3( Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z ) // TODO: fix hack for z-axis
                        )
                    );
                }
            }
        }
        #endregion
    }
}
