using System;
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
                        var listBoxItem = new ListBoxItem
                        {
                            Content = $"{user.lastname}, {user.firstname} ({user.username}) - {user.email}",
                            Tag = user.id
                        };

                        UsersListBox.Items.Add(listBoxItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (UsersListBox.SelectedItem is ListBoxItem selectedItem)
            {
                var userId = (int)selectedItem.Tag;
                var changePasswordWindow = new ChangePasswordWindow(userId);
                changePasswordWindow.ShowDialog();
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Выберите пользователя для изменения пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedItem is ListBoxItem selectedItem)
            {
                var userId = (int)selectedItem.Tag;

                var result = MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                                await context.SaveChangesAsync();
                                MessageBox.Show("Пользователь удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadUsers();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Data.Entity;

//namespace WpfDemoExam
//{
//    /// <summary>
//    /// Логика взаимодействия для AdminWindow.xaml
//    /// </summary>
//    public partial class AdminWindow : Window
//    {
//        public AdminWindow()
//        {
//            InitializeComponent();
//            LoadUsers();
//        }

//        private async void LoadUsers()
//        {
//            try
//            {
//                using (var context = new HotelManagementEntities1())
//                {
//                    var users = await context.Users.ToListAsync();
//                    UsersListBox.Items.Clear();

//                    foreach (var user in users)
//                    {
//                        var listBoxItem = new ListBoxItem
//                        {
//                            Content = $"{user.lastname}, {user.firstname} ({user.username}) - {user.email}",
//                            Tag = user.id
//                        };

//                        UsersListBox.Items.Add(listBoxItem);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private void OpenAddEmployeeWindow(object sender, RoutedEventArgs e)
//        {
//            var addEmployeeWindow = new AddEmployeeWindow();
//            addEmployeeWindow.ShowDialog();
//            LoadUsers();
//        }

//        private void OpenChangePasswordWindow(object sender, RoutedEventArgs e)
//        {
//            if (UsersListBox.SelectedItem is ListBoxItem selectedItem)
//            {
//                var userId = (int)selectedItem.Tag;
//                var changePasswordWindow = new ChangePasswordWindow(userId);
//                changePasswordWindow.ShowDialog();
//                LoadUsers();
//            }
//            else
//            {
//                MessageBox.Show("Выберите пользователя для изменения пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (UsersListBox.SelectedItem is ListBoxItem selectedItem)
//            {
//                var userId = (int)selectedItem.Tag;

//                var result = MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
//                if (result == MessageBoxResult.Yes)
//                {
//                    try
//                    {
//                        using (var context = new HotelManagementEntities1())
//                        {
//                            var user = await context.Users.FirstOrDefaultAsync(u => u.id == userId);
//                            if (user != null)
//                            {
//                                context.Users.Remove(user);
//                                await context.SaveChangesAsync();
//                                MessageBox.Show("Пользователь удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
//                                LoadUsers();
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                    }
//                }
//            }
//            else
//            {
//                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }
//    }
//}
