using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.ResetPassword;

namespace Library.Domain.UseCase
{
    public interface IResetPasswordDataManager
    {
        void ResetPassword(ResetPasswordRequest request, ResetPasswordCallback response);//call back
    }

    public interface IPersenterResetPasswordCallback : IResponseCallbackBaseCase<bool> { }
    public class ResetPassword :UseCaseBase<bool>
    {
        private IResetPasswordDataManager _resetPasswordDataManager;
        private ResetPasswordRequest _resetPasswordRequest;
        IPersenterResetPasswordCallback _resetPasswordResponse;

        public ResetPassword(ResetPasswordRequest request, IPersenterResetPasswordCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _resetPasswordDataManager = serviceProviderInstance.Services.GetService< IResetPasswordDataManager>();
            _resetPasswordRequest = request;
            _resetPasswordResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._resetPasswordDataManager.ResetPassword(_resetPasswordRequest, new ResetPasswordCallback(this));
        }
     

        public class ResetPasswordCallback:ZResponse<bool>
        {
            private ResetPassword _resetPassword;
            public ResetPasswordCallback(ResetPassword resetPassword)
            {
                this._resetPassword = resetPassword;
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _resetPassword._resetPasswordResponse?.OnSuccessAsync(response);
            }
            public void OnResponseFailure(ZResponse<bool> response)
            {
                _resetPassword._resetPasswordResponse?.OnFailure(response);
            }
        }
       
    }
   
    public class ResetPasswordRequest : IRequest
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }

        public ResetPasswordRequest(string userId, string newPassword,CancellationTokenSource cts)
        {
            UserId = userId;
            NewPassword = newPassword;
            CtsSource = cts;    
        }

    }


}
