using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class UseCaseBase
    {
        public UseCaseBase()
        {

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

                }
            });
        }

        public virtual void Action() { }

        public virtual bool GetIfAvailableInCache()
        {
            return false;
        }
    }
}
