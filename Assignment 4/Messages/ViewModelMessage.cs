using Assignment_4;

namespace Assignment_4.Messages
{
    public class MessageMember : Customer
    {
        public MessageMember(Customer customer, string message)
        {
            Customer = customer;
            Message = message;
        }
        public string Message { get; private set; }
        public Customer Customer { get; private set; }
    }
}
