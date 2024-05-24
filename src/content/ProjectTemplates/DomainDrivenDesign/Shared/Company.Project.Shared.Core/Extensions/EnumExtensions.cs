using System.ComponentModel.DataAnnotations;

namespace System;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var type = enumValue.GetType();
        var fieldInfo = type.GetField(enumValue.ToString());

        if (fieldInfo is null)
        {
            return enumValue.ToString();
        }
        
        var displayAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            
        // Return the description, if it exists; otherwise, return the enum name
        return displayAttribute?.Description ?? enumValue.ToString();
    }
}
