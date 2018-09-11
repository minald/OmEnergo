using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Section
    {
        public int Id { get; set; }

        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;
    }
}
