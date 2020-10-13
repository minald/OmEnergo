using System.Data;

namespace OmEnergo.Infrastructure.Excel
{
	interface IDataTableCreator
	{
		DataTable Create(object data);
	}
}
