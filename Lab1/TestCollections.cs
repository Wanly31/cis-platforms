using System.Collections.Immutable;

namespace Lab1
{
    public class TestCollections
    {
        private readonly List<Edition> listKeys;
        private readonly List<string> listStrings;
        private readonly Dictionary<Edition, Magazine> dictKeyValue;
        private readonly Dictionary<string, Magazine> dictStringValue;

        //immutable
        private readonly ImmutableList<Edition> immutableListKeys;
        private readonly ImmutableList<string> immutableListStrings;
        private readonly ImmutableDictionary<Edition, Magazine> immutableKeyValue;
        private readonly ImmutableDictionary<string, Magazine> immutableStringValue;

        //sorted
        private readonly SortedList<Edition, Magazine> sortedListKeys;
        private readonly SortedList<string, Magazine> sortedListStrings;
        private readonly SortedDictionary<Edition, Magazine> sortedDictKeyValue;
        private readonly SortedDictionary<string, Magazine> sortedDictStringValue;

        public static Magazine GenerateMagazine(int n)
        {
            var edition = new Edition($"Edition {n}", DateTime.Now.AddDays(n), n + 1 * 100);

            var magazine = new Magazine($"Magazine {n}", DateTime.Now.AddDays(n), (n + 1) * 100, Frequency.Monthly);

            return magazine;
        }

        public TestCollections(int count)
        {
            listKeys = new List<Edition>();
            listStrings = new List<string>();
            dictKeyValue = new Dictionary<Edition, Magazine>();
            dictStringValue = new Dictionary<string, Magazine>();

            sortedListKeys = new SortedList<Edition, Magazine>(new Edition());
            sortedListStrings = new SortedList<string, Magazine>();
            sortedDictKeyValue = new SortedDictionary<Edition, Magazine>(new Edition());
            sortedDictStringValue = new SortedDictionary<string, Magazine>();

            for (int i = 1; i < count; i++)
            {
                Magazine mag = GenerateMagazine(i);
                Edition key = mag.EditionData;
                string keyStr = key.ToString();

                listKeys.Add(key);
                listStrings.Add(keyStr);
                dictKeyValue.Add(key, mag);
                dictStringValue.Add(keyStr, mag);
                
                sortedListKeys.Add(key, mag);
                sortedListStrings.Add(keyStr, mag);
                sortedDictKeyValue.Add(key, mag);
                sortedDictStringValue.Add(keyStr, mag);
            }
            
            immutableListKeys = listKeys.ToImmutableList();
            immutableListStrings = listStrings.ToImmutableList();
            immutableKeyValue = dictKeyValue.ToImmutableDictionary();
            immutableStringValue = dictStringValue.ToImmutableDictionary();
        }
        public bool FindInListKeys(Edition key) => listKeys.Contains(key);
        public bool FindInListStrings(string key) => listStrings.Contains(key);
        public bool FindInDictByKey(Edition key) => dictKeyValue.ContainsKey(key);
        public bool FindInDictByStringKey(string key) => dictStringValue.ContainsKey(key);
        public bool FindInDictByValue(Magazine value) => dictKeyValue.ContainsValue(value);

        public bool FindInImmutableListKeys(Edition key) => immutableListKeys.Contains(key);
        public bool FindInImmutableListStrings(string key) => immutableListStrings.Contains(key);
        public bool FindInImmutableKeyValue(Edition key) => immutableKeyValue.ContainsKey(key);
        public bool FindInImmutableStringValue(Magazine value) => immutableStringValue.ContainsValue(value);

        public bool FindInSortedListKeys(Edition key) => sortedListKeys.ContainsKey(key);
        public bool FindInSortedListStrings(string key) => sortedListStrings.ContainsKey(key);
        public bool FindInSortedDictKeyValue(Edition key) => sortedDictKeyValue.ContainsKey(key);
        public bool FindInSortedDictStringValue(Magazine value) => sortedDictStringValue.ContainsValue(value);
    }
}

