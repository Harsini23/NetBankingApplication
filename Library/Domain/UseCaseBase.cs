using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain
{
    public abstract class UseCaseBase<T>
    {
      
        public CancellationTokenSource Source { get; }
        private readonly CancellationToken _token;
        private IResponseCallbackBaseCase<T> responseCallback;

        //public UseCaseBase()
        //{
        //    Source = new CancellationTokenSource();
        //    _token = Source.Token;

        //}
        public UseCaseBase() { }
        protected UseCaseBase(IResponseCallbackBaseCase<T> responseCallback,CancellationTokenSource CtsSource)
        {
            this.responseCallback = responseCallback;
            Source = CtsSource;
            _token = Source.Token;
        }

        public void Execute()
        {
            if (GetIfAvailableInCache())
            {
                return;
            }
            Task.Run(delegate ()
            {
                try
                {
                    Action();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Exception has been caught:");
                    Debug.WriteLine(ex.ToString());
                    responseCallback?.OnError((BException)ex);
                }
            }, _token);
        }

        public virtual void Action() { }
        public virtual bool GetIfAvailableInCache()
        {
            return false;
        }
    }
}
