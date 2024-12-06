using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Clients
{
    public sealed class ClientService(BoulangerieContext _context): IClientService
    {
        public async Task<int> RecupererIdAsync(string _idPublicClient)
        {
            if (Guid.TryParse(_idPublicClient, out Guid idPublic))
                return await _context.Clients.Where(x => x.IdPublic == idPublic).Select(x => x.Id).FirstOrDefaultAsync();

            return 0;
        }

        public async Task<bool> ExisteAsync(string _idPublicClient, int _idGroupe)
        {
            if (Guid.TryParse(_idPublicClient, out Guid idPublic))
                return await _context.Clients.AnyAsync(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic);

            return false;
        }
    }
}
