namespace KassenProjekt
{
    internal class Article
    {
        /// <summary>
        /// is the name of the article 
        /// </summary>
        public string Name { get; set; }  
        /// <summary>
        /// is the price of the article 
        /// </summary>
        public double Price { get; set; }

        public Article(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}