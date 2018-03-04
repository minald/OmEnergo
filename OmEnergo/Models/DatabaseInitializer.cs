using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public static class DatabaseInitializer
    {
        public static void Initialize(OmEnergoContext db)
        {
            if (!db.Stabilizers.Any())
            {
                db.Stabilizers.AddRange(
                    new Stabilizer
                    {
                        Series = "АСН"
                    },
                    new Stabilizer
                    {
                        Series = "Voltron"
                    }
                    );
                db.SaveChanges();
            }
        }
    }
}
