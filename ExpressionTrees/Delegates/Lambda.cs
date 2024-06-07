namespace Delegates
{
    public class Lambda
    {
        public void Main()
        {
              var guitars = new List<Guitar>
            {
                new Guitar("Cherry Red Strat", PickupType.Electric, StringType.Steel),
                new Guitar("Takamine EG-166", PickupType.AcousticElectric, StringType.Nylon),
                new Guitar("Martin D-X1E", PickupType.Acoustic, StringType.Steel),
                };

            Func<Guitar, bool> nylon = g => g.Strings == StringType.Nylon;

            IEnumerable<Guitar> electricGuitars = Enumerable.Where<Guitar>(guitars, g => g.PickupType == PickupType.Electric);

            var nylonGuitars = guitars.Where(nylon);
        }
    }
}
