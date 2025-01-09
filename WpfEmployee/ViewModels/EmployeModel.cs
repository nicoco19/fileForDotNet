using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfEmployee.Models;

namespace WpfEmployee.ViewModels
{
    class EmployeModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        private Employee _employee;

        public EmployeModel(Employee employee)
        {
            _employee = employee;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Employee Employee { get { return _employee; } }
        public string FullName
        {
            get { return _employee.FirstName + " " + _employee.LastName; }
        }

        public DateTime? DisplayBirthDate 
        {
            get { return _employee.BirthDate; }
        }

        public string LastName { 
            get { return _employee.LastName; }
            set { _employee.LastName = value; }

        }

        public string FirstName
        { 
            get { return _employee.FirstName; }
            set { _employee.FirstName = value; }

        }

        public DateTime? BirthDate
        {
            get { return _employee.BirthDate; }
            set { _employee.BirthDate = value; }


        }
        public DateTime? HireDate 
        {
            get { return _employee.HireDate; }
            set { _employee.HireDate = value; }

        }

        public string TitleOfCourtesy
        {
            get { return _employee.TitleOfCourtesy; }
            set { _employee.TitleOfCourtesy = value; }
        }
    }
}
