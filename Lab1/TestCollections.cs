namespace Lab1
{
    public class TestCollections
    {
        private List<Edition> listKeys;
        private List<string> listStrings;
        private Dictionary<Edition, Magazine> dictKeyValue;
        private Dictionary<string, Magazine> dictStringValue;

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

            for (int i = 1; i < count; i++)
            {
                Magazine mag = GenerateMagazine(i);
                Edition key = mag.EditionData;
                string keyStr = key.ToString();

                listKeys.Add(key);
                listStrings.Add(keyStr);
                dictKeyValue.Add(key, mag);
                dictStringValue.Add(keyStr, mag);
            }
            
        }
        public bool FindInListKeys(Edition key) => listKeys.Contains(key);
        public bool FindInListStrings(string key) => listStrings.Contains(key);
        public bool FindInDictByKey(Edition key) => dictKeyValue.ContainsKey(key);
        public bool FindInDictByStringKey(string key) => dictStringValue.ContainsKey(key);
        public bool FindInDictByValue(Magazine value) => dictKeyValue.ContainsValue(value);

    }
}

