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
        public virtual DbSet<ANALYSE_REQUEST> ANALYSE_REQUEST { get; set; }
        public virtual DbSet<ANALYSE_REQUEST_ANSWER> ANALYSE_REQUEST_ANSWER { get; set; }
        public virtual DbSet<E_MAIL_ACCOUNT> E_MAIL_ACCOUNT { get; set; }
        public virtual DbSet<FILE> FILE { get; set; }
        public virtual DbSet<FILE_TYPE> FILE_TYPE { get; set; }
        public virtual DbSet<MESSAGE_QUERY> MESSAGE_QUERY { get; set; }
        public virtual DbSet<MESSAGE_TEMPLATE> MESSAGE_TEMPLATE { get; set; }
        public virtual DbSet<ROLE> ROLE { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<USER_ACCOUNT> USER_ACCOUNT { get; set; }
    }
}
