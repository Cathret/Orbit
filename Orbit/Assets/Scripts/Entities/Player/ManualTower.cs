using UnityEngine;

namespace Orbit.Entity.Unit
{
    public class ManualTower : ATower
    {
        [SerializeField]
        private AudioClip _shootClip;

        public override void ExecuteOnClick( Vector3 target )
        {
            Shoot( target - transform.position );
            PlaySound(_shootClip);
        }
    }
}