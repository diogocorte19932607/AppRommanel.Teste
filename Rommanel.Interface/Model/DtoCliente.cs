using System;
using Thomagreg.Interface.Model;

namespace Thomagreg.Interface.Model
{
    public class DtoCliente
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; }
        public string IE { get; set; }
        public bool IsentoIE { get; set; }
        public string TipoPessoaDescricao { get; set; }
        public string IsentoIEDescricao { get; set; }

    }
    public class DtoClienteCreate
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; } 
        public string IE { get; set; }
        public bool IsentoIE { get; set; }
        public DtoEnderecoCreate Endereco { get; set; }
    }

    public class DtoEnderecoCreate
    {
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
   
}