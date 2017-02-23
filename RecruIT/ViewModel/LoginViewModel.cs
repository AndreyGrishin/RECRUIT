using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RecruIT.Model;
using RecruIT.Model.EmployeesModel;
using RecruIT.Model.Users;


namespace RecruIT.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

        private string _title;
        public string ConfirmPassword { get; set; }

        private User _newUser;
        private User _currentUser;
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

        public User NewUser
        {
            get
            {
                if (_newUser == null)
                    _newUser = new User();
                return _newUser;
            }
            set
            {

                _newUser = value;
                RaisePropertyChanged(() => NewUser);
            }
        }
        

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = new User();
                return _currentUser;
            }
            set
            {
               
                _currentUser = value;
                RaisePropertyChanged(() => CurrentUser);
            }
        }

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

        RelayCommand _addUserCommand;
        RelayCommand _loginCommand;
        RelayCommand _showRegistrarionCommand;

        public ICommand LogIn
        {
            get
            {
               
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
                return _loginCommand;
            }
        }
        public ICommand ShowRegistration
        {
            get
            {
                if (_showRegistrarionCommand == null)
                    _showRegistrarionCommand = new RelayCommand(ExecuteShowRegistrationCommand);
                return _showRegistrarionCommand;
            }
        }

        public ICommand AddUser
        {
            
            get
            {
                
                if (_addUserCommand == null)
                    _addUserCommand = new RelayCommand(ExecuteAddUserCommand,
                        () =>
                        {
                            //if (string.IsNullOrEmpty(NewUser.Name))
                            //    return false;
                            return true;
                        });
                return _addUserCommand;
            }
        }


        public void ExecuteShowRegistrationCommand()
        {
            RegisterVisibility = !RegisterVisibility;
        }

        public void ExecuteAddUserCommand()
        {
            using (var db = new UsersContext())
            {
                db.Users.Add(_newUser);
                db.SaveChanges();
            }
        }

        public bool CanExecuteLoginCommand()
        {
            //if (string.IsNullOrEmpty(NewUser.Name) ||
            //    string.IsNullOrEmpty(NewUser.Login) ||
            //    string.IsNullOrEmpty(NewUser.Password))
            //    return false;
            return true;
        }

        public void ExecuteLoginCommand()
        {
            _addUserCommand.RaiseCanExecuteChanged();
            using (var db = new UsersContext())
            {
                if (db.Users.Any(x => x.Login == CurrentUser.Login && x.Password == CurrentUser.Password))
                {
                    _navigationService.NavigateTo("MainPage");
                }
            }
        }


        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Title = "LoginPage";
            NavigateCommand = new RelayCommand(NavigateCommandAction);
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("RegisterPage");
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
