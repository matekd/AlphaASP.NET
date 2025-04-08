using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class MemberAddressRepository(DataContext context) : BaseRepository<MemberAddressEntity>(context), IMemberAddressRepository
{
}
