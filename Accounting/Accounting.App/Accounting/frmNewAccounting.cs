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
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmNewAccounting : Form
    {
        UnitOfWork db;
        public int AccountID = 0;
        public frmNewAccounting()
        {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomer();
            if (AccountID != 0)
            {
                this.Text = "ویرایش تراکنش";
                btnSave.Text = "ویرایش";
                var account = db.AccountingRepository.GetById(AccountID);
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                txtAmount.Value = account.Amount;
                txtDiscription.Text = account.Description;
                db.Dispose();
                if (account.TypeID == 1)
                    rbRecive.Checked = true;
                else
                    rbPay.Checked = true;

            }

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomer(txtFilter.Text);
            db.Dispose();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbPay.Checked || rbRecive.Checked)
                {
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = (int)txtAmount.Value,
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        TypeID = (rbRecive.Checked) ? 1 : 2,
                        DateTime = DateTime.Now,
                        Description = txtDiscription.Text
                    };
                    if (AccountID == 0)
                        db.AccountingRepository.Insert(accounting);
                    else
                    {
                        accounting.ID = AccountID;
                        db.AccountingRepository.Update(accounting);
                    }
                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                    RtlMessageBox.Show("لطفا نوع تراکنش را انتخاب کنید");
            }
        }
    }
}
