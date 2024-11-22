using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grape_Fruit_Bar
{
    internal interface ScreenInterface
    {
        public Screen nextScreen { get; set; }
        public Screen previousScreen { get; set; }
        public UserInput userInput { get; set; }
        public string title { get; }
        public string text { get; set; }
        public string text2 { get; set; }
        public string commandInfo { get; set; }
        public string subInfo { get; set; }

        public string userInputRow { get; set; }

        public string[] validActions { get; set; }
        public void PrintScreenContents();
        public void Start();
        public void GoToNextScreen();
        public void GoToPreviousScreen();
        
        public string Button2String(ConsoleKey ck);
        public bool cursorVisible { get; set; }
        public Dictionary<ConsoleKey, string> consoleKey2Action { get; set; }
        public string customText { get; set; }

        public static Dictionary<string, string> action2Screen { get; set; }
        public string screenID { get; set; }
        public List<Screen> screenList { get; set; }

    }
}
