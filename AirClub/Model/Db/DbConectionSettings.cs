using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    internal enum SqlServerType
    {
        MSSQLLocalDb, MSSQLEXPRESS
    }

    internal static class DbConectionSettings
    {
        public static string ConnectionStr
            => @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = pl; Integrated Security = True";

        //=> $@"Server={SqlServer}; Trusted_Connection=True; Database=DB_A5DB0D_AirClub; User ID=DB_A5DB0D_AirClub_admin;Password=68gfKG43 Connection Timeout = 15";

        //=> $@"Server={SqlServerTypeStr}; Database='{Environment.CurrentDirectory}\{DbFileName}';Trusted_Connection=True; Connection Timeout = 15";
        //
        //@"Data Source=sql5059.site4now.net;Persist Security Info=True;User ID=DB_A5DB0D_AirClub_admin;Password=68gfKG43";

        public static SqlServerType SqlServerType { get; private set; } = SqlServerType.MSSQLEXPRESS;

        public static string DbFileName { get; set; } = "AirClubDb.mdf";

        private static string SqlServer => "SQL5059.site4now.net";
        //=> SqlServerType == SqlServerType.MSSQLEXPRESS ? @".\SQLEXPRESS" : @"(LocalDB)\MSSQLLocalDB";

        //Data Source=SQL5059.site4now.net;Persist Security Info=True;User ID=DB_A5DB0D_AirClub_admin;Password=***********

        public static void ChangeServerType(SqlServerType type)
        {
            SqlServerType = type;
        }

    }
}
