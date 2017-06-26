namespace Orbit.Entity
{
    public interface IDropResources
    {
        uint ResourcesToDrop { get; }

        void DropResources();
    }
}