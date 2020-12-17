using Microsoft.AspNetCore.Razor.TagHelpers;
using OmEnergo.Infrastructure.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OmEnergo.Tests.Infrastructure.TagHelpers
{
	public class SidebarLinkTagHelperTests
	{
		private TagHelperContext tagHelperContext { get; set; }
		private TagHelperOutput tagHelperOutput { get; set; }

		public SidebarLinkTagHelperTests()
		{
			tagHelperContext = new TagHelperContext(
				new TagHelperAttributeList(),
				new Dictionary<object, object>(),
				"uniqueId");
			tagHelperOutput = new TagHelperOutput(
				"a",
				new TagHelperAttributeList(),
				(cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
		}

		[Fact]
		public void Process_LinkWithChildren_AddsClass()
		{
			//Arrange
			var tagHelper = new SidebarLinkTagHelper() { OmChildrenAmount = 1 };

			//Act
			tagHelper.Process(tagHelperContext, tagHelperOutput);
			var actual = tagHelperOutput.Attributes["class"].Value.ToString();

			//Assert
			Assert.Equal("fas fa-angle-right", actual);
		}

		[Fact]
		public void Process_LinkWithoutChildren_ClassIsNull()
		{
			//Arrange
			var tagHelper = new SidebarLinkTagHelper() { OmChildrenAmount = 0 };

			//Act
			tagHelper.Process(tagHelperContext, tagHelperOutput);
			var actual = tagHelperOutput.Attributes["class"]?.Value;

			//Assert
			Assert.Null(actual);
		}

		[Fact]
		public void Process_NegativeChildrenAmount_ThrowsException()
		{
			//Arrange
			var tagHelper = new SidebarLinkTagHelper() { OmChildrenAmount = -1 };

			//Act
			Action action = () => tagHelper.Process(tagHelperContext, tagHelperOutput);

			//Assert
			Assert.Throws<ArgumentOutOfRangeException>(action);
		}
	}
}
