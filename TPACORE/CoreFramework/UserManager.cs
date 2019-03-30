using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CryptoXML;
using System.Data;

namespace TPACORE.CoreFramework
{
    public class UserManager
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private const string phrase = "myKey123";
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        static string baseUserDataFile = baseOutputDirectory + "u.xml";

        private static DataTable ReadUserFile(){
            DataSet dsUsers = new DataSet("user");
            dsUsers.ReadXml(baseUserDataFile);
            DataTable dtUsers = dsUsers.Tables[0];
            return dtUsers;
        }

        public static List<User> GetUsers() {
            DataTable dtUsers = ReadUserFile();
            
            var result =  dtUsers.AsEnumerable()
         .Select(x=> new User{UserId = x["id"].ToString(),
         Username = x["username"].ToString(),
         Firstname =x["firstname"].ToString(),
         Lastname =x["lastname"].ToString(),
         Password=x["password"].ToString(),
         Email=x["email"].ToString(),
         ContactNo=x["contactNo"].ToString()})
         .ToList();
            return result;
        }

        public static string CreateUser(User user) {
            
            DataTable dtUser = ReadUserFile();
            if (dtUser.Rows.Count < 15) // purposed limit imposed for 15 users
            {
                DataRow dRowUser = dtUser.NewRow();
                dRowUser["id"] = user.UserId;
                dRowUser["username"] = user.Username;
                dRowUser["password"] = user.Password;
                dRowUser["firstname"] = user.Firstname;
                dRowUser["lastname"] = user.Lastname;
                dRowUser["email"] = user.Email;
                dRowUser["contactNo"] = user.ContactNo;
                dtUser.Rows.Add(dRowUser);
                dtUser.DataSet.WriteXml(baseUserDataFile);

                Directory.CreateDirectory(Path.Combine(baseOutputDirectory, user.UserId));
                //DataSet dsUsers = new DataSet("users");
                //dsUsers.Tables.Add(dtUser);
                //dsUsers.WriteXml(baseUserDataFile);
                return "created";
            }
            else return "limit exceeded";
        }

        public static User GetUserById(string userId) {
            DataTable dtUser = ReadUserFile();
            return dtUser.AsEnumerable()
            .Where(x => x["id"].ToString() == userId).Select(x => new User
            {
                UserId = x["id"].ToString(),
                Username = x["username"].ToString(),
                Firstname = x["firstname"].ToString(),
                Lastname = x["lastname"].ToString(),
                Password = x["password"].ToString(),
                Email = x["email"].ToString(),
                ContactNo = x["contactNo"].ToString()
            }).SingleOrDefault();
            
        }

        public static User GetUserByCredentials(string username, string password) {
            DataTable dtUser = ReadUserFile();
            return dtUser.AsEnumerable()
            .Where(x => x["username"].ToString() == username &&
                x["password"].ToString() == password).Select(x => new User
            {
                UserId = x["id"].ToString(),
                Username = x["username"].ToString(),
                Firstname = x["firstname"].ToString(),
                Lastname = x["lastname"].ToString(),
                Password = x["password"].ToString(),
                Email = x["email"].ToString(),
                ContactNo = x["contactNo"].ToString()
            }).SingleOrDefault();
        }

        public static void DeleteUser(string userId) {
            DataTable dtUser = ReadUserFile();
            var dRow = dtUser.Select(string.Format("id='{0}'",userId)).SingleOrDefault();
            if (dRow != null)
                dtUser.Rows.Remove(dRow);
            dtUser.DataSet.WriteXml(baseUserDataFile);
            Directory.Delete(Path.Combine(baseOutputDirectory, userId), true);
            //DataSet dsUsers = new DataSet("users");
            //dsUsers.Tables.Add(dtUser);
            //dsUsers.WriteXml(baseUserDataFile);
        }

        //private static DataTable GetUserSchema(out DataSet dsUser, out DataTable dtUser){
            
        //    DataColumn dcId = new DataColumn("uDcId", typeof(string));
        //    DataColumn dcUsername = new DataColumn("uDcUsername", typeof(string));
        //    DataColumn dcPassword = new DataColumn("uDcPassword", typeof(string));
        //    DataColumn dcFirstname = new DataColumn("uDcFirstname", typeof(string));
        //    DataColumn dcLastname = new DataColumn("uDcLastname", typeof(string));
        //    DataColumn dcEmail = new DataColumn("uDcEmail", typeof(string));
        //    DataColumn dcContactNo = new DataColumn("uDcContactNo", typeof(string));
        //    dtUser.Columns.Add(dcId);
        //    dtUser.Columns.Add(dcUsername);
        //    dtUser.Columns.Add(dcPassword);
        //    dtUser.Columns.Add(dcFirstname);
        //    dtUser.Columns.Add(dcLastname);
        //    dtUser.Columns.Add(dcEmail);
        //    dtUser.Columns.Add(dcContactNo);
        //    return dtUser;
        //}

        //encryption
        //create new user
        // get all users
        // authenticate user
        // get single user
        // update user
        //delete user
        //public static bool CreateUser(User user) {
        //    var xmlEncryptor = new XMLEncryptor(phrase, phrase);
        //    DataTable dtUser = new DataTable("dtUser");
        //    DataSet dsUser = new DataSet("dsUser");


        //    DataTable userSchema = GetUserSchema();
        //    DataRow drowUser = userSchema.NewRow();
        //    drowUser["uDcId"] = user.UserId;
        //    drowUser["uDcUsername"] = user.Username;
        //    drowUser["uDcFirstname"] = user.Firstname;
        //    drowUser["uDcLastname"] = user.Lastname;
        //    drowUser["uDcPassword"] = user.Password;
        //    drowUser["uDcContactNo"] = user.ContactNo;
        //    drowUser["uDcEmail"] = user.Email;

        //    userSchema.Rows.Add(drowUser);
        //    dsUser.Tables.Add(dtUser);

        //    xmlEncryptor.WriteEncryptedXML(dsUser, evalOutputFilepath);
        //}

    }
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
    }
}
