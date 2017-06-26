using UnityEngine;

namespace Orbit.Entity
{
    public interface IShieldingEntity
    {
        void CreateShield();
        void OnShieldDestroyed();
    }
}