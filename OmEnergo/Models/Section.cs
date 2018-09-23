using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public override string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;

        public bool HasChildSections() => ChildSections.Count != 0;
    }
}
