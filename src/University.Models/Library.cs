using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class Library
    {
        public long LibraryId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int NumberOfFloors { get; set; }
        public int NumberOfRooms { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Book> Books { get; set; } = null;
        private List<string> Facilities { get; set; } = [];
        public string Librarian { get; set; } = string.Empty;

        public Library()
        {
                
        }
    }
}
