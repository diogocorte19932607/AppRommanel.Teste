namespace Rommanel.Domain.Domain
{
    public class LogradouroModel : BaseDomain
    {
        public Guid IdCliente { get; set; }
        public string CEP { get; set; } = null!;
        public string Rua { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string? Complemento { get; set; }

    }
}
