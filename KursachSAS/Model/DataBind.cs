using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public interface IData
    {

    }
    public class DataBind<T>:IData
    {
        public T data { get; set; }
        public string[] information { get; set; }
        public DataBind( T data, string[] information)
        {
            this.data = data;
            this.information = information;
        }
    }
}
