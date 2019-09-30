//using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Office2010.ExcelAc;
//using Models;
//using Nest;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ElasticSearch
//{
//    public class Class1
//    {
//        ElasticClient client = null;
//        public Class1()
//        {
//                var uri = new Uri("http://localhost:9200");
//                var settings = new ConnectionSettings(uri);
//                client = new ElasticClient(settings);
          
//            settings.DefaultIndex("city");
//            }
//            public List<ProductInformation> GetResult()
//            {
//                if (client.IndexExists("city").Exists)
//                {
//                    var response = client.Search<ProductInformation>();
//                    return response.Documents.ToList();
//                }
//                return null;
//            }

//        public List QueryById(string id)
//        {
//            QueryContainer queryById = new TermQuery() { Field = "_id", Value = id.Trim() };

//            var hits = client.curre
//                                   .Search(s => s.Query(q => q.MatchAll() && queryById))
//                                   .Hits;

//            List typedList = hits.Select(hit => ConvertHitToCustumer(hit)).ToList();

//            return typedList;
//        }

//        public void AddNewIndex(City model)
//            {
//                client.IndexAsync<City>(model, null);
//            }

//        }


    
//    }
//}
