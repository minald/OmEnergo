using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public abstract class Repository
	{
		protected readonly OmEnergoContext db;

		public Repository() => db = new OmEnergoContext();

		public Repository(OmEnergoContext context) => db = context;

		public T Get<T>(Func<T, bool> predicate) where T : UniqueObject => db.Set<T>().FirstOrDefault(predicate);

		public IEnumerable<T> GetAll<T>() where T : UniqueObject => GetAllQueryable<T>();

		protected virtual IQueryable<T> GetAllQueryable<T>() where T : UniqueObject => db.Set<T>();

		public T GetItemByEnglishName<T>(string name) where T : CommonObject =>
			GetAllQueryable<T>().FirstOrDefault(x => x.EnglishName == name);

		public T GetById<T>(int id) where T : UniqueObject => GetAll<T>().FirstOrDefault(obj => obj.Id == id);

		public void Update<T>(T obj) where T : UniqueObject
		{
			var existingItem = db.Set<T>().Find(obj.Id);
			if (existingItem == null)
			{
				db.Add(obj);
			}
			else
			{
				db.Entry(existingItem).CurrentValues.SetValues(obj);
			}

			db.SaveChanges();
		}

		public void UpdateRange<T>(IEnumerable<T> obj) where T : UniqueObject
		{
			db.Set<T>().UpdateRange(obj);
			db.SaveChanges();
		}

		public void Delete<T>(int id) where T : UniqueObject
		{
			db.Set<T>().Remove(db.Set<T>().FirstOrDefault(x => x.Id == id));
			db.SaveChanges();
		}
	}
}
