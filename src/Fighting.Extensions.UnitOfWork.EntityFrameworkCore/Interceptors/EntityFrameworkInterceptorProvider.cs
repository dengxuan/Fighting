using Fighting.Aspects.Interceptors;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Interceptors
{
    public class EntityFrameworkInterceptorProvider : InterceptorProvider
    {
        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<EntityFrameowkrInterceptor>(Order);
        }
    }
}
