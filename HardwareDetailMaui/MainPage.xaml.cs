using HardwareDetailMaui.MVVM.ViewModels;

namespace HardwareDetailMaui
{
    public partial class MainPage : ContentPage
    {
      private readonly  CpuViewModels _vm;

        public MainPage()
        {
            InitializeComponent();

            _vm = new CpuViewModels();
            BindingContext = _vm;
        }
        //protected async override void OnAppearing()
        //{
        //   await _vm.MonitorCpuTemperature();
        //}

    }

}
