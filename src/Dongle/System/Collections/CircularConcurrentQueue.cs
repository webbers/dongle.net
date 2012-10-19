using System.Collections.Concurrent;

namespace Dongle.System.Collections
{
    /// <summary>
    /// Fila concorrente com tamanho fixo. Quando um novo item é inserido e a fila extrapola o tamanho maximo estabelecido, o primeiro item é removido automaticamente.
    /// </summary>
    public class CircularConcurrentQueue<T> : ConcurrentQueue<T>
    {
        public long MaxSize { get; set;}

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public CircularConcurrentQueue(long maxSize)
        {
            MaxSize = maxSize;
        }

        /// <summary>
        /// Adiciona um item a fila. Caso a fila esteja cheia, automaticamente remove o primeiro item.
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            while (Count >= MaxSize)
            {
                T first;
                TryDequeue(out first);
            }
            base.Enqueue(item);
        }
    }
}
