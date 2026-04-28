namespace Lab7;

[Couple(Pair = "Student", Probability = 0.4, ChildType = "PrettyGirl")]
[Couple(Pair = "Botan",   Probability = 0.1, ChildType = "PrettyGirl")]
public sealed class PrettyGirl : Human
{
    public override Gender Gender => Gender.Female;
    public string Patronymic { get; set; } = string.Empty;

    public string GetName() => Name;
}
