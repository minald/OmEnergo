using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmEnergo.Models
{
    public class Section : CommonObject
    {
        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Свойства продукта")]
        public string ProductProperties { get; set; }

        [NotMapped]
        [Display(Name = "Свойства модели продукта")]
        public string ProductModelProperties { get; set; }

        public override string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;

        public bool HasChildSections() => ChildSections.Count != 0;
    }
}
