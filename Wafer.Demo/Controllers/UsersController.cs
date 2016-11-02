using System;
using System.Collections.Generic;
using System.Web.Http;
using Wafer.Demo.Models;

namespace Wafer.Demo.Controllers
{
    public class UsersController : ApiController
    {
        public IEnumerable<User> Get()
        {
            return new List<User>();
        }
    }
}
