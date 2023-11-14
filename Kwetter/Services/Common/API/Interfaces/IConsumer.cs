using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IConsumer<T> where T : class
    {
        public Task<T> ReadMessages();
    }
}
