using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ListaZadan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        List<Task> tasks = new List<Task>();
        List<Step> steps = new List<Step>();
        public MainWindow()
        {
            InitializeComponent();

            tasks.Add(new Task() { Name = "Posprzataj_pokoj", Category = "Dom", Priority = 2, TimeLimit = DateTime.Now });
            tasks.Add(new Task() { Name = "Zmyj_podloge_w_kuchni", Category = "Dom" });
            tasks.Add(new Task() { Name = "Nastaw_pranie", Category = "Dom" });
            tasks.Add(new Task() { Name = "Odrób_lekcje", Category = "Szkoła" });
            steps.Add(new Step() { Name = "to jest maly krok dla czlowieka, ale wielki skok dla ludzkosci" });
            TasksListView.ItemsSource = tasks;
            StepsListView.ItemsSource = steps;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TasksListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
            view.GroupDescriptions.Add(groupDescription);
            
        }

        private void TasksListViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                TasksListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            TasksListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void NewTask_Click(object sender, RoutedEventArgs e)
        {
            EditTask view = new EditTask();
            view.ShowDialog();
            tasks.Add(view.task);
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            EditTask view = new EditTask();
            view.ShowDialog();
        }
    }

    public class Task : INotifyPropertyChanged
    {
        public string name;
        public string category;
        public int priority;
        public DateTime timeLimit;
        public Task()
        {
            Name = "Nowy";
            Category = "Task";
        }
        public string Name {
            get { return name; } 
            set { name = value; onPropertyChanged("TaskDetails"); }
        }

        public string Category
        {
            get { return category; }
            set { category = value; onPropertyChanged("TaskDetails"); }
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; onPropertyChanged("TaskDetails"); }
        }
        public DateTime TimeLimit
        {
            get { return timeLimit; }
            set { timeLimit = value; onPropertyChanged("TaskDetails"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Step
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir) : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
