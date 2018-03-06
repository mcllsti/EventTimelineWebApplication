using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Controllers
{
    /// <summary>
    /// Abstract base controller that all controllers will inherit from
    /// </summary>
    public abstract class BaseController : Controller
    {
        public IdeagenAPI API = new IdeagenAPI(); //initilize global API wrapper 

    }
}
