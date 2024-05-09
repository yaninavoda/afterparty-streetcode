﻿using Streetcode.BLL.Entities.Payment;

namespace Streetcode.BLL.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<InvoiceInfo> CreateInvoiceAsync(Invoice invoice);
    }
}
