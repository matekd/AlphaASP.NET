using Business.Factories;
using Business.Models;
using Data.Interfaces;
using Domain.Models;

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
}
