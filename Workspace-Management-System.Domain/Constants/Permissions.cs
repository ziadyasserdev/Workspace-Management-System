using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Constants
{
    public static class Permissions
    {
        public const string CustomersView = "customers.view";
        public const string CustomersCreate = "customers.create";
        public const string CustomersUpdate = "customers.update";
        public const string CustomersDelete = "customers.delete";

        public const string SessionsView = "sessions.view";
        public const string SessionsCreate = "sessions.create";
        public const string SessionsUpdate = "sessions.update";
        public const string SessionsCheckout = "sessions.checkout";
    }
}
