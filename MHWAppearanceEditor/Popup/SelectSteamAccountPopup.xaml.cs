using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using System.Collections.Generic;
using System.Windows;

namespace MHWAppearanceEditor.Popup
{
    /// <summary>
    /// Interaction logic for SelectSteamAccountPopup.xaml
    /// </summary>
    public partial class SelectSteamAccountPopup : Window
    {
        public RelayCommand SelectUserCommand { get; }

        public bool ManualSelect { get; private set; } = false;

        public SelectSteamAccountPopup()
        {
            InitializeComponent();
            DataContext = this;

            Users = Utility.GetSteamUsersWithMhw();
            SelectUserCommand = new RelayCommand(ConfirmSelectUser, CanConfirmSelectUser);
        }

        public SelectSteamAccountPopup(List<SteamAccount> users) : this()
        {
            Users = users;
        }

        public static readonly DependencyProperty UsersProperty =
            DependencyProperty.Register("Users", typeof(List<SteamAccount>),
                typeof(SelectSteamAccountPopup), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedUserProperty =
            DependencyProperty.Register("SelectedUser", typeof(SteamAccount),
                typeof(SelectSteamAccountPopup), new PropertyMetadata(null));

        public List<SteamAccount> Users
        {
            get { return (List<SteamAccount>)GetValue(UsersProperty); }
            set { SetValue(UsersProperty, value); }
        }

        public SteamAccount SelectedUser
        {
            get { return (SteamAccount)GetValue(SelectedUserProperty); }
            set { SetValue(SelectedUserProperty, value); }
        }

        private void ManualSelect_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            ManualSelect = true;
            Close();
        }

        private bool CanConfirmSelectUser() => SelectedUser != null;
        private void ConfirmSelectUser()
        {
            DialogResult = true;
            Close();
        }
    }
}
