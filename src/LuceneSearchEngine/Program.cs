using System.Collections.Generic;
using LuceneSearchEngine.Entities;

namespace LuceneSearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            LuceneEngine engine = new LuceneEngine();
            List<Product> products = new List<Product>();

            products.Add(new Product() { Id = 1, Name = "Apple Iphone 6" });
            products.Add(new Product() { Id = 2, Name = "MacBook Air" });
            products.Add(new Product() { Id = 3, Name = "Sony Xperia Z Ultra" });
            products.Add(new Product() { Id = 4, Name = "Samsung Ultra HD Tv" });
            products.Add(new Product() { Id = 5, Name = "Asus Zenphone 6" });
            products.Add(new Product() { Id = 6, Name = "Sony Xperia Z 3" });
            products.Add(new Product() { Id = 7, Name = "Sony Playstation 3" });

            // Oluşturmuş olduğumuz test dokümanlarını ilgili indexleme metotumuza gönderelim ve indexlesin. Bu metotu sadece bir kere çalıştırmamız yeterlidir, daha sonrasında ise kapatabilirsiniz.
            engine.AddToIndex(products);

            // Oluşturmuş olduğumuz index üzerinden Name field'ına bakarak ve içerisinde "Sony" geçen en uygun kelimeleri scoring işlemi yaparak bize getirecektir.
            var result = engine.Search("Name", "Sony");
        }
    }
}
