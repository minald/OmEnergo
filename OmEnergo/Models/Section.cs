using System.Collections.Generic;

namespace OmEnergo.Models
{
    public class Section
    {
        public int Id { get; set; }

        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        public string Name { get; set; }

        public string MainImageLink { get; set; }

        public string Description { get; set; }

        public string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;
    }
}
