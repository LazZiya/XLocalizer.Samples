using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBLocalizationSample.Pages
{
    public class ModelBindingTestModel : PageModel
    {
        [Display(Name = "What is your age?")]
        [BindProperty(SupportsGet = true)]
        public int Number { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}
