namespace GoogleMapsCoreApi.Entities.Common
{
    public class AddressLocation: ILocationString
    {
        public string Address { get; private set; }

        public AddressLocation(string address)
        {
            Address = address;
        }

        public string LocationString
        {
            get { return Address; }
        }
    }
}
