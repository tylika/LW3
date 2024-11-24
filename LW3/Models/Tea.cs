using System;

namespace LW3.Models
{
    public class Tea
    {
        public int Id { get; set; } // Унікальний ідентифікатор чаю
        public string Name { get; set; } // Назва чаю
        public string Brand { get; set; } // Виробник
        public double Price { get; set; } // Ціна чаю
        public int Year { get; set; } // Рік збору або випуску
        public string Category { get; set; } // Категорія чаю (зелений, чорний тощо)
        public int Stock { get; set; } // Кількість на складі
    }
}
