namespace OrleansServer
{
    internal static class DatabaseConfigPostgreSql
    {
        private static readonly string DbHost = "localhost";
        private static readonly string DbPort = "5432";

        private static readonly string DbUser = "postgres";
        private static readonly string DbPassword = "12590227227";

        private static readonly string DbName = "orleans-database";


        public static readonly string ConnectionUrl = 
            $"host={DbHost};" +
            $"port={DbPort};" +
            $"username={DbUser};" +
            $"password={DbPassword};" +
            $"database={DbName}";
    }
}
