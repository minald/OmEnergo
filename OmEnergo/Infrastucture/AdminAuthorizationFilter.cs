using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace OmEnergo.Models
{
    public class AdminAuthorizationFilter : Attribute, IAuthorizationFilter
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
