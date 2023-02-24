using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain
{
    public interface IRequest
    {
        string UserId { get; set; }
        CancellationTokenSource CtsSource { get; set; }
    }

    public interface IResponseCallbackBaseCase<R>
    {
        //contain functions to call presenter callback
         void OnSuccessAsync(ZResponse<R> response);
        void OnError(BException errorMessage);
        void OnFailure(ZResponse<R> response);
    }


    public interface IUsecaseCallbackBaseCase<R>
    {
        void OnResponseError(BException response);
        void OnResponseFailure(ZResponse<R> response);
       void OnResponseSuccess(ZResponse<R> response);
    }
    public interface IResponseType
    {
        string Response { get; set; }
    }

    public class ZResponse<R> : IResponseType
    {
        public string Response { get; set; }

        public R Data;
    }
}
