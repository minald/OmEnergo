using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace OmEnergo.Infrastructure
{
    public class AdminAuthorizationAttribute : Attribute, IAuthorizationFilter
	{ 
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.Session.GetString("isLogin") != "true")
			{
                context.Result = new NotFoundResult();
			}
		}
	}
}
