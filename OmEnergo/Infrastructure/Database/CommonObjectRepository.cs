using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public abstract class CommonObjectRepository<T> : Repository<T> where T : CommonObject
	{
		public CommonObjectRepository() : base() { }

		public CommonObjectRepository(OmEnergoContext context) : base(context) { }

		public Task<List<T>> GetSearchedItemsAsync(string searchString) =>
			GetAllSearchedItemsQueryable().Where(x => x.Name.Contains(searchString)).ToListAsync<T>();

		public Task<T> GetItemByEnglishNameAsync(string name) =>
			GetAllQueryable().FirstOrDefaultAsync(x => x.EnglishName == name);
	}
}
