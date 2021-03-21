
using EFCore.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFCore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CarDbContext context)
        {
            context.Database.Migrate();

            if (context.Cars.Any())
            {
                return;   // DB has been seeded
            }

            var cars = new Car[]
            {
            new Car{Make="Jeep",Price=300},
            new Car{Make="Honda",Price=130},
            new Car{Make="Mazda",Price=390},
            new Car{Make="Fiat",Price=390},
            new Car{Make="Subaru",Price=390},
            new Car{Make="Toyota",Price=390},
            new Car{Make="Mitsubishi",Price=390},
            new Car{Make="BMW",Price=390},
            new Car{Make="VW",Price=390},
            new Car{Make="Dodge",Price=390}
            };
            foreach (Car c in cars)
            {
                context.Cars.Add(c);
            }
            context.SaveChanges();
        }
    }
}