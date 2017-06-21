using UnityEngine;

namespace Orbit.Entity
{
    public class Projectile : ABaseEntity,
                              IMovingEntity
    {
        #region Members
        public uint Power
        {
            get { return _power; }
            set { _power = value; }
        }
        [SerializeField]
        private uint _power = 1;

        public uint Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }
        [SerializeField]
        private uint _speed = 2;

        public bool IsFriend
        {
            get { return _bFriend; }
            set
            {
                _bFriend = value;
                gameObject.layer = LayerMask.NameToLayer( _bFriend ? "Player" : "Opponent" );
            }
        }
        private bool _bFriend;
        #endregion

        #region Protected functions
        protected override void Start()
        {
            base.Start();

            Destroy( gameObject, 5.0f );
        }

        protected override void Update()
        {
            base.Update();

            Vector3 position = transform.localPosition;
            position = Vector3.Lerp( position, position + transform.up, Time.deltaTime * Speed );
            transform.localPosition = position;
        }
        #endregion

        private void OnTriggerEnter2D( Collider2D other )
        {
            ALivingEntity livingEntity = other.gameObject.GetComponent<ALivingEntity>();

            if ( livingEntity == null )
                return;

            livingEntity.ReceiveDamages( (int)Power );

            Destroy( gameObject );
        }
    }
}