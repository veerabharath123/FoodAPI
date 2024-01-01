using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Helpers
{
    public static class Pager
    {
        private static PagerResponse<T> PaginationLogic<T>(this PagerRequest request,int totalItems)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)request.pageSize);
            if (request.currentPage < 1)
            {
                request.currentPage = 1;
            }
            else if (request.currentPage > totalPages)
            {
                request.currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= request.maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)request.maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)request.maxPages / (decimal)2) - 1;
                if (request.currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = request.maxPages;
                }
                else if (request.currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - request.maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = request.currentPage - maxPagesBeforeCurrentPage;
                    endPage = request.currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (request.currentPage - 1) * request.pageSize;
            var endIndex = Math.Min(startIndex + request.pageSize - 1, totalItems - 1);
            return new PagerResponse<T>
            {
                TotalItems = totalItems,
                CurrentPage = request.currentPage,
                PageSize = request.pageSize,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage,
                StartIndex = startIndex,
                EndIndex = endIndex,
            };
        }
        public async static Task<PagerResponse<T>> PaginateAsync<T>(this IQueryable<T> Pages, PagerRequest request)
        {
            var result = new PagerResponse<T>();
            var totalItems = await Pages.CountAsync();
            if(totalItems == 0) return result;
            result = request.PaginationLogic<T>(totalItems);
            result.Pages = await Pages.Skip(result.StartIndex).Take(request.pageSize).ToListAsync();
            return result;
        }
        public static PagerResponse<T> Paginate<T>(IQueryable<T> Pages, PagerRequest request)
        {
            var result = new PagerResponse<T>();
            var totalItems = Pages.Count();
            if (totalItems == 0) return result;
            result = request.PaginationLogic<T>(totalItems);
            result.Pages = Pages.Skip(result.StartIndex).Take(request.pageSize).ToList();
            return result;
        }
        public static async Task<PagerResponse<T>> PaginateAsync<T>(IEnumerable<T> Pages, PagerRequest request)
        {
            var task = Task.Run(() =>
            {
                var result = new PagerResponse<T>();
                var totalItems = Pages.Count();
                if (totalItems == 0) return result;
                result = request.PaginationLogic<T>(totalItems);
                result.Pages = Pages.Skip(result.StartIndex).Take(request.pageSize).ToList();
                return result;
            });
            return await task;
        }
    }
    public class PagerRequest
    {
        public int currentPage { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int maxPages { get; set; } = 10;
        public string? view { get; set; } = "table-view";
    }
    public class PagerResponse<T>
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public IEnumerable<T> Pages { get; set; } = new List<T>();
    }
}
