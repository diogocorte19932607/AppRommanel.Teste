using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rommanel.Domain.Service;
using Rommanel.Domain.Domain;
using Rommanel.Infra.Entity;
using Rommanel.Infra.Repositories.Interfaces;
using Rommanel.Infra.UnitofWork;
using Rommanel.Infra.Context;

namespace Rommanel.Domain.Tests.Service
{
    public class LogradouroServiceTests
    {
        private readonly Mock<ILogradouroRepository> _logradouroRepositoryMock;
        private readonly Mock<IUnitofWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ClientContext _dbContext;
        private readonly LogradouroService<LogradouroModel, Logradouro> _service;

        public LogradouroServiceTests()
        {
            _logradouroRepositoryMock = new Mock<ILogradouroRepository>();
            _unitOfWorkMock = new Mock<IUnitofWork>();
            _mapperMock = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<ClientContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ClientContext(options);

            _service = new LogradouroService<LogradouroModel, Logradouro>(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _logradouroRepositoryMock.Object,
                _dbContext
            );
        }

        [Fact]
        public void ModelarLogradouro_DeveCriarObjetoCorretamente()
        {
            var model = new LogradouroCreateModel
            {
                IdCliente = Guid.NewGuid(),
                CEP = "12345-678",
                Rua = "Rua Teste",
                Cidade = "Cidade",
                Bairro = "Bairro",
                Estado = "SP",
                Numero = "100",
                Complemento = "Apto 1"
            };

            var result = _service.ModelarLogradouro(model);

            Assert.Equal(model.CEP, result.CEP);
            Assert.Equal(model.Rua, result.Rua);
            Assert.Equal(model.Estado, result.Estado);
        }

        [Fact]
        public async Task DeletarLogradouro_DeveRemoverComSucesso()
        {
            var id = Guid.NewGuid();
            var logradouro = new Logradouro { Id = id };
            _logradouroRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<System.Linq.Expressions.Expression<Func<Logradouro, bool>>>())).Returns(logradouro);

            var result = await _service.DeletarLogradouro(id.ToString());

            _logradouroRepositoryMock.Verify(r => r.Remove(logradouro), Times.Once);
            _logradouroRepositoryMock.Verify(r => r.Save(), Times.Once);
            Assert.Equal(id.ToString(), result);
        }

        [Fact]
        public async Task DeletarLogradouro_DeveLancarExcecao_SeNaoEncontrado()
        {
            _logradouroRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<System.Linq.Expressions.Expression<Func<Logradouro, bool>>>())).Returns((Logradouro)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeletarLogradouro(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task ListarLogradouros_DeveRetornarLista()
        {
            _logradouroRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Logradouro>
            {
                new Logradouro
                {
                    Id = Guid.NewGuid(),
                    CEP = "00000-000",
                    Rua = "Rua 1",
                    Cidade = "Cidade 1",
                    Bairro = "Bairro 1",
                    Estado = "SP",
                    Numero = "10",
                    Complemento = "Comp 1",
                    IdCliente = Guid.NewGuid()
                }
            });

            var result = await _service.ListarLougradouros();

            Assert.Single(result);
            Assert.Equal("Rua 1", result[0].Rua);
        }
    }
}

