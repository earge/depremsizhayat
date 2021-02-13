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
        public int DataCount { get; set; }
        public int DataPerPage { get; set; }
    }
}
