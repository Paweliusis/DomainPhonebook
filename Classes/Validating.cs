using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Classes
{
    class Validating
    {
        public static bool ValidateLogIn(string login, SecureString password)
        {
            try
            {


                //if (DBConnection.CheckUsernameInDB(login, Encoding.CreateHashFromSecureString(password))) { return true; }
                //else { return false; }
                if (String.IsNullOrEmpty(login)) { MessageBox.Show("Пожалуйста введите имя пользователя", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error); }
                if (!String.IsNullOrEmpty(login) && password.Length > 0)
                {
                    if (DBConnection.CheckUsernameInDB(login, Encoding.CreateHashFromSecureString(password))) { return true; }
                    else 
                    {
                        MessageBox.Show("Неверно введено имя пользователя или пароль", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false; 
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Что-то пошло не так - {e.Message}", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        // Test
        public static bool ValidateLogInNew(string login, SecureString password)
        {
            try
            {
                if (Classes.DBConnection.SQLiteCheckLoginInDB(login))
                {
                    var passAndSaltUser = DBConnection.SQLiteGetPassAndSalt(login);
                    string salt = passAndSaltUser[1];
                    string hashedAndSalted = Classes.Encoding.HashAndSaltPassword(Classes.Encoding.DecodingSecureString(password), salt);
                    if (passAndSaltUser[0] == hashedAndSalted)
                    {
                        //MessageBox.Show($"Password is correct, DB hashSalt - {passAndSaltUser[0]}, and inserted - {hashedAndSalted}");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show($"Password in incorrect, DB hashSalt - {passAndSaltUser[0]}, and inserted - {hashedAndSalted}");;
                        return false;
                    }
                }
                else { return false; }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Что-то пошло не так - {e.Message}");
                return false;
            }
        }
        // Test
        public static bool ValidateLogIn(string login)
        {
            if (DBConnection.CheckUsernameInDB(login)) { return true; }
            else { return false; }
        }
        public static bool CheckLoginAndPassword(string login, SecureString password)
        {
            if (!String.IsNullOrEmpty(login) && (!(password == null)))
            {
                if (password.Length >= 5)
                {
                    if (!ValidateLogIn(login))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Набранное имя пользователя уже используется, пожалуйста введите другое",
                            "Создание/редактирование аккаунта", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //return false;
                    }
                }
                else
                {
                    MessageBox.Show("Пароль должен превышать 4 знака", "Создание/редактирование аккунта", MessageBoxButton.OK,
                 MessageBoxImage.Error);
                    //return false;
                }
                return false;
            }
            else
            {
                if (String.IsNullOrEmpty(login))
                {
                    MessageBox.Show($"Имя пользователя не может быть пустым, пожалуйста, введите имя пользователя",
                        "Создание/редактирование аккаунта", MessageBoxButton.OK, MessageBoxImage.Error);
                    //return false;
                }
                if (password == null)
                {
                    MessageBox.Show("Пароль не может быть пустым, пожалуйста, введите пароль", "Создание/редактирование аккаунта",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    //return false;
                }
                return false;
            }
        }
        public static bool CheckLogin(string login)
        {
            if (!String.IsNullOrEmpty(login))
            {
                if (!ValidateLogIn(login))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"Набранное имя пользователя уже используется, пожалуйста введите другое",
                        "Создание/редактирование аккаунта", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //return false;
                }
                return false;
            }
            else
            {
                if (String.IsNullOrEmpty(login))
                {
                    MessageBox.Show($"Имя пользователя не может быть пустым, пожалуйста, введите имя пользователя",
                        "Создание/редактирование аккаунта", MessageBoxButton.OK, MessageBoxImage.Error);
                    //return false;
                }
                return false;
            }
        }
        public static bool CheckPassword(SecureString password)
        {
            if (password.Length >= 5)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Пароль должен превышать 4 знака", "Создание/редактирование аккунта", MessageBoxButton.OK,
                 MessageBoxImage.Error);
                return false;
            }
        }
    }
}
