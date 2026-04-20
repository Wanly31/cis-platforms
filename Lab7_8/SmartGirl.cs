namespace Lab7_8;

[Couple(Pair = "Student", Probability = 0.2, ChildType = "Girl")]
[Couple(Pair = "Botan",   Probability = 0.5, ChildType = "Book")]
public sealed class SmartGirl : Human
{
    public override Gender Gender => Gender.Female;
    public string Patronymic { get; set; } = string.Empty;

    public string GetName() => Name;
}
