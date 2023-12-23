using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface ITableRepository {
        Task<Table> GetTable(int tableID);
        void CreateTable(Table table);        
        void UpdateTable(int tableID, Table updatedTable);
        void DeleteTable(int tableID);
    }
}
