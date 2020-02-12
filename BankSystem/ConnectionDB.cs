using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using AS.IBAN.Model;
using System.Data.Common;
using System.Data;

namespace BankSystem
{
    public class ConnectionDB
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankSystem"].ConnectionString);
        String constring = ConfigurationManager.ConnectionStrings["BankSystem"].ToString();
        int ret = 0;

        public int InsertAccount(string code, string firstname, string surname
            , string phone, string SSN, DateTime date, decimal amount, string Username, string PIN)
        {

            // create and open a connection object
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_InsertAccount", connection);

                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramIBAN = new SqlParameter();
                paramIBAN.ParameterName = "@code";
                paramIBAN.Value = code;
                cmd.Parameters.Add(paramIBAN);

                SqlParameter paramFirst = new SqlParameter();
                paramFirst.ParameterName = "@firstname";
                paramFirst.Value = firstname;
                cmd.Parameters.Add(paramFirst);

                SqlParameter paramSurname = new SqlParameter();
                paramSurname.ParameterName = "@surname";
                paramSurname.Value = surname;
                cmd.Parameters.Add(paramSurname);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@phone";
                paramPhone.Value = phone;
                cmd.Parameters.Add(paramPhone);

                SqlParameter paramSSN = new SqlParameter();
                paramSSN.ParameterName = "@SSN";
                paramSSN.Value = SSN;
                cmd.Parameters.Add(paramSSN);

                SqlParameter paramDate = new SqlParameter();
                paramDate.ParameterName = "@date";
                paramDate.Value = date;
                cmd.Parameters.Add(paramDate);

                SqlParameter paramAmount = new SqlParameter();
                paramAmount.ParameterName = "@amount";
                paramAmount.Value = amount;
                cmd.Parameters.Add(paramAmount);

                SqlParameter paramUsername = new SqlParameter();
                paramUsername.ParameterName = "@Username";
                paramUsername.Value = Username;
                cmd.Parameters.Add(paramUsername);

                SqlParameter paramPIN = new SqlParameter();
                paramPIN.ParameterName = "@PIN";
                paramPIN.Value = PIN;
                cmd.Parameters.Add(paramPIN);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }

        public int UpdateAccount(string code, string firstname, string surname
            , string phone,  string PIN)
        {

            // create and open a connection object
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateAccount", connection);

                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramIBAN = new SqlParameter();
                paramIBAN.ParameterName = "@code";
                paramIBAN.Value = code;
                cmd.Parameters.Add(paramIBAN);

                SqlParameter paramFirst = new SqlParameter();
                paramFirst.ParameterName = "@firstname";
                paramFirst.Value = firstname;
                cmd.Parameters.Add(paramFirst);

                SqlParameter paramSurname = new SqlParameter();
                paramSurname.ParameterName = "@surname";
                paramSurname.Value = surname;
                cmd.Parameters.Add(paramSurname);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@phone";
                paramPhone.Value = phone;
                cmd.Parameters.Add(paramPhone);               

                SqlParameter paramPIN = new SqlParameter();
                paramPIN.ParameterName = "@PIN";
                paramPIN.Value = PIN;
                cmd.Parameters.Add(paramPIN);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }

        public List<Account_IBAN> SelectAccount(string code)
        {
            Account_IBAN ac = new Account_IBAN();
            List<Account_IBAN> acList = new List<Account_IBAN>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_SelectAccount", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@code";
                paramName.Value = code;
                cmd.Parameters.Add(paramName);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Account");
                connection.Close();

                if (AccDS.Tables["Account"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Account"].Rows)
                    {
                        ac = new Account_IBAN();
                        ac.Code = row["Code"].ToString();
                        ac.account_id = Convert.ToInt32(row["Account_ID"].ToString());
                        ac.firstname = row["Account_name"].ToString();
                        ac.surname = row["Account_Surname"].ToString();
                        ac.phone = row["Account_Phone"].ToString();
                        ac.ssn = row["Account_SSN"].ToString();
                        ac.amount = Convert.ToDecimal(row["Amount"]);
                        ac.Username = row["Username"].ToString();
                        ac.PIN = row["PIN"].ToString();

                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }

        public List<ListAccount> SelectAccountNotIn(int AccID)
        {
            ListAccount ac = new ListAccount();
            List<ListAccount> acList = new List<ListAccount>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_SelectAccount_ByAnoAccID", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramAccID = new SqlParameter();
                paramAccID.ParameterName = "@AccID";
                paramAccID.Value = AccID;
                cmd.Parameters.Add(paramAccID);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Account");
                connection.Close();

                if (AccDS.Tables["Account"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Account"].Rows)
                    {
                        ac = new ListAccount();
                        ac.Code = row["Code"].ToString();
                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }
       
        public List<ListAccount> SelectAccountAll()
        {
            ListAccount ac = new ListAccount();
            List<ListAccount> acList = new List<ListAccount>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_SelectAccount_All", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Account");
                connection.Close();

                if (AccDS.Tables["Account"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Account"].Rows)
                    {
                        ac = new ListAccount();
                        ac.Code = row["Code"].ToString();
                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }

        public List<ListAccount> SelectAccountByAccID(int AccID)
        {
            ListAccount ac = new ListAccount();
            List<ListAccount> acList = new List<ListAccount>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_SelectAccount_ByAccID", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramAccID = new SqlParameter();
                paramAccID.ParameterName = "@AccID";
                paramAccID.Value = AccID;
                cmd.Parameters.Add(paramAccID);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Account");
                connection.Close();

                if (AccDS.Tables["Account"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Account"].Rows)
                    {
                        ac = new ListAccount();
                        ac.Code = row["Code"].ToString();
                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }


        public int UpdateAmount(string code, decimal amount)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateAmount", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramCode = new SqlParameter();
                paramCode.ParameterName = "@code";
                paramCode.Value = code;
                cmd.Parameters.Add(paramCode);

                SqlParameter paramAmount = new SqlParameter();
                paramAmount.ParameterName = "@amount";
                paramAmount.Value = amount;
                cmd.Parameters.Add(paramAmount);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }

        public int InsertDeposit(string code, decimal amount, decimal dfee
            , decimal dAmount, DateTime date, string by)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_InsertDeposit", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramCode = new SqlParameter();
                paramCode.ParameterName = "@code";
                paramCode.Value = code;
                cmd.Parameters.Add(paramCode);

                SqlParameter paramAmount = new SqlParameter();
                paramAmount.ParameterName = "@amount";
                paramAmount.Value = amount;
                cmd.Parameters.Add(paramAmount);

                SqlParameter paramDepFee = new SqlParameter();
                paramDepFee.ParameterName = "@dep_fee";
                paramDepFee.Value = dfee;
                cmd.Parameters.Add(paramDepFee);

                SqlParameter paramDepAmount = new SqlParameter();
                paramDepAmount.ParameterName = "@dep_Amount";
                paramDepAmount.Value = dAmount;
                cmd.Parameters.Add(paramDepAmount);

                SqlParameter paramDate = new SqlParameter();
                paramDate.ParameterName = "@date";
                paramDate.Value = date;
                cmd.Parameters.Add(paramDate);

                SqlParameter paramBy = new SqlParameter();
                paramBy.ParameterName = "@by";
                paramBy.Value = by;
                cmd.Parameters.Add(paramBy);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }

        public int InsertTranfer(string fo_Code, string to_Code, decimal Tran_Amount,
            decimal Fo_Amount, decimal To_Amount, DateTime date, string by)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_InsertTranfer", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramFoCode = new SqlParameter();
                paramFoCode.ParameterName = "@Fo_Code";
                paramFoCode.Value = fo_Code;
                cmd.Parameters.Add(paramFoCode);

                SqlParameter paramToCode = new SqlParameter();
                paramToCode.ParameterName = "@To_Code";
                paramToCode.Value = to_Code;
                cmd.Parameters.Add(paramToCode);

                SqlParameter paramTran_Amount = new SqlParameter();
                paramTran_Amount.ParameterName = "@tranAmount";
                paramTran_Amount.Value = Tran_Amount;
                cmd.Parameters.Add(paramTran_Amount);

                SqlParameter paramFo_Amount = new SqlParameter();
                paramFo_Amount.ParameterName = "@FromAmount";
                paramFo_Amount.Value = Fo_Amount;
                cmd.Parameters.Add(paramFo_Amount);

                SqlParameter paramTo_Amount = new SqlParameter();
                paramTo_Amount.ParameterName = "@ToAmount";
                paramTo_Amount.Value = To_Amount;
                cmd.Parameters.Add(paramTo_Amount);

                SqlParameter paramDate = new SqlParameter();
                paramDate.ParameterName = "@date";
                paramDate.Value = date;
                cmd.Parameters.Add(paramDate);

                SqlParameter paramBy = new SqlParameter();
                paramBy.ParameterName = "@by";
                paramBy.Value = by;
                cmd.Parameters.Add(paramBy);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }

        public List<Account> CheckLogin(string Username, string PIN)
        {
            Account ac = new Account();
            List<Account> acList = new List<Account>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM FN_CheckLogin(@Username,@PIN)", connection);
                cmd.CommandType = CommandType.Text;

                SqlParameter paramUsername = new SqlParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = Username;
                cmd.Parameters.Add(paramUsername);

                SqlParameter paramPIN = new SqlParameter();
                paramPIN.ParameterName = "@PIN";
                paramPIN.Value = PIN;
                cmd.Parameters.Add(paramPIN);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Account");
                connection.Close();

                if (AccDS.Tables["Account"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Account"].Rows)
                    {
                        ac = new Account();
                        ac.Account_ID = Convert.ToInt32(row["Account_ID"].ToString());
                        ac.Account_name = row["Account_name"].ToString();
                        ac.Account_Surname = row["Account_Surname"].ToString();
                        ac.Account_Phone = row["Account_Phone"].ToString();
                        ac.Account_SSN = row["Account_SSN"].ToString();
                        ac.Account_Type = row["Account_Type"].ToString();
                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }

        public List<LoginLog> Login(int Account_ID, DateTime Login_Date)
        {
            LoginLog ac = new LoginLog();
            List<LoginLog> acList = new List<LoginLog>();
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_Login", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramAccID = new SqlParameter();
                paramAccID.ParameterName = "@AccID";
                paramAccID.Value = Account_ID;
                cmd.Parameters.Add(paramAccID);

                SqlParameter paramDate = new SqlParameter();
                paramDate.ParameterName = "@date";
                paramDate.Value = Login_Date;
                cmd.Parameters.Add(paramDate);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet AccDS = new DataSet();
                adapt.Fill(AccDS, "Login");
                connection.Close();

                if (AccDS.Tables["Login"].Rows.Count > 0)
                {
                    foreach (DataRow row in AccDS.Tables["Login"].Rows)
                    {
                        ac = new LoginLog();
                        ac.Log_ID = Convert.ToInt32(row["Log_ID"].ToString());
                        acList.Add(ac);

                    }
                }
            }
            return acList;
        }

        public int Logout(int Account_ID, DateTime Logout_Date)
        {
            using (SqlConnection connection = new SqlConnection(constring))
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_Logout", connection);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramAccID = new SqlParameter();
                paramAccID.ParameterName = "@AccID";
                paramAccID.Value = Account_ID;
                cmd.Parameters.Add(paramAccID);

                SqlParameter paramDate = new SqlParameter();
                paramDate.ParameterName = "@date";
                paramDate.Value = Logout_Date;
                cmd.Parameters.Add(paramDate);

                ret = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return ret;
        }
    }
}