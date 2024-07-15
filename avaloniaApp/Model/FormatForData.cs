using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avaloniaApp.Model
{
    public class FormatForData<T>
    {
        public bool Status { set; get; }
        public string? Error { set; get; }
        public int Total { set; get; }
        public List<T> Records { set; get; } = new List<T>();
    }
}
