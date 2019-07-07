using Microsoft.AspNetCore.Razor.TagHelpers;
using OmEnergo.Infrastructure.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OmEnergo.Tests.Infrastructure.TagHelpers
{
    public class SidebarLinkTagHelperTests
    {
        private TagHelperContext TagHelperContext { get; set; }
        private TagHelperOutput TagHelperOutput { get; set; }

        public SidebarLinkTagHelperTests()
        {
            TagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "uniqueId");
            TagHelperOutput = new TagHelperOutput(
                "a",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
        }

        [Fact]
        public void LinkWithChildren()
        {
            //Arrange
            var tagHelper = new SidebarLinkTagHelper() { OmChildrenAmount = 1 };

            //Act
            tagHelper.Process(TagHelperContext, TagHelperOutput);

            //Assert
            string actual = TagHelperOutput.Attributes["class"].Value.ToString();
            Assert.Equal("fas fa-angle-right", actual);
        }

        [Fact]
        public void LinkWithoutChildren()
        {
            //Arrange
            var tagHelper = new SidebarLinkTagHelper() { OmChildrenAmount = 0 };

            //Act
            tagHelper.Process(TagHelperContext, TagHelperOutput);

            //Assert
            object actual = TagHelperOutput.Attributes["class"]?.Value;
            Assert.Null(actual);
        }
    }
}
