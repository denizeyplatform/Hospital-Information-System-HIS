using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; set; }
        public Email(string _value)
        {
            var regex = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";  // add email format

            if (!Regex.IsMatch(_value, regex))
                throw new Exception("Email format is invalid.");

            Value = _value;
        }
    }
}
