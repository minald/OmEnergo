using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OmEnergo.Infrastucture.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "om-children-amount")]
    public class SidebarLinkTagHelper : TagHelper
    {
        public int OmChildrenAmount { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (OmChildrenAmount != 0)
            {
                output.Attributes.SetAttribute("class", "fas fa-angle-right");
            }
        }
    }
}
