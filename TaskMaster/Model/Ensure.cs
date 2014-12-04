using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Model
{
    public static class Ensure
    {
        public static void IsNotNull(string argDescription, object arg)
        {
            if (arg != null)
                return;

            throw new ArgumentNullException("ENSURE: the argument " + argDescription + " cannot be null");
        }
    }
}
