using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {
        public bool Correct { get; set; }
        public string Message { get; set; }
        public Exception EX { get; set; }
        public Object Object { get; set; }
        public List<Object> Objects { get; set; }
    }
}
