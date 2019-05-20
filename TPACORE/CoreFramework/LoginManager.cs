using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPA.CoreFramework;

namespace TPACORE.CoreFramework
{
    public class LoginManager
    {
        public static bool CheckIfTeacherLoggedIn()
        {
            return TPACache.GetItem(TPACache.LOGIN_KEY) != null;
        }

        public static bool CheckIfStudentLoggedIn()
        {
            return TPACache.GetItem(TPACache.STUDENT_LOGIN_INFO) != null;
        }

        public static bool CheckIfAnyUserLoggedIn()
        {
            return TPACache.GetItem(TPACache.LOGIN_KEY) != null || TPACache.GetItem(TPACache.STUDENT_LOGIN_INFO) != null;
        }
        
        public static bool CheckIfStudentToEvaluateSet()
        {
            return TPACache.GetItem(TPACache.STUDENT_ID_TO_EVALUATE) != null;
        }
    }
}
