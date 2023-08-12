using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Accounting.DataLayer;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;

namespace Accounting.Utility.Context
{
    public class UnitOfWork : IDisposable
    {
        Accounting_DBEntities db = new Accounting_DBEntities();

        private ICustomerRepository _customerRepository;

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new CustomerRepository(db);
                return _customerRepository;
            }
        }

        private GenericRepository<Accounting.DataLayer.Accounting> _accountingRepository;

        public GenericRepository<Accounting.DataLayer.Accounting> AccountingRepository {
            get
            {
                if (_accountingRepository == null)
                    _accountingRepository = new GenericRepository<Accounting.DataLayer.Accounting>(db);
                return _accountingRepository;
            }
        }

        private GenericRepository<Login> _loginRepository;

        public GenericRepository<Login> LoginRepository
        {
            get
            {
                if (_loginRepository == null)
                    _loginRepository = new GenericRepository<Login>(db);
                return _loginRepository;
            }
        }


        public void Dispose()
        {
            db.Dispose();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
