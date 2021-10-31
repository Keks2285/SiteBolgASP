using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class FilterViewModel
    {
        public int? SelectID { get; set; }
        public string SelectEmail { get; private set; }
        public FilterViewModel(int? id, string email)
        {
            SelectEmail = email;
            SelectID = id;
       
        }
    }
}
