using prac7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace prac7
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        int port;
        string ip;
        string username;
        TcpClient _tcpClient;

        public UserWindow(string ip, int port, string username)
        {
            InitializeComponent();
            this.ip = ip;
            this.port = port;
            this.username = username;

            InitializeClient();
        }

        private async Task InitializeClient()
        {
            _tcpClient = new TcpClient(ip, port, username);
            await _tcpClient.ConnectAsync();
            _tcpClient.MessageReceived += TcpClient_MessageReceived;
            _tcpClient.ClientListReceived += TcpClient_UserListReceived;
            _tcpClient.DisconnectEvent += TcpClient_DisconnectEvent;
            _tcpClient.ReceiveAsync();
        }

        private void TcpClient_MessageReceived(object sender, string message)
        {
            Chat.Items.Add(message);
        }

        private void TcpClient_UserListReceived(object sender, string[] userList)
        {
            UserList.ItemsSource = userList;
        }

        private void TcpClient_DisconnectEvent(object sender)
        {
            Close();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = Messagee.Text.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                await _tcpClient.SendAsync(message);
                Messagee.Clear();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tcpClient.GetConnection())
            {
                _tcpClient.SendAsync("/disconnect");
            }
            else
            {
                TcpClient_DisconnectEvent(sender);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _tcpClient.Stop();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}