using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AS.IBAN;
using AS.IBAN.Model;

namespace BankSystem
{

    public partial class Bank : Form
    {

        decimal DepositFee = Convert.ToDecimal(ConfigurationManager.AppSettings["DepositFee"]);
        int Result = 0;
        Account Acc = new Account();
        List<Account> acList = new List<Account>();
        List<LoginLog> accLogin = new List<LoginLog>();

        decimal Tran_Amount_From, Tran_Amount_To;
        public Bank()
        {
            InitializeComponent();


        }

        #region Login& Logout
        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (Log_txt_Username.Text == string.Empty || Log_txt_Username.Text == "")
            {
                MessageBox.Show("Please, Input Username");
                Log_txt_Username.Focus();

            }
            else if (Log_txt_PIN.Text == string.Empty || Log_txt_PIN.Text == "")
            {
                MessageBox.Show("Please, Input PIN ");
                Log_txt_PIN.Focus();

            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();

                acList = connectionDB.CheckLogin(Log_txt_Username.Text, Log_txt_PIN.Text);

                if (acList.Count > 0)
                {
                    accLogin = connectionDB.Login(acList[0].Account_ID, DateTime.Now);
                    if (accLogin.Count > 0)
                    {
                        Acc = acList.FirstOrDefault();
                        if (Acc.Account_Type != "S")
                        {
                            Log_txt_PIN.Clear();
                            Log_txt_Username.Clear();

                            btn_C_CreateAccount.Hide();
                            btn_C_UpdateAccount.Hide();
                            pn_Login.Hide();
                            pn_CreateAccount.Hide();
                            pn_Deposit.Hide();
                            pn_Tranfer.Hide();

                        }
                        else
                        {
                            Log_txt_PIN.Clear();
                            Log_txt_Username.Clear();

                            btn_C_CreateAccount.Show();
                            btn_C_UpdateAccount.Show();
                            pn_Login.Hide();
                            pn_CreateAccount.Hide();
                            pn_Deposit.Hide();
                            pn_Tranfer.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cann't Login Please, Check Username and Password.");
                    }
                }
                else
                {
                    MessageBox.Show("Username or PIN Incorrect!");
                }
            }
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            ConnectionDB connectionDB = new ConnectionDB();
            if (Acc.Account_ID > 0)
            {
                Result = connectionDB.Logout(Acc.Account_ID, DateTime.Now);
                if (Result > 0)
                {
                    pn_Deposit.Hide();
                    pn_Deposit.SendToBack();
                    pn_CreateAccount.Hide();
                    pn_CreateAccount.SendToBack();
                    pn_Tranfer.Hide();
                    pn_Tranfer.SendToBack();

                    pn_Login.Show();
                    pn_Login.BringToFront();
                }
            }
        }
        #endregion

        #region Create Account

        public void ClearAccount()
        {
            tb_Code.Clear();
            tb_Firstname.Clear();
            tb_Surname.Clear();
            tb_Phone.Clear();
            tb_Surname.Clear();
            tb_SSN.Clear();
            num_Amount.Value = 0;
            txt_PIN.Clear();
            txt_Username.Clear();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            if (tb_Code.Text == string.Empty || tb_Code.Text == "")
            {
                MessageBox.Show("Please, Input IBAN Code");
                tb_Code.Focus();
            }
            else if (tb_Firstname.Text == string.Empty || tb_Firstname.Text == "")
            {
                MessageBox.Show("Please, Input Firstname");
                tb_Firstname.Focus();
            }
            else if (tb_Surname.Text == string.Empty || tb_Surname.Text == "")
            {
                MessageBox.Show("Please, Input Surname");
                tb_Surname.Focus();
            }
            else if (tb_Phone.Text == string.Empty || tb_Phone.Text == "")
            {
                MessageBox.Show("Please, Input Phone");
                tb_Phone.Focus();
            }
            else if (tb_SSN.Text == string.Empty || tb_SSN.Text == "")
            {
                MessageBox.Show("Please, Input CitizenID");
                tb_SSN.Focus();

            }
            else if (num_Amount.Value <= 0)
            {
                MessageBox.Show("Please, Input Amount");
                num_Amount.Focus();

            }
            else if (txt_Username.Text == string.Empty || txt_Username.Text == "")
            {
                MessageBox.Show("Please, Input Username");
                txt_Username.Focus();

            }
            else if (txt_PIN.Text == string.Empty || txt_PIN.Text == "")
            {
                MessageBox.Show("Please, Input PIN ");
                txt_PIN.Focus();

            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();

                Result = connectionDB.InsertAccount(tb_Code.Text, tb_Firstname.Text
                      , tb_Surname.Text, tb_Phone.Text
                      , tb_SSN.Text, DateTime.Now, num_Amount.Value
                      , txt_Username.Text, txt_PIN.Text);
                if (Result >= 0)
                {
                    ClearAccount();
                    MessageBox.Show("Complete");

                }
            }
        }

        private void tb_SSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tb_Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tb_Firstname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void tb_Surname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void txt_Username_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void txt_PIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void bt_GetCode_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://randomiban.com/?country=Netherlands");
        }


        #endregion

        #region Up Account
        public void ClearUpAccount()
        {

            txt_Up_Firstname.Clear();
            txt_Up_Surname.Clear();
            txt_Up_Phone.Clear();
            txt_Up_SSN.Clear();
            txt_Up_Username.Clear();
            txt_Up_PIN.Clear();
            num_Up_Amount.Value = 0;
            cb_Up_Code.Text = string.Empty;
            cb_Up_Code.Items.Clear();

        }

        public void GetAccountForUpdate()
        {
            List<ListAccount> list_Code = new List<ListAccount>();
            ConnectionDB connectionDB = new ConnectionDB();
            if (Acc.Account_Type == "S")
            {
                list_Code = connectionDB.SelectAccountAll();
                foreach (ListAccount l in list_Code)
                {
                    cb_Up_Code.Items.Add(l.Code);
                }
            }
        }

        public void SetAccount()
        {
            Account_IBAN a = new Account_IBAN();
            List<Account_IBAN> ab = new List<Account_IBAN>();
            ConnectionDB connectionDB = new ConnectionDB();
            string Code = cb_Up_Code.SelectedItem.ToString();
            ab = connectionDB.SelectAccount(Code);
            a = ab.FirstOrDefault();
            
            num_Up_Amount.Value = a.amount;
            txt_Up_Firstname.Text = a.firstname;
            txt_Up_Surname.Text = a.surname;
            txt_Up_Phone.Text = a.phone;
            txt_Up_SSN.Text = a.ssn;
            txt_Up_Username.Text = a.Username;
            txt_Up_PIN.Text = a.PIN;
            lbl_Up_AccID.Text = a.account_id.ToString();
        }

        private void cb_Up_Code_SelectedValueChanged(object sender, EventArgs e)
        {
            SetAccount();
        }

        private void txt_Up_Firstname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void txt_Up_Surname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void txt_Up_Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt_Up_PIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btn_UpdateAccount_Click(object sender, EventArgs e)
        {
            if (cb_Up_Code.SelectedIndex < 0)
            {
                MessageBox.Show("Please, Select Code");
            }
            else if (txt_Up_Firstname.Text == string.Empty || txt_Up_Firstname.Text == "")
            {
                MessageBox.Show("Please, Input Firstname");
                txt_Up_Firstname.Focus();
            }
            else if (txt_Up_Surname.Text == string.Empty || txt_Up_Surname.Text == "")
            {
                MessageBox.Show("Please, Input Surname");
                txt_Up_Surname.Focus();
            }
            else if (txt_Up_Phone.Text == string.Empty || txt_Up_Phone.Text == "")
            {
                MessageBox.Show("Please, Input Phone");
                txt_Up_Phone.Focus();
            }
            else if (txt_Up_PIN.Text == string.Empty || txt_Up_PIN.Text == "")
            {
                MessageBox.Show("Please, Input PIN ");
                txt_Up_PIN.Focus();

            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();

                Result = connectionDB.UpdateAccount(cb_Up_Code.SelectedItem.ToString(), txt_Up_Firstname.Text
                      , txt_Up_Surname.Text, txt_Up_Phone.Text,txt_Up_PIN.Text);
                if (Result >= 0)
                {
                    ClearUpAccount();
                    MessageBox.Show("Complete");

                }
            }
        }
        #endregion

        #region Depostion

        public void ClearDeposit()
        {
            Dep_Cb_Code.Items.Clear();
            Dep_Cb_Code.Text = string.Empty;
            Dep_num_Amount.Value = 0;
            Dep_num_Total.Value = 0;
        }

        public void GetCodeDeposit()
        {
            List<ListAccount> list_Code = new List<ListAccount>();
            ConnectionDB connectionDB = new ConnectionDB();
            if (Acc.Account_Type == "S")
            {
                list_Code = connectionDB.SelectAccountAll();
                foreach (ListAccount l in list_Code)
                {
                    Dep_Cb_Code.Items.Add(l.Code);
                }
            }
            else
            {
                list_Code = connectionDB.SelectAccountByAccID(Acc.Account_ID);

                foreach (ListAccount l in list_Code)
                {
                    Dep_Cb_Code.Items.Add(l.Code);
                }
            }
            //return list_Code;
        }

        public decimal CalDepositFee(decimal depFee, decimal amount)
        {
            decimal amountFee;
            amountFee = amount - ((amount * depFee) / 100);
            Dep_lbl_DepF.Hide();
            Dep_lbl_DepF.Text = ((amount * depFee) / 100).ToString();
            return amountFee;
        }

        public void SetDepositByCode()
        {
            List<Account_IBAN> ab = new List<Account_IBAN>();
            ConnectionDB connectionDB = new ConnectionDB();
            string Code = Dep_Cb_Code.SelectedItem.ToString();
            ab = connectionDB.SelectAccount(Code);

            decimal Amount = ab.Select(x => x.amount).FirstOrDefault();
            decimal Deposit = CalDepositFee(DepositFee, Amount);

            Dep_num_Amount.Value = Amount;
            Dep_num_Fee.Value = DepositFee;
            Dep_num_Total.Value = Deposit;
        }

        private void Dep_Cb_Code_SelectedValueChanged(object sender, EventArgs e)
        {
            SetDepositByCode();
        }

        private void Dep_num_Fee_ValueChanged(object sender, EventArgs e)
        {
            decimal Deposit = CalDepositFee(Dep_num_Fee.Value, Dep_num_Amount.Value);
            Dep_num_Total.Value = Deposit;
        }

        private void btn_Deposit_Click(object sender, EventArgs e)
        {
            if (Dep_Cb_Code.SelectedIndex < 0)
            {
                MessageBox.Show("Please, Select IBAN Code");
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                Result = connectionDB.InsertDeposit(Dep_Cb_Code.SelectedItem.ToString()
                    , Dep_num_Amount.Value, Dep_num_Fee.Value, Dep_num_Total.Value
                    , DateTime.Now, Acc.Account_name);
                if (Result >= 0)
                {
                    Result = connectionDB.UpdateAmount(Dep_Cb_Code.SelectedItem.ToString(), Dep_num_Total.Value);
                    if (Result >= 0)
                    {
                        Dep_num_Amount.Value = 0;
                        Dep_num_Fee.Value = 0;
                        Dep_num_Total.Value = 0;
                        Dep_Cb_Code.Text = string.Empty;
                        MessageBox.Show("Complete");
                    }
                }
            }
        }

        #endregion

        #region Tranfer
        public void ClearTranfer()
        {
            Tran_cb_From.Items.Clear();
            Tran_cb_To.Items.Clear();
            Tran_cb_From.Text = string.Empty;
            Tran_cb_To.Text = string.Empty;

        }

        public void GetCodeForm()
        {
            List<ListAccount> list_Code = new List<ListAccount>();
            ConnectionDB connectionDB = new ConnectionDB();
            if (Acc.Account_Type != "S")
            {
                list_Code = connectionDB.SelectAccountByAccID(Acc.Account_ID);
                foreach (ListAccount l in list_Code)
                {
                    Tran_cb_From.Items.Add(l.Code);
                }

            }
            else
            {
                list_Code = connectionDB.SelectAccountAll();
                foreach (ListAccount l in list_Code)
                {
                    Tran_cb_From.Items.Add(l.Code);
                    //Tran_cb_To.Items.Add(l.Code);
                }

            }
        }

        public void GetCodeTo()
        {

            List<ListAccount> list_Code = new List<ListAccount>();
            ConnectionDB connectionDB = new ConnectionDB();
            if (Acc.Account_Type != "S")
            {
                list_Code = connectionDB.SelectAccountNotIn(Acc.Account_ID);
                foreach (ListAccount l in list_Code)
                {
                    Tran_cb_To.Items.Add(l.Code);
                }


            }
            else
            {
                list_Code = connectionDB.SelectAccountAll();
                foreach (ListAccount l in list_Code)
                {
                    //Tran_cb_From.Items.Add(l.Code);
                    Tran_cb_To.Items.Add(l.Code);
                }
            }
        }       

        private void btn_Tranfer_Click(object sender, EventArgs e)
        {
            if (Tran_cb_From.SelectedIndex < 0)
            {
                MessageBox.Show("Please, Select Iban Code");
            }
            else if (Tran_Amount.Value <= 0)
            {
                MessageBox.Show("Please, Input Tranfer Amount");
            }
            else if (Tran_cb_To.SelectedIndex < 0)
            {
                MessageBox.Show("Please, Select Iban Code");
            }
            else if (Tran_cb_From.SelectedItem.ToString() == Tran_cb_To.SelectedItem.ToString())
            {
                MessageBox.Show("Account From and Account To is Duplicate Please, Select difference Account.");
            }
            else
            {
                ConnectionDB connectionDB = new ConnectionDB();
                string CodeFrom = Tran_cb_From.SelectedItem.ToString();
                string CodeTo = Tran_cb_To.SelectedItem.ToString();
                decimal Amount = Tran_Amount.Value;

                List<Account_IBAN> accForm = connectionDB.SelectAccount(CodeFrom);
                List<Account_IBAN> accTo = connectionDB.SelectAccount(CodeTo);

                decimal AccForm_Amount = accForm.Select(x => x.amount).FirstOrDefault();
                decimal AccTo_Amount = accTo.Select(x => x.amount).FirstOrDefault();


                Result = connectionDB.InsertTranfer(CodeFrom, CodeTo
                    , Tran_Amount.Value, Tran_Amount_From, Tran_Amount_To, DateTime.Now, Acc.Account_name);

                if (Result >= 0)
                {
                    Result = connectionDB.UpdateAmount(CodeFrom, AccForm_Amount - Tran_Amount.Value);
                    if (Result >= 0)
                    {
                        Result = connectionDB.UpdateAmount(CodeTo, AccTo_Amount + Tran_Amount.Value);
                        if (Result >= 0)
                        {
                            Tran_Amount.Value = 0;
                            Tran_cb_From.Text = string.Empty;
                            Tran_cb_To.Text = string.Empty;
                            Tran_From.Value = 0;
                            Tran_To.Value = 0;
                            MessageBox.Show("Complete");
                        }
                    }
                }

            }
        }

        private void Tran_cb_From_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Account_IBAN> list_Code = new List<Account_IBAN>();
            ConnectionDB connectionDB = new ConnectionDB();
            string Code = Tran_cb_From.SelectedItem.ToString();
            list_Code = connectionDB.SelectAccount(Code);
            decimal Amount = list_Code.Select(x => x.amount).FirstOrDefault();
            Tran_From.Value = Amount;
            Tran_Amount_From = Amount;
            if (Tran_Amount.Value > 0)
            {
                Tran_From.Value = Amount - Tran_Amount.Value;
            }
        }

        private void Tran_cb_To_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Account_IBAN> list_Code = new List<Account_IBAN>();
            ConnectionDB connectionDB = new ConnectionDB();
            string Code = Tran_cb_To.SelectedItem.ToString();
            list_Code = connectionDB.SelectAccount(Code);
            decimal Amount = list_Code.Select(x => x.amount).FirstOrDefault();
            Tran_To.Value = Amount;
            Tran_Amount_To = Amount;
            if (Tran_Amount.Value > 0)
            {
                Tran_To.Value = Amount + Tran_Amount.Value;
            }
        }

        private void Tran_Amount_ValueChanged(object sender, EventArgs e)
        {

            Tran_From.Value = Tran_Amount_From - Tran_Amount.Value;
            Tran_To.Value = Tran_Amount_To + Tran_Amount.Value;
        }
        #endregion

        #region Button
        private void btn_C_Deposit_Click(object sender, EventArgs e)
        {
            ClearDeposit();
            GetCodeDeposit();

            pn_Login.Hide();
            pn_Login.SendToBack();
            pn_CreateAccount.Hide();
            pn_CreateAccount.SendToBack();
            pn_Tranfer.Hide();
            pn_Tranfer.SendToBack();
            pn_Up_Acc.Hide();
            pn_Up_Acc.SendToBack();

            pn_Deposit.Show();
            pn_Deposit.BringToFront();
        }

        private void btn_C_Tranfer_Click(object sender, EventArgs e)
        {
            ClearTranfer();
            GetCodeForm();
            GetCodeTo();

            pn_Login.Hide();
            pn_Login.SendToBack();
            pn_CreateAccount.Hide();
            pn_CreateAccount.SendToBack();
            pn_Deposit.Hide();
            pn_Deposit.SendToBack();
            pn_Up_Acc.Hide();
            pn_Up_Acc.SendToBack();

            pn_Tranfer.Show();
            pn_Tranfer.BringToFront();
        }

        private void btn_C_MsgAccount_Click(object sender, EventArgs e)
        {
            pn_Login.Hide();
            pn_Login.SendToBack();
            pn_Tranfer.Hide();
            pn_Tranfer.SendToBack();
            pn_Deposit.Hide();
            pn_Deposit.SendToBack();
            pn_Up_Acc.Hide();
            pn_Up_Acc.SendToBack();

            pn_CreateAccount.Show();
            pn_CreateAccount.BringToFront();
        }

        private void btn_C_UpdateAccount_Click(object sender, EventArgs e)
        {
            ClearUpAccount();
            GetAccountForUpdate();
            pn_Login.Hide();
            pn_Login.SendToBack();
            pn_Tranfer.Hide();
            pn_Tranfer.SendToBack();
            pn_Deposit.Hide();
            pn_Deposit.SendToBack();
            pn_CreateAccount.Hide();
            pn_CreateAccount.SendToBack();

            pn_Up_Acc.Show();
            pn_Up_Acc.BringToFront();
        }

        #endregion

        private void Bank_Load(object sender, EventArgs e)
        {
            pn_Login.Show();
            pn_CreateAccount.Hide();
            pn_Deposit.Hide();
            pn_Tranfer.Hide();
            pn_Up_Acc.Hide();
        }

       

        private void Bank_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConnectionDB connectionDB = new ConnectionDB();
            if (acList.Count > 0)
            {
                Result = connectionDB.Logout(acList[0].Account_ID, DateTime.Now);
            }

        }
        
    }
}
