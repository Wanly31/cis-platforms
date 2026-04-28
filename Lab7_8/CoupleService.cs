namespace Lab7;

public class CoupleService
{
    private static readonly Random _rnd = new();

    /// <summary>
    /// Обробляє зустріч двох людей.
    /// Повертає IHasName (дитину / книгу) або null, якщо симпатії не виникло.
    /// Кидає SameGenderException якщо стать однакова.
    /// </summary>
    public IHasName? Couple(Human h1, Human h2)
    {
        if (h1.Gender == h2.Gender)
            throw new SameGenderException(
                $"Неможлива зустріч: обидва є {h1.Gender}.");

        //чи подобається h2 для h1? 
        CoupleAttribute? attr1 = FindAttr(h1, h2);
        Console.WriteLine($"  [{h1.Name}] шукає пару для [{h2.GetType().Name}]… " +
                          (attr1 != null ? $"знайдено (p={attr1.Probability:P0})" : "атрибут не знайдено"));

        if (attr1 == null) return null;

        bool h1LikesH2 = _rnd.NextDouble() < attr1.Probability;
        Console.WriteLine($"  [{h1.Name}] → [{h2.Name}]: {(h1LikesH2 ? "подобається ❤" : "не подобається ✗")}");
        if (!h1LikesH2) return null;

        //чи подобається h1 для h2?
        CoupleAttribute? attr2 = FindAttr(h2, h1);
        Console.WriteLine($"  [{h2.Name}] шукає пару для [{h1.GetType().Name}]… " +
                          (attr2 != null ? $"знайдено (p={attr2.Probability:P0})" : "атрибут не знайдено"));

        if (attr2 == null) return null;

        bool h2LikesH1 = _rnd.NextDouble() < attr2.Probability;
        Console.WriteLine($"  [{h2.Name}] → [{h1.Name}]: {(h2LikesH1 ? "подобається ❤" : "не подобається ✗")}");
        if (!h2LikesH1) return null;

        //Взаємна симпатія — отримати ім'я дитини через рефлексію
        string childName = GetChildNameViaReflection(h2);

        //Створити дитину через Activator
        string childTypeName = attr1.ChildType;
        Type? childType = Type.GetType($"Lab7_8.{childTypeName}")
                       ?? AppDomain.CurrentDomain.GetAssemblies()
                              .SelectMany(a => a.GetTypes())
                              .FirstOrDefault(t => t.Name == childTypeName);

        if (childType == null)
            throw new InvalidOperationException($"Тип «{childTypeName}» не знайдено.");

        IHasName child = (IHasName)Activator.CreateInstance(childType)!;

        // Задати Name через рефлексію
        var nameProp = childType.GetProperty("Name");
        nameProp?.SetValue(child, childName);

        //По батькові
        var patronymicProp = childType.GetProperty("Patronymic");
        if (patronymicProp != null)
        {
            // Батько
            Human father = h1.Gender == Gender.Male ? h1 : h2;

            Gender childGender = Gender.Female;
            var genderProp = childType.GetProperty("Gender");
            if (genderProp != null && child is Human humanChild)
                childGender = humanChild.Gender;

            string suffix = childGender == Gender.Male ? "ович" : "овна";
            patronymicProp.SetValue(child, father.Name + suffix);
        }

        return child;
    }

    private static CoupleAttribute? FindAttr(Human owner, Human partner)
    {
        string partnerTypeName = partner.GetType().Name;
        var collection = new CoupleAttributeCollection(owner.GetType());
        foreach (CoupleAttribute attr in collection)
        {
            if (attr.Pair == partnerTypeName)
                return attr;
        }
        return null;
    }

    private static string GetChildNameViaReflection(Human h2)
    {
        var method = Array.Find(
            h2.GetType().GetMethods(),
            m => m.ReturnType == typeof(string) && m.GetParameters().Length == 0);

        if (method == null) return "Немовля";

        try
        {
            return (string?)method.Invoke(h2, null) ?? "Немовля";
        }
        catch (Exception)
        {
            return "Немовля";
        }
    }
}
