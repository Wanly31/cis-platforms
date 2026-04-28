namespace Lab7;

public abstract class Human : IHasName
{
    public string Name { get; set; } = string.Empty;
    public abstract Gender Gender { get; }
}
