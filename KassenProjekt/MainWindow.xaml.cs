using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace KassenProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// max amount of articles in one row
        /// </summary>
        const int MaxArticleNumber = 5;
        /// <summary>
        /// is the file path for the article data 
        /// </summary>
        const string FILE_PATH = @"ArticleList.json";
        /// <summary>
        /// list of all available articles
        /// </summary>
        List<Article> Articles;
        /// <summary>
        /// list of all selected articles
        /// </summary>
        List<ArticleQnt> SelectedArticleList = new List<ArticleQnt>();
        

        public MainWindow()
        {  
            InitializeComponent();
            ReadArticles();
            ArticleList.RowDefinitions.Add(new RowDefinition());
            //creating colmuns depending the max article for one line
            for (int k = 0;k<MaxArticleNumber-1;k++)
            {
                ArticleList.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            CreateArticleButtons();
          
        }
        /// <summary>
        /// reads the article data file and store it in the available articles
        /// </summary>
        private void ReadArticles()
        {
          
            var text= File.ReadAllText(FILE_PATH);
            Articles = JsonSerializer.Deserialize<List<Article>>(text);
            
        }

        /// <summary>
        /// create a button for each available article
        /// </summary>
        private void CreateArticleButtons()
        {
            var i = 0;
            var j = 0;
            foreach (var article in Articles)
            {
                var button = new Button();
                button.Margin = new Thickness(10);
                button.Click += Article_Click;
                button.Content = article.Name;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                ArticleList.Children.Add(button);
                i++;
                if (i == MaxArticleNumber - 1)
                {
                    ArticleList.RowDefinitions.Add(new RowDefinition());
                    j++;
                    i = 0;
                }
            }
        }



        /// <summary>
        /// event that gets triggeredby each article button<br></br>
        /// adds the article to the article list
        /// </summary>
        private void Article_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var name = button.Content;
            //gets the article quantity from the selected article list
            var item = SelectedArticleList.Where(se => se.Article.Name == name).FirstOrDefault();

            if (item==null)//if the item is not yet in the selected article list it gets added
            {
                SelectedArticleList.Add(new ArticleQnt( Articles.Where(a => a.Name == name).First()));

            }
            else //increments the quantity if the article is already present
            {
                SelectedArticleList.Where(se => se.Article.Name == name).First().Count++;
                
            }
            UpdateArticleListView();
            //set the selected articles text 
            
        }
        /// <summary>
        /// updates the ordered article list view 
        /// </summary>
        private void UpdateArticleListView()
        {
            SelectedArticles.Items.Clear();
            for (int i = 0; i < SelectedArticleList.Count; i++)//convert the selected article list to string
            {
                var selectedArticle = SelectedArticleList[i];
                SelectedArticles.Items.Add(CreateArticleListItem(selectedArticle));
            }
        }
        /// <summary>
        /// creates an element which contains the name, price, quantity and a button which can decrease the quantity
        /// </summary>
        /// <param name="selectedArticle">contains the data about the name prize and quantity</param>
        /// <returns>an element which contains the upper mentioned data</returns>
        private Grid CreateArticleListItem(ArticleQnt selectedArticle)
        {
            var listItem = new Grid();
            listItem.Children.Add(new TextBlock { Text = selectedArticle.Article.Name + " x" + selectedArticle.Count });
            var button = new Button();
            button.Content = "-";
            button.FontSize = 15;
            button.Tag = selectedArticle.Article.Name;
            button.HorizontalAlignment= HorizontalAlignment.Right;
            button.Click += RemoveArticleQnt_Click;
            listItem.Children.Add(button);
            return listItem;
        }
        /// <summary>
        /// the button event to decrease the quantity of an article
        /// </summary>
        private void RemoveArticleQnt_Click(object sender, RoutedEventArgs e)
        {
            var button= (Button)sender;
            var article= SelectedArticleList.Where(sa => sa.Article.Name == button.Tag).FirstOrDefault();
            if (article.Count == 1)
            {
                SelectedArticleList.Remove(article);
            }
            else
            {
                article.Count--;
            }
            UpdateArticleListView();
        }
        /// <summary>
        /// the button event to end an order 
        /// </summary>
        private void Cashout_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedArticleList.Count == 0)
            {
                MessageBox.Show("Keine Artikel ausgewählt");
                return;
            }
            double totalPrice = 0;
            foreach (var article in SelectedArticleList)
            {
                totalPrice += article.Count * article.Article.Price;
            }
            MessageBox.Show($"Kosten: {totalPrice}");
            ClearSelectedItems();
        }
        /// <summary>
        /// clears the selected article list and the children contained in the selected article list view
        /// </summary>
        private void ClearSelectedItems()
        {
            SelectedArticles.Items.Clear();
            SelectedArticleList.Clear();
        }
    }
}