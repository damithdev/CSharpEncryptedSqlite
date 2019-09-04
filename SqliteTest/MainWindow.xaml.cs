using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SqliteTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            var res = SQLiteHandler.Instance.CreateDatabase(baseDir + "testDB.sqlite", "testpassword");
            SetItemSourceUpdate();
        }

        private ObservableCollection<User> usersCollection = new ObservableCollection<User>();

        private void SetItemSourceUpdate()
        {
            try
            {
                dgUsers.ItemsSource = usersCollection;
                var resQuery = SQLiteHandler.Instance.LoadUsers();
                if (resQuery != null && resQuery.Count > 0)
                {
                    usersCollection.Clear();
                    foreach(var u in resQuery)
                    {
                        usersCollection.Add(u);
                    }
                    
                }
                
            }
            catch (Exception)
            {
                
            }

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text.Length > 3)
            {
                var resSave = SQLiteHandler.Instance.SaveUser(new User() { Name = txtName.Text });
                if(resSave)
                {
                    MessageBox.Show("Save Success");
                    SetItemSourceUpdate();
                }
            }
            else
            {
                MessageBox.Show("Proper Name Please");
            }

        }

        private void TestFunction()
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            var res = SQLiteHandler.Instance.CreateDatabase(baseDir + "testDB.sqlite", "testpassword");

            if (res)
            {
                var resSave = SQLiteHandler.Instance.SaveUser(new User() { Name = "Ruwan" });

                if (resSave)
                {
                    var resQuery = SQLiteHandler.Instance.LoadUsers();
                    if (resQuery != null && resQuery.Count > 0)
                    {
                        var resDelete = SQLiteHandler.Instance.DeleteUser(resQuery[0].Id);
                        if (resDelete)
                        {
                            var resQuery2 = SQLiteHandler.Instance.LoadUsers();
                            if (resQuery2 != null && resQuery2.Count == resQuery.Count - 1)
                            {
                                MessageBox.Show("All Success");
                            }
                            else
                            {
                                MessageBox.Show("Final Query Failed");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Delete Failed");
                        }
                    }
                    else
                    {
                        MessageBox.Show("User Load Success");
                    }
                }
                else
                {
                    MessageBox.Show("Save Failed");
                }

            }
            else
            {
                MessageBox.Show("Failed");

            }
        }
    }
}
