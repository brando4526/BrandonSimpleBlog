using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    public static class AdminNavPages
    {
        public static string Index => "Index";

        public static string AccountInfo => "AccountInfo";

        public static string ManageUsers => "ManageUsers";

        public static string ManageBlogPosts => "ManageBlogPosts";

        public static string ManageCategories => "ManageCategories";

        public static string ManageBlogSettings => "ManageBlogSettings";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string AccountInfoNavClass(ViewContext viewContext) => PageNavClass(viewContext, AccountInfo);

        public static string ManageUsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageUsers);

        public static string ManageBlogPostsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageBlogPosts);

        public static string ManageCategoriesNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageCategories);

        public static string ManageBlogSettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageBlogSettings);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
