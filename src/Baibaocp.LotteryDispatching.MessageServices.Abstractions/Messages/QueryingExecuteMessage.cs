namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class QueryingExecuteMessage : ExecuteMessage
    {
        public QueryingExecuteMessage(string ldpOrderId, string ldpVenderId) : base(ldpOrderId, ldpVenderId)
        {
        }
    }
}