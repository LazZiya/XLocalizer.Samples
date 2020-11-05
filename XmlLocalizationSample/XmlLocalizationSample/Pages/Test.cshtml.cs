using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XmlLocalizationSample.Pages
{
    public class TestModel : PageModel
    {
        [Required]
        [MinLength(3)]
        [Display(Name = "Min")]
        public string Min { get; set; }
        
        [Required]
        [MaxLength(3)]
        [Display(Name = "Max")]
        public string Max { get; set; }
        
        [Required]
        [Range(3, 6)]
        [Display(Name = "Range")]
        public string Range { get; set; }


        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}
