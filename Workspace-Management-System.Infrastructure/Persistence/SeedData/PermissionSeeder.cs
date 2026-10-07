using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Infrastructure.Persistence.SeedData;

public static class RolePermissionSeeder
{
    private const string PermissionClaimType = "permission";

    public static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager)
    {
        var rolePermissions = new Dictionary<string, string[]>
        {
            [Roles.Owner] =
            [
                Permissions.CompaniesView,
                Permissions.CompaniesViewDetails,
                Permissions.CompaniesCreate,
                Permissions.CompaniesUpdate,
                Permissions.CompaniesDelete,
                Permissions.CompaniesRestore,
                Permissions.CompaniesSearch,
                Permissions.CompaniesViewCustomers,
                Permissions.CompaniesChangeStatus,

                Permissions.CustomersView,
                Permissions.CustomersViewDetails,
                Permissions.CustomersCreate,
                Permissions.CustomersUpdate,
                Permissions.CustomersDelete,
                Permissions.CustomersRestore,
                Permissions.CustomersSearch,
                Permissions.CustomersViewTypes,

                Permissions.DiscountsView,
                Permissions.DiscountsViewDetails,
                Permissions.DiscountsCreate,
                Permissions.DiscountsUpdate,
                Permissions.DiscountsDelete,
                Permissions.DiscountsRestore,

                Permissions.PackagesView,
                Permissions.PackagesViewDetails,
                Permissions.PackagesCreate,
                Permissions.PackagesUpdate,
                Permissions.PackagesDelete,
                Permissions.PackagesRestore,
                Permissions.PackagesActivate,
                Permissions.PackagesDeactivate,
                Permissions.PackagesAssignCustomer,
                Permissions.PackagesRemoveCustomer,
                Permissions.PackagesUpgradeCustomer,
                Permissions.PackagesViewCustomers,

                Permissions.PricingPlansView,
                Permissions.PricingPlansViewDetails,
                Permissions.PricingPlansCreate,
                Permissions.PricingPlansUpdate,
                Permissions.PricingPlansDelete,
                Permissions.PricingPlansRestore,
                Permissions.PricingPlansViewRules,

                Permissions.PricingRulesView,
                Permissions.PricingRulesViewDetails,
                Permissions.PricingRulesCreate,
                Permissions.PricingRulesUpdate,
                Permissions.PricingRulesDelete,
                Permissions.PricingRulesRestore,

                Permissions.ProductCategoriesView,
                Permissions.ProductCategoriesViewDetails,
                Permissions.ProductCategoriesCreate,
                Permissions.ProductCategoriesUpdate,
                Permissions.ProductCategoriesDelete,
                Permissions.ProductCategoriesRestore,

                Permissions.ProductsView,
                Permissions.ProductsViewDetails,
                Permissions.ProductsCreate,
                Permissions.ProductsUpdate,
                Permissions.ProductsDelete,
                Permissions.ProductsRestore,

                Permissions.ServicesView,
                Permissions.ServicesViewDetails,
                Permissions.ServicesCreate,
                Permissions.ServicesUpdate,
                Permissions.ServicesDelete,
                Permissions.ServicesRestore,

                Permissions.SessionProductsView,
                Permissions.SessionProductsAdd,
                Permissions.SessionProductsUpdate,
                Permissions.SessionProductsRemove,
                Permissions.SessionProductsClear,

                Permissions.SessionServicesView,
                Permissions.SessionServicesViewDetails,
                Permissions.SessionServicesAdd,
                Permissions.SessionServicesUpdate,
                Permissions.SessionServicesRemove,
                Permissions.SessionServicesClear,

                Permissions.CheckoutSession
            ],

            [Roles.Admin] =
            [
                Permissions.CompaniesView,
                Permissions.CompaniesViewDetails,
                Permissions.CompaniesCreate,
                Permissions.CompaniesUpdate,
                Permissions.CompaniesDelete,
                Permissions.CompaniesRestore,
                Permissions.CompaniesSearch,
                Permissions.CompaniesViewCustomers,
                Permissions.CompaniesChangeStatus,

                Permissions.CustomersView,
                Permissions.CustomersViewDetails,
                Permissions.CustomersCreate,
                Permissions.CustomersUpdate,
                Permissions.CustomersDelete,
                Permissions.CustomersRestore,
                Permissions.CustomersSearch,
                Permissions.CustomersViewTypes,

                Permissions.DiscountsView,
                Permissions.DiscountsViewDetails,
                Permissions.DiscountsCreate,
                Permissions.DiscountsUpdate,
                Permissions.DiscountsDelete,
                Permissions.DiscountsRestore,

                Permissions.PackagesView,
                Permissions.PackagesViewDetails,
                Permissions.PackagesCreate,
                Permissions.PackagesUpdate,
                Permissions.PackagesDelete,
                Permissions.PackagesRestore,
                Permissions.PackagesActivate,
                Permissions.PackagesDeactivate,
                Permissions.PackagesAssignCustomer,
                Permissions.PackagesRemoveCustomer,
                Permissions.PackagesUpgradeCustomer,
                Permissions.PackagesViewCustomers,

                Permissions.PricingPlansView,
                Permissions.PricingPlansViewDetails,
                Permissions.PricingPlansCreate,
                Permissions.PricingPlansUpdate,
                Permissions.PricingPlansDelete,
                Permissions.PricingPlansRestore,
                Permissions.PricingPlansViewRules,

                Permissions.PricingRulesView,
                Permissions.PricingRulesViewDetails,
                Permissions.PricingRulesCreate,
                Permissions.PricingRulesUpdate,
                Permissions.PricingRulesDelete,
                Permissions.PricingRulesRestore,

                Permissions.ProductCategoriesView,
                Permissions.ProductCategoriesViewDetails,
                Permissions.ProductCategoriesCreate,
                Permissions.ProductCategoriesUpdate,
                Permissions.ProductCategoriesDelete,
                Permissions.ProductCategoriesRestore,

                Permissions.ProductsView,
                Permissions.ProductsViewDetails,
                Permissions.ProductsCreate,
                Permissions.ProductsUpdate,
                Permissions.ProductsDelete,
                Permissions.ProductsRestore,

                Permissions.ServicesView,
                Permissions.ServicesViewDetails,
                Permissions.ServicesCreate,
                Permissions.ServicesUpdate,
                Permissions.ServicesDelete,
                Permissions.ServicesRestore,

                Permissions.SessionProductsView,
                Permissions.SessionProductsAdd,
                Permissions.SessionProductsUpdate,
                Permissions.SessionProductsRemove,
                Permissions.SessionProductsClear,

                Permissions.SessionServicesView,
                Permissions.SessionServicesViewDetails,
                Permissions.SessionServicesAdd,
                Permissions.SessionServicesUpdate,
                Permissions.SessionServicesRemove,
                Permissions.SessionServicesClear,

                Permissions.CheckoutSession
            ],

            [Roles.Manager] =
            [
                Permissions.CompaniesView,
                Permissions.CompaniesViewDetails,
                Permissions.CompaniesSearch,
                Permissions.CompaniesViewCustomers,

                Permissions.CustomersView,
                Permissions.CustomersViewDetails,
                Permissions.CustomersCreate,
                Permissions.CustomersUpdate,
                Permissions.CustomersSearch,
                Permissions.CustomersViewTypes,

                Permissions.DiscountsView,
                Permissions.DiscountsViewDetails,

                Permissions.PackagesView,
                Permissions.PackagesViewDetails,
                Permissions.PackagesCreate,
                Permissions.PackagesUpdate,
                Permissions.PackagesActivate,
                Permissions.PackagesDeactivate,
                Permissions.PackagesAssignCustomer,
                Permissions.PackagesRemoveCustomer,
                Permissions.PackagesUpgradeCustomer,
                Permissions.PackagesViewCustomers,

                Permissions.PricingPlansView,
                Permissions.PricingPlansViewDetails,
                Permissions.PricingPlansViewRules,

                Permissions.PricingRulesView,
                Permissions.PricingRulesViewDetails,

                Permissions.ProductCategoriesView,
                Permissions.ProductCategoriesViewDetails,

                Permissions.ProductsView,
                Permissions.ProductsViewDetails,

                Permissions.ServicesView,
                Permissions.ServicesViewDetails,

                Permissions.SessionProductsView,
                Permissions.SessionProductsAdd,
                Permissions.SessionProductsUpdate,
                Permissions.SessionProductsRemove,
                Permissions.SessionProductsClear,

                Permissions.SessionServicesView,
                Permissions.SessionServicesViewDetails,
                Permissions.SessionServicesAdd,
                Permissions.SessionServicesUpdate,
                Permissions.SessionServicesRemove,
                Permissions.SessionServicesClear,

                Permissions.CheckoutSession
            ],

            [Roles.Receptionist] =
            [
                Permissions.CustomersView,
                Permissions.CustomersViewDetails,
                Permissions.CustomersCreate,
                Permissions.CustomersUpdate,
                Permissions.CustomersSearch,
                Permissions.CustomersViewTypes,

                Permissions.PackagesView,
                Permissions.PackagesViewDetails,
                Permissions.PackagesAssignCustomer,
                Permissions.PackagesRemoveCustomer,
                Permissions.PackagesViewCustomers,

                Permissions.ProductsView,
                Permissions.ProductsViewDetails,

                Permissions.ServicesView,
                Permissions.ServicesViewDetails,

                Permissions.SessionProductsView,
                Permissions.SessionProductsAdd,
                Permissions.SessionProductsUpdate,
                Permissions.SessionProductsRemove,
                Permissions.SessionProductsClear,

                Permissions.SessionServicesView,
                Permissions.SessionServicesViewDetails,
                Permissions.SessionServicesAdd,
                Permissions.SessionServicesUpdate,
                Permissions.SessionServicesRemove,
                Permissions.SessionServicesClear,

                Permissions.CheckoutSession
            ],

            [Roles.Cashier] =
            [
                Permissions.CustomersView,
                Permissions.CustomersViewDetails,
                Permissions.CustomersSearch,

                Permissions.ProductsView,
                Permissions.ProductsViewDetails,

                Permissions.ServicesView,
                Permissions.ServicesViewDetails,

                Permissions.SessionProductsView,
                Permissions.SessionProductsAdd,
                Permissions.SessionProductsUpdate,
                Permissions.SessionProductsRemove,
                Permissions.SessionProductsClear,

                Permissions.SessionServicesView,
                Permissions.SessionServicesViewDetails,
                Permissions.SessionServicesAdd,
                Permissions.SessionServicesUpdate,
                Permissions.SessionServicesRemove,
                Permissions.SessionServicesClear,

                Permissions.CheckoutSession
            ]
        };

        foreach (var rolePermission in rolePermissions)
        {
            var role = await roleManager.FindByNameAsync(rolePermission.Key);

            if (role is null)
                continue;

            var existingClaims = await roleManager.GetClaimsAsync(role);

            foreach (var permission in rolePermission.Value)
            {
                var exists = existingClaims.Any(c =>
                    c.Type == PermissionClaimType &&
                    c.Value == permission);

                if (exists)
                    continue;

                await roleManager.AddClaimAsync(
                    role,
                    new Claim(PermissionClaimType, permission));
            }
        }
    }
}
