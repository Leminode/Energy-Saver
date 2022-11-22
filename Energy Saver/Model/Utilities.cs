using Energy_Saver.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Model
{
    public static class Utilities
    {
        public static List<T> OrderList<T, U>(SortDirection sortDirection, List<T> data, Func<T, U> sortBy)
        {
            List<T> orderedList = new List<T>();

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    orderedList = (from n in data.OrderBy(sortBy) select n).ToList();
                    break;
                case SortDirection.Descending:
                    orderedList = (from n in data.OrderByDescending(sortBy) select n).ToList();
                    break;
                default:
                    return data;
            }
            return orderedList;
        }

        public static string FormatMonth(Months month)
        {
            return ((int)month).ToString().PadLeft(2, '0');
        }

        public enum SortDirection
        {
            Ascending,
            Descending
        }
    }
}
