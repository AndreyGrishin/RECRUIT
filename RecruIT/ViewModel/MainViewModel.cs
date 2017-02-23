using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace RecruIT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }

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

        RelayCommand _addNewEmployeeCommand;

        public ICommand AddNewEmployeeCommand
        {
            get
            {
                if (_addNewEmployeeCommand == null)
                    _addNewEmployeeCommand = new RelayCommand(() =>
                    {
                        PivotPage = 1;
                    });
                return _addNewEmployeeCommand;
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


    }
}
