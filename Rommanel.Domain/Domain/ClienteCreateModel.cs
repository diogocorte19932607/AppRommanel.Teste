using System.ComponentModel.DataAnnotations;

namespace Rommanel.Domain.Domain
{
    public class ClienteCreateModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; }
        public string IE { get; set; }
        public bool IsentoIE { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        public EnderecoCreateModel Endereco { get; set; }
    }

    public class ClienteEditModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; }
        public string IE { get; set; }
        public bool IsentoIE { get; set; }
    }

    public class EnderecoCreateModel
    {
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}