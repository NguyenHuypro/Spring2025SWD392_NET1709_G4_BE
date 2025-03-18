using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class SupportFunction : ISupportFunction
    {
        public List<T> SortByArrivalDate<T>(List<T> list, Func<T, DateTime?> getDate)
        {
            return list.OrderBy(item => getDate(item).GetValueOrDefault(DateTime.MinValue)).ToList();
        }
    }
}
