﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Web.Http.Cors;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        //---------------------------------------------------------------------------------
        [HttpGet]
        [Route("categories")]
        public IHttpActionResult GetCategories()
        {
            PenocEntities db = new PenocEntities();

            var categories = from category in db.lutCategory
                         select new LookupValue
                         {
                             name = category.strCategory,
                             id = category.idCategory
                         };

            return Ok(categories);
        }
        //---------------------------------------------------------------------------------
    }
}
