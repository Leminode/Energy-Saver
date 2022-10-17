using Energy_Saver.Model;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Energy_Saver.Services
{
    public class TableService : ITableService
    {
        public void DeleteEntry()
        {
            Taxes taxes;

            taxes = this.ReadFromFile();
            taxes[yearIndex].RemoveAt(index);
            this.WriteText(taxes.SelectMany(list => list).Distinct().ToList());
        }
    }
}
