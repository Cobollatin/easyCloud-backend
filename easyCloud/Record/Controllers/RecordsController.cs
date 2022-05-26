using AutoMapper;
using easyCloud.Quote.Resources;
using easyCloud.Record.Domain.Services;
using easyCloud.Record.Resources;
using Microsoft.AspNetCore.Mvc;

namespace easyCloud.Record.Controllers;
[Route("/api/v1/[controller]")]

public class RecordsController: ControllerBase
{
    private readonly IRecordService _recordService;
    private readonly IMapper _mapper;
    

    public RecordsController(IRecordService recordService, IMapper mapper)
    {
        _recordService = recordService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<RecordResource>> GetAllAsync()
    {
        var records = await _recordService.ListAsync();
        var resources = _mapper.Map<IEnumerable<Domain.Models.Record>, IEnumerable<RecordResource>>(records);

        return resources;
    }
}