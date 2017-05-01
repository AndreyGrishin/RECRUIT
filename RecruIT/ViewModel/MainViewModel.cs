using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RecruIT.Model;
using RecruIT.Model.DBModel;
using RecruIT.View;

namespace RecruIT.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

        public static ObservableCollection<Employees> Employeese => GetAllEmployees();
        public static ObservableCollection<Departments> Departments => GetDepartments();

        

        public IView View { get; set; }

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


        private ObservableCollection<Posts> selectablePosts;
        public ObservableCollection<Posts> SelectablePosts
        {
            get
            {
                if (selectablePosts == null)
                    selectablePosts = new ObservableCollection<Posts>();
                return selectablePosts;
            }
            set
            {
                selectablePosts = value;
                RaisePropertyChanged(() => SelectablePosts);
            }
        }

        private Posts selectedPost;
        public Posts SelectedPost
        {
            get
            {
                if (selectedPost == null)
                    selectedPost = new Posts();
                return selectedPost;
            }
            set
            {
                selectedPost = value;
                RaisePropertyChanged(() => SelectedPost);
            }
        }

        private Departments selectedDepartment;
        public Departments SelectedDepartment
        {
            get
            {
                if (selectedDepartment == null)
                    selectedDepartment = new Departments();
                return selectedDepartment;
            }
            set
            {
                selectedDepartment = value;
                RaisePropertyChanged(() => SelectedDepartment);
            }
        }

        private Departments currentDepartment;
        public Departments CurrentDepartment
        {
            get
            {
                if (currentDepartment == null)
                    currentDepartment = new Departments();
                return _newDepartment;
            }
            set
            {
                _newDepartment = value;
                RaisePropertyChanged(() => CurrentDepartment);
            }
        }

        private Departments _newDepartment;
        public Departments NewDepartment
        {
            get
            {
                if (_newDepartment == null)
                    _newDepartment = new Departments();
                return _newDepartment;
            }
            set
            {
                _newDepartment = value;
                RaisePropertyChanged(() => NewDepartment);
            }
        }

        private Posts _newPost;
        public Posts NewPost
        {
            get
            {
                if (_newPost == null)
                    _newPost = new Posts();
                return _newPost;
            }
            set
            {
                _newPost = value;
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
        public ICommand AddNewEmployeeCommand
        {
            get
            {
                if (_addNewEmployeeCommand == null)
                    _addNewEmployeeCommand = new RelayCommand(() =>
                    {
                        switch (PivotPage)
                        {
                            case 0:
                                PivotPage = 1;
                                return;
                            case 1:
                                using (var db = new HrContext())
                                {
                                    _newEmployee.ContactInfo = NewContactInfo;
                                    _newEmployee.PostId = SelectedPost.Id;                              
                                    db.Employees.Add(_newEmployee);
                                    db.SaveChanges();
                                    ClearEmployeeForm();
                                }
                                break;
                            case 2:
                                if (View == null) return;
                                View.ShowAddDepartmentDialogWindow();
                                break;
                        }
                    });
                return _addNewEmployeeCommand;
            }
        }

        private RelayCommand _logOutCommand;
        public ICommand LogOutCommand
        {
            get
            {
                if (_logOutCommand == null)
                    _logOutCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("LoginPage");
                    });
                return _logOutCommand;
            }
        }

        private RelayCommand showDialog_AddPostCommand;
        public ICommand ShowDialogShowDialogAddPostCommand
        {
            get
            {
                if (showDialog_AddPostCommand == null)
                    showDialog_AddPostCommand = new RelayCommand(() =>
                    {
                        if (View == null) return;
                        View.ShowAddPostsDialogWindow();
                    });
                return showDialog_AddPostCommand;
            }
        }

        private RelayCommand addPostCommand;
        public ICommand AddPostCommand
        {
            get
            {
                if (addPostCommand == null)
                    addPostCommand = new RelayCommand(() =>
                    {
                        NewPost.DepartmentId = CurrentDepartment.Id;
                        using (var db = new HrContext())
                        {
                            db.AddAsync(NewPost);
                            db.SaveChangesAsync();
                            
                        }
                    });
                return addPostCommand;
            }
        }

        private RelayCommand addDepartmentCommand;
        public ICommand AddDepartmentCommand
        {
            get
            {
                if (addDepartmentCommand == null)
                    addDepartmentCommand = new RelayCommand(() =>
                    {
                        using (var db = new HrContext())
                        {
                            db.AddAsync(NewDepartment);
                            db.SaveChangesAsync();
                        }
                    });
                return addDepartmentCommand;
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
                DbLoader.AddDefaultDepartments();

            return new ObservableCollection<Departments>(DbLoader.GetDepartments());
        }
        private  ObservableCollection<Posts> GetPosts()
        {
            if (DbLoader.IsPostsEmpty())
                DbLoader.AddDefaultPosts();

            return new ObservableCollection<Posts>(DbLoader.GetPosts(SelectedDepartment.Id));
        }


        private void ClearEmployeeForm()
        {
            _newEmployee.ContactInfo = null;
            _newEmployee.ContactsInfoId = 0;
            _newEmployee.FirstName = null;
            _newEmployee.LastName = null;
            _newEmployee.MiddleName = null;
            _newEmployee.Gender = null;
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
            SelectablePosts = GetPosts();
        }
        public void DepartmentsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as Microsoft.Toolkit.Uwp.UI.Controls.AdaptiveGridView;
            CurrentDepartment = (Departments) control.SelectedItem;
        }

        public void PostsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPost = (sender as ComboBox)?.SelectedItem as Posts;
            if (selectedPost != null)
                SelectedPost = selectedPost;
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
