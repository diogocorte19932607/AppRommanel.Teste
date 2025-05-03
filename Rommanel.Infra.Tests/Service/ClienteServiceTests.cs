using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Rommanel.Domain.Configuration;
using Rommanel.Domain.Domain;
using Rommanel.Domain.Service;
using Rommanel.Infra.Entity;
using Rommanel.Infra.Repositories.Interfaces;
using Rommanel.Infra.UnitofWork;

namespace Rommanel.Domain.Tests.Service
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<ILogradouroRepository> _logradouroRepositoryMock;
        private readonly Mock<IUnitofWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ClienteService<ClienteModel, Cliente> _service;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _logradouroRepositoryMock = new Mock<ILogradouroRepository>();
            _unitOfWorkMock = new Mock<IUnitofWork>();
            _mapperMock = new Mock<IMapper>();
            _appSettings = Options.Create(new AppSettings());

            _service = new ClienteService<ClienteModel, Cliente>(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _clienteRepositoryMock.Object,
                _logradouroRepositoryMock.Object,
                _appSettings
            );
        }

        [Fact]
        public async Task AdicionarCliente_DeveRetornarErro_SeEmailJaExistente()
        {
            var model = new ClienteCreateModel { Email = "existe@email.com", CPF_CNPJ = "12345678901", TipoPessoa = 'F', DataNascimento = DateTime.Today.AddYears(-20) };
            _clienteRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<Cliente, bool>>>())).Returns(new List<Cliente> { new Cliente() });

            var result = await _service.AdicionarCliente(model);

            Assert.Equal(400, result.ExibicaoMensagem.StatusCode);
            Assert.Contains("E-mail já cadastrado", result.ExibicaoMensagem.MensagemCurta);
        }

        [Fact]
        public async Task AdicionarCliente_DeveRetornarErro_SeCpfJaExistente()
        {
            var model = new ClienteCreateModel { Email = "novo@email.com", CPF_CNPJ = "12345678901", TipoPessoa = 'F', DataNascimento = DateTime.Today.AddYears(-20) };

            _clienteRepositoryMock.Setup(x => x.Find(It.Is<Expression<Func<Cliente, bool>>>(e => e.Compile().Invoke(new Cliente { Email = "outro@email.com" })))).Returns(new List<Cliente>());
            _clienteRepositoryMock.Setup(x => x.Find(It.Is<Expression<Func<Cliente, bool>>>(e => e.Compile().Invoke(new Cliente { CPF_CNPJ = "12345678901" })))).Returns(new List<Cliente> { new Cliente() });

            var result = await _service.AdicionarCliente(model);

            Assert.Equal(400, result.ExibicaoMensagem.StatusCode);
            Assert.Contains("CPF/CNPJ já cadastrado", result.ExibicaoMensagem.MensagemCurta);
        }

        [Fact]
        public async Task AdicionarCliente_DeveRetornarErro_SeMenorDeIdade()
        {
            var model = new ClienteCreateModel
            {
                Email = "menor@email.com",
                CPF_CNPJ = "00000000000",
                TipoPessoa = 'F',
                DataNascimento = DateTime.Today.AddYears(-17)
            };

            var result = await _service.AdicionarCliente(model);

            Assert.Equal(400, result.ExibicaoMensagem.StatusCode);
            Assert.Contains("Idade mínima", result.ExibicaoMensagem.MensagemCurta);
        }

        [Fact]
        public async Task AtualizarCliente_DeveAtualizarComSucesso()
        {
            var id = Guid.NewGuid();
            var clienteExistente = new Cliente { Id = id, Nome = "Antigo", Email = "antigo@email.com" };
            var clienteEdit = new ClienteEditModel
            {
                Id = id,
                Nome = "Novo Nome",
                Email = "novo@email.com",
                CPF_CNPJ = "123",
                TipoPessoa = 'F',
                IE = "Isento"
            };

            _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>())).Returns(new List<Cliente> { clienteExistente });

            var result = await _service.AtualizarCliente(clienteEdit);

            _clienteRepositoryMock.Verify(r => r.Update(It.IsAny<Cliente>()), Times.Once);
            _clienteRepositoryMock.Verify(r => r.Save(), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AtualizarCliente_DeveLancarExcecao_SeNaoEncontrado()
        {
            _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>())).Returns(new List<Cliente>());

            var clienteEdit = new ClienteEditModel { Id = Guid.NewGuid() };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AtualizarCliente(clienteEdit));
        }

        [Fact]
        public async Task DeletarCliente_DeveRemoverComSucesso()
        {
            var id = Guid.NewGuid();
            var cliente = new Cliente { Id = id };

            _clienteRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Cliente, bool>>>())).Returns(cliente);

            var result = await _service.DeletarCliente(id.ToString());

            _clienteRepositoryMock.Verify(r => r.Remove(cliente), Times.Once);
            _clienteRepositoryMock.Verify(r => r.Save(), Times.Once);
            Assert.Equal(id.ToString(), result);
        }

        [Fact]
        public async Task DeletarCliente_DeveLancarExcecao_SeNaoEncontrado()
        {
            _clienteRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Cliente, bool>>>())).Returns((Cliente)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeletarCliente(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task ListarClientes_DeveRetornarClientes()
        {
            _clienteRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Cliente>
            {
                new Cliente { Id = Guid.NewGuid(), Nome = "Nome 1", Email = "email1@x.com" },
                new Cliente { Id = Guid.NewGuid(), Nome = "Nome 2", Email = "email2@x.com" }
            });

            var result = await _service.ListarClientes();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ListarClientesPorId_DeveRetornarClienteFiltrado()
        {
            var id = Guid.NewGuid();
            _clienteRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Cliente>
            {
                new Cliente { Id = id, Nome = "Filtrado", Email = "f@x.com" },
                new Cliente { Id = Guid.NewGuid(), Nome = "Outro", Email = "z@x.com" }
            });

            var result = await _service.ListarClientesPorId(id);

            Assert.Single(result);
            Assert.Equal("Filtrado", result[0].Nome);
        }

        [Fact]
        public async Task BuscarClienteEmail_DeveRetornarCliente()
        {
            _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                .Returns(new List<Cliente> { new Cliente { Email = "email@email.com" } });

            var result = await _service.BuscarClienteEmail("email@email.com");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task BuscarClienteCpfCnpj_DeveRetornarCliente()
        {
            _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                .Returns(new List<Cliente> { new Cliente { CPF_CNPJ = "123456" } });

            var result = await _service.BuscarClienteCpfCnpj("123456");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task BuscarClienteId_DeveRetornarCliente()
        {
            var id = Guid.NewGuid();
            _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                .Returns(new List<Cliente> { new Cliente { Id = id } });

            var result = await _service.BuscarClienteId(id);

            Assert.NotNull(result);
        }
    }
}