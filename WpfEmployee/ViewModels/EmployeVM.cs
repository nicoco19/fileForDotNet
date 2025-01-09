using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1.ViewModels;
using WpfEmployee.Models;

namespace WpfEmployee.ViewModels
{
    class EmployeVM : INotifyPropertyChanged
    {
        private NorthwindContext dc = new NorthwindContext();

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<EmployeModel> _employeesList;
        private EmployeModel _selectedEmployee;
        private List<string> _listTitle;

        private DelegateCommand _addCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _deleteCommand;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       public ObservableCollection<EmployeModel> EmployeesList 
        {
            get 
            {
                if (_employeesList == null) 
                {
                    _employeesList = LoadEmployees();
                }
                return _employeesList;
            }
        }

        private ObservableCollection<EmployeModel> LoadEmployees()
        {
            ObservableCollection<EmployeModel> localCollection = new ObservableCollection<EmployeModel>();
            foreach (var item in dc.Employees)
            {
                localCollection.Add(new EmployeModel(item));
            }
            return localCollection;
        }

        public EmployeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    OnPropertyChanged(nameof(SelectedEmployee));
                }
            }
        }

        private List<string> LoadTitleOfCourtesy()
        {
            return dc.Employees.Select(e => e.TitleOfCourtesy).Distinct().ToList();
        }

        public List<string> ListTitle
        {
            get { return _listTitle = _listTitle ?? LoadTitleOfCourtesy(); }

        }

        private void SaveEmployee()
        {
            Employee verif = dc.Employees.Where(e => e.EmployeeId == SelectedEmployee.Employee.EmployeeId).SingleOrDefault();
            if (verif == null)
            {
                dc.Employees.Add(SelectedEmployee.Employee);
                dc.SaveChanges();
                MessageBox.Show("Enregistrement en base de données fait");
            }

            MessageBox.Show("L'employé existe déjà");
        }

        public DelegateCommand SaveCommand
        {
            get { return _saveCommand = _saveCommand ?? new DelegateCommand(SaveEmployee); }
        }

        public DelegateCommand AddCommand
        {
            get
            {
                return _addCommand = _addCommand ?? new DelegateCommand(AddEmployee);
            }

        }

        public DelegateCommand DeleteCommand
        {
            get
            {
                return _deleteCommand = _deleteCommand ?? new DelegateCommand(DeleteEmployee);
            }

        }

        private void AddEmployee()
        {
            Employee EGlobal = new Employee();
            EmployeModel EModel = new EmployeModel(EGlobal);
            EmployeesList.Add(EModel);
            SelectedEmployee = EModel;
        }

        private void DeleteEmployee()
        {
            Employee verif = dc.Employees.Where(e => e.EmployeeId == SelectedEmployee.Employee.EmployeeId).SingleOrDefault();
            if (verif != null)
            {
                dc.Employees.Remove(SelectedEmployee.Employee);
                dc.SaveChanges();
                EmployeesList.Remove(SelectedEmployee);
                MessageBox.Show("supprésion en base de données fait");
            }

            MessageBox.Show("L'employé n'existe pas ");
        }

       
    }
}
