namespace Rommanel.Infra.Entity
{

    public class Cliente : BaseEntity
    {
        public string CPF_CNPJ { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public char TipoPessoa { get; set; } // 'F' ou 'J'
        public string IE { get; set; }
        public bool IsentoIE { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Logradouro> Logradouros { get; set; }
    }
}
