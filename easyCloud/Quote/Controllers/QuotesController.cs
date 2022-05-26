using AutoMapper;
using easyCloud.Quote.Domain.Services;
using easyCloud.Quote.Resources;
using Microsoft.AspNetCore.Mvc;

namespace easyCloud.Quote.Controllers;
[Route("/api/v1/[controller]")]

public class QuotesController: ControllerBase
{
    private readonly IQuoteService _quoteService;
    private readonly IMapper _mapper;
    

    public QuotesController(IQuoteService quoteService, IMapper mapper)
    {
        _quoteService = quoteService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<QuoteResource>> GetAllAsync()
    {
        var quotes = await _quoteService.ListAsync();
        var resources = _mapper.Map<IEnumerable<Domain.Models.Quote>, IEnumerable<QuoteResource>>(quotes);

        return resources;
    }
}