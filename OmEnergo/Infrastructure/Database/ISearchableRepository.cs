using OmEnergo.Models;
using System.Collections.Generic;

namespace OmEnergo.Infrastructure.Database
{
	interface ISearchableRepository<T> where T : UniqueObject
	{
		IEnumerable<T> GetSearchedItems(string searchString);
	}
}
