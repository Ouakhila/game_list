using NewWpfApp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Table = NewWpfApp.Model.Table;

namespace NewWpfApp.AccessData
{
    public class GameRepository
    {
        private DatabaseGameEntities gameContext = null;
        private int _Id = -1;

        public static int InvalidId = -1;

        public GameRepository()
        {
            gameContext = new DatabaseGameEntities();
        }

        private int GetUniqueId()
        {
            _Id++;
            return _Id;
        }

        public Table Get(int id)
        {
            return gameContext.Tables.Find(id);
        }

        public List<Table> GetAll()
        {
            return gameContext.Tables.ToList();
        }

        public int AddGame(string name, string description)
        {
            var game = new Table(GetUniqueId(), name, description);

            if (game != null)
            {
                gameContext.Tables.Add(game);
                gameContext.SaveChanges();

                return game.Id;
            }

            return -1;
        }

        public void UpdateGame(int id, string name, string description)
        {
            var gameFind = this.Get(id);
            if (gameFind != null)
            {
                gameFind.GameName = name;
                gameFind.GameDescription = description;
                gameContext.SaveChanges();
            }
        }

        public void RemoveGame(int id)
        {
            var gameObj = gameContext.Tables.Find(id);
            if (gameObj != null)
            {
                gameContext.Tables.Remove(gameObj);
                gameContext.SaveChanges();
            }
        }
    }
}
