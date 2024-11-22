using System.Security.Cryptography;

namespace Grape_Fruit_Bar
{

    internal class Program
    {
        public static bool CursorVisibleFalse()
        {
            return false;
        }
        public static bool CursorVisibleTrue()
        {
            return true;
        }
        static void Main(string[] args)
        {
            Dictionary<ConsoleKey, string> SCCommandToString = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.D1,"NEW_CUSTOMER" },
                {ConsoleKey.NumPad1,"NEW_CUSTOMER" },
                {ConsoleKey.D0,"QUIT" },
                {ConsoleKey.NumPad0,"QUIT" },
            };
            Screen startScreen = new Screen("GRAPE FRUIT BAR", InputType.buttonInput, SCCommandToString.Values.ToArray(), CursorVisibleFalse(),SCCommandToString,"START_SCREEN");
            startScreen.text = "1. New Customer"+
                               "\n0. Quit";
            startScreen.commandInfo = "Make your choice with the Number Pad";


            Dictionary<ConsoleKey, string> SSCommandToString = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.D1,"NEXT" },
                {ConsoleKey.NumPad1,"NEXT" },
                {ConsoleKey.D0,"BACK" },
                {ConsoleKey.NumPad0,"BACK" },
            };
            string commandInfo = "Commandos:" +
                                 "\n*Add items:<Product_Id><Space><Count>" +
                                 "\n Example:301 2" +
                                 "\n*Enter a searchword and press Enter" +
                                 "\n*Type and press Enter for:" +
                                 "\n NEXT"+
                                 "\n BACK";
            Screen shoppingScreen = new Screen("GRAPE FRUIT BAR", InputType.writeLine, SSCommandToString.Values.ToArray(), CursorVisibleTrue(),SSCommandToString,"MENU_SCREEN");
            shoppingScreen.text = "RECEIPT   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            shoppingScreen.commandInfo = GeneralMethods.TitleDesign(commandInfo, "-", "|");
            shoppingScreen.userInputRow = "Commando:";

            Dictionary<ConsoleKey, string> PSCommandToString = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.D1,"PAY" },
                {ConsoleKey.NumPad1,"PAY" },
                {ConsoleKey.D2,"PAYMENT_OPTIONS" },
                {ConsoleKey.NumPad2,"PAYMENT_OPTIONS" },
                {ConsoleKey.D3,"RECEIPT" },
                {ConsoleKey.NumPad3,"RECEIPT" },
                {ConsoleKey.D4,"REPORT" },
                {ConsoleKey.NumPad4,"REPORT" },
                {ConsoleKey.D0,"BACK" },
                {ConsoleKey.NumPad0,"BACK" },
            };
            Screen paymentScreen = new Screen("GRAPE FRUIT BAR", InputType.buttonInput, PSCommandToString.Values.ToArray(), CursorVisibleFalse(),PSCommandToString,"PAYMENT_SCREEN");
            paymentScreen.text = "1. PAY\n2. See Payment Options\n3. Previous Receipts\n0. Go Back";
            paymentScreen.commandInfo = "Make your choice with the Number Pad";

            Dictionary<ConsoleKey, string> paymentMethodsCommandToString = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.D1,"VISA" },
                {ConsoleKey.NumPad1,"VISA" },
                {ConsoleKey.D2,"Mastercard" },
                {ConsoleKey.NumPad2,"Mastercard" },
                {ConsoleKey.D3,"Cash" },
                {ConsoleKey.NumPad3,"Cash" },
                {ConsoleKey.D4,"SWISH" },
                {ConsoleKey.NumPad4,"SWISH" },
                {ConsoleKey.D5,"Klarna" },
                {ConsoleKey.NumPad5,"Klarna" },
                {ConsoleKey.D0,"BACK" },
                {ConsoleKey.NumPad0,"BACK" },
            };
            Screen paymentMethodsScreen = new Screen("GRAPE FRUIT BAR", InputType.buttonInput, 
                                                     paymentMethodsCommandToString.Values.ToArray(), CursorVisibleFalse(), paymentMethodsCommandToString, "PAYMENT_OPTIONS_SCREEN");
                                                     paymentMethodsScreen.text = "1. VISA\n2. Mastercard\n3. Cash\n4. SWISH\n5. Klarna\n0. Go Back";
                                                     paymentMethodsScreen.commandInfo = "Make your choice with the Number Pad";

            Dictionary<ConsoleKey, string> receiptCommandToString = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.LeftArrow ,"LEFT" },
                {ConsoleKey.RightArrow,"RIGHT" },
                {ConsoleKey.D0,"BACK" },
                {ConsoleKey.NumPad0,"BACK" },
            };
            Screen receiptScreen = new Screen("GRAPE FRUIT BAR", InputType.buttonInput,
                                                     receiptCommandToString.Values.ToArray(), CursorVisibleFalse(), receiptCommandToString, "RECEIPT_SCREEN");
            receiptScreen.text2 = Receipt.GetLastReceipt();
            receiptScreen.commandInfo = "\nNavigate old receipts with arrow buttons left and right\nPress 0 to go back";


            //Link the payment screen with the startup screen.
            //Link menu screen with payment screen.

            //Does not have previous screen.
            startScreen.nextScreen = shoppingScreen;
            
            shoppingScreen.previousScreen = startScreen;
            shoppingScreen.nextScreen = paymentScreen;
            
            paymentScreen.previousScreen = shoppingScreen;
            //Has MULTIPLE next screens.

            paymentMethodsScreen.previousScreen = paymentScreen;
            //Does not have next screen.

            receiptScreen.previousScreen = paymentScreen;
            //Does not have next screen.


            List<Screen> screenList = new List<Screen>();
            screenList.Add(startScreen);
            screenList.Add(shoppingScreen);
            screenList.Add(paymentScreen);
            screenList.Add(paymentMethodsScreen);
            screenList.Add(receiptScreen);
            
            //All screens contain eachother.
            foreach (Screen screen in screenList)
            {
                screen.screenList = screenList;
            }

            startScreen.Start();
        }
    }
}
