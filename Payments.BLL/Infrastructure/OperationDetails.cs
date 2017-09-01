namespace Payments.BLL.Infrastructure
{
    // contain information about operation
    public class OperationDetails
    {
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Prop { get; private set; }

        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Prop = prop;
        }
    }
}