using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.Domain.ValueObjects
{
    public class HashedPassword
    {
        public string Value { get; set; }
        public HashedPassword(string _value)
        {
            var regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";  // add password format

            if (!Regex.IsMatch(_value, regex))
                throw new Exception("Password format is invalid.");

            Value =  _value;
        }
    }
}
