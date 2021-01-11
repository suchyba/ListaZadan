using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ListaZadan
{
    /// <summary>
    /// Logika interakcji dla klasy EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        public Task task { get; private set; }
        public EditTask()
        {
            InitializeComponent();
        }
        public EditTask(Task task)
        {
            InitializeComponent();
            this.task = task;

            TaskName.Text = task.Name;
            Category.Text = task.Category;
            Priority.Value = task.Priority;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(task == null)
            {
                task = new Task();
            }
            task.Name = TaskName.Text;
            task.Category = Category.Text;
            task.Priority = (int)Priority.Value;
            task.TimeLimit = (DateTime)TimeLimit.SelectedDate;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
