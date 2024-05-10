namespace Price_Checker
{
    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string UOM { get; internal set; }
        public string Manufacturer { get; set; }
        public string Generic { get; set; }
        public string Vendor { get; set; }
        //public string UOM { get; set; }
    }
}
