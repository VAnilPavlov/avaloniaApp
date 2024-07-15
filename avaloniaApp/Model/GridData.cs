using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avaloniaApp.Model
{
    public class GridData
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusId Status { get; set; }
    }

    public enum StatusId
    {
        done = 0,
        sent = 1,
        closed = 2
    }
}
