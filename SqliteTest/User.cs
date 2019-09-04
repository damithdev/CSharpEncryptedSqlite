using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    class User
    {



        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Details
        {
            get
            {
                return String.Format("{0} was crated on {1} .", this.Name, this.TimeStamp.ToLongDateString());
            }
        }
    }

    
}
