using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ManualTower : ATower
    {
        [SerializeField]
        private GameObject _head;

        public override void ExecuteOnClick( Vector3 target )
        {
            Shoot( target - transform.position );
        }

        protected override void Update()
        {
            base.Update();

            if ( IsSelected && _head )
            {
                Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                _head.transform.right = (target - transform.position).normalized;
            }
        }
    }
}
