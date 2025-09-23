using System.Runtime.Serialization;

namespace Byway.Core.Entities.Enums;

public enum CourseLevel
{
    [EnumMember(Value = "Beginner")]
    Beginner = 1,
    [EnumMember(Value = "Intermediate")]
    Intermediate = 2,
    [EnumMember(Value = "Advanced")]
    Advanced = 3
}
