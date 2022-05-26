using AutoMapper;
using easyCloud.Provider.Domain.Services;
using easyCloud.Provider.Resources;
using Microsoft.AspNetCore.Mvc;

namespace easyCloud.Provider.Controllers;
[Route("/api/v1/[controller]")]
public class ProvidersController: ControllerBase
{
    private readonly IProviderService _providerService;
    private readonly IMapper _mapper;
    

    public ProvidersController(IProviderService providerService, IMapper mapper)
    {
        _providerService = providerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ProviderResource>> GetAllAsync()
    {
        var providers = await _providerService.ListAsync();
        var resources = _mapper.Map<IEnumerable<Domain.Models.Provider>, IEnumerable<ProviderResource>>(providers);

        return resources;
    }
}