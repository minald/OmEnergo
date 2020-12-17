using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace OmEnergo.Infrastructure.TagHelpers
{
	[HtmlTargetElement("a", Attributes = "om-children-amount")]
	public class SidebarLinkTagHelper : TagHelper
	{
		public int OmChildrenAmount { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (OmChildrenAmount < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			if (OmChildrenAmount != 0)
			{
				output.Attributes.SetAttribute("class", "fas fa-angle-right");
			}
		}
	}
}
