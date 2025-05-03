using Rommanel.Infra.Context;
using Rommanel.Infra.Entity;
using Rommanel.Infra.Repositories.Interfaces;

namespace Rommanel.Infra.Repositories
{
    public class LogradouroRepository : RepositoryGeneric<Logradouro>, ILogradouroRepository
    {
        private ClientContext _appContext => (ClientContext)_context;

        public LogradouroRepository(ClientContext context) : base(context)
        { }
    }
}
