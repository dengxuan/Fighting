using Fighting.Aspects.Interceptors;

namespace Fighting.Aspects.DynamicProxy
{
    internal class DynamicProxyInterceptor : IInterceptor
    {
        private InterceptorDelegate _interceptor;

        public DynamicProxyInterceptor(InterceptorDelegate inteceptor)
        {
            _interceptor = inteceptor;
        }
        public void Intercept(IInvocation invocation)
        {
            InterceptDelegate next =  context => (context).ProceedAsync();
            var intercepterDelegate = _interceptor(next);
            _interceptor(next)(new InvocationContext(invocation)).Wait();
        }
    }
}
