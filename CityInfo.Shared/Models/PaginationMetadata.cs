﻿namespace CityInfo.Shared.Models;

public class PaginationMetadata
{
    public int TotalItemCount { get; set; }

    public int TotalPageCount { get; set; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }

    public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
    {
        TotalPageCount = totalItemCount;

        PageSize = pageSize;

        CurrentPage = currentPage;  

        TotalPageCount = (int)Math.Ceiling(TotalItemCount /(double)pageSize);
    }
}