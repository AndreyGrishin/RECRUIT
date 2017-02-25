using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RecruIT.Model.DBModel;

namespace RecruIT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

        public static ObservableCollection<Employees> Employeese => GetAllEmployees();
        private Employees _newEmployee;
        private ContactInfo _newContactInfo;
        private int _pivotPage;


        public bool IsCountEmployeeEmpty
        {
            get
            {
                return Employeese.Count == 0;
            }
            set
            {
                RaisePropertyChanged(() => IsCountEmployeeEmpty);
            }
        }

        public int PivotPage
        {
            get
            {
                return _pivotPage;
            }
            set
            {
                _pivotPage = value;
                RaisePropertyChanged(() => PivotPage);
            }
        }
        public Employees NewEmployee
        {
            get
            {
                if (_newEmployee == null)
                    _newEmployee = new Employees();
                return _newEmployee;
            }
            set
            {
                _newEmployee = value;
                RaisePropertyChanged(() => NewEmployee);
            }
        }

        public ContactInfo NewContactInfo
        {
            get
            {
                if (_newContactInfo == null)
                    _newContactInfo = new ContactInfo();
                return _newContactInfo;
            }
            set
            {

                _newContactInfo = value;
                RaisePropertyChanged(() => NewContactInfo);
            }
        }

        
        private RelayCommand _addNewEmployeeCommand;
        private RelayCommand _logOut;
        
        public ICommand AddNewEmployeeCommand
        {
            get
            {
                if (_addNewEmployeeCommand == null)
                    _addNewEmployeeCommand = new RelayCommand(() =>
                    {
                        if (PivotPage == 0)
                        {
                            PivotPage = 1;
                            return;
                        }
                        if (PivotPage == 1)
                        {
                            using (var db = new HrContext())
                            {
                                _newEmployee.ContactInfo = NewContactInfo;
                                db.Employees.Add(_newEmployee);
                                db.SaveChanges();
                                ClearEmployeeForm();
                            }
                            
                        }
                       
                    });
                return _addNewEmployeeCommand;
            }
        }

        private static ObservableCollection<Employees> GetAllEmployees()
        {
            ObservableCollection<Employees> employeese = new ObservableCollection<Employees>();
            using (var db = new HrContext())
            {
                foreach (var emp in db.Employees)
                {
                    employeese.Add(emp);
                }   
            }
            return employeese;
        }


        private void ClearEmployeeForm()
        {
            _newEmployee.ContactInfo = null;
            _newEmployee.ContactsInfoId = 0;
            _newEmployee.FirstName = null;
            _newEmployee.LastName = null;
            _newEmployee.MiddleName = null;
            _newEmployee.Gender = null;
            _newEmployee.Post = null;
            _newContactInfo.City = null;
            _newContactInfo.Country = null;
            _newContactInfo.Email = null;
            _newContactInfo.Home = null;
            _newContactInfo.LinkedIn = null;
            _newContactInfo.Phone = null;
            _newContactInfo.Skype = null;
            _newContactInfo.Street = null;
        }

        public ICommand LogOut
        {
            get
            {
                if (_logOut == null)
                    _logOut = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("LoginPage");
                    });
                return _logOut;
            }
        }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand(NavigateCommandAction);
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("MainPage");
        }

        public void TestTimeChangedEvent(object sender, DatePickerValueChangedEventArgs e)
        {
            _newEmployee.BirthDate = e.NewDate.DateTime;
        }
    }
}
