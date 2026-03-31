
namespace Lab1
{
    public class ListEntry
    {
        public string CollectionName { get; set; }
        public string ChangeInfo { get; set; }
        public int ElementIndex { get; set; }

        public ListEntry(string collectionName, string changeInfo, int elementIndex)
        {
            CollectionName = collectionName;
            ChangeInfo = changeInfo;
            ElementIndex = elementIndex;
        }

        public override string ToString()
        {
            return $"Collection Name: {CollectionName}, Change Info: {ChangeInfo}, Element Index: {ElementIndex}";
        }
    }

}
