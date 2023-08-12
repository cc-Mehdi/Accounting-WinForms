using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.Utility.Context;
using Accounting.DataLayer;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;

namespace Accounting.App.Accounting
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomer());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }
            if (TypeID == 1)
                this.Text = "گزارش دریافتی ها";
            else
                this.Text = "گزارش پرداختی ها";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;
                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID && a.CustomerID == customerId));
                }
                else
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID));
                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    //روش های تبدیل تاریخ شمسی به میلادی
                    //روش اول
                    startDate = startDate.Value.ToMiladi();
                    result = result.Where(r => r.DateTime >= startDate).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    //روش های تبدیل تاریخ شمسی به میلادی
                    //روش دوم
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTime <= endDate).ToList();
                }







                dgReport.AutoGenerateColumns = false;
                dgReport.Rows.Clear();
                foreach (var acconting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(acconting.CustomerID);
                    dgReport.Rows.Add(acconting.ID, customerName, acconting.Amount, acconting.DateTime.ToShamsi(), acconting.Description);
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int Id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (RtlMessageBox.Show("آیا از حذف  مطمئن هستید ؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(Id);
                        db.Save();
                    }
                    Filter();
                }
                else
                    RtlMessageBox.Show("لطفا یک مورد را انتخاب کنید", "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int Id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frmNew = new frmNewAccounting();
                frmNew.AccountID = Id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                    Filter();
            }
            else
                RtlMessageBox.Show("لطفا یک مورد را انتخاب کنید", "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Customer");
            dtReport.Columns.Add("Amount");
            dtReport.Columns.Add("Date");
            dtReport.Columns.Add("Description");
            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtReport.Rows.Add(
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString(),
                    item.Cells[4].Value.ToString()
                    );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT", dtReport);
            stiPrint.Show();
        }
    }
}
