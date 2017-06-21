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
        private uint _power;

        public uint Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        [SerializeField]
        private uint _speed;

        public bool IsFriend
        {
            get { return _bFriend; }
            set { _bFriend = value; }
        }

        private bool _bFriend;
        #endregion

        #region Protected functions
        protected override void Start()
        {
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
    }
}