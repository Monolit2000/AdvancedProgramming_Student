using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class ResearchProject
    {
        public long ResearchProjectId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Student>? TeamMembers { get; set; } = null;
        public string Supervisor { get; set; } = string.Empty; 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Budget { get; set; }

        public ResearchProject()
        {
                
        }
    }
}
