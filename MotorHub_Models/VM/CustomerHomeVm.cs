using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotorHub_Models.VM
{
    public class CustomerHomeVm
    {
        [ValidateNever]
        public IEnumerable<Car> CarList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
    }
}
