namespace Lab7_8;

[Couple(Pair = "Girl",       Probability = 0.7, ChildType = "Girl")]
[Couple(Pair = "PrettyGirl", Probability = 1.0, ChildType = "PrettyGirl")]
[Couple(Pair = "SmartGirl",  Probability = 0.5, ChildType = "Girl")]
public class Student : Human
{
    public override Gender Gender => Gender.Male;
    public string Patronymic { get; set; } = string.Empty;

    public string GetName() => Name;
}
