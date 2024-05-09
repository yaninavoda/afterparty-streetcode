using AutoMapper;
using Streetcode.BLL.Dto.Payment;
using Streetcode.BLL.Dto.Toponyms;
using Streetcode.BLL.Entities.Payment;
using Streetcode.BLL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Payment
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<InvoiceInfo, PaymentResponseDto>().ReverseMap();
        }
    }
}