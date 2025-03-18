using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface ISupportFunction
    {
        List<T> SortByArrivalDate<T>(List<T> list, Func<T, DateTime?> getDate);
    }
}
