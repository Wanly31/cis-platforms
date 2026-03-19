namespace Lab1
{
    internal interface IRateAndCopy
    {
        double Rating { get; }
        object DeepCopy();
    }
}