using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Services
{
    public abstract class BaseService : IDisposable, IAsyncDisposable
    {
        protected Data.CommonDataContext _commonDataContext;

        public BaseService(Data.CommonDataContext commonDataContext)
        {
            this._commonDataContext = commonDataContext;
        }

        public void Dispose()
        {
            _commonDataContext.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _commonDataContext.DisposeAsync();
        }
    }
}
