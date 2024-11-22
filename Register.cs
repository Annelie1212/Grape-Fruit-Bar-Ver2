using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Grape_Fruit_Bar
{
    internal class Register
    {
        static public Dictionary<int, int> ShoppingCart = new Dictionary<int, int>();
        public static List<Product> ProductList { get; set; } = new List<Product> { };

        public static Dictionary<int, Product> ProductDict = new Dictionary<int, Product>();

        public static Dictionary<int, string> PaymentMethods = new Dictionary<int, string>
            {
                {1,"VISA" },
                {2,"Mastercard" },
                {3,"Cash" },
                {4,"SWISH" },
                {5,"Klarna" }
            };

        public static string PaymentMethod = null;

        public int receiptNr = 0;
        public Register() 
        {
            this.ImportProductList();
            this.InitiateShoppingCart();
            this.BuildProductDict();
            this.receiptNr = Receipt.GetAllReceipts().Count-1;
        }

        public List<Product> Search(string searchWord)
        {
            List<Product> searchList = new List<Product>();
            foreach (var item in ProductList)
            {
                bool foundWord = item.Name.StartsWith(searchWord.ToLower());
                if (foundWord)
                {
                    searchList.Add(item);
                    //TODO sort list alphabet or nr.
                }  
            }
            return searchList;
        }

        public string GetSearchStringProducts(List<Product> productList)
        {
            if (productList.Count == 0)
            {
                string searchList = "SEARCH RESULTS: 0";
                searchList += "\n" + GeneralMethods.RepeatString("-", 50);
                return searchList;
            }
            else
            {
                string searchList = "SEARCH RESULTS:";
                foreach (var item in productList)
                {
                    string[] row = { item.Id.ToString(), item.Name, item.Price.ToString(), PriceType2String(item.PriceType) };
                    string rowStr = StringFormatter.AlignText(row, 50, ' ', "LEFT", null);
                    searchList += "\n" + rowStr;
                }
                searchList += "\n" + GeneralMethods.RepeatString("-", 50);
                return searchList;
            }

        }

        //We can pretend that the register is a server that sends back a JSON.
        public string Execute(string command,bool isCommandValid)
        {
            //Searches for products if the user command is invalid.
            List<Product> foundProducts = new List<Product>();
            if (!isCommandValid)
            {
                foundProducts = this.Search(command);
            }


            if (command == "PAY" && isCommandValid)
            {
                if (Register.PaymentMethod == null)
                {
                    Console.WriteLine("Please select a payment method!");
                    Console.ReadLine();
                    return "STAY";
                }
                else
                {
                    this.PAY();
                    return Screen.action2Screen[command];
                }
            }
            //Sets the payment option.
            else if (PaymentMethods.Values.ToArray().Contains(command) && isCommandValid)
            {
                PaymentMethod = command;
                return Screen.action2Screen[command];
            }
            else if(command == "PAYMENT_OPTIONS" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if(command == "NEXT" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if(command== "STAY" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if(command == "BACK" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if(command == "QUIT" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if(command == "NEW_CUSTOMER" && isCommandValid)
            {
                this.InitiateShoppingCart();
                PaymentMethod = null;
                return Screen.action2Screen[command];
            }
            else if (command == "RECEIPT" && isCommandValid)
            {
                return Screen.action2Screen[command];
            }
            else if (command == "LEFT" && isCommandValid)
            {
                receiptNr = receiptNr - 1;
                int modNr = Receipt.GetAllReceipts().Count;
                if (modNr == 0)
                {
                    return "STAY";
                }
                else
                {
                    receiptNr = GeneralMethods.Mod(receiptNr, modNr);
                    return Receipt.GetOneReceipt(this.receiptNr);
                }
                
            }
            else if (command == "RIGHT" && isCommandValid)
            {
                receiptNr = receiptNr + 1;
                int modNr = Receipt.GetAllReceipts().Count;
                if (modNr == 0)
                {
                    return "STAY";
                }
                else
                {
                    receiptNr = GeneralMethods.Mod(receiptNr, modNr);
                    return Receipt.GetOneReceipt(this.receiptNr);
                }
            }
            else if(command == "INVALID")
            {
                return Screen.action2Screen[command];
            }
            else if (!isCommandValid && foundProducts.Count>0)
            { 
                string searchStringProducts = this.GetSearchStringProducts(foundProducts);
                return searchStringProducts;
            }
            else if (!isCommandValid && foundProducts.Count == 0)
            {
                string searchStringProducts = this.GetSearchStringProducts(foundProducts);
                return searchStringProducts;
            }
            //Add products command.
            else if(isCommandValid)
            {
                string cartProducts = this.AddProducts(command);
                return cartProducts;
            }
            else
            {
                return "STAY";
            }
        }

        public void PAY()
        {
            //Creates a receipt for the customer.
            Receipt receipt1 = new Receipt("GRAPE FRUIT BAR\n", "Grape Fruit Bar", "Address: Beachstreet 10", "12 123 SunsetCity", "+81 70-11 111 111",
                                           ShoppingCart, ProductDict, [""], "Thank you! Please come again!", "",this);

            //Update the cashier name,register number and receipt number in the receipt object.
            receipt1.UpdateRegisterInfo();

            try { receipt1.UpdateSerialNr(); }
            catch { receipt1.CashRegInfo["SerialNr:"] = "0"; }

            //Receives the receipt text as a string array.
            string[] receiptTextArr = receipt1.GetReceiptText( GetShoppingCartAsMatrix() );

            //Creates a digital receipt by printing it to a file.
            using (StreamWriter sw = new StreamWriter(Path.Combine("..//..//..//" + "Files/", "AllReceipts.txt"), true))
            { foreach (string l in receiptTextArr) { if (l != null) { sw.WriteLine(l); } } }

        }
        public string AddProducts(string userInput)
        {
            try
            {
                string[] userInputArr = userInput.Split(" ");
                int productId = int.Parse(userInputArr[0]);
                int productCount = int.Parse(userInputArr[1]);
                
                ShoppingCart[productId] += productCount;
                
                if (ShoppingCart[productId] < 0)
                {
                    Console.WriteLine("Your amount is too low!");
                    Console.ReadLine();
                    ShoppingCart[productId] -= productCount;
                }

                return GetShoppingCartAsString();
            }
            catch 
            {
                
                Console.WriteLine("Product not found!");
                Console.ReadLine();
                return "STAY";
            }

            

        }

        public string GetShoppingCartAsString()
        {
            string shoppingList = "";

            double totalSum = 0;
            int[] allKeys = ProductDict.Keys.ToArray();
            for (int i = 0; i < allKeys.Length; i++)
            {
                Product tempProduct = ProductDict[allKeys[i]];
                string productName = tempProduct.Name;
                int productCount = ShoppingCart[tempProduct.Id];
                double productPrice = tempProduct.Price;
                double sum1 = productCount * productPrice;
                PriceType pt = tempProduct.PriceType;
                string productPriceType = PriceType2String(pt);

                if (productCount > 0)
                {
                    string productLine = StringFormatter.AlignText(new string[] { productName, productCount.ToString("F2"), "*", productPrice.ToString("F2"), productPriceType, "=", sum1.ToString("F2") },
                                                            50, ' ', new string[]{"LEFT","RIGHT","LEFT","RIGHT","RIGHT","RIGHT","RIGHT" }, new int[] { 16, 6, 1, 7, 7, 1, 12 });
                    shoppingList += productLine + "\n";

                    totalSum = totalSum + sum1;
                }
            }
            int sumInt = (int)totalSum;
            int sumLength = sumInt.ToString().Length;
            string padding = "".PadRight(21, ' ');
            string lines = "".PadRight(36 + sumLength - 1+14, '-');
            shoppingList += lines + "\n";
            string sumText = $"TOTAL SUM: " + padding + $"{totalSum.ToString("F2")}" + "\n";
            shoppingList += sumText;
            string shopString = shoppingList;
            shopString = "CART ITEMS:\n" + shopString;

            return shopString;
        }

        public List<List<string>> GetShoppingCartAsMatrix()
        {
            List<List<string>> shoppingMat = new List<List<string>>();

            double totalSum = 0;
            int[] allKeys = ProductDict.Keys.ToArray();
            for (int i = 0; i < allKeys.Length; i++)
            {
                Product tempProduct = ProductDict[allKeys[i]];
                string productName = tempProduct.Name;
                int productCount = ShoppingCart[tempProduct.Id];
                double productPrice = tempProduct.Price;
                double sum1 = productCount * productPrice;
                PriceType pt = tempProduct.PriceType;
                string productPriceType = PriceType2String(pt);

                if (productCount > 0)
                {
                    List<string> row = new List<string>() { productName, productCount.ToString(),"*", productPrice.ToString("F2"),productPriceType,"=",sum1.ToString("F2") };
                    shoppingMat.Add(row);

                    totalSum = totalSum + sum1;
                }
            }
            int sumInt = (int)totalSum;
            int sumLength = sumInt.ToString().Length;
            string padding = "".PadRight(21, ' ');
            string lines = "".PadRight(50, '-');
            shoppingMat.Add( new List<string>(){ lines } );
            shoppingMat.Add( new List<string>() { "TOTAL SUM: ", totalSum.ToString("F2") });

            return shoppingMat;
        }

        public static string PriceType2String(PriceType pt)
        {
            if ("PerItem" == pt.ToString())
            {
                return "Pcs";
            }
            else if ("PerKilogram" == pt.ToString())
            {
                return "SEK/Kg";
            }
            else
            {
                Console.WriteLine("Faulty conversion!");
                return "";
            }
        }

        public void InitiateShoppingCart()
        {
            foreach (Product product in ProductList)
            {
                int tempID = product.Id;
                ShoppingCart[tempID] = 0;
            }
        }
        private void BuildProductDict()
        {
            foreach (Product product in ProductList)
            {
                ProductDict[product.Id] = product;
            }

        }
        public void ImportProductList()
        {
            string filePath = "Files/product_list.csv";
            string line;
            ProductList.Clear();
            using (StreamReader reader = new StreamReader("..//..//..//" + filePath))
            {
                int rowNr = 0;
                while (reader.Peek() >= 0)
                {

                    rowNr++;
                    try
                    {
                        line = reader.ReadLine();
                        string[] listArr = line.Split(";");
                        Product p = new Product(int.Parse(listArr[0]), listArr[1], double.Parse(listArr[2]),
                                                (PriceType)int.Parse(listArr[3]));
                        ProductList.Add(p);
                    }
                    catch
                    {
                        //Erroneus data filtered.
                    }
                }
            }
            foreach (Product p in ProductList)
            {
                //Console.WriteLine(p.Name);
            }

        }
    }
}
