using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLab.Model;

namespace TestLab.Class
{
    class DataBaseContext
    {
        public static TestLabEntities DbContext = new TestLabEntities();
    }
}
