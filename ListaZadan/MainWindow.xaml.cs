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
        public MainWindow()
        {
            InitializeComponent();

            List<Task> items = new List<Task>();
            List<Step> steps = new List<Step>();
            items.Add(new Task() { Name = "Posprzataj_pokoj", Category = "Dom", Priority = 2, TimeLimit = DateTime.Now });
            items.Add(new Task() { Name = "Zmyj_podloge_w_kuchni", Category = "Dom" });
            items.Add(new Task() { Name = "Nastaw_pranie", Category = "Dom" });
            items.Add(new Task() { Name = "Odrób_lekcje", Category = "Szkoła" });
            steps.Add(new Step() { Name = "to jest maly krok dla czlowieka, ale wielki skok dla ludzkosci" });
            TasksListView.ItemsSource = items;
            StepsListView.ItemsSource = steps;
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
            NewTask view = new NewTask();
            view.ShowDialog();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            EditTask view = new EditTask();
            view.ShowDialog();
        }
    }

    public class Task
    {
        public string Name { get; set; }

        public string Category { get; set; }
        public int Priority { get; set;}
        public DateTime TimeLimit { get; set; }
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
