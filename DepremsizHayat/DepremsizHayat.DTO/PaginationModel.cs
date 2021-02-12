using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO
{
    public class PaginationModel<T>
    {
        public List<T> DataList { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int DataPerPage { get; set; }
    }
}
