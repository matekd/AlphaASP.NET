using Business.Factories;
using Business.Models;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Business.Services;

public class AddressService(IMemberAddressRepository memberAddressRepository) : IAddressService
{
    private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

    public async Task<MemberAddressResult> CreateAsync(MemberAddressDto model)
    {
        if (model == null)
            return new MemberAddressResult { Success = false, StatusCode = 400, Error = "Address can't be null." };

        var result = await _memberAddressRepository.AddAsync(AddressFactory.Create(model)!);

        return result.Success
            ? new MemberAddressResult { Success = true, StatusCode = 201, Result = AddressFactory.Create(result.Result!) }
            : new MemberAddressResult { Success = false, StatusCode = 400, Error = "Failed to create address." };
    }

    public async Task<BoolResult> UpdateAsync(MemberAddressDto model)
    {
        if (model == null)
            return new BoolResult { Success = false, StatusCode = 400, Error = "Model can't be null." };
        var result = await _memberAddressRepository.UpdateAsync(AddressFactory.Create(model)!);
        return result.Success
            ? new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Result = result.Result }
            : new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<BoolResult> DeleteAsync(string id)
    {
        var entityRes = await _memberAddressRepository.GetAsync(x => x.MemberId == id);
        if (!entityRes.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Address not found." };

        var result = await _memberAddressRepository.DeleteAsync(entityRes.Result!);
        return result.Success
            ? new BoolResult { Success = true, StatusCode = result.StatusCode }
            : new BoolResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public bool IsValid(MemberAddress address)
    {
        return !string.IsNullOrEmpty(address.StreetName) && !string.IsNullOrEmpty(address.PostalCode) && !string.IsNullOrEmpty(address.City);
    }

    // Clearing an address is interpreted as wanting to delete it
    public bool IsEmpty(MemberAddress address)
    {
        return string.IsNullOrEmpty(address.StreetName) && string.IsNullOrEmpty(address.PostalCode) && string.IsNullOrEmpty(address.City);
    }

    public async Task<bool> Exists(string memberId)
    {
        var result = await _memberAddressRepository.AnyAsync(x => x.MemberId == memberId);
        return result.Success;
    }
}
