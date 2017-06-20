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

        public GameCell Cell
        {
            get { return _gameCell; }
            protected set { _gameCell = value; }
        }
        private GameCell _gameCell = null;
        #endregion

        #region Public functions
        public abstract void ExecuteOnClick( Vector3 target );
        #endregion

        #region Protected functions
        protected override void Start()
        {
            Cell = GetComponent<GameCell>();
            if ( Cell == null )
                Debug.LogError( "AUnitController.Awake() - Cell is null, there's no GameCell component in object", this );

            Cell.OnSelection +=  ModifySelected;
            Cell.OnActionLaunched += ExecuteOnClick;
        }

        protected virtual void OnDestroy()
        {
            if ( Cell )
            {
                Cell.OnSelection -= ModifySelected;
                Cell.OnActionLaunched -= ExecuteOnClick;
            }
        }
        #endregion

        #region Private functions
        public void ModifySelected( bool selected )
        {
            IsSelected = selected;
        }
        #endregion
    }
}
