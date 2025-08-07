using System.ComponentModel;

public enum BloodGroup
{
    [Description("A+")]
    A_Positive,
    [Description("A-")]
    A_Negative,
    [Description("B+")]
    B_Positive,
    [Description("B-")]
    B_Negative,
    [Description("AB+")]
    AB_Positive,
    [Description("AB-")]
    AB_Negative,
    [Description("O+")]
    O_Positive,
    [Description("O-")]
    O_Negative

}


public static class EnumExt
{
    public static T GetEnumFromDesc<T>(string description)
    {

        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                {
                    return (T)field.GetValue(null)!;
                }
            }
            else
            {
                if (field.Name == description)
                {
                    return (T)field.GetValue(null)!;
                }
            }
        }

        throw new ArgumentException("Enum value not found", nameof(description));
    }
}