using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OmEnergo.Controllers;
using Microsoft.AspNetCore.Http;

namespace OmEnergo.Models
{
    public class AdminFilter : Attribute, IAuthorizationFilter
	{ 
		void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.Session.GetString("isLogin") != "true")
			{
				context.Result = new ContentResult { StatusCode = 404 };
			}
		}
	}
}
