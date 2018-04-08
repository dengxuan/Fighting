using Fighting.Aspects.DynamicProxy;
using Fighting.Aspects.Interceptors;
using Fighting.Extensions.Threading;
using Fighting.Extensions.UnitOfWork.Abstractions;
using System.Threading.Tasks;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Interceptors
{
    public class EntityFrameowkrInterceptor
    {
        private readonly InterceptDelegate _next;
        public EntityFrameowkrInterceptor(InterceptDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(InvocationContext context, IUnitOfWorkManager unitOfWorkManager, IUnitOfWorkDefaultOptions unitOfWorkOptions)
        {
            using (var uow = unitOfWorkManager.Begin())
            {
                await _next(context);
                await uow.CompleteAsync();
            }
        }

        private void PerformUow(IInvocation invocation, IUnitOfWorkManager unitOfWorkManager, UnitOfWorkOptions options)
        {
            if (TaskHelper.IsAsyncMethod(invocation.Method))
            {
                PerformAsyncUow(invocation, unitOfWorkManager, options);
            }
            else
            {
                PerformSyncUow(invocation, unitOfWorkManager, options);
            }
        }

        private void PerformSyncUow(IInvocation invocation, IUnitOfWorkManager unitOfWorkManager, UnitOfWorkOptions options)
        {
            using (var uow = unitOfWorkManager.Begin(options))
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        private void PerformAsyncUow(IInvocation invocation, IUnitOfWorkManager unitOfWorkManager, UnitOfWorkOptions options)
        {
            var uow = unitOfWorkManager.Begin(options);

            try
            {
                invocation.Proceed();
            }
            catch
            {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = TaskHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = TaskHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
        }
    }
}
