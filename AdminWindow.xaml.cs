using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WpfDemoExam;

namespace WpfDemoExam
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                using (var context = new HotelManagementEntities1())
                {
                    var users = await context.Users.ToListAsync();
                    UsersListBox.Items.Clear();

                    foreach (var user in users)
                    {
                        var userItem = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5)
                        };

                        var textBlock = new TextBlock
                        {
                            Text = $"{user.lastname}, {user.firstname} ({user.username}) - {user.email} — ",
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        var checkBox = new CheckBox
                        {
                            Content = "Заблокирован",
                            IsChecked = user.isLocked,
                            Tag = user.id,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        checkBox.Click += LockUserCheckbox_Click;

                        userItem.Children.Add(textBlock);
                        userItem.Children.Add(checkBox);
                        UsersListBox.Items.Add(userItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAddEmployeeWindow(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
            LoadUsers();
        }

        private void OpenChangePasswordWindow(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedItem is StackPanel selectedItem)
            {
                var checkBox = selectedItem.Children.OfType<CheckBox>().FirstOrDefault();
                if (checkBox != null)
                {
                    var userId = (int)checkBox.Tag;
                    var changePassword = new ChangePasswordWindow(userId);
                    changePassword.ShowDialog();
                    LoadUsers();
                    this.Close();
                    return;
                }
            }

            MessageBox.Show("Пожалуйста, выберите пользователя для изменения пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedItem is StackPanel selectedItem)
            {
                var checkBox = selectedItem.Children.OfType<CheckBox>().FirstOrDefault();
                if (checkBox != null)
                {
                    var userId = (int)checkBox.Tag;

                    var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var context = new HotelManagementEntities1())
                            {
                                var user = await context.Users.FirstOrDefaultAsync(u => u.id == userId);
                                if (user != null)
                                {
                                    context.Users.Remove(user);
                                    await context.SaveChangesAsync(); MessageBox.Show("Пользователь успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                    LoadUsers();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    return;
                }
            }

            MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void LockUserCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                var userId = (int)checkBox.Tag;
                bool isLocked = checkBox.IsChecked ?? false;
                await UpdateUserLockStatus(userId, isLocked);
                LoadUsers();
            }
        }

        private async Task UpdateUserLockStatus(int userId, bool isLocked)
        {
            try
            {
                using (var context = new HotelManagementEntities1())
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.id == userId);
                    if (user != null)
                    {
                        user.isLocked = isLocked;
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении статуса блокировки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}



//-- Таблица: Guests
//CREATE TABLE Guests (
//    id INT PRIMARY KEY IDENTITY,
//    first_name NVARCHAR(255) NOT NULL,
//    last_name NVARCHAR(255) NOT NULL,
//    email NVARCHAR(255) NOT NULL,
//    phone NVARCHAR(20) NOT NULL,
//    document_number NVARCHAR(50) NOT NULL,
//    check_in DATE,
//    check_out DATE
//);

//--Таблица: Rooms
//CREATE TABLE Rooms (
//    id INT PRIMARY KEY IDENTITY,
//    floor VARCHAR(255) NOT NULL,
//    number INT NOT NULL,
//    category NVARCHAR(255),
//    status NVARCHAR(50)
//);

//--Таблица: Room_Access
//CREATE TABLE Room_Access (
//    id INT PRIMARY KEY IDENTITY,
//    guest_id INT NOT NULL,
//    room_id INT NOT NULL,
//    access_card_code NVARCHAR(50),
//    status NVARCHAR(50),
//    FOREIGN KEY (guest_id) REFERENCES Guests(id),
//    FOREIGN KEY (room_id) REFERENCES Rooms(id)
//);

//--Таблица: Users
//CREATE TABLE Users (
//    id INT PRIMARY KEY IDENTITY,
//    lastname NVARCHAR(255) NOT NULL,
//    firstname NVARCHAR(255) NOT NULL,
//    username NVARCHAR(255) NOT NULL,
//    role NVARCHAR(50) NOT NULL,
//    email NVARCHAR(255),
//    phone NVARCHAR(20),
//    password NVARCHAR(255) NOT NULL,
//    FailedLoginAttempts INT,
//    isLocked BIT,
//    firstLogin BIT,
//    lastLoginDate DATETIME
//);

//--Таблица: Cleaning_Schedule
//CREATE TABLE Cleaning_Schedule (
//    id INT PRIMARY KEY IDENTITY,
//    room_id INT NOT NULL,
//    cleaning_date DATE,
//    cleaner_id INT,
//    status NVARCHAR(50),
//    FOREIGN KEY (room_id) REFERENCES Rooms(id),
//    FOREIGN KEY (cleaner_id) REFERENCES Users(id)
//);

//--Таблица: Statistics_hotel
//CREATE TABLE Statistics_hotel (
//    id INT PRIMARY KEY IDENTITY,
//    date DATE NOT NULL,
//    occupancy_rate DECIMAL(5,2),
//    adr DECIMAL(10,2),
//    revpar DECIMAL(10,2),
//    revenue DECIMAL(10,2)
//);

//--Таблица: Integrations
//CREATE TABLE Integrations (
//    id INT PRIMARY KEY IDENTITY,
//    name NVARCHAR(255) NOT NULL,
//    integration_details NVARCHAR(MAX)
//);

//--Таблица: Services
//CREATE TABLE Services (
//    id INT PRIMARY KEY IDENTITY,
//    name NVARCHAR(255) NOT NULL,
//    price DECIMAL(10,2) NOT NULL,
//    description NVARCHAR(MAX)
//);

//--Таблица: Reservations
//CREATE TABLE Reservations (
//    id INT PRIMARY KEY IDENTITY,
//    guest_id INT NOT NULL,
//    room_id INT NOT NULL,
//    check_in_date DATE NOT NULL,
//    check_out_date DATE NOT NULL,
//    status NVARCHAR(50),
//    FOREIGN KEY (guest_id) REFERENCES Guests(id),
//    FOREIGN KEY (room_id) REFERENCES Rooms(id)
//);

//--Таблица: Payments
//CREATE TABLE Payments (
//    id INT PRIMARY KEY IDENTITY,
//    reservation_id INT NOT NULL,
//    amount DECIMAL(10,2),
//    payment_date DATE NOT NULL,
//    payment_method NVARCHAR(50) NOT NULL,
//    FOREIGN KEY (reservation_id) REFERENCES Reservations(id)
//);

//--Таблица: Guest_Services
//CREATE TABLE Guest_Services (
//    id INT PRIMARY KEY IDENTITY,
//    reservation_id INT NOT NULL,
//    service_id INT NOT NULL,
//    quantity INT NOT NULL,
//    status NVARCHAR(50),
//    FOREIGN KEY (reservation_id) REFERENCES Reservations(id),
//    FOREIGN KEY (service_id) REFERENCES Services(id)
//);