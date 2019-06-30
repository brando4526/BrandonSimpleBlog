using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Globalization;

namespace BrandonSimpleBlog.Pages
{
    public class ArchiveModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public ArchiveModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }

        public BlogResult MonthResults { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Resultpage { get; set; }
        public void OnGet(int year, int month)
        {
            MonthResults = _blogRepo.GetPublishedPostsByMonth(month,year);
            Year = year;
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
    }
}