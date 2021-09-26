using FastMember;
using System.Collections.Generic;
using System.Data;

namespace FurnitureInventorySystem.View
{
    internal abstract class ListManager<T>
    {
        public static DataTable LoadDataTableFrom(IEnumerable<T> list)
        {
            var table = new DataTable();
            using (var reader = ObjectReader.Create(list))
            {
                table.Load(reader);
            }
            table.Columns.Remove("factory_id");

            return table;
        }
    }
}