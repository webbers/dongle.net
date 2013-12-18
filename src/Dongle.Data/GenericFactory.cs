namespace Dongle.Data
{
    public class GenericFactory<TIn> : IFactory<TIn, TIn>
    {
        public TIn Create(TIn model)
        {
            return model;
        }
    }
}