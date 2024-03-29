﻿using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class EditPayeeDataManager : BankingDataManager, IEditPayeeDataManager
    {
        public EditPayeeDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void EditPayee(EditPayeeRequest request, EditPayee.EditPayeeCallback callback)
        {
            DbHandler.EditPayee(request.EditedPayee);
            ZResponse<String> response = new ZResponse<String>();
            response.Response = "Payee Edited successfully";
            response.Data = "Payee Edited";
            BankingNotification.BankingNotification.NotifyPayeeUpdated(request.EditedPayee);
            callback.OnResponseSuccess(response);
        }
    }
}
