using System.Security.Principal;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BankomatApp
{
    public partial class Form1 : Form
    {
        int bankId=0;
        int bankomatIn=0;
        Bank[] banks = {
            new Bank(1, "Приват банк"),
            new Bank(2, "Ощад банк")
            };
        Account account = new Account();
        int selectedMethod = 0;
        public Form1()
        {
            InitializeComponent();
            panelMenu1.Visible = false;

            Bankomat bankomat = new Bankomat(1, 20000, "Київська 37");
            banks[0].AddBankomat(bankomat);
            bankomat = new Bankomat(1, 0, "Велика Бердичiвська 37");
            banks[0].AddBankomat(bankomat);
            bankomat = new Bankomat(2, 1200, "Чуднiвська 107");
            banks[1].AddBankomat(bankomat);

            foreach (var bank in banks)
            {
                listBoxBanks.Items.Add(bank.GetBankName());
            }

            listBoxBanks.SelectedIndexChanged += ListBoxBanks_SelectedIndexChanged;
            buttonChoose.Click += ButtonChoose_Click;

        }

        bool Autorized = false;
        bool EnteringCardNumber = false;

        int cardNumber = 0;
        int password = 0;

        bool EnteringVmoney = false;
        bool goNext = false;

        int getterNumCard = 0;
        private void buttonNumOK_Click(object sender, EventArgs e)
        {
            if (Autorized == false)
            {
                if (EnteringCardNumber == false)
                {
                    labelMessage.Text = "Введіть номер карти";
                    cardNumber = int.Parse(labelInput.Text);
                    if (cardNumber.ToString().Length == 6)
                    {
                        EnteringCardNumber = true;
                        labelMessage.Text = "Введіть пароль карти";
                        labelInput.Text = "";
                    }
                    else
                    {
                        labelInput.Text = "";
                    }
                }
                else
                {
                    password = int.Parse(labelInput.Text);
                    account = new Account();
                    account.OnError += PrintError;
                    account = account.AutorizationApp(banks,cardNumber, password);
                    if (account != null)
                    {
                        MessageBox.Show($"Вiтаємо {account.GetFullName()}!");
                        Autorized = true;
                        panelMenu1.Visible = true;
                        labelUserName.Text = account.GetFullName();
                    }
                    else
                    {
                        labelMessage.Text = "Введіть номер карти";
                        labelInput.Text = "";
                        EnteringCardNumber = false;
                    }
                }

            }
            if(Autorized == true && selectedMethod != 0)
            {
                switch (selectedMethod) {
                    case 1:
                        {
                            if(EnteringVmoney == true)
                            {
                                labelHelp.Visible = true;
                                int getM = int.Parse(labelInput.Text);
                                bool check = account.GetMoneyApp(banks[bankId].BankomatsList[bankomatIn], getM);
                                if (check==true) {
                                    labelMessage.Text = "Не забудьте забрати ваші гроші";
                                    labelInput.Text = $"-{getM}";
                                }
                                else
                                {
                                    labelMessage.Text = "Не достатньо коштів";
                                    labelInput.Text = "";
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            if (EnteringVmoney == true)
                            {
                                labelHelp.Visible = true;
                                int putM = int.Parse(labelInput.Text);
                                account.PutMoneyApp(banks[bankId].BankomatsList[bankomatIn], putM);

                                labelMessage.Text = "Баланс поповнено на";
                                labelInput.Text = $"{putM}";

                            }
                            break;
                        }
                    case 3:
                        {
                            break;
                        }
                    case 4:
                        {
                            if (EnteringVmoney == false) {
                                getterNumCard = int.Parse(labelInput.Text);
                                labelMessage.Text = "Введіть суму для переказу";
                                labelInput.Text = "";
                                EnteringVmoney = true;
                                break;
                            }
                            if (EnteringVmoney == true)
                            {
                                labelHelp.Visible = true;
                                int sendM = int.Parse(labelInput.Text);
                                bool check = account.ShareMoneyApp(banks, getterNumCard, sendM);
                                if (check == true)
                                {
                                    labelMessage.Text = "Гроші надіслано";
                                    labelInput.Text = $"";
                                }
                                else
                                {
                                    labelMessage.Text = "Помилка";
                                    labelInput.Text = "";
                                    EnteringVmoney = false;
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            selectedMethod = 0;
                            break;
                        }
                }
            }
        }

        private void buttonGetMoney_Click(object sender, EventArgs e)
        {
            selectedMethod = 1;
            panelMenu1.Visible = false;
            if (EnteringVmoney == false)
            {
                labelMessage.Text = "Введіть суму";
                labelInput.Text = "";

                EnteringVmoney = true;
            }
        }

        private void buttonPushMoney_Click(object sender, EventArgs e)
        {
            selectedMethod = 2;
            panelMenu1.Visible = false;
            if (EnteringVmoney == false)
            {
                labelMessage.Text = "Введіть суму на поповнення";
                labelInput.Text = "";
                EnteringVmoney = true;
            }
        }

        private void buttonCheckMoney_Click(object sender, EventArgs e)
        {
            selectedMethod = 3;
            panelMenu1.Visible = false;
            if (EnteringVmoney == false)
            {
                labelHelp.Visible = true;
                labelMessage.Text = "На вашому рахунку";
                labelInput.Text = account.GetCardBalance().ToString();
            }
        }

        private void buttonSendMoney_Click(object sender, EventArgs e)
        {
            selectedMethod = 4;
            panelMenu1.Visible = false;
            if (EnteringVmoney == false)
            {
                labelMessage.Text = "Введіть номер одержувача";
                labelInput.Text = "";
                EnteringVmoney = false;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
     
            if (selectedMethod == 0) {
                Autorized = false;
                panelMenu1.Visible = false;
                selectedMethod = 0;
                labelHelp.Visible = false;
                EnteringVmoney = false;
                EnteringCardNumber = false;

                labelMessage.Text = "Введіть номер карти";
                labelInput.Text = "";
            }
            else
            {
                selectedMethod = 0;
                panelMenu1.Visible = true;
                labelHelp.Visible = false;
                EnteringVmoney = false;
            }
        }

        private void buttonNum1_Click(object sender, EventArgs e)
        {
            labelInput.Text += 1;
        }
        private void buttonNum2_Click(object sender, EventArgs e)
        {
            labelInput.Text += 2;
        }
        private void buttonNum3_Click(object sender, EventArgs e)
        {
            labelInput.Text += 3;
        }
        private void buttonNum4_Click(object sender, EventArgs e)
        {
            labelInput.Text += 4;
        }
        private void buttonNum5_Click(object sender, EventArgs e)
        {
            labelInput.Text += 5;
        }
        private void buttonNum6_Click(object sender, EventArgs e)
        {
            labelInput.Text += 6;
        }
        private void buttonNum7_Click(object sender, EventArgs e)
        {
            labelInput.Text += 7;
        }
        private void buttonNum8_Click(object sender, EventArgs e)
        {
            labelInput.Text += 8;
        }
        private void buttonNum9_Click(object sender, EventArgs e)
        {
            labelInput.Text += 9;
        }
        private void buttonNum0_Click(object sender, EventArgs e)
        {
            labelInput.Text += 0;
        }
        private void buttonNumX_Click(object sender, EventArgs e)
        {
            labelInput.Text = "";
        }



        private void ListBoxBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxBankomats.Items.Clear();
            int selectedIndex = listBoxBanks.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < banks.Length)
            {
                Bank selectedBank = banks[selectedIndex];
                foreach (var bankomat in selectedBank.GetBankomatList())
                {
                    listBoxBankomats.Items.Add(bankomat.GetBankomatAddress());
                }
            }
        }

        private void ButtonChoose_Click(object sender, EventArgs e)
        {
            Bank totalBank;
            Bankomat totalBankomat;
            int selectedBankIndex = listBoxBanks.SelectedIndex;
            bankId = selectedBankIndex;
            int selectedBankomatIndex = listBoxBankomats.SelectedIndex;
            
            if (selectedBankIndex >= 0 && selectedBankIndex < banks.Length &&
                selectedBankomatIndex >= 0 && selectedBankomatIndex < banks[selectedBankIndex].GetBankomatList().Length)
            {                
                totalBank = banks[selectedBankIndex];
                totalBankomat = banks[selectedBankIndex].GetBankomatList()[selectedBankomatIndex];
                buttonRegistration.Visible = true;
                buttonAutorization.Visible = true;
                bankomatIn = selectedBankomatIndex;
                MessageBox.Show($"Вибраний банк: {totalBank.GetBankName()} та банкомат за адресою: {totalBankomat.GetBankomatAddress()}");
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть банк та банкомат.");
            }
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            panelRegistration.Visible = true;
        }

        private void buttonAutorization_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
        }
        string surName;
        string name;
        string lastName;
        string email;
        string acPassword;
        int cardNumberReg=0;
        int cardPasswordReg;

        private void buttonCheckInfo_Click(object sender, EventArgs e)
        {            
            Random rand = new Random();
            cardNumberReg = rand.Next(100000, 1000000);
            labelCardNumber.Text = cardNumberReg.ToString();
        }

        private void buttonRegistrate_Click(object sender, EventArgs e)
        {
            surName = textBoxSurname.Text;
            name = textBoxName.Text;
            lastName = textBoxLastName.Text;
            email = textBoxEmail.Text;
            acPassword = textBoxAcPassword.Text;

            bool f = true;
            f = int.TryParse(textBoxCardPassword.Text, out cardPasswordReg);

            if (f && cardPasswordReg.ToString().Length ==4 && cardNumberReg!=0) {
                Account account = new Account();
                account.OnError += PrintError;
                account.RegistrationApp(surName, name, lastName, email, acPassword, cardNumberReg, cardPasswordReg);

                banks[bankId].AddUser(account);
                MessageBox.Show("Реєстрацiя успiшна!");
                panelRegistration.Visible = false;
            }
            else
            {
                MessageBox.Show("Введіть коректно пароль для карти, або Створіть номер карти");
            }
        }
        static void PrintError(string message)
        {
            MessageBox.Show("Помилка: " + message);
        }

    }
}
