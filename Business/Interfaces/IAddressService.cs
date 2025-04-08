using Business.Models;
using Domain.Models;

namespace Business.Services;

public interface IAddressService
{
    Task<MemberAddressResult> CreateAsync(MemberAddressDto model);
}