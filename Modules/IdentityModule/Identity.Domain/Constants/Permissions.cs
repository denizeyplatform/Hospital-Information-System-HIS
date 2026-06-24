using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Constants
{
    public static class Permissions
    {
        public static class Users
        {
            public const string Create = "User.Create";
            public const string Update = "User.Update";
            public const string Delete = "User.Delete";
            public const string View = "User.View";
        }

        public static class Roles
        {
            public const string Create = "Role.Create";
            public const string Update = "Role.Update";
            public const string Delete = "Role.Delete";
            public const string Assign = "Role.Assign";
        }

        public static class Courses
        {
            public const string Create = "Course.Create";
            public const string Update = "Course.Update";
            public const string Delete = "Course.Delete";
        }
    }
}
