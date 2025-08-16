using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Add(string key, object value, int duration = 15);
        void Remove(string key);
    }
}
