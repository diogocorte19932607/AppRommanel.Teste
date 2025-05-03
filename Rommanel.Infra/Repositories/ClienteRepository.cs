using Rommanel.Infra.Context;
using Rommanel.Infra.Entity;
using Rommanel.Infra.Repositories.Interfaces;

namespace Rommanel.Infra.Repositories
{
    public class ClienteRepository : RepositoryGeneric<Cliente>, IClienteRepository
    {
        private ClientContext _appContext => (ClientContext)_context;

        public ClienteRepository(ClientContext context) : base(context)
        { }
    }
}
