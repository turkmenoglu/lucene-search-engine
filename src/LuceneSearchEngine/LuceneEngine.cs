using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneSearchEngine.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LuceneSearchEngine
{
    public class LuceneEngine
    {
        private Analyzer _Analyzer;
        private Directory _Directory;
        private IndexWriter _IndexWriter;
        private IndexSearcher _IndexSearcher;
        private Document _Document;
        private QueryParser _QueryParser;
        private Query _Query;
        private string _IndexPath = @"C:\LuceneIndex";

        public LuceneEngine()
        {
            _Analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            _Directory = FSDirectory.Open(_IndexPath);
        }

        public void AddToIndex(IEnumerable<Product> values)
        {
            using (_IndexWriter = new IndexWriter(_Directory, _Analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var loopEntity in values)
                {
                    _Document = new Document();

                    foreach (var loopProperty in loopEntity.GetType().GetProperties())
                    {
                        // Dilersek indexlenmesini istemediğimiz bir alan varsa, kontrol ederek Field.Index.NO vermemiz yeterli olacaktır.
                        _Document.Add(new Field(loopProperty.Name, loopProperty.GetValue(loopEntity).ToString(), Field.Store.YES, Field.Index.ANALYZED));

                        _IndexWriter.AddDocument(_Document);
                        _IndexWriter.Optimize();
                        _IndexWriter.Commit();
                    }
                }
            }
        }

        public List<Product> Search(string field, string keyword)
        {
            // Üzerinde arama yapmak istediğimiz field için bir query oluşturuyoruz.
            _QueryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, field, _Analyzer);
            _Query = _QueryParser.Parse(keyword);

            using (_IndexSearcher = new IndexSearcher(_Directory, true))
            {
                List<Product> products = new List<Product>();
                var result = _IndexSearcher.Search(_Query, 10);

                foreach (var loopDoc in result.ScoreDocs.OrderBy(s => s.Score))
                {
                    _Document = _IndexSearcher.Doc(loopDoc.Doc);

                    products.Add(new Product() { Id = System.Convert.ToInt32(_Document.Get("Id")), Name = _Document.Get("Name") });
                }

                return products;
            }
        }
    }
}