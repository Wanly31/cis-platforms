
namespace Lab1
{
    public class EditionCirculationComparer : IComparer<Edition>
    {
        public int Compare(Edition? x, Edition? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }

            return x.Circulation.CompareTo(y.Circulation);
        }
    }
}
