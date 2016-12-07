using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace MVCLogin.Controllers
{
    public class PartsController : Controller
    {
        
        public ActionResult Index(int page =1, string sort = "PartName", string sortdir ="asc", string search ="" )
        {
            int pageSize = 10;
            int totalRecords = 0;

            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetParts(search, sort, sortdir, skip, pageSize, out totalRecords);
            ViewBag.TotalRows = totalRecords;
            ViewBag.search = search;

            return View(data);
        }



        //get data from DB

        public List<Part> GetParts(string search, string sort, string sortdir, int skip, int pageSize,out int totalRecords)
        {
            using (MyDBEntities dc =new MyDBEntities())
            {
                var v = (from each in dc.Parts
                         where
                            each.PartName.Contains(search) ||
                            each.PartDescription.Contains(search) ||
                            each.Manufacturer.Contains(search) ||
                            each.ModelNo.Contains(search)
                         select each);

                totalRecords = v.Count();

                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);

                }

                return v.ToList();
            }
        }
    }
}