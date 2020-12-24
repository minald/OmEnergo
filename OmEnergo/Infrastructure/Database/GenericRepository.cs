using OmEnergo.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	// This class should be used when it's impossible to use concrete subclasses of the Repository<T>.
	public class GenericRepository
	{
		private readonly OmEnergoContext db;

		public GenericRepository() => db = new OmEnergoContext();

		public GenericRepository(OmEnergoContext context) => db = context;

		public T Get<T>(Func<T, bool> predicate) where T : UniqueObject => db.Set<T>().FirstOrDefault(predicate);

		public async Task CreateOrUpdateAsync<T>(T obj) where T : UniqueObject
		{
			var existingItem = await db.Set<T>().FindAsync(obj.Id);
			if (existingItem == null)
			{
				await db.AddAsync(obj);
			}
			else
			{
				db.Entry(existingItem).CurrentValues.SetValues(obj);
			}

			await db.SaveChangesAsync();
		}
	}
}
