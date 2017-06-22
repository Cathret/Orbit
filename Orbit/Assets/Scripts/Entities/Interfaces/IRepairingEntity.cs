using System.Collections;

namespace Orbit.Entity
{
    public interface IRepairingEntity
    {
        IEnumerator Repair();
    }
}