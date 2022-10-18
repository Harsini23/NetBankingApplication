using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public interface IRequest
    {
         string UserId { get; set; }
    }

    public interface IResponseBaseCase<R>
    {
        //contain functions to call presenter callback
        void OnResponseSuccess(IResponseType<R> response);
        void OnResponseFailure(IResponseType<R> response);
        void OnResponseError(IResponseType<R> response);
    }
    public interface IResponseType<R> 
    {
       string Response { get; set; }
    }
}
