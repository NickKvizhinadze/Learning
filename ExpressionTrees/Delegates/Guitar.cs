namespace Delegates
{
    public class Guitar
    {
        public Guitar(string name, PickupType pickupType, StringType strings)
        {
            Name = name;
            PickupType = pickupType;
            Strings = strings;
        }


        public string Name { get; set; }
        public PickupType PickupType { get; set; }
        public StringType Strings { get; set; }
    }

    public enum PickupType
    {
        Acoustic,
        Electric,
        AcousticElectric
    }

    public enum StringType
    {
        Steel,
        Nylon
    }
}
