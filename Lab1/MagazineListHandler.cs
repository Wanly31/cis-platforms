
namespace Lab1
{
    public delegate void MagazineListHandler(object source, MagazineListHandlerEventArgs args);

    public class MagazineListHandlerEventArgs : EventArgs
    {
        public string NameCollection { get; set; }
        public string ChangeType { get; set; }
        public int ElementIndex { get; set; }


        public MagazineListHandlerEventArgs(string nameCollection, string changeType, int elementIndex)
        {
            NameCollection = nameCollection;
            ChangeType = changeType;
            ElementIndex = elementIndex;

        }

        public override string ToString()
        {
            return $"Name Collection: {NameCollection} \n Change Type: {ChangeType} \n Element Index: {ElementIndex}";
        }
    }
}
