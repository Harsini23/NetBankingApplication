using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class UseCaseBase
    {
        public CancellationTokenSource Source { get; }

        private readonly CancellationToken _token;

        public UseCaseBase()
        {
            Source = new CancellationTokenSource();
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
