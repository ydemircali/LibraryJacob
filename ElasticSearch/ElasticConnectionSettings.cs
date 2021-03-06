using System;
using System.Collections.Generic;
using System.Text;
 
namespace LibraryJacob.ElasticSearch
{
    public class ElasticConnectionSettings
    {           
        public string ElasticSearchHost { get; set; }
        public string ElasticBookIndex { get; set; }
        public string ElasticUserBookIndex { get; set; }
        public string ElasticErrorIndex { get; set; }        
    }
}