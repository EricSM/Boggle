using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{

    class Controller
    {
        private Model APIModel = new Model();
        private Interface window;
        public Controller(Interface window)
        {

            this.window = window;
           

            AddEvents();
        }
        private void AddEvents()
        {
            window.TestEvent += HandleTestEvent;

        }

        private void HandleTestEvent(string input)
        {
            window.Title = "Value received: " + input;
         
            APIModel.CreateUser("Meysam");

            //Console.WriteLine(APIModel.CurrentUID);

            APIModel.JoinGame(APIModel.CurrentUID, 100);

        }
    }

}
