using Payments.Common.NLog;

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
            NLog.LogInfo(this.GetType(), "Constructor OperationDetails execution");

            Succedeed = succedeed;
            Message = message;
            Prop = prop;
        }
    }
}