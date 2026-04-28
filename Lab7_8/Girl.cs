namespace Lab7;

[Couple(Pair = "Student", Probability = 0.7, ChildType = "Girl")]
[Couple(Pair = "Botan",   Probability = 0.3, ChildType = "SmartGirl")]
public class Girl : Human
{
    public override Gender Gender => Gender.Female;
    public string Patronymic { get; set; } = string.Empty;

    public string GetName() => Name;
}
