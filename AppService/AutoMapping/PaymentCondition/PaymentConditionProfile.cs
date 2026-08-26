using AppService.AutoMapping.Base;
using Crosscutting.DTO.PaymentCondition;
using Domain.Entity;

namespace AppService.AutoMapping.PaymentCondition
{
    public class PaymentConditionProfile : BaseProfile
    {
        public PaymentConditionProfile()
        {
            CreateMap<CreatePaymentConditionRequestDTO, PaymentConditionEntity>();
            CreateMap<PaymentConditionEntity, PaymentConditionResponseDTO>();
        }
    }
}
