using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        public string EnglishName { get; set; }

        public string RussianName { get; set; }

        public string MainImageLink { get; set; }

        public string Description { get; set; }

        public string GetImageFullLink() => $"/images/{EnglishName}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;
    }
}
