using UnityEngine;
using UnityEngine.Events;

namespace Orbit.Entity
{
    public abstract class AUnitController : AEntityController
    {
        #region Static members
        public static UnityEvent DmgTakenEvent = new UnityEvent();
        #endregion

        #region Members
        public string UnitName
        {
            get { return _unitName; }
        }
        [SerializeField]
        [Header( "Unit Stats" )]
        private string _unitName;

        public uint Price
        {
            get { return _price; }
        }
        [SerializeField]
        private uint _price;

        public uint CurrentPrice
        {
            get { return ( uint )( Hp / MaxHP * Price ); }
        }

        public uint Level
        {
            get { return _level; }
            protected set { _level = value; }
        }
        [SerializeField]
        private uint _level = 1;

        public bool IsSelected { get; protected set; }

        public GameCell Cell
        {
            get
            {
                if ( _gameCell == null )
                    _gameCell = GetComponent<GameCell>();
                return _gameCell;
            }
            protected set { _gameCell = value; }
        }
        private GameCell _gameCell;


        [SerializeField]
        [Header( "Unit Visual Feedbacks" )]
        private ParticleSystem _awakeParSysPrefab;

        public Sprite Icon
        {
            get
            {
                if ( _icon == null )
                    _icon = OwnSpriteRenderer ? OwnSpriteRenderer.sprite : null;
                return _icon;
            }
        }
        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        protected GameObject Head;

        protected bool FollowMouse = true;

        public AUnitController()
        {
            IsSelected = false;
        }
        #endregion

        #region Public functions
        public virtual void ExecuteOnDrag( Vector3 target )
        {
        }

        public abstract void ExecuteOnClick( Vector3 target );
        #endregion

        #region Protected functions
        protected override void Start()
        {
            base.Start();

            if ( Cell == null )
                Debug.LogError( "AUnitController.Awake() - Cell is null, there's no GameCell component in object"
                                , this );

            if ( _awakeParSysPrefab )
            {
                ParticleSystem particle = Instantiate( _awakeParSysPrefab, transform );
                particle.Play();
            }


            TriggerHit += DmgTakenEvent.Invoke;
            Cell.OnSelection += ModifySelected;
            Cell.OnActionLaunched += ExecuteOnClick;
            Cell.OnDraggedActionLaunched += ExecuteOnDrag;
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

        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            if ( IsSelected && FollowMouse && Head )
            {
                Vector3 target =
                    Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y,
                                                                 -Camera.main.transform.position.z ) );
                Head.transform.right = ( target - transform.position ).normalized;
            }
        }

        protected virtual void ModifySelected( bool selected )
        {
            IsSelected = selected;
        }
        #endregion
    }
}