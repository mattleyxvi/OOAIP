using System.Numerics;

namespace Gameserver.Movement
{
    public class Vector
    {
        public int dim => _coords.Length;
        private readonly int[] _coords;

        public Vector(int[] coords)
        {
            if (coords.Length == 0)
            {
                throw new ArgumentException("Enormous vector input");
            }

            _coords = coords;
        }
        public static Vector operator +(Vector vec1, Vector vec2)
        {

            if (vec1.dim != vec2.dim)
            {
                throw new ArgumentException("Dimensions doesn't match");
            }
            return new Vector(vec1._coords.Zip(vec2._coords, (a, b) => a + b).ToArray());
        }
        public override bool Equals(object? obj)
        {
            return obj != null && _coords.SequenceEqual(((Vector)obj)._coords);
        }
        public override int GetHashCode() => _coords.GetHashCode();
    }
}
