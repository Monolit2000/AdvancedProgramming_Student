using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class AthleticsFacility
    {
        public long AthleticsFacilityId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        private List<string> Availability { get; set; }
        private List<string> Equipment { get; set; }
        public int Capacity { get; set; }

        public virtual ICollection<Student>? Students { get; set; } = null;
    }
}
