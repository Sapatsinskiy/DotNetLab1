using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace BankomatApp
{
    class Bank
    {
        private int bankId;
        private string bankName;
        private Bankomat[] bankomatList;
        private Account[] userList;
        public Bank(int id, string name)
        {
            bankId = id;
            bankName = name;
            bankomatList = new Bankomat[0];
            userList = new Account[0];
        }
        public Bankomat[] BankomatsList { get { return bankomatList; } }
        public string GetBankName()
        {
            return bankName;
        }
        public Bankomat[] GetBankomatList()
        {
            return bankomatList;
        }

        public void AddUser(Account account)
        {
            Array.Resize(ref userList, userList.Length + 1);
            userList[userList.Length - 1] = account;
        }
        public void AddBankomat(Bankomat bankomat)
        {
            Array.Resize(ref bankomatList, bankomatList.Length + 1);
            bankomatList[bankomatList.Length - 1] = bankomat;
        }

        public Account[] GetUserList()
        {
            return userList;
        }
    }

    class Bankomat
    {
        private int bankIdMereza;
        private double bankomatBalance;
        private string bankomatAddress;

        public Bankomat(int bankIdMereza, double bankomatBalance, string bankomatAddress)
        {
            this.bankIdMereza = bankIdMereza;
            this.bankomatBalance = bankomatBalance;
            this.bankomatAddress = bankomatAddress;
        }
        public int GetBankIdMereza()
        {
            return bankIdMereza;
        }
        public int BankomatBalance { get; set; }
        public string GetBankomatAddress()
        {
            return bankomatAddress;
        }
    }
    public delegate void ErrorHandler(string message);

    class Account
    {
        public event ErrorHandler OnError;

        private string surname;
        private string name;
        private string lastName;
        private string email;
        private string acountPassword;
        private int cardNumber;
        private int cardPassword;
        private double cardBalance;

        public Account()
        {
            cardBalance = 0.0;
        }
        public int GetCardNumber()
        {
            return cardNumber;
        }
        public int GetCardPassword()
        {
            return cardPassword;
        }
        public double GetCardBalance()
        {
            return Math.Round(cardBalance, 2);
        }

        public string GetFullName()
        {
            return $"{surname} {name} {lastName}";
        }
        public bool ShareMoneyApp(Bank[] banks, int getter, int sendM)
        {
            for (int i = 0; i < banks.Length; i++)
            {
                Account[] userList = banks[i].GetUserList();

                for (int j = 0; j < userList.Length; j++)
                {
                    if (userList[j].cardNumber == getter)
                    {
                        MessageBox.Show($"Одержувач {userList[j].GetFullName()}");
                        if (sendM <= cardBalance)
                        {
                            userList[j].cardBalance += sendM;
                            cardBalance -= sendM;
                            return true;
                        }
                    }
                }
            }
            OnError?.Invoke("Одержувача не знайдено, або недостатньо коштів.");
            return false;
        }
        public bool GetMoneyApp(Bankomat bk, int getM)
        {         
            if (getM <= cardBalance && getM <= bk.BankomatBalance)
            {
                cardBalance -= getM;
                bk.BankomatBalance -= getM;
                return true;
            }
            else
            {
                OnError?.Invoke("На рахунку, або в банкоматi не достатньо коштiв.");
                return false;
            }           
        }
        public void PutMoneyApp(Bankomat bk, int putM)
        {
            cardBalance += putM;
            bk.BankomatBalance += putM;
        }
        public void RegistrationApp(string surName,string Name,string LastName,string Email,string acPassword,int cardNumberReg,int cardPasswordReg)
        {        
            if (acPassword.Length < 8)
            {
                OnError?.Invoke("Пароль має бути не менше 8 символiв!");                
            }
            else
            {
                surname = surName;
                name = Name;
                lastName = LastName;
                email = Email;
                acountPassword = acPassword;
                cardNumber = cardNumberReg;
                cardPassword = cardPasswordReg;
            }
        }
        public Account AutorizationApp(Bank[] allBanks, int cardNumber, int cardPass)
        {
            for (int i = 0; i < allBanks.Length; i++)
            {

                Account[] userList = allBanks[i].GetUserList();

                for (int j = 0; j < userList.Length; j++)
                {
                    if (userList[j].cardNumber == cardNumber)
                    {
                        if (userList[j].cardPassword == cardPass)
                        {                            
                            return userList[j];
                        }                     
                    }
                }
            }
            OnError?.Invoke("Не вiрний пароль або номер карти!");
            return null;
        }
    }
}