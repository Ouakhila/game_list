using NewWpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWpfApp.Model
{
    public class GameRecord : ViewModelBase
    {
        private int _id = -1;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _gameName;
        public string GameName
        {
            get
            {
                return _gameName;
            }
            set
            {
                _gameName = value;
                OnPropertyChanged("GameName");
            }
        }

        private string _gameDescription;
        public string GameDescription
        {
            get
            {
                return _gameDescription;
            }
            set
            {
                _gameDescription = value;
                OnPropertyChanged("GameDescription");
            }
        }
    }

}
