using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmers.DataModel
{
    public class OrderResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int OrderId { get; set; }
    }

}
