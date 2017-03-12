using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RecruIT.Model;
using RecruIT.Model.DBModel;

namespace RecruIT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

        public static ObservableCollection<Employees> Employeese => GetAllEmployees();
        public static ObservableCollection<Departments> Departments => GetDepartments();
        public  ObservableCollection<Posts> Posts
        {
            get { return GetPosts(); }
            set { RaisePropertyChanged(() => Posts); }
        }

        #region Properties      
        public bool IsCountEmployeeEmpty
        {
            get
            {
                if (Employeese == null)
                    return true;
                return Employeese.Count == 0;
            }
            set
            {
                RaisePropertyChanged(() => IsCountEmployeeEmpty);
            }
        }

        private int _pivotPage;
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

        private Employees _newEmployee;
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

        private Departments _newDepartments;
        public Departments NewDepartment
        {
            get
            {
                if (_newDepartments == null)
                    _newDepartments = new Departments();
                return _newDepartments;
            }
            set
            {
                _newDepartments = value;
                RaisePropertyChanged(() => NewDepartment);
            }
        }

        private Posts _newPosts;
        public Posts NewPost
        {
            get
            {
                if (_newPosts == null)
                    _newPosts = new Posts();
                return _newPosts;
            }
            set
            {
                _newPosts = value;
                RaisePropertyChanged(() => NewPost);
            }
        }

        private ContactInfo _newContactInfo;
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
        #endregion

        #region Commands
      
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
                                _newEmployee.Post = NewPost;                              
                                db.Employees.Add(_newEmployee);
                                db.SaveChanges();
                                ClearEmployeeForm();
                            }
                            
                        }
                       
                    });
                return _addNewEmployeeCommand;
            }
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
        #endregion

        #region Methods
        private static ObservableCollection<Employees> GetAllEmployees()
        {
            using (var db = new HrContext())
            {
                return new ObservableCollection<Employees>(db.Employees.ToList());
            }
        }

        private static ObservableCollection<Departments> GetDepartments()
        {
            if (DbLoader.IsDepartmentsEmpty())
            {
               return new ObservableCollection<Departments>(DbLoader.GetDefaultDepartments());
            }
            using (var db  = new HrContext())
            {
                return new ObservableCollection<Departments>(db.Departments.ToList());
            }
        }
        private  ObservableCollection<Posts> GetPosts()
        {
            if (DbLoader.IsPostsEmpty())
            {
                return new ObservableCollection<Posts>(DbLoader.GetDefaultPosts(NewDepartment.Id));
            }
            using (var db = new HrContext())
            {
                return new ObservableCollection<Posts>(db.Posts.Where(x => x.DepartmentId == 1).ToList());
            }
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
        #endregion

        #region Events

        public void TestTimeChangedEvent(object sender, DatePickerValueChangedEventArgs e)
        {
            _newEmployee.BirthDate = e.NewDate.DateTime;
        }
        public void DepartmentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Posts = GetPosts();
        }
        public void PostsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPost = (sender as ComboBox)?.SelectedItem as Posts;
            if (selectedPost != null)
                NewPost = selectedPost;
            
        }
        #endregion
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand(NavigateCommandAction);
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("MainPage");
        }


    }
}
