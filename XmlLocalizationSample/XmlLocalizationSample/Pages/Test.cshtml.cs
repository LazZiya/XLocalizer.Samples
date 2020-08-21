using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XLocalizer.DataAnnotations;

namespace XmlLocalizationSample.Pages
{
    public class TestModel : PageModel
    {
        [ExRequired]
        [ExMinLength(3)]
        [Display(Name = "Min")]
        public string Min { get; set; }
        
        [ExRequired]
        [ExMaxLength(3)]
        [Display(Name = "Max")]
        public string Max { get; set; }
        
        [ExRequired]
        [ExRange(3, 6)]
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
