
using System.Text;

namespace Lab1
{
    public class Listener
    {
        private List<ListEntry> ChangeList;

        public Listener()
        {
            ChangeList = new List<ListEntry>();
        }

        public void HandleMagazineEvent(object source, MagazineListHandlerEventArgs args)
        {
            ChangeList.Add(new ListEntry(args.NameCollection, args.ChangeType, args.ElementIndex));
        }

        public override string ToString()
        {
            if (ChangeList.Count == 0)
            {
                return "Жодних подій не зафіксовано";
            }

            StringBuilder sb = new StringBuilder();

            foreach (var change in ChangeList)
            {
                sb.AppendLine($"Change info: {change.ChangeInfo}, Collection name: {change.CollectionName} , Element index: {change.ElementIndex}");
            }

            return sb.ToString();
        }
    }
}
