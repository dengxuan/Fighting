namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class QueryingDispatchMessage : DispatchMessage
    {

        public string LvpOrderId { get; }

        public string LvpMerchanerId { get; }

        public int LotteryId { get; set; }

        public QueryingTypes QueryingType { get; }

        public QueryingDispatchMessage(string ldpOrderId, string ldpMerchanerId, string lvpOrderId, string lvpMerchanerId, int lotteryId, QueryingTypes queryingType) : base(ldpOrderId, ldpMerchanerId)
        {
            QueryingType = queryingType;
            LvpOrderId = lvpOrderId;
            LvpMerchanerId = lvpMerchanerId;
            LotteryId = lotteryId;
        }
    }
}