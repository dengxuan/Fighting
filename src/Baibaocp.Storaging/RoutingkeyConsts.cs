namespace Baibaocp.Storaging.Entities
{
    public static class RoutingkeyConsts
    {
        /// <summary>
        /// 订单队列
        /// </summary>
        public static class Orders
        {
            /// <summary>
            /// 接受投注
            /// </summary>
            public static class Accepted
            {

                /// <summary>
                /// 三方订单
                /// </summary>
                public static class Customer
                {
                    public const string General = "Orders.Accepted.Customer.General";

                    public const string PointExchange = "Orders.Accepted.Customer.PointExchange";
                }

                /// <summary>
                /// 自营订单
                /// </summary>
                public static class PrivateVender
                {
                    public const string Hongdan = "Orders.Accepted.PrivateVender.Hongdan";

                    public const string Mobiles = "Orders.Accepted.PrivateVender.Mobiles";
                }
            }

            /// <summary>
            /// 待投注
            /// </summary>
            public const string Storaged = "Orders.Storaged";

            /// <summary>
            /// 投注完成
            /// </summary>
            public static class Completed
            {
                /// <summary>
                /// 投注成功
                /// </summary>
                public const string Success = "Orders.Completed.Success";

                /// <summary>
                /// 投注失败
                /// </summary>
                public const string Failure = "Orders.Completed.Failure";
            }
        }

        /// <summary>
        /// 出票队列
        /// </summary>
        public static class Tickets
        {

            /// <summary>
            /// 出票完成
            /// </summary>
            public static class Completed
            {
                /// <summary>
                /// 出票成功
                /// </summary>
                public const string Success = "Tickets.Completed.Success";

                /// <summary>
                /// 出票失败
                /// </summary>
                public const string Failure = "Tickets.Completed.Failure";
            }
        }

        /// <summary>
        /// 返奖队列
        /// </summary>
        public static class Awards
        {
            /// <summary>
            /// 接受返奖
            /// </summary>
            public static class Accepted
            {
                public const string Success = "Awards.Accepted.Success";
            }

            /// <summary>
            /// 返奖完成
            /// </summary>
            public static class Completed
            {
                /// <summary>
                /// 中奖
                /// </summary>
                public const string Winning = "Awards.Completed.Winning";

                /// <summary>
                /// 未中奖
                /// </summary>
                public const string Loseing = "Awards.Completed.Loseing";
            }
        }
    }
}
