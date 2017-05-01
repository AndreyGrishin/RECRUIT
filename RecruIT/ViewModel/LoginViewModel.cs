using System;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using RecruIT.Model.DBModel;


namespace RecruIT.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

        #region Properties

        public string ConfirmPassword { get; set; }

        private bool _registerVisibility;
        public bool RegisterVisibility
        {
            get
            {
                return _registerVisibility;
            }
            set
            {
                _registerVisibility = value;
                RaisePropertyChanged(() => RegisterVisibility);
            }
        }

        private Users _newUser;
        public Users NewUser
        {
            get
            {
                if (_newUser == null)
                    _newUser = new Users();
                
                return _newUser;
            }
            set
            {
                _newUser = value;
                RaisePropertyChanged(() => NewUser);
            }
        }

        private Users _currentUser;
        public Users CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = new Users();
                return _currentUser;
            }
            set
            {
               
                _currentUser = value;
                RaisePropertyChanged(() => CurrentUser);
            }
        }
        private string _title;
        public string Title
        {

            get
            {
                return _title;
            }
            set
            {
                if (value == _title) return;
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        #endregion // Properties

        #region Commands
        private RelayCommand _loginCommand;
        public ICommand LogIn
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(ExecuteLoginCommand, () =>
                    {
                        if (string.IsNullOrEmpty(CurrentUser.Login) || string.IsNullOrEmpty(CurrentUser.Password))
                            return false;
                        return true;
                    });
                return _loginCommand;
            }
        }

        private RelayCommand _showRegistrarionCommand;
        public ICommand ShowRegistration
        {
            get
            {
                if (_showRegistrarionCommand == null)
                    _showRegistrarionCommand = new RelayCommand(ExecuteShowRegistrationCommand);
                return _showRegistrarionCommand;
            }
        }

        private RelayCommand _addUserCommand;
        public ICommand AddUser
        {
            get
            {
                
                if (_addUserCommand == null)
                    _addUserCommand = new RelayCommand(ExecuteAddUserCommand,
                        () =>
                        {
                            
                            if (string.IsNullOrEmpty(NewUser.Name) || string.IsNullOrEmpty(NewUser.Login) 
                            || string.IsNullOrEmpty(NewUser.Password))
                                return false;
                            return true;
                        });
                
                return _addUserCommand;
            }
        }
        #endregion // Commands

        #region Methods
        public void ExecuteShowRegistrationCommand()
        {
            RegisterVisibility = !RegisterVisibility;
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("RegisterPage");
        }

        public async void ExecuteAddUserCommand()
        {
            if (string.IsNullOrEmpty(_newUser.Name) && string.IsNullOrEmpty(_newUser.Login)
                && string.IsNullOrEmpty(_newUser.Password) && string.IsNullOrEmpty(ConfirmPassword))
            {
                await new MessageDialog("Заполните все необходимые поля").ShowAsync();
                return;
            }
            if (_newUser.Password != ConfirmPassword)
            {
                await new MessageDialog("Введёные пароли не совпадают").ShowAsync();
                return;
            }
            using (var db = new HrContext())
            {
                db.Users.Add(_newUser);
                db.SaveChanges();
            }
            await new MessageDialog($"Новый пользователь {_newUser.Name} успешно добавлен").ShowAsync();
            ClearRegisterForm();
            ExecuteShowRegistrationCommand();
        }

        private void ClearRegisterForm()
        {
            _newUser.Login = null;
            _newUser.Name = null;
            _newUser.Password = null;
            ConfirmPassword = null;
        }

        public async void ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(_currentUser.Login) && string.IsNullOrEmpty(_currentUser.Password))
            {
                await new MessageDialog("Введите логин и пароль для входа").ShowAsync();
                return;
            }
            if (string.IsNullOrEmpty(_currentUser.Login))
            {
                await new MessageDialog("Введите логин").ShowAsync();
                return;
            }
            if (string.IsNullOrEmpty(_currentUser.Password))
            {
                await new MessageDialog("Введите пароль").ShowAsync();
                return;
            }

            _addUserCommand.RaiseCanExecuteChanged();

            using (var db = new HrContext())
            {
                await db.Users.ForEachAsync(async p =>
                {
                    if (p.Login == CurrentUser.Login)
                    {
                        if (p.Password == CurrentUser.Password)
                        {
                            _navigationService.NavigateTo("MainPage");
                            ClearLoginForm();
                        }
                        else
                        {
                            await new MessageDialog("Вы ввели неверный пароль. Попробуйте снова.").ShowAsync();
                        }
                    }
                    else
                    {
                        await new MessageDialog("Пользователя с таким логином не существует").ShowAsync();
                    }
                });
            }

        }

        private void ClearLoginForm()
        {
            _currentUser.Login = null;
            _currentUser.Password = null;
        }
        #endregion

        #region Events     
        public void TextBoxChangedEvent_LogIn(object sender, TextBoxTextChangingEventArgs e)
        {
            _loginCommand.RaiseCanExecuteChanged();
        }
        public void PasswordBoxChangedEvent_LogIn(object sender, RoutedEventArgs e)
        {
            _loginCommand.RaiseCanExecuteChanged();
        }
        public void TextBoxChangedEvent_AddNewUser(object sender, TextBoxTextChangingEventArgs e)
        {
            _addUserCommand.RaiseCanExecuteChanged();
        }
        public void PasswordBoxChangedEvent_AddNewUser(object sender, RoutedEventArgs e)
        {
            _addUserCommand.RaiseCanExecuteChanged();
        }

        #endregion

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Title = "LoginPage";
            NavigateCommand = new RelayCommand(NavigateCommandAction);
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
