namespace OmEnergo.Models.ViewModels
{
    public class ProductPropertyVM
    {
        public Stabilizer Model { get; set; }

        public string Value { get; set; }

        public ProductPropertyVM(Stabilizer name, string value)
        {
            Model = name;
            Value = value;
        }
    }
}
