namespace DevPace.Models
{
    public class EventsHub
    {
        private static EventsHub? _object;

        public event EventHandler<Customer?>? OpenCustomerEvent;

        private EventsHub() { }

        public static EventsHub GetInstance()
        {
            if (_object == null)
                _object = new();

            return _object;
        }

        public void OpenCustomer(object? sender, Customer? customer)
        {
            OpenCustomerEvent?.Invoke(sender, customer);
        }
    }
}
