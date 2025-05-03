using AutoMapper;
using Microsoft.Extensions.Options;
using Rommanel.Domain.Configuration;
using Rommanel.Domain.Domain;
using Rommanel.Domain.Service.Generic;
using Rommanel.Infra.Entity;
using Rommanel.Infra.Repositories.Interfaces;
using Rommanel.Infra.UnitofWork;

namespace Rommanel.Domain.Service
{
    public class ClienteService<Tv, Te> : GenericServiceAsync<Tv, Te>
                                               where Tv : ClienteModel
                                               where Te : Cliente
    {
        IClienteRepository _clienteRepository;
        ILogradouroRepository _logradouroRepository;
        private readonly AppSettings _appSettings;
        public ClienteService(IUnitofWork unitOfWork, IMapper mapper,
                             IClienteRepository clienteRepository, ILogradouroRepository logradouroRepository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _clienteRepository = clienteRepository;
            _logradouroRepository = logradouroRepository;
        }
        public async Task<Cliente> ModelarCliente(ClienteCreateModel cliente)
        {
            var result = new Cliente
            {
                Nome = cliente.Nome,
                Email = cliente.Email,
                CPF_CNPJ = cliente.CPF_CNPJ,
                DataNascimento = cliente.DataNascimento,
                Telefone = cliente.Telefone,
                TipoPessoa = cliente.TipoPessoa,
                IE = cliente.IE,
                IsentoIE = cliente.IsentoIE
            };
            var tempId = Guid.NewGuid();
            return result;
        }
        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AdicionarCliente(ClienteCreateModel cliente)
        {
            var clienteExistente = await BuscarClienteEmail(cliente.Email);
            if (clienteExistente != null)
            {
                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "E-mail",
                        Detalhes = "E-mail já cadastrado!",
                        MensagemCurta = "E-mail já cadastrado!",
                        StatusCode = 400
                    },
                    Objeto = Guid.Empty
                };
            }

            var cpfexistente = await BuscarClienteCpfCnpj(cliente.CPF_CNPJ);

            if (cpfexistente != null)
            {
                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "CPF/CNPJ",
                        Detalhes = "Já existe um cliente com este CPF/CNPJ.",
                        MensagemCurta = "CPF/CNPJ já cadastrado!",
                        StatusCode = 400
                    },
                    Objeto = Guid.Empty
                };
            }

            if (cliente.TipoPessoa == 'F')
            {
                if (!cliente.DataNascimento.HasValue || DateTime.Today.Year - cliente.DataNascimento.Value.Year < 18)
                {
                    return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                    {
                        ExibicaoMensagem = new ExibicaoMensagemViewModel
                        {
                            Cabecalho = "Data de Nascimento",
                            Detalhes = "Clientes pessoa física devem ter no mínimo 18 anos.",
                            MensagemCurta = "Idade mínima: 18 anos.",
                            StatusCode = 400
                        },
                        Objeto = Guid.Empty
                    };
                }
            }

            if (cliente.TipoPessoa == 'J' && !cliente.IsentoIE && string.IsNullOrWhiteSpace(cliente.IE))
            {
                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "Inscrição Estadual",
                        Detalhes = "Pessoa jurídica deve informar a IE ou marcar como isento.",
                        MensagemCurta = "IE obrigatório ou marcar como isento.",
                        StatusCode = 400
                    },
                    Objeto = Guid.Empty
                };
            }

            try
            {
                var entityCliente = await ModelarCliente(cliente);
                _clienteRepository.Add(entityCliente);
                _clienteRepository.Save();
                var logradouro = new Logradouro
                {
                    Id = Guid.NewGuid(),
                    IdCliente = entityCliente.Id,
                    CEP = cliente.Endereco.CEP,
                    Rua = cliente.Endereco.Endereco,
                    Numero = cliente.Endereco.Numero,
                    Bairro = cliente.Endereco.Bairro,
                    Cidade = cliente.Endereco.Cidade,
                    Estado = cliente.Endereco.Estado
                };

                _logradouroRepository.Add(logradouro);
                _logradouroRepository.Save();

                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "Cliente",
                        Detalhes = "Cliente cadastrado com sucesso!",
                        MensagemCurta = "Cadastrado com sucesso!",
                        StatusCode = 201
                    },
                    Objeto = entityCliente.Id
                };
            }
            catch (Exception e)
            {
                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "Erro",
                        Detalhes = e.Message,
                        MensagemCurta = "Falha ao salvar cliente",
                        StatusCode = 500
                    },
                    Objeto = Guid.Empty
                };
            }
        }
        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AtualizarCliente(ClienteEditModel cliente)
        {
            var clienteAtualizar = await BuscarClienteId(cliente.Id);

            if (clienteAtualizar == null)
            {
                throw new ArgumentNullException(nameof(clienteAtualizar), "Ciente não existe.");
            }

            var retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();

            try
            {
                clienteAtualizar.Email = cliente.Email;
                clienteAtualizar.Nome = cliente.Nome;
                clienteAtualizar.CPF_CNPJ = cliente.CPF_CNPJ;
                clienteAtualizar.TipoPessoa = cliente.TipoPessoa;
                clienteAtualizar.IE = cliente.IE;
                _clienteRepository.Update(clienteAtualizar);
                _clienteRepository.Save();

                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> DeletarCliente(string idCliente)
        {

            var result = _clienteRepository.GetSingleOrDefault(x => x.Id.ToString() == idCliente);

            if (result == null)
                throw new Exception("Cliente não encontrado.");

            _clienteRepository.Remove(result);
            _clienteRepository.Save();

            return idCliente;
        }
        public async Task<List<ClienteModel>> ListarClientes()
        {
            var ClienteAtivos = _clienteRepository.GetAll();

            List<ClienteModel> clientes = new List<ClienteModel>();
            foreach (var elem in ClienteAtivos)
            {
                var lista = new ClienteModel();
                lista.Id = elem.Id;
                lista.Nome = elem.Nome;
                lista.Email = elem.Email;
                clientes.Add(lista);
            }
            return clientes.ToList();
        }
        public async Task<List<ClienteModel>> ListarClientesPorId(Guid id)
        {
            var ClienteAtivos = _clienteRepository.GetAll().Where(x => x.Id == id);

            List<ClienteModel> clientes = new List<ClienteModel>();
            foreach (var elem in ClienteAtivos)
            {
                var lista = new ClienteModel();
                lista.Id = elem.Id;
                lista.Nome = elem.Nome;
                lista.Email = elem.Email;
                lista.IsentoIE = elem.IsentoIE;
                lista.DataNascimento = elem.DataNascimento;
                lista.TipoPessoa = elem.TipoPessoa;
                lista.CPF_CNPJ = elem.CPF_CNPJ;
                lista.IE = elem.IE;
                lista.Telefone = elem.Telefone;
                clientes.Add(lista);
            }
            return clientes.ToList();
        }
        public async Task<Cliente> BuscarClienteEmail(string email)
        {
            var cliente = _clienteRepository.Find(c => c.Email == email).FirstOrDefault();

            return cliente;
        }
        public async Task<Cliente> BuscarClienteCpfCnpj(string cpf)
        {
            var cliente = _clienteRepository.Find(c => c.CPF_CNPJ == cpf).FirstOrDefault();

            return cliente;
        }
        public async Task<Cliente> BuscarClienteId(Guid id)
        {
            var cliente = _clienteRepository.Find(c => c.Id == id).FirstOrDefault();

            return cliente;
        }
    }
}