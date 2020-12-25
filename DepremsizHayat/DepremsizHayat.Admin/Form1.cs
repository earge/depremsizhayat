using DepremsizHayat.Business;
using DepremsizHayat.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DepremsizHayat.Admin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var userManager = new UserAccountManager();

            //1
            var model = new UserAccountListModel();
            userManager.GetList(model);

            //bu alanda model dolu gelir. 
            // gelen liste grid de dolacak
            // rol ve aktif pasif filtresi
            // düzenleme ekranı

            int i = model.Items.Count;
        }
    }
}
