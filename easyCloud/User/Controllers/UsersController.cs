using AutoMapper;
using easyCloud.User.Domain.Services;
using easyCloud.User.Resources;
using Microsoft.AspNetCore.Mvc;

namespace easyCloud.User.Controller;
[Route("/api/v1/[controller]")]
    
public class UsersController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<UserResource>> GetAllAsync()
    {
        var users = await _userService.ListAsync();
        var resources = _mapper.Map<IEnumerable<Domain.Models.User>, IEnumerable<UserResource>>(users);

        return resources;
    }
}