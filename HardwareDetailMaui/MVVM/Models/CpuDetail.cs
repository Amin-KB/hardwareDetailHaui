using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HardwareDetailMaui.MVVM.Models
{
    public class CpuDetail : INotifyPropertyChanged
    {
        private string _color1;

        public string Color1
        {
            get { return _color1; }
            set
            {
                _color1 = value;
                OnPropertyChanged();
            }
        }
        private string _color2;

        public string Color2
        {
            get { return _color2; }
            set
            {
                _color2 = value;
                OnPropertyChanged();
            }
        }
        private float _voltage;

        public float Voltage
        {
            get { return _voltage; }
            set 
            {
                _voltage = value;
                OnPropertyChanged();
            }
        }
        private float _clock;

        public float Clock
        {
            get { return _clock; }
            set
            {
                _clock = value;
                OnPropertyChanged();
            }
        }
        private float _coreTemp;

        public float CoreTemp
        {
            get { return _coreTemp; }
            set
            {
                _coreTemp = value;
                OnPropertyChanged();
            }
        }
        private float _coreTempMin;

        public float CoreTempMin
        {
            get { return _coreTempMin; }
            set
            {
                _coreTempMin = value;
                OnPropertyChanged();
            }
        }
        private float _coreTempMax;

        public float CoreTempMax
        {
            get { return _coreTempMax; }
            set
            {
                _coreTempMax = value;
                OnPropertyChanged();
            }
        }
        private float _coreLoad;

        public float CoreLoad
        {
            get { return _coreLoad; }
            set
            {
                _coreLoad = value;
                OnPropertyChanged();
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }



        public CpuDetail()
        {

        }
        public CpuDetail(float coreTemp, string name, float min, float max)
        {
            CoreTemp = coreTemp;
            Name = name;
            CoreTempMin = min;
            CoreTempMax = max;
            if (CoreTemp >= 80)
            {
                Color1 = "#e20c2d";
                Color2 = "#f1d8dc";
            }
            else if (CoreTemp < 80)
            {
                Color1 = "#538ab6";
                Color2 = "#938ed9";
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
