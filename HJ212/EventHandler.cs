using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJ212
{
    public delegate Task<T1> ActivelyAskDataEventHandler<T, T1>(T objects);
}
