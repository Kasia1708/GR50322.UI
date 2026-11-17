using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GR50322.UI.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "asp-action, asp-controller")]
    public class ImageTagHelper(LinkGenerator linkGenerator) :
  TagHelper
    {
        public string AspController { get; set; }
        public string AspAction { get; set; } = "Index";
        public override void Process(TagHelperContext context,
        TagHelperOutput output)
        {
            output.Attributes.Add("src",
            linkGenerator.GetPathByAction(AspAction, AspController, new  {t=Guid.NewGuid() }));
        }
    }
}
