namespace Dongle.Data
{
    public interface IFactory<in TIn, out TOut>
    {
        TOut Create(TIn model);
    }
}
