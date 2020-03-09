namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public interface IHashService
    {
        string Compute(string source);

        bool Compare(string source1, string source2);
    }
}
