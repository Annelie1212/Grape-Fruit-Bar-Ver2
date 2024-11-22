using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Grape_Fruit_Bar
{
    class Receipt
    {
        public string Title { get; set; } = "";
        public string CoName { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string PhoneNr { get; set; } = "";
        public Dictionary<string, string> CashRegInfo = new Dictionary<string, string>
        {
            {"Cashier:","" },
            {"RegisterNr:","" },
            {"SerialNr:","" }
        };
        public Dictionary<string, string> DateAndTime = new Dictionary<string, string>
        {
            {"Date:","" },
            {"Time:","" }
        };
        public Dictionary<int, int> ShoppingCart = new Dictionary<int, int>();

        public Dictionary<int, Product> ProductDict = new Dictionary<int, Product>();

        public List<List<string>> CartList = new List<List<string>>();

        public string[] SumRow = new string[2];

        public string[] TaxType = { "VAT", "Amount", "Net", "Gross" };
        public float[] TaxNr = { 0.00f, 0.00f, 0.00f, 0.00f };
        
        public string Greeting = "";

        public string LineBreak = "Thank you! Please come again!";

        public int receiptRowLength = 40;
        
        public Receipt(string Title,string CoName,string Address1,string Address2,string PhoneNr, Dictionary<int,int> ShoppingCart,
                        Dictionary<int,Product> ProductDict, string[] SumRow, string Greeting, string LineBreak,Register register)
        {
            this.Title = Title;
            this.CoName = CoName;
            this.Address1 = Address1;
            this.Address2 = Address2;
            this.PhoneNr = PhoneNr;
            this.ShoppingCart = ShoppingCart;
            this.ProductDict = ProductDict;
            this.SumRow = SumRow;
            this.Greeting = Greeting;
            this.LineBreak = LineBreak;
            this.register = register;
        }

        public Register register { get; set; }

        public static List<string> GetAllReceipts()
        {
            string filePath = "Files/AllReceipts.txt";
            List<string> receipts = new List<string>();
            try
            {
                using (StreamReader reader1 = new StreamReader("..//..//..//" + filePath))
                {
                    int rowNr = 0;
                    string[][] allReceipts = new string[3][];
                    string line = "";

                    string receipt = "";
                    while (reader1.Peek() >= 0)
                    {
                        rowNr++;

                        try
                        {
                            line = reader1.ReadLine();
                            receipt += "\n" + line;
                            if (line.StartsWith("__________"))
                            {
                                
                                receipts.Add(receipt);
                                receipt = "";
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Problems occured when reading from file!");
                        }
                    }

                }
            }
            catch
            {
                //Receipts not read.
            }
            
            return receipts;
        }
        public static string GetLastReceipt()
        {
            try
            {
                return "RECEIPT:\n" + GetAllReceipts().Last();
            }
            catch
            {
                return "";
            }


        }

        public static string GetOneReceipt(int nr)
        {
            return "RECEIPT:\n" + GetAllReceipts()[nr];
        }

        public void UpdateSerialNr()
        {
            string filePath = "Files/AllReceipts.txt";
            int serialNr;
            using (StreamReader reader1 = new StreamReader("..//..//..//" + filePath))
            {
                int rowNr = 0;
                string[][] allReceipts = new string[3][];
                string line = "";
                
                while (reader1.Peek() >= 0)
                {
                    rowNr++;
                    
                    try
                    {
                        line = reader1.ReadLine();
                        char[] charArray = line.ToCharArray();
                        if (charArray.Contains(':'))
                        {
                            string[] listArr = line.Split(":");
                            int stringLen = listArr[2].Length;
                            string result = listArr[2].Substring(stringLen-8);
                            //Console.WriteLine(result);
                            if ("SerialNr" == result)
                            {
                                serialNr = int.Parse(listArr[3]);
                                CashRegInfo["SerialNr:"] = Convert.ToString(serialNr + 1);
                            }
                        }
                        else
                        {
                            //Do not update serial.
                        }
 
                    }
                    catch
                    {
                        //Erroneus data filtered.

                    }
                }
            }
            
        }

        public string[] GetReceiptText(List<List<string>> shoppingMatrix)
        {
            string[] receiptText = new string[17];

            receiptText[0] = StringFormatter.CentrateText(Title,receiptRowLength,' ');
            receiptText[1] = StringFormatter.CentrateText(CoName, receiptRowLength, ' ');
            receiptText[2] = StringFormatter.CentrateText(Address1, receiptRowLength, ' ');
            receiptText[3] = StringFormatter.CentrateText(Address2, receiptRowLength, ' ');
            receiptText[4] = StringFormatter.CentrateText(PhoneNr, receiptRowLength, ' ');

            receiptText[5] = "";

            string[] keysArray = CashRegInfo.Keys.ToArray();
            string[] CashRegInfoArr = { keysArray[0]+CashRegInfo[keysArray[0]] , keysArray[1] + CashRegInfo[keysArray[1]], keysArray[2] + CashRegInfo[keysArray[2]] };
            receiptText[6] = StringFormatter.AlignText(CashRegInfoArr, receiptRowLength, ' ', "LEFTRIGHT",null);

            DateAndTime["Date:"] = DateTime.Now.ToString("yyyy-MM-dd");
            DateAndTime["Time:"] = DateTime.Now.ToString("HH:mm:ss");
            string[] dtArray = DateAndTime.Keys.ToArray();
            string[] DateAndTimeArr = { dtArray[0] + DateAndTime[dtArray[0]], dtArray[1] + DateAndTime[dtArray[1]] };
            receiptText[7] = StringFormatter.AlignText(DateAndTimeArr, receiptRowLength, ' ', "LEFTRIGHT",null);

            receiptText[8] = StringFormatter.CentrateText(GeneralMethods.RepeatString("-", receiptRowLength), receiptRowLength, ' ');

            //The puts an extra linebreak after the sum is printed.
            string shoppingCartString = "";
            foreach (List<string> item in shoppingMatrix)
            {
                if (item.Count == 7)
                {
                    shoppingCartString += StringFormatter.AlignText(item.ToArray(), receiptRowLength, ' ', new string[] { "LEFT", "RIGHT", "LEFT", "RIGHT", "RIGHT", "LEFT", "RIGHT" }, new int[] { 15, 3, 1, 7, 6, 1, 7 }) + "\n";
                }
                else
                {
                    shoppingCartString += StringFormatter.AlignText(item.ToArray(), receiptRowLength, ' ', "LEFTRIGHT", null) + "\n";
                }
                
            }
            receiptText[9] = shoppingCartString;

            double VAT = 25;
            double amount = Convert.ToDouble(shoppingMatrix.Last().Last())*VAT/100;
            double net = Convert.ToDouble(shoppingMatrix.Last().Last());
            double gross = net * (1 + VAT / 100);
            receiptText[10] = StringFormatter.AlignText(new string[]{ "VAT","Amount","Net","Gross"}, receiptRowLength, ' ', "LEFTRIGHT",null);
            receiptText[11] = StringFormatter.AlignText(new string[] { VAT.ToString("F0"),amount.ToString("F2"),net.ToString("F2"), gross.ToString("F2") }, receiptRowLength, ' ', "LEFTRIGHT",null);

            receiptText[12] = StringFormatter.AlignText(new string[] { "Payment Method:", Register.PaymentMethod }, receiptRowLength, ' ', "LEFTRIGHT", null);

            receiptText[13] = "";

            receiptText[14] = StringFormatter.CentrateText(this.Greeting,receiptRowLength,' ');

            receiptText[15] = "\n";

            receiptText[16] = StringFormatter.CentrateText(GeneralMethods.RepeatString("_", 40),receiptRowLength,' ');

            return receiptText;
        }
        public string[][] GetCartAsArray()
        {
            string[][] cartArray = new string[CartList.Count][];
            string[] tempArray = { };
            int i = 0;
            foreach(List<string> l in this.CartList)
            {
                tempArray = l.ToArray();
                cartArray[i] = tempArray;
                i++;
            }
            return cartArray;
        }

        public void UpdateRegisterInfo()
        {
            CashRegInfo["Cashier:"] = "1";
            CashRegInfo["RegisterNr:"] = "1";
            CashRegInfo["SerialNr:"] = "1";

        }


    }
}
