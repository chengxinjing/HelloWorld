using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWeb.Models;
namespace HelloWeb.Controllers
{
    public class ProductController : ApiController
    {
        Product[] product = new Product[] {
            new Product{Number ="1",Name="上海" }, new Product{Number ="2",Name="北京" }, new Product{Number ="3",Name="大连" }

        };

        public IEnumerable<Product> GetProductAll() {
            return product;
        }
    }
}
