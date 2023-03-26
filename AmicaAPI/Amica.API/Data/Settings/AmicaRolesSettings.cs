namespace Amica.API.WebServer.Data.Settings {
    public static class AmicaRolesSettings {
        public const string User = "User";
        public const string Moderator = "Moderator";
        public const string Admin = "Admin";
        public const string Root = "Root";

        private static string[] reg = new[] {
            User
        };

        private static string[] adm = new[] {
            User, Admin
        };

        public static List<string> AllRoles = new List<string>() {
            User, Moderator, Admin, Root,
        };

        public static string[] RegUser = reg;
        public static string[] AdminArr = adm;

    }
}
