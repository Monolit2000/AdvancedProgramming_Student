using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class StudentOrganization
    {
        public long StudentOrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Advisor { get; set; } = string.Empty;
        public string President { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MeetingSchedule { get; set; } = string.Empty;
        private List<string> Membership { get; set; } = [];
        public virtual ICollection<Student>? Students { get; set; } = null;

        public string Email { get; set; }
    }
}
