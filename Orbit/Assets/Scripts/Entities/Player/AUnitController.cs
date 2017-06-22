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

        [SerializeField]
        protected GameObject Head;

        protected bool FollowMouse = true;

        public uint Level
        {
            get { return _level; }
            protected set { _level = value; }
        }
        [SerializeField]
        private uint _level = 1;

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

        [SerializeField]
        private ParticleSystem _awakeParSysPrefab;
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

            if ( _awakeParSysPrefab )
            {
                ParticleSystem particle = Instantiate( _awakeParSysPrefab, transform );
                particle.Play();
            }

            Cell.OnSelection +=  ModifySelected;
            Cell.OnActionLaunched += ExecuteOnClick;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if ( Cell )
            {
                Cell.OnSelection -= ModifySelected;
                Cell.OnActionLaunched -= ExecuteOnClick;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (IsSelected && FollowMouse && Head)
            {
                Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                Head.transform.right = (target - transform.position).normalized;
            }
        }
        #endregion

        #region Private functions
        protected virtual void ModifySelected( bool selected )
        {
            IsSelected = selected;
        }
        #endregion
    }
}
