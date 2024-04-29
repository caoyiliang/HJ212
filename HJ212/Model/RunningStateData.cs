using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJ212.Model
{
    public class RunningStateData(string name, string rs)
    {
        public string Name { get; set; } = name;
        public string RS { get; set; } = rs;
    }
}
