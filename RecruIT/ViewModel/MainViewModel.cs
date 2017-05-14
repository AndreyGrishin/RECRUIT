using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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

        public IView View { get; set; }

        #region Properties      

        public static ObservableCollection<Employees> employeese;
        public ObservableCollection<Employees> Employeese
        {
            get => GetAllEmployees();
            set
            {
                employeese = value;
                RaisePropertyChanged(() => Employeese);
            }
        }

        public static ObservableCollection<Departments> departments;
        public ObservableCollection<Departments> Departments
        {
            get => GetDepartments();
            set
            {
                departments = value;
                RaisePropertyChanged(() => Departments);
            }
        }

        public Visibility IsCountEmployeeEmpty
        {
            get
            {
                if (Employeese == null || Employeese.Count == 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            set
            {
                RaisePropertyChanged(() => IsCountEmployeeEmpty);
            }
        }

        private int _pivotPage;
        public int PivotPage
        {
            get => _pivotPage;
            set
            {
                _pivotPage = value;
                RaisePropertyChanged(() => PivotPage);
            }
        }

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get => _currentImage ?? (_currentImage = new BitmapImage());
            set
            {
                _currentImage = value;
                RaisePropertyChanged(() => CurrentImage);
            }
        }

        private Employees _newEmployee;
        public Employees NewEmployee
        {
            get => _newEmployee ?? (_newEmployee = new Employees());
            set
            {
                _newEmployee = value;
                RaisePropertyChanged(() => NewEmployee);
            }
        }


        private ObservableCollection<Posts> _selectablePosts;
        public ObservableCollection<Posts> SelectablePosts
        {
            get => _selectablePosts ?? (_selectablePosts = new ObservableCollection<Posts>());
            set
            {
                _selectablePosts = value;
                RaisePropertyChanged(() => SelectablePosts);
            }
        }

        private bool _isEnablePostComboBox;
        public bool IsEnablePostComboBox
        {
            get => _isEnablePostComboBox;
            set
            {
                _isEnablePostComboBox = value;
                RaisePropertyChanged(() => IsEnablePostComboBox);
            }
        }

        private Posts _selectedPost;
        public Posts SelectedPost
        {
            get => _selectedPost ?? (_selectedPost = new Posts());
            set
            {
                _selectedPost = value;
                RaisePropertyChanged(() => SelectedPost);
            }
        }

        private Departments _selectedDepartment;
        public Departments SelectedDepartment
        {
            get => _selectedDepartment ?? (_selectedDepartment = new Departments());
            set
            {
                _selectedDepartment = value;
                RaisePropertyChanged(() => SelectedDepartment);
            }
        }

        private int _selectedDepartmentIndex;
        public int SelectedDepartmentIndex
        {
            get => _selectedDepartmentIndex;
            set
            {
                _selectedDepartmentIndex = value;
                RaisePropertyChanged(() => SelectedDepartmentIndex);
            }
        }

        private Employees _selectedEmployee;
        public Employees SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged(() => SelectedEmployee);
            }
        }

        private Departments _currentDepartment;
        public Departments CurrentDepartment
        {
            get => _currentDepartment ?? (_currentDepartment = new Departments());
            set
            {
                _currentDepartment = value;
                RaisePropertyChanged(() => CurrentDepartment);
            }
        }

        private Departments _newDepartment;
        public Departments NewDepartment
        {
            get => _newDepartment ?? (_newDepartment = new Departments());
            set
            {
                _newDepartment = value;
                RaisePropertyChanged(() => NewDepartment);
            }
        }

        private Posts _newPost;
        public Posts NewPost
        {
            get => _newPost ?? (_newPost = new Posts());
            set
            {
                _newPost = value;
                RaisePropertyChanged(() => NewPost);
            }
        }

        private ContactInfo _newContactInfo;
        public ContactInfo NewContactInfo
        {
            get => _newContactInfo ?? (_newContactInfo = new ContactInfo());
            set
            {
                _newContactInfo = value;
                RaisePropertyChanged(() => NewContactInfo);
            }
        }
        #endregion

        #region Commands

        private RelayCommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(() =>
                    {
                        switch (PivotPage)
                        {
                            case 0:
                                PivotPage = 1;
                                return;
                            case 1:
                                AddNewEmployee();
                                break;
                            case 2:
                                if (View == null) return;
                                View.ShowAddDepartmentDialogWindow();
                                break;
                        }
                    });
                return _addCommand;
            }
        }

        private RelayCommand _updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                    _updateCommand = new RelayCommand(() =>
                    {
                        switch (PivotPage)
                        {
                            case 0:
                                PivotPage = 1;
                                ChangeEmployee();
                                return;
                            case 1:

                                break;
                            case 2:

                                break;
                        }
                    });
                return _updateCommand;
            }
        }


        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(() =>
                    {
                        switch (PivotPage)
                        {
                            case 0:
                                DeleteEmployee();
                                return;
                            case 1:

                                break;
                            case 2:
                                DeleteDepartment();
                                break;
                        }
                    });
                return _deleteCommand;
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

        private RelayCommand _showDialogAddPostCommand;
        public ICommand ShowDialogShowDialogAddPostCommand
        {
            get
            {
                if (_showDialogAddPostCommand == null)
                    _showDialogAddPostCommand = new RelayCommand(() =>
                    {
                        if (View == null) return;
                        View.ShowAddPostsDialogWindow();
                    });
                return _showDialogAddPostCommand;
            }
        }

        private RelayCommand _addPostCommand;
        public ICommand AddPostCommand
        {
            get
            {
                if (_addPostCommand == null)
                    _addPostCommand = new RelayCommand(() =>
                    {
                        NewPost.DepartmentId = CurrentDepartment.Id;
                        using (var db = new HrContext())
                        {
                            db.AddAsync(NewPost);
                            db.SaveChangesAsync();
                        }
                        RaisePropertyChanged(() => Departments);
                    });
                return _addPostCommand;
            }
        }

        private RelayCommand _addDepartmentCommand;
        public ICommand AddDepartmentCommand
        {
            get
            {
                if (_addDepartmentCommand == null)
                    _addDepartmentCommand = new RelayCommand(() =>
                    {
                        using (var db = new HrContext())
                        {
                            db.AddAsync(NewDepartment);
                            db.SaveChangesAsync();
                        }
                        RaisePropertyChanged(() => Departments);
                    });
                return _addDepartmentCommand;
            }
        }

        private RelayCommand _browseFileCommand;
        public ICommand BrowseFileCommand
        {
            get
            {
                if (_browseFileCommand == null)
                    _browseFileCommand = new RelayCommand(async () =>
                    {
                        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                        FileOpenPicker openPicker = new FileOpenPicker
                        {
                            ViewMode = PickerViewMode.List,
                            SuggestedStartLocation = PickerLocationId.PicturesLibrary
                        };
                        openPicker.FileTypeFilter.Add(".jpg");
                        openPicker.FileTypeFilter.Add(".png");

                        StorageFile file = await openPicker.PickSingleFileAsync();
                        if (file is null)
                            return;

                        var previousImage = CurrentImage;
                        string fileName = NewEmployee.FirstName + NewEmployee.LastName + file.FileType;
                        await file.CopyAsync(localFolder, fileName, NameCollisionOption.ReplaceExisting);
                        CurrentImage = new BitmapImage(new Uri(@"ms-appdata:///local/" + fileName));

                        if (previousImage.UriSource != null)
                        {
                            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(CurrentImage.UriSource);
                            await storageFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                        }
                    });
                return _browseFileCommand;
            }
        }
        #endregion

        #region Methods

        private void AddNewEmployee()
        {
            using (var db = new HrContext())
            {
                NewContactInfo.Id = GetLastContactInfoId() + 1;
                NewEmployee.ContactsInfoId = NewContactInfo.Id;
                NewEmployee.PhotoPath = CurrentImage.UriSource.AbsoluteUri;
                NewEmployee.ContactInfo = NewContactInfo;
                NewEmployee.PostId = SelectedPost.Id;
                db.Employees.Add(_newEmployee);
                db.SaveChanges();
                ClearEmployeeForm();

                RaisePropertyChanged(() => Employeese);
                RaisePropertyChanged(() => IsCountEmployeeEmpty);

                int GetLastContactInfoId()
                {
                    return db.Employees.Max(x => x.Id);
                }
            }
        }

        private async void DeleteDepartment()
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Удалить отдел?",
                Content = "Вы уверены в том что хотите удалить отдел?",
                PrimaryButtonText = "Удалить",
                SecondaryButtonText = "Отменить"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
                DeleteCurrentDepartment();

            void DeleteCurrentDepartment()
            {
                using (var db = new HrContext())
                {
                    db.Departments.Remove(CurrentDepartment);
                    db.SaveChangesAsync();
                }
            }
            RaisePropertyChanged(() => Departments);
        }

        private async void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;
            try
            {
                using (var db = new HrContext())
                {
                    var selectedContactInfo = db.ContectInfo.First(x => x.Id == SelectedEmployee.ContactsInfoId);

                    db.Employees.Remove(SelectedEmployee);
                    await db.SaveChangesAsync();

                    db.ContectInfo.Remove(selectedContactInfo);
                    await db.SaveChangesAsync();
                }
                // var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(SelectedEmployee.PhotoPath));
                //  await storageFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception e)
            {
                await new MessageDialog("Ошибка удаления").ShowAsync();
            }
            finally
            {
                RaisePropertyChanged(() => IsCountEmployeeEmpty);
                RaisePropertyChanged(() => Employeese);
                SelectedEmployee = null;
            }
        }

        private void ChangeEmployee()
        {
            NewContactInfo = GetGontactInfo();
            NewEmployee = SelectedEmployee;
            SelectedPost = GetPost();
            SelectedDepartment = GetDepartment();
            SelectedDepartmentIndex = GetDepartmentIndex();
            CurrentImage = new BitmapImage(new Uri(SelectedEmployee.PhotoPath));
            RaisePropertyChanged(() => NewEmployee);
            RaisePropertyChanged(() => NewContactInfo);
            RaisePropertyChanged(() => CurrentImage);

            int GetDepartmentIndex()
            {
                for (int i = 0; i < Departments.Count; i++)
                {
                    if (Departments[i].Id == SelectedPost.DepartmentId)
                        return i;
                }
                return -1;
            }
            Departments GetDepartment()
            {
                using (var db = new HrContext())
                {
                    return db.Departments.First(x => x.Id == SelectedPost.DepartmentId);
                }
            }

            Posts GetPost()
            {
                using (var db = new HrContext())
                {
                    return db.Posts.First(x => x.Id == SelectedEmployee.PostId);
                }
            }

            ContactInfo GetGontactInfo()
            {
                using (var db = new HrContext())
                {
                  return  db.ContectInfo.First(x => x.Id == SelectedEmployee.ContactsInfoId);
                }
            }
        }

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
        private ObservableCollection<Posts> GetPosts()
        {
            if (DbLoader.IsPostsEmpty())
                DbLoader.AddDefaultPosts();

            return new ObservableCollection<Posts>(DbLoader.GetPosts(SelectedDepartment.Id));
        }


        private void ClearEmployeeForm()
        {
            NewContactInfo = null;
            NewEmployee = null;
            CurrentImage = null;
            SelectedDepartment = null;
            SelectablePosts = null;
        }
        #endregion

        #region Events

        public void TestTimeChangedEvent(object sender, DatePickerValueChangedEventArgs e)
        {
            _newEmployee.BirthDate = e.NewDate.DateTime;
        }

        public void DepartmentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
                IsEnablePostComboBox = true;
            else
                IsEnablePostComboBox = false;

            SelectablePosts = GetPosts();
        }
        public void DepartmentsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as Microsoft.Toolkit.Uwp.UI.Controls.AdaptiveGridView;
            CurrentDepartment = (Departments)control.SelectedItem;
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
