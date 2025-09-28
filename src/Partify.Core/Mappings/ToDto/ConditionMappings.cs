using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.Condition;

namespace CSOS.Core.Mappings.ToDto
{
    public static class ConditionMappings
    {
        public static ConditionResponse ToConditionResponse(this Condition condition)
        {
            return new ConditionResponse(condition.Id,
                condition.ConditionTitle,
                condition.ConditionDescription
                );
        }
    }
}
