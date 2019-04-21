using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Command
{
    //пример паттерна singleton - одиночки
    public class BufferClass
    {
        private static BufferClass instance;
        public object currentCopyObject { get; set; }
        private BufferClass()
        {
        }
        public static BufferClass getInstance()
        {
            if (instance == null)
                instance = new BufferClass();
            return instance;
        }
        
    }
}
