using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using HelloWeb.Models;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using MyLibrary;
using HelloWeb.DatabaseRelation;
using MyApplicationLibrary.DatabaseOperator;
namespace HelloWeb.Controllers
{
    public class MyController : Controller
    {
        Product[] product = new Product[] {
            new Product{Number ="1",Name="上海" }, new Product{Number ="2",Name="北京" }, new Product{Number ="3",Name="大连" }

        };
        // GET: My
        public ActionResult Index()
        {
            
            return View();
        }
        public JsonResult GetProductAll()
        {
         
            return Json(product,JsonRequestBehavior.AllowGet);
        }
        public ViewResult Name()
        {
            List<AdminUser> adminUserList = new List<AdminUser>();

            MyFirst myFirst = new MyFirst();
            myFirst.Name = "程序集引";
            AdminUser admin = new AdminUser();
            admin.MyFirstName = myFirst;
            adminUserList.Add(admin);
            adminUserList.Add(admin);
            adminUserList.Add(admin);
            return View("_Cheng", adminUserList);
        }
      
        public ViewResult UserInfo(string Name, string Age) {
         
              //  DatabaseRelation.DatabaseRelation.CreateModel( DatabaseRelation.DatabaseRelation.getDbConnectionString("BDConnectionStrings"));
            DatabaseRelation.DatabaseRelation.CreateModel(DatabaseRelation.DatabaseRelation.getDbConnectionString("loan-after"),"D:/mysql/Modelss");
            DatabaseBasicOperator.CreateModel(DatabaseBasicOperator.getDbConnectionString("192.168.1.145", "loan-after", "root", "loanadmin"));
        
            return View("user");
        }
        

    }
}