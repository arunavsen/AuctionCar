﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();

        // Adding a default sort order to avoid the "Paging without sorting" error
        query.Sort(x => x.Ascending(a => a.CreatedAt));

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        //query = searchParams.FilterBy switch
        //{
        //    "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
        //    "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
        //        && x.AuctionEnd > DateTime.UtcNow),
        //    _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        //};



        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        Console.WriteLine($"Total Count: {result.TotalCount}, Page Count: {result.PageCount}, Results Count: {result.Results.Count}");

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }




}