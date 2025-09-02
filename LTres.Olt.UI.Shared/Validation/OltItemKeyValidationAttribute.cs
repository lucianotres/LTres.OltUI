
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using LTres.Olt.UI.Shared.Models;

namespace LTres.Olt.UI.Shared.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
sealed public class OltItemKeyValidationAttribute : ValidationAttribute
{
    const string RegexValidSNMP = @"^(?:\.\d+)+$";
    const string RegexValidSNMPwithIndex = @"^(?:\.\d+)+\.{index}(?:\.\d+)*$";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not OLT_Host_Item itemObj)
            return null;

        if (value is not string valueString)
            return null;

        if (itemObj.Action == OLT_Host_ItemExtensions.ActionPing)
            return string.IsNullOrEmpty(valueString) ? ValidationResult.Success : new ValidationResult("When action is Ping, it should not have a key.", [validationContext.MemberName!]);
        else if (itemObj.Action == OLT_Host_ItemExtensions.ActionSnmpGet || itemObj.Action == OLT_Host_ItemExtensions.ActionSnmpWalk)
        {
            if (string.IsNullOrEmpty(valueString))
                return new ValidationResult("When action is SNMP, it should have a key.", [validationContext.MemberName!]);

            if (itemObj.Template.GetValueOrDefault())
                return Regex.IsMatch(valueString, RegexValidSNMPwithIndex, RegexOptions.None) ?
                        ValidationResult.Success :
                        new ValidationResult("The key value isn't a valid SNMP key or is missing {index}.", [validationContext.MemberName!]);
            else
                return Regex.IsMatch(valueString, RegexValidSNMP, RegexOptions.None) ?
                        ValidationResult.Success :
                        new ValidationResult("The key value isn't a valid SNMP key.", [validationContext.MemberName!]);
        }
        else
            return ValidationResult.Success;
    }
}
