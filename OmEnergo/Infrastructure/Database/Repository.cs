using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public class Repository
	{
		protected readonly OmEnergoContext db;

		public Repository() => db = new OmEnergoContext();

		public Repository(OmEnergoContext context) => db = context;

		public T Get<T>(Func<T, bool> predicate) where T : UniqueObject => db.Set<T>().FirstOrDefault(predicate);

		public virtual IEnumerable<T> GetAll<T>() where T : UniqueObject => GetAllQueryable<T>();

		protected virtual IQueryable<T> GetAllQueryable<T>() where T : UniqueObject => db.Set<T>();

		protected virtual IQueryable<T> GetAllSearchedItemsQueryable<T>() where T : UniqueObject => db.Set<T>();

		public Task<List<T>> GetSearchedItemsAsync<T>(string searchString) where T : CommonObject =>
			GetAllSearchedItemsQueryable<T>().Where(x => x.Name.Contains(searchString)).ToListAsync();

		public Task<T> GetItemByEnglishNameAsync<T>(string name) where T : CommonObject =>
			GetAllQueryable<T>().FirstOrDefaultAsync(x => x.EnglishName == name);

		public T GetById<T>(int id) where T : UniqueObject => GetAll<T>().FirstOrDefault(obj => obj.Id == id);

		public async Task UpdateAsync<T>(T obj) where T : UniqueObject
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

		public Task UpdateRangeAsync<T>(IEnumerable<T> obj) where T : UniqueObject
		{
			db.Set<T>().UpdateRange(obj);
			return db.SaveChangesAsync();
		}

		public async Task DeleteAsync<T>(int id) where T : UniqueObject
		{
			T item = await db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
			db.Set<T>().Remove(item);
			await db.SaveChangesAsync();
		}
	}
}
