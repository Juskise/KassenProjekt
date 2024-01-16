namespace KassenProjekt
{
    internal class ArticleQnt
    {
        /// <summary>
        /// contains information about the article 
        /// </summary>
        public Article Article;
        /// <summary>
        /// is the quantity of the article
        /// </summary>
        public int Count;

      public ArticleQnt(Article article)
        {
            Article = article;
            Count = 1;
        }
    }
}
