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
        void OnSuccess(ZResponse<R> response);
        void OnError(ZResponse<R> errorMessage);
        void OnFailure(ZResponse<R> response);
    }
    public interface IResponseType 
    {
       string Response { get; set; }
    }

    public class ZResponse<R> : IResponseType
    {
       public string Response { get ; set ; }

        public R Data;
    }


    //public interface IPresenterCallback<R>
    //{
    //    void OnSuccess(ZResponse<R> response);
    //    void OnError(ZResponse<R> response);
    //    void OnFailure(ZResponse<R> response);
    //}
}
