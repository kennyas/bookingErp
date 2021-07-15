using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Tornado.Shared.ViewModels
{
    public abstract class BaseViewModel<T>
    {
        public BaseViewModel()
        {
            ErrorList = new List<string>();
        }

        public T Id { get; set; }
        public virtual Boolean HasError
        {
            get
            {
                if (this.ErrorList.Any())
                    return true;

                return false;
            }
        }

        public virtual List<string> ErrorList { get; set; }

        [JsonIgnore]
        public Guid CreatedId { get; set; }
        [JsonIgnore]
        public int TotalCount { get; set; }
    }

    public abstract class BaseViewModel : BaseViewModel<string>, IValidatableObject
    {


        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CreatedId == Guid.Empty)
            {
                yield return null;// new ValidationResult("User editing record couldn't be determined");
            }
        }
    }
}
