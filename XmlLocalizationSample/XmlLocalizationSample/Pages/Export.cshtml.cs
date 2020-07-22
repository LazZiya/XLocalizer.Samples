using System;
using System.Threading.Tasks;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using XLocalizer;
using XmlLocalizationSample.LocalizationResources;

namespace XmlLocalizationSample.Pages
{
    [ValidateAntiForgeryToken]
    public class ExportModel : PageModel
    {
        private readonly IXResourceExporter _exporter;
        private readonly IStringLocalizer _localizer;
        public ExportModel(IXResourceExporter exporter, IStringLocalizer<ExportModel> localizer)
        {
            _exporter = exporter;

            // It is possible to create model based localizer instance,
            // by passing the model as generic argument IStringLocalizer<ExportModel>
            // XLocaizer will automatically generate relevant xml resource file
            _localizer = localizer;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string culture, bool approvedOnly, bool overwrite)
        {
            try
            {
                var totalExported = await _exporter.ExportToResxAsync<LocSource>(culture, approvedOnly, overwrite);

                if (totalExported == 0)
                {
                    var msg = _localizer["No resources has been exported!"];
                    TempData.Warning(msg.Value);
                }
                else
                {
                    var msg = _localizer["Total {0} resources exported.", totalExported];
                    TempData.Success(msg.Value);
                }
            }
            catch (Exception e)
            {
                TempData.Danger(e.Message);
            }

            return Page();
        }
    }
}
