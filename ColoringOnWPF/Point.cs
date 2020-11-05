namespace ColoringWithWPF
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point() {
            X = Y = 0;
        }

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            return (obj as Point).X == X && (obj as Point).Y == Y;
        }
    }
}
