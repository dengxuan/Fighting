namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class QueryingExecuteMessage : ExecuteMessage
    {
        public QueryingTypes QueryingType { get; }

        public QueryingExecuteMessage(string ldpOrderId, string ldpVenderId, QueryingTypes queryingType) : base(ldpOrderId, ldpVenderId)
        {
            QueryingType = queryingType;
        }
    }
}