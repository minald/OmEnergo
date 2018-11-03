using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Product : ProductObject
    {
        public Section Section { get; set; }

        public List<ProductModel> Models { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public Product() { }

        public Product(Section section)
        {
            Section = section;
            UpdateProperties(section.GetProductPropertyList());
        }

        public override string GetImageFullLink() => $"/images/{Section.Name}/{Name.Replace('"', '\'')}/{MainImageLink}";
    }
}
