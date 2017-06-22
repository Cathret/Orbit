using System.Collections.Generic;
using UnityEngine;

namespace Orbit.Entity
{
    public abstract class AOpponentController : AEntityController,
                                                IMovingEntity,
                                                IDropResources
    {
        protected List<Vector3> WayPoints = new List<Vector3>();

        protected int currentWayPoint = 0;

        #region Members
        public uint Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }
        [SerializeField]
        protected uint _speed = 2;

        public uint RotateSpeed
        {
            get { return _rotateSpeed; }
            protected set { _rotateSpeed = value; }
        }
        [SerializeField]
        protected uint _rotateSpeed = 20;

        public uint ResourcesToDrop
        {
            get { return _resourcesToDrop; }
            protected set { _resourcesToDrop = value; }
        }
        [SerializeField]
        private uint _resourcesToDrop;
        #endregion

        #region Protected functions
        protected override void Update()
        {
            base.Update();

            if ( currentWayPoint < WayPoints.Count )
            {
                Vector3 target = WayPoints[currentWayPoint];
                Vector3 moveDirection = target - transform.position;
                //move towards waypoint
                if ( moveDirection.magnitude < 1 )
                {
                    currentWayPoint++;
                }
                else
                {
                    Vector3 delta = target - transform.position;
                    delta.Normalize();
                    float moveSpeed = _speed * Time.deltaTime;
                    transform.position = transform.position + ( delta * moveSpeed );
                    //Rotate Towards
                    Vector3 direction = ( target - transform.position ).normalized;
                    transform.up = Vector3.Slerp( transform.up, direction, Time.deltaTime * _rotateSpeed );
                }
            }
            else
            {
                Destroy( gameObject );
            }
        }

        protected override void OnDeath()
        {
            DropResources();

            base.OnDeath();
        }
        #endregion

        public void DropResources()
        {
            GameManager.Instance.ResourcesCount += ResourcesToDrop;
        }
    }
}