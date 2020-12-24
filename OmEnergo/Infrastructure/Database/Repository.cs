using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public abstract class Repository<T> where T : UniqueObject
	{
		protected readonly OmEnergoContext db;

		public Repository() => db = new OmEnergoContext();

		public Repository(OmEnergoContext context) => db = context;

		public T Get(Func<T, bool> predicate) => db.Set<T>().FirstOrDefault(predicate);

		public virtual IEnumerable<T> GetAll() => GetAllQueryable();

		protected virtual IQueryable<T> GetAllQueryable() => db.Set<T>();

		protected virtual IQueryable<T> GetAllSearchedItemsQueryable() => db.Set<T>();

		public Task<T> GetById(int id) => GetAllQueryable().FirstOrDefaultAsync(obj => obj.Id == id);

		public async Task CreateOrUpdateAsync(T obj)
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

		public Task UpdateRangeAsync(IEnumerable<T> obj)
		{
			db.Set<T>().UpdateRange(obj);
			return db.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			T item = await db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
			db.Set<T>().Remove(item);
			await db.SaveChangesAsync();
		}
	}
}
