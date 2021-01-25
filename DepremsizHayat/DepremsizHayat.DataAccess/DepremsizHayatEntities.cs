using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class DepremsizHayatEntities:DbContext
    {
        public DepremsizHayatEntities()
            : base("name=cs")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public virtual DbSet<ANALYSIS_IMAGE> ANALYSIS_IMAGE { get; set; }
        public virtual DbSet<ANALYSE_REQUEST> ANALYSIS_REQUEST { get; set; }
        public virtual DbSet<ANALYSE_REQUEST_ANSWER> ANALYSIS_REQUEST_ANSWER { get; set; }
        public virtual DbSet<ROLE> ROLE { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<USER_ACCOUNT> USER { get; set; }
    }
}
