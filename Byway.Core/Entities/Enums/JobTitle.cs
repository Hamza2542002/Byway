using System.Runtime.Serialization;

namespace Byway.Core.Entities.Enums;

public enum JobTitle
{
    [EnumMember(Value = "Back-end Developer")]
    BackendDeveloper = 1,
    [EnumMember(Value = "Front-end Developer")]
    FrontendDeveloper = 2,
    [EnumMember(Value = "Full-Stack Developer")]
    FullStackDeveloper = 3,
    [EnumMember(Value = "Flutter Developer")]
    FlutterDeveloper = 4,
    [EnumMember(Value = "Mobile Developer")]
    MobileDeveloper = 5,
    [EnumMember(Value = "DevOps Engineer")]
    DevOpsEngineer = 6,
    [EnumMember(Value = "Software Engineer")]
    SoftwareEngineer = 7,
    [EnumMember(Value = "Data Scientist")]
    DataScientist = 8,
    [EnumMember(Value = "Data Engineer")]
    DataEngineer = 9,
    [EnumMember(Value = "Machine Learning Engineer")]
    MachineLearningEngineer = 10,
    [EnumMember(Value = "AI Engineer")]
    AIEngineer = 11,
    [EnumMember(Value = "Cloud Engineer")]
    CloudEngineer = 12,
}