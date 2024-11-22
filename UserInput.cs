using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grape_Fruit_Bar
{
    //This class plays the role of an user interface. It is connected to the screen as well as to the cash register.
    //For some reason this user interface can only be used with cash registers.
    internal class UserInput
    {
        public InputType inputType { get; set; }
        public Register register { get; set; }
        public Screen screen { get; set; }
        public UserInput(InputType inputType,Screen screen)
        {
            this.inputType = inputType;
            this.screen = screen;
            this.register = new Register();
        }

        public bool IsCommandValid(string userInput)
        {
            if (this.screen.validActions.Contains(userInput))
            {
                return true;
            }

            //If not, we have a search or an add item command.
            try 
            {
                string[] commandArray = userInput.Split(" ");
                int[] productIDs = Register.ProductDict.Keys.ToArray();

                int productID = int.Parse(commandArray[0]);
                int productCount = int.Parse(commandArray[1]);

                if ( productIDs.Contains(productID) && productCount<999 ){

                    return true;
                }
                else
                {
                    Console.WriteLine("Please use an amount <999 and a product ID from the list below:");
                    foreach (var pID in Register.ProductDict.Keys.ToArray())
                    {
                        Console.WriteLine($"{pID}");
                    }
                    Console.ReadLine();
                    return false;
                }
            }
            catch
            {
                if(this.screen.screenID != "MENU_SCREEN")
                {
                    Console.WriteLine("Wrong command");
                }
                //This means we have a search.
                return false;
            }
        }

        public string HandleUserInput2(string userInput)
        {
            bool isCommandValid = IsCommandValid(userInput);

            string registerAnswer = register.Execute(userInput, isCommandValid);

            bool isStringASearch = registerAnswer.StartsWith("SEARCH RESULTS:");
            
            return registerAnswer;

        }

        public string HandleUserInput()
        {
            if (this.inputType == InputType.writeLine)
            {
                string userInput = Console.ReadLine()!;
                return this.HandleUserInput2(userInput);
            }
            else if (this.inputType == InputType.buttonInput)
            {
                var pressedKey = Console.ReadKey(true);
                string userInput = screen.Button2String(pressedKey.Key);
                return HandleUserInput2(userInput);
            }
            else
            {
                throw (new Exception("NonExistentInputType"));   
            }
            
        }
    }
}
