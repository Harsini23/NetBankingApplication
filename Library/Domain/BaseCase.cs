﻿using System;
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
        void OnSuccess(ZResponse<R> response);
        void OnError(String errorMessage);
        void OnFailure(ZResponse<R> response);
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
