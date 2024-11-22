using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Grape_Fruit_Bar
{
    internal class Screen : ScreenInterface
    {
        public Screen nextScreen { get; set; }
        public Screen previousScreen { get; set; }
        public UserInput userInput { get; set; }
        public string title { get; } = "";
        public string text { get; set; } = "";
        public string text2 { get; set; } = "";
        public string commandInfo { get; set; } = "";
        public string subInfo { get; set; } = "";
        public string userInputRow { get; set; } = "";
        public string[] validActions { get; set; }
        public bool cursorVisible { get; set; }
        public Dictionary<ConsoleKey, string> consoleKey2Action {get;set;}
        public string customText { get; set; } = "";

        public static Dictionary<string, string> action2Screen { get; set; } = new Dictionary<string, string>
            {
                {"NEW_CUSTOMER","MENU_SCREEN"},
                {"QUIT","QUIT"},
                {"PAY","START_SCREEN"},
                {"PAYMENT_OPTIONS","PAYMENT_OPTIONS_SCREEN"},
                {"RECEIPT","RECEIPT_SCREEN"},
                {"REPORT","REPORT_SCREEN" },
                {"BACK","BACK"},
                {"VISA","PAYMENT_SCREEN" },
                {"Mastercard" ,"PAYMENT_SCREEN"},
                {"SWISH","PAYMENT_SCREEN" },
                {"Cash","PAYMENT_SCREEN" },
                {"Klarna","PAYMENT_SCREEN" },
                {"NEXT","NEXT" },
                {"STAY","STAY"},
                {"INVALID","INVALID" }
            };
        public string screenID { get; set; }
        public List<Screen> screenList { get; set; }

        
        public Screen(string title, InputType inputType, string[] validActions,bool cursorVisible, Dictionary<ConsoleKey, string> consoleKey2Action, string screenID)
        {
            this.title = GeneralMethods.TitleDesign(title);
            this.userInput = new UserInput(inputType,this);
            this.validActions = validActions;
            this.cursorVisible = cursorVisible;
            this.consoleKey2Action = consoleKey2Action;
            this.screenID = screenID;
            
        }

        public void PrintScreenContents()
        {
            Console.Clear();
            Console.CursorVisible = this.cursorVisible;
            Console.WriteLine(this.title);
            Console.WriteLine(this.text);
            Console.WriteLine(this.text2);
            Console.WriteLine(this.commandInfo);
            Console.WriteLine(this.subInfo);
            Console.Write(this.userInputRow);
        }

        public void ClearScreenContents()
        {
            this.subInfo = "";
            this.text2 = "";
            this.text = "RECEIPT   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string Button2String(ConsoleKey ck)
        {
            try
            {
                return this.consoleKey2Action[ck];
            }
            catch (Exception e)
            {
                Console.WriteLine("That button is not available!");
                Console.ReadLine();
                return action2Screen["INVALID"];
            }
        }

        

        public void Start()
        {
            bool stayOnScreen = true;
            while (stayOnScreen)
            {
                this.PrintScreenContents();

                string registerAnswer = userInput.HandleUserInput();
                string navigation = registerAnswer;

                if(registerAnswer.StartsWith("SEARCH RESULTS:"))
                {
                    this.subInfo = "\n" + registerAnswer;
                    navigation = "STAY";
                }
                else if(registerAnswer.StartsWith("CART ITEMS:"))
                {
                    this.text2 = "\n" + registerAnswer;
                    navigation = "STAY";
                }
                else if (registerAnswer.StartsWith("RECEIPT:"))
                {
                    this.text2 = registerAnswer;
                    navigation = "STAY";
                }

                //This means we have a new customer
                else if (registerAnswer == "MENU_SCREEN")
                foreach (Screen s in this.screenList)
                {
                    if ("MENU_SCREEN" == s.screenID)
                    {
                        s.ClearScreenContents();
                    }
                }

                switch (navigation)
                {
                    case "BACK":
                        stayOnScreen = false;
                        this.GoToPreviousScreen();
                        break;
                    case "NEXT":
                        stayOnScreen = false;
                        this.GoToNextScreen();
                        break;
                    case "STAY":
                        break;
                    case "QUIT":
                        stayOnScreen = false;
                        this.GoToPreviousScreen();
                        break;
                    case "INVALID":
                        break;
                    default:
                        stayOnScreen=false;
                        foreach (Screen s in this.screenList)
                        {
                            if (navigation == s.screenID)
                            {
                                s.Start();
                            }
                        }
                        break;
                }
            }
        }
        public void GoToNextScreen()
        {
            if (this.nextScreen == null)
            {
                return;
            }
            else
            {
                this.nextScreen.Start();
            }
        }
        public void GoToPreviousScreen()
        {
            if (this.previousScreen == null)
            {
                return;
            }
            else
            {
                this.previousScreen.Start();
            }
        }
    }
}
