namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class QueryingExecuteMessage : ExecuteMessage
    {

        public string LvpOrderId { get; }

        public string LvpMerchanerId { get; }

        public QueryingTypes QueryingType { get; }

        public QueryingExecuteMessage(long ldpOrderId, string ldpMerchanerId, string lvpOrderId, string lvpMerchanerId,QueryingTypes queryingType) : base(ldpOrderId, ldpMerchanerId)
        {
            QueryingType = queryingType;
            LvpOrderId = lvpOrderId;
            LvpMerchanerId = lvpMerchanerId;
        }
    }
}