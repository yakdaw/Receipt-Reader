namespace Receipt.Web.Services
{
    using Domain.Entities;
    using System.Collections.Generic;

    public static class CategoriesService
    {
        public static IEnumerable<Category> CategoryList = new List<Category>
        {
           new Category { Id = 1, Name = "Alkohole i tytoń" },
           new Category { Id = 2, Name = "Apteczka" },
           new Category { Id = 3, Name = "Art. biurowe" },
           new Category { Id = 4, Name = "Art. dla zwierząt" },
           new Category { Id = 5, Name = "Art. domowe" },
           new Category { Id = 6, Name = "Art. dziecięce" },
           new Category { Id = 7, Name = "Art. sypkie" },
           new Category { Id = 8, Name = "Chemia gospodarcza" },
           new Category { Id = 9, Name = "Ciasta, desery, dodatki" },
           new Category { Id = 10, Name = "Dania gotowe" },
           new Category { Id = 11, Name = "Higiena" },
           new Category { Id = 12, Name = "Inne" },
           new Category { Id = 13, Name = "Kawa, herbata, kakao" },
           new Category { Id = 14, Name = "Konserwy" },
           new Category { Id = 15, Name = "Kosmetyki" },
           new Category { Id = 16, Name = "Mięso i wędliny" },
           new Category { Id = 17, Name = "Motoryzacja" },
           new Category { Id = 18, Name = "Mrożonki i lody" },
           new Category { Id = 19, Name = "Nabiał" },
           new Category { Id = 20, Name = "Pieczywo" },
           new Category { Id = 21, Name = "Prasa, książki, płyty" },
           new Category { Id = 22, Name = "Przetwory" },
           new Category { Id = 23, Name = "Przyprawy, sosy, dodatki" },
           new Category { Id = 24, Name = "Ryby" },
           new Category { Id = 25, Name = "Sprzęt AGD i RTV" },
           new Category { Id = 26, Name = "Słodycze i przekąski" },
           new Category { Id = 27, Name = "Tłuszcze" },
           new Category { Id = 28, Name = "Ubrania" },
           new Category { Id = 29, Name = "Warzywa i owoce" },
           new Category { Id = 30, Name = "Woda i napoje" },
           new Category { Id = 31, Name = "Żywność dietetyczna" }
        };
    }
}