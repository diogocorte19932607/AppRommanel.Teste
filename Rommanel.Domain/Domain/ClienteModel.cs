namespace Rommanel.Domain.Domain
{
    public class ClienteModel : BaseDomain
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; }
        public string TipoPessoaDescricao
        {
            get
            {
                return TipoPessoa == 'F' ? "Física" :
                       TipoPessoa == 'J' ? "Jurídica" : "Desconhecido";
            }
        }
        public string IE { get; set; }
        public bool IsentoIE { get; set; }
        public string IsentoIEDescricao
        {
            get
            {
                return IsentoIE ? "Sim" : "Não";
            }
        }
    }
}
