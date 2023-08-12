using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.App.Accounting;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Accounting;
using Accounting.Business;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers frm = new App.frmCustomers();
            frm.ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e)
        {
            frmNewAccounting frmNewAccounting = new App.frmNewAccounting();
            if (frmNewAccounting.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void btnReporetPay_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new Accounting.frmReport();
            frmReport.TypeID = 2;
            if (frmReport.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void btnReportRecive_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new Accounting.frmReport();
            frmReport.TypeID = 1;
            if (frmReport.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frmLogin = new App.frmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                lblDate.Text = DateTime.Now.ToShamsi();
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();
            }
            else
                Application.Exit();
        }
        void Report()
        {
            ReportViewModel report = new ReportViewModel();
            report = Account.ReportFormMain();
            lblRecive.Text = report.Recive.ToString("#,0");
            lblPay.Text = report.Pay.ToString("#,0");
            lblAccountBalance.Text = report.AccountBalance.ToString("#,0");
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnEditLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new App.frmLogin();
            frmLogin.IsEdit = true;
            frmLogin.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Report();
        }
    }
}
