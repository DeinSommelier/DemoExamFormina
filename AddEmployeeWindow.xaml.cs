﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity; 
using System.Linq.Expressions;
namespace WpfDemoExam
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private readonly HotelManagementEntities1 _hotelData;
        public AddEmployeeWindow()
        {
            InitializeComponent();
            _hotelData = new HotelManagementEntities1();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtLogin.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Пожалуйста, заполните обязательные поля (Имя, Фамилия, Логин, Пароль).",
                                    "Недостаточно данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = new Users
                {
                    firstname = txtFirstName.Text.Trim(),
                    lastname = txtLastName.Text.Trim(),
                    username = txtLogin.Text.Trim(),
                    password = txtPassword.Password.Trim(),
                    role = cmbRole.Text.Trim(),
                    FailedLoginAttempts = 0,
                    isLocked = false,
                    firstLogin = true,
                    lastLoginDate = null,
                    position = txtPosition.Text.Trim()
                };

                _hotelData.Users.Add(user);
                _hotelData.SaveChanges();

                MessageBox.Show("Сотрудник успешно добавлен.", "Информация обновлена", MessageBoxButton.OK, MessageBoxImage.Information);

                var adminWindow = new AdminWindow();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}