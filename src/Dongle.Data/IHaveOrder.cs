namespace Dongle.Data
{
    /// <summary>
    /// Sinaliza tabelas cujo registro possuem uma ordem. Pode ser usado para inserir botões de ordenação nos itens da lista.
    /// </summary>
    public interface IHaveOrder
    {
        /// <summary>
        /// Ordem
        /// </summary>
        long Order { get; set; }
    }
}
