using UnityEngine;

namespace Orbit.Entity
{
    public class Projectile
        : ABaseEntity
          , IMovingEntity
    {
        #region Members
        public uint Power
        {
            get { return _power; }
            set { _power = value; }
        }
        [SerializeField]
        private uint _power = 1;

        public float Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }
        [SerializeField]
        [Range( 0, 50 )]
        private float _speed = 2;

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
        protected override void UpdateAttackMode()
        {
            base.UpdateAttackMode();

            Move();
        }

        protected override void UpdateBuildMode()
        {
            base.UpdateBuildMode();

            Move();
        }
        #endregion

        #region Private functions
        private void OnTriggerEnter2D( Collider2D other )
        {
            ALivingEntity livingEntity = other.gameObject.GetComponent<ALivingEntity>();

            if ( livingEntity == null )
                return;

            livingEntity.ReceiveDamages( ( int )Power );

            Destroy( gameObject );
        }

        private void OnBecameInvisible()
        {
            Destroy( gameObject );
        }

        private void Move()
        {
            Vector3 position = transform.localPosition;
            position = Vector3.Lerp( position, position + transform.up, Time.deltaTime * Speed );
            transform.localPosition = position;
        }
        #endregion
    }
}