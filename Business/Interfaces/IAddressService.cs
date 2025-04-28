using Business.Models;
using Domain.Models;

namespace Business.Services;

public interface IAddressService
{
    Task<MemberAddressResult> CreateAsync(MemberAddressDto model);
    Task<BoolResult> UpdateAsync(MemberAddressDto model);
    Task<BoolResult> DeleteAsync(string id);
    bool IsValid(MemberAddress address);
    bool IsEmpty(MemberAddress address);
    Task<bool> Exists(string memberId);
}