using HardwareDetailMaui.MVVM.Models;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareDetailMaui.MVVM.ViewModels
{
    public class CpuViewModels : INotifyPropertyChanged
    {
        private ObservableCollection<CpuDetail> _cpuDetails;
        private bool _isMonitoring;
        private Computer _computer;
        private string _cpuName;
        private CpuDetail _coreAverage;

        public CpuDetail CoreAverage
        {
            get { return _coreAverage; }
            set 
            { 
                _coreAverage = value;
                OnPropertyChanged();
            }
        }

        public string CpuName
        {
            get { return _cpuName; }
            set
            {
                _cpuName = value;
                OnPropertyChanged();
            }

        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public CpuViewModels()
        {
            CpuDetails = new ObservableCollection<CpuDetail>();
            StartCommand = new Command(() => StartMonitoringButton());
            StopCommand = new Command(() => StopMonitoringButton());
            CoreAverage = new CpuDetail();
            _computer = new Computer()
            {
                IsCpuEnabled = true
            };
            _computer.Open();
            GetCpuName().GetAwaiter().GetResult();
            _isMonitoring=true;
            Task.Run(async() => await MonitorCpuTemperature());

        }

        private void StopMonitoringButton()
        {
            _isMonitoring = false;
        }
        private void StartMonitoringButton()
        {
            _isMonitoring = true;
            Task.Run(() => MonitorCpuTemperature());
        }
        public ObservableCollection<CpuDetail> CpuDetails
        {
            get { return _cpuDetails; }
            set
            {
                _cpuDetails = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        async Task GetCpuName()
        {
            foreach (IHardware hardware in _computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    CpuName = hardware.Name;
                }
            }
        }
        public async Task MonitorPowerAsync()
        {
            while (true)
            {
                foreach (IHardware hardware in _computer.Hardware)
                {
                }
            }
        }
       public async Task MonitorCpuTemperature()
        {

            // Run the monitoring loop
            while (_isMonitoring)
            {
                // Iterate through all hardware components
                foreach (IHardware hardware in _computer.Hardware)
                {
                    // Check if the hardware is a CPU
                    if (hardware.HardwareType == HardwareType.Cpu)
                    {
                        // Update the hardware data
                        hardware.Update();

                        // Iterate through all sensors for the CPU
                        foreach (ISensor sensor in hardware.Sensors)
                        {                           
                            if (sensor.SensorType == SensorType.Temperature)
                            {                           
                                if (CpuDetails.Count() <= 0 && !sensor.Name.Contains("TjMax") && !sensor.Name.Contains("Core Average") && !sensor.Name.Contains("Core Max") && !sensor.Name.Contains("CPU Package"))
                                    CpuDetails.Add(new CpuDetail(sensor.Value.GetValueOrDefault(), sensor.Name,(float) sensor.Min, (float)sensor.Max));
                                else if (CpuDetails.Count() >= 1 && !sensor.Name.Contains("TjMax") && !sensor.Name.Contains("Core Average") && !sensor.Name.Contains("Core Max") && !sensor.Name.Contains("CPU Package"))
                                {
                                    var core = CpuDetails.Where(x => x.Name == sensor.Name).FirstOrDefault();
                                    if (core != null)
                                    {
                                        core.CoreTemp = sensor.Value.GetValueOrDefault();
                                        core.CoreTempMin = (float)sensor.Min;
                                        core.CoreTempMax = (float)sensor.Max;
                                        if (sensor.Value.GetValueOrDefault() >= 80)
                                        {
                                            core.Color1 = "#e20c2d";
                                            core.Color2 = "#f1d8dc";
                                        }
                                        else if (sensor.Value.GetValueOrDefault() < 80)
                                        {
                                            core.Color1 = "#538ab6";
                                            core.Color2 = "#938ed9";
                                        }
                                    }
                                    else
                                    {
                                        CpuDetails.Add(new CpuDetail(sensor.Value.GetValueOrDefault(), sensor.Name, (float)sensor.Min, (float)sensor.Max));
                                    }
                                }
                                else if(CpuDetails.Count() >= 1 && sensor.Name.Contains("Core Average"))
                                {
                                    CoreAverage.Name = sensor.Name;
                                    CoreAverage.CoreTemp = sensor.Value.GetValueOrDefault();
                                    CoreAverage.CoreTempMin = (float)sensor.Min;
                                    CoreAverage.CoreTempMax = (float)sensor.Max;
                                   

                                }
                            
                            }
                            if (sensor.SensorType == SensorType.Load)
                            {
                                 if (CpuDetails.Count() >= 1 && !sensor.Name.Contains("TjMax") && !sensor.Name.Contains("Core Average") && !sensor.Name.Contains("Core Max") && !sensor.Name.Contains("CPU Package"))
                                {
                                    var core = CpuDetails.Where(x => x.Name == sensor.Name).FirstOrDefault();
                                    if (core != null)
                                    {
                                        core.CoreLoad = sensor.Value.GetValueOrDefault();
                                      
                                    } 
                                }

                            }
                            if (sensor.SensorType == SensorType.Voltage)
                            {
                                if (CpuDetails.Count() >= 1 && !sensor.Name.Contains("TjMax") && !sensor.Name.Contains("Core Average") && !sensor.Name.Contains("Core Max") && !sensor.Name.Contains("CPU Package"))
                                {
                                    var core = CpuDetails.Where(x => x.Name == sensor.Name).FirstOrDefault();
                                    if (core != null)
                                    {
                                        core.Voltage = sensor.Value.GetValueOrDefault();

                                    }
                                }

                            }
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                if (CpuDetails.Count() >= 1 && !sensor.Name.Contains("TjMax") && !sensor.Name.Contains("Core Average") && !sensor.Name.Contains("Core Max") && !sensor.Name.Contains("CPU Package"))
                                {
                                    var core = CpuDetails.Where(x => x.Name == sensor.Name).FirstOrDefault();
                                    if (core != null)
                                    {
                                        core.Clock = sensor.Value.GetValueOrDefault();

                                    }
                                }

                            }
                        }
                    }
                }

                // Sleep for a while before the next reading (e.g., 2 seconds)
                Thread.Sleep(2000);

            }
            // Close the computer object when done (unreachable in this loop)
            // computer.Close();
        }
    }
}
