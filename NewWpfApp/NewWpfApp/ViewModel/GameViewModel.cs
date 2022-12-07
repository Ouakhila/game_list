using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using NewWpfApp.Model;
using NewWpfApp.AccessData;
using System.Collections.Specialized;
using System.Windows.Markup;

namespace NewWpfApp.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private ICommand _saveCommand;
        private ICommand _resetCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private GameRepository _repository;

        public GameRecord GameRecord { get; set; }
        public DatabaseGameEntities GameEntities { get; set; }

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => ResetData(), null);

                return _resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveData(), null);

                return _saveCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => EditData((int)param), null);

                return _editCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => DeleteGame((int)param), null);

                return _deleteCommand;
            }
        }

        public GameViewModel()
        {
            _repository = new GameRepository();
            GameRecord = new GameRecord();
            GetAll();
        }

        public void ResetData()
        {
            GameRecord.GameName = string.Empty;
            GameRecord.Id = 0;
            GameRecord.GameDescription = string.Empty;
        }

        public void DeleteGame(int id)
        {
            if (MessageBox.Show("Confirm delete of this record?", "Game", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.RemoveGame(id);
                    MessageBox.Show("Record successfully deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                }
            }
        }

        public void SaveData()
        {
            if (GameRecord != null)
            {
                try
                {
                    if (GameRecord.Id < 0)
                    {
                        var added_id = _repository.AddGame(GameRecord.GameName, GameRecord.GameDescription);

                        if (added_id == -1)
                        {
                            MessageBox.Show("Failed to add record.");
                        }
                        else
                        {
                            GamesRecord.Add(new GameRecord()
                            {
                                Id = added_id,
                                GameName = GameRecord.GameName,
                                GameDescription = GameRecord.GameDescription,

                            });

                            MessageBox.Show("New record successfully saved.");
                        }
                    }
                    else
                    {
                        _repository.UpdateGame(GameRecord.Id, GameRecord.GameName, GameRecord.GameDescription);

                        var record_to_update = GamesRecord.FirstOrDefault(record => record.Id == GameRecord.Id);
                        if (record_to_update != null)
                        {
                            record_to_update.GameName = GameRecord.GameName;
                            record_to_update.GameDescription = GameRecord.GameDescription;
                        }

                        MessageBox.Show("Record successfully updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    ResetData();
                }
            }
        }

        public void EditData(int id)
        {
            var model = _repository.Get(id);
            GameRecord.Id = model.Id;
            GameRecord.GameName = model.GameName;
            GameRecord.GameDescription = model.GameDescription;
        
        }

        public void GetAll()
        {
            GamesRecord = new ObservableCollection<GameRecord>();
            _repository.GetAll().ForEach(data => GamesRecord.Add(new GameRecord()
            {
                Id = data.Id,
                GameName = data.GameName,
                GameDescription = data.GameDescription,

            }));
        }

        private ObservableCollection<GameRecord> _gamesRecord;
        public ObservableCollection<GameRecord> GamesRecord
        {
            get
            {
                return _gamesRecord;
            }
            set
            {
                _gamesRecord = value;
                OnPropertyChanged("GamesRecord");
            }
        }

        private void DatabaseGameModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("GamesRecord");
        }
    }
}
