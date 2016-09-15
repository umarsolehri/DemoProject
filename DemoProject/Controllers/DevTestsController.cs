using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoProjectBOL.Models;
using DemoProjectDAL;
using DemoProjectBLL;
using System.Data.SqlClient;
using System.Configuration;
using DemoProject.Hubs;

namespace DemoProject.Controllers
{
    public class DevTestsController : Controller
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DemoProjectContext"].ConnectionString;
        //private DemoProjectContext db = new DemoProjectContext();
        private DevTestService devTest;
        public DevTestsController() : this(new DevTestService()) { }

        public DevTestsController(DevTestService _devTest)
        {
            devTest = _devTest;
        }

        public async Task<ActionResult> Index()
        {
            var list = await devTest.ListDevs();
            return View(list);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await devTest.DevsDetails(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DevTest devtest)
        {
            if (ModelState.IsValid)
            {
                var check = await devTest.FindbyAffiliation(devtest.AffiliateName);
                if (check == null)
                {
                    await devTest.AddDevs(devtest);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Dev test Already Exists");
                    return View(devtest);
                }
            }
            return View(devTest);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var check = await devTest.FindById(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DevTest dev)
        {
            if (ModelState.IsValid)
            {
                await devTest.Update(dev);
                return RedirectToAction("Index");
            }
            return View(devTest);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            await devTest.DeleteDev(id);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await devTest.DeleteDev(id);
            return RedirectToAction("Index");
        }



        public ActionResult DevTestNotification()
        {
            return PartialView("_PartialNotification", GetAllToys());
        }



        private IEnumerable<DevTest> GetAllToys()
        {
            List<DevTest> messages = new List<DevTest>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"SELECT [ID], [CampaignName], [Date], [Clicks], [Conversions], [Impressions], [AffiliateName] FROM [dbo].[DevTests]", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        using (DemoProjectContext db = new DemoProjectContext())
                        {
                            messages = db.DevTest.OrderByDescending(i=>i.ID).ToList();
                        }
                        //messages.Add(item: new DevTest { ID = (int)reader["ID"], CampaignName = (string)reader["CampaignName"], Date = (DateTime)reader["Date"], });
                    }
                }

            }
            return messages;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                DevHub.DevNoti();
            }
        }
    }
}
