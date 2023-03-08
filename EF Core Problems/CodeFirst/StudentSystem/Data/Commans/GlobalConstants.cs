using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Commans
{
    public static class GlobalConstants
    {
        public const int Course_Name_Max_Length = 80;

        public const int Student_Name_Max_Length = 100;

        public const int Resource_Name_Max_Length = 50;

        public const string Connection_String = @"Server=.\SQLEXPRESS;Database=SoftUni;Integrated Security=True;Encrypt=False;";
    }
}
