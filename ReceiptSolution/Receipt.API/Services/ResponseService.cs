namespace Receipt.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.ModelBinding;

    public class ResponseService
    {
        public string ModelStateErrorsToString(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();

            return string.Join(" | ", errorList);
        }

        public string IdentityResultErrorsToString(IEnumerable<string> errors)
        {
            return string.Join(" | ", errors);
        }
    }
}