using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface ITableRepository {
        void CreateTable(Table table);
        void GetTable(int tableID);
        void UpdateTable(int tableID, Table updatedTable);
        void DeleteTable(int tableID);
    }
}
