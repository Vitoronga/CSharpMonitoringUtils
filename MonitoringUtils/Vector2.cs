namespace MonitoringUtils
{
    public class Vector2
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            //return base.Equals(obj);

            if (obj == null) return false;

            if (obj.GetType() != typeof(Vector2)) return false;


            Vector2 vec = (Vector2)obj;

            if (X != vec.X || Y != vec.Y) return false;

            return true;
        }

        public static bool operator ==(Vector2 vec1, Vector2 vec2)
        {
            return vec1.Equals(vec2);
        }

        public static bool operator !=(Vector2 vec1, Vector2 vec2)
        {
            return !(vec1 == vec2);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
