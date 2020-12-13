using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class ConfigKeyRepository : Repository
	{
		public ConfigKeyRepository() : base() { }

		public ConfigKeyRepository(OmEnergoContext context) : base(context) { }

		public string GetConfigValue(string key) =>
			GetAllQueryable<ConfigKey>().FirstOrDefault(x => x.Key.ToLower() == key.ToLower())?.Value ?? "";
	}
}
