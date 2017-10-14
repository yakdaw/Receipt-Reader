using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CannotBeEmptyAttribute : ValidationAttribute
{
    private const string defaultError = "{0} must have at least one element.";
    public CannotBeEmptyAttribute() : base(defaultError)
    {
    }

    public override bool IsValid(object value)
    {
        IList list = value as IList;

        if (list != null)
        {
            return list.Count > 0;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(this.ErrorMessageString, name);
    }
}