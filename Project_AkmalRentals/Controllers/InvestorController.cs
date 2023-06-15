using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Project_AkmalRentals.Models;
using Antlr.Runtime;

namespace Project_AkmalRentals.Controllers
{
    public class InvestorController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Investor
        public ActionResult Index()
        {
            string role = (string)Session["role"];
            if (role != "Admin")
            {
                return View("Error");
            }
            else
            {
                var tb_investor = db.tb_investor.Include(t => t.tb_building).Include(t => t.tb_user).OrderBy(t => t.tb_user.u_name);
                return View(tb_investor.ToList());
            }
        }
        public ActionResult Share()
        {
            var b = db.tb_building.ToList();
            var tb_share = db.tb_share.Include(t => t.tb_investor);
            var partner = db.tb_user.Where(u => u.u_role == 3).ToList();
            int partnerCount = db.tb_user.Where(u => u.u_role == 3).Count();
            var partnerProfit = db.tb_profit.ToList();
            ViewBag.count = partnerCount;
            ViewBag.pProfit = partnerProfit;
            ViewBag.partner = partner;
            ViewBag.b = b;
            return View(tb_share.ToList());
        }

        public ActionResult InvDashboard(tb_profit tb_profit)
        {
            if (Session["uid"] == null)
            {
               return RedirectToAction("Index", "Login");
            }
            else
            {
                int currYear = DateTime.Now.Year;
                int currMonth = DateTime.Now.Month;
                int currDay = DateTime.Now.Day;

                var profitChart = db.tb_profit.Where(p => p.profit_date.Year == currYear).ToList();

                if(currMonth == 1)
                {
                    currMonth = 13;
                    currYear = currYear - 1;
                }

                var investorsList = db.tb_investor.Where(i=>i.tb_building.b_status == "Full").ToList();
                var partner = db.tb_user.Where(u => u.u_role == 3).ToList();

                //count total building, partner and investor
                int investors = db.tb_investor.Count();
                int totalLot = db.tb_building.Where(b=>b.b_status =="Full").Count();
                var partners = db.tb_user.Where(u => u.u_role == 3).Count();

                var income = db.tb_expenses
                    .Where(e => e.e_Date.Year == currYear && e.e_Date.Month == currMonth-1 && e.e_Type == "Income" && e.e_Category!=1)
                    .ToList();

                var expenses = db.tb_expenses
                    .Where(e => e.e_Date.Year == currYear && e.e_Date.Month == currMonth-1 && e.e_Type == "Expense" && e.e_Category != 1)
                    .ToList();

                double totalincome = income.Sum(e => e.e_Amount);
                double totalexpense = expenses.Sum(e => e.e_Amount);
                double currProfit = totalincome - totalexpense;
                double lotProfit = currProfit * 0.6 / (partners + totalLot);

                if (currDay == 13)
                {
                    var profitCheck = db.tb_profit.FirstOrDefault(s => s.profit_date.Year == currYear && s.profit_date.Month == currMonth - 1);

                    if (profitCheck == null)
                    {
                        foreach (var investor in investorsList)
                        {
                            double share = currProfit * 0.6 / (partners + totalLot) * investor.i_percentage;

                            var shareObject = new tb_share
                            {
                                share_amount = share,
                                share_date = DateTime.Now.AddDays(-1),
                                share_investor = investor.i_id
                            };

                            db.tb_share.Add(shareObject);
                        }

                        tb_profit.profit_amount = currProfit;
                        tb_profit.profit_AR = currProfit * 0.4;
                        tb_profit.profit_inv = currProfit * 0.6;
                        tb_profit.profit_lot = lotProfit;
                        tb_profit.profit_date = DateTime.Now.AddDays(-1);
                        db.tb_profit.Add(tb_profit);
                        //db.SaveChanges();
                    }
                }
                var tb_share = db.tb_share.Include(t => t.tb_investor).Where(t => t.share_date.Year == currYear && t.share_date.Month == currMonth - 1).OrderByDescending(t => t.share_date);
                var profit = db.tb_profit.FirstOrDefault(s => s.profit_date.Year == currYear && s.profit_date.Month == currMonth - 1);

                ViewBag.ChartData1 = profitChart.Select(p=>p.profit_amount);
                ViewBag.Label = profitChart.Select(p => p.profit_date.ToString("MMM"));

                ViewBag.currDate = profit.profit_date.ToString("MMM yyyy");
                ViewBag.profit = profit.profit_amount.ToString("N2");
                ViewBag.share = profit.profit_inv.ToString("N2");
                ViewBag.sharePartner = profit.profit_lot.ToString("N2");
                ViewBag.totalInvestor = investors;
                ViewBag.partner = partner;

                return View(tb_share.ToList());

            }
   
        }

        public ActionResult Profit()
        {
            return View(db.tb_profit.ToList());
        }

        // GET: Investor/Create
        public ActionResult Create()
        {
            //Get only user that has not been added as investors
            var id = db.tb_user.Where(u => !db.tb_investor.Any(i => i.i_id == u.u_id)).Where(i=>i.tb_role.role_id ==2).ToList();

            //Get only available lots
            var lots = db.tb_building.Where(l => l.b_status == "Available").ToList();

            ViewBag.i_lot = new SelectList(lots, "b_id", "b_name");
            ViewBag.i_id = new SelectList(id, "u_id", "u_email");
            return View();
            
        }

        // POST: Investor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "i_id,i_investment,i_percentage,i_lot")] tb_investor tb_investor)
        {
            var id = db.tb_user.Where(u => !db.tb_investor.Any(i => i.i_id == u.u_id)).Where(i => i.tb_role.role_id == 2).ToList();
            var lots = db.tb_building.Where(l => l.b_status == "Available").ToList();
            ViewBag.i_lot = new SelectList(lots, "b_id", "b_name", tb_investor.i_lot);
            ViewBag.i_id = new SelectList(id, "u_id", "u_email", tb_investor.i_id);

            if (ModelState.IsValid)
            {
                var maxInvest = db.tb_investor.Where(m => m.i_lot == tb_investor.i_lot).ToList();
                double max = 50000;
                double? currentTotal = maxInvest.Sum(e => e.i_investment);
                double? totalInvest = maxInvest.Sum(e => e.i_investment) + tb_investor.i_investment;

                if(tb_investor.i_investment >= 10000)
                {
                    if (totalInvest <= 50000)
                    {
                        if(totalInvest == 50000)
                        {
                            var lot = db.tb_building.Where(m => m.b_id == tb_investor.i_lot).FirstOrDefault();
                            lot.b_status = "Full";
                            db.Entry(lot).State = EntityState.Modified;
                        }
                        tb_investor.i_percentage = tb_investor.i_investment / 50000;
                        db.tb_investor.Add(tb_investor);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        double? totalLeft = max - currentTotal;
                        ViewBag.maxError = "Maximum amount allowed for Lot" + tb_investor.i_lot + " is RM 50000. You can only invest RM " + totalLeft + " more.";
                        return View(tb_investor);
                    }
                }
                else
                {
                    ViewBag.maxError = "Minimum amount to invest is RM 10000.";
                    return View(tb_investor);
                }
            }
            ViewBag.i_lot = new SelectList(lots, "b_id", "b_name", tb_investor.i_lot);
            ViewBag.i_id = new SelectList(id, "u_id", "u_email", tb_investor.i_id);
            return View(tb_investor);
        }

        // GET: Investor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_investor tb_investor = db.tb_investor.Find(id);
            if (tb_investor == null)
            {
                return HttpNotFound();
            }
            return View(tb_investor);
        }

        // POST: Investor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_investor tb_investor = db.tb_investor.Find(id);
            db.tb_investor.Remove(tb_investor);
            var lot = db.tb_building.Where(m => m.b_id == tb_investor.i_lot).FirstOrDefault();
            lot.b_status = "Available";
            db.Entry(lot).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ViewLot()
        {
            var tb_building = db.tb_building.Include(t => t.tb_investor);
            return View(tb_building.ToList());
        }

        // GET: tb_building/Create
        public ActionResult AddLot()
        {
            return View();
        }

        // POST: tb_building/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLot([Bind(Include = "b_id,b_name,b_status")] tb_building tb_building)
        {
            if (ModelState.IsValid)
            {
                tb_building.b_status = "Available";
                db.tb_building.Add(tb_building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_building);
        }

        // GET: tb_building/Edit/5
        public ActionResult EditLot(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_building tb_building = db.tb_building.Find(id);
            if (tb_building == null)
            {
                return HttpNotFound();
            }
            return View(tb_building);
        }

        // POST: tb_building/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLot([Bind(Include = "b_id,b_name,b_status")] tb_building tb_building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewLot");
            }
            return View(tb_building);
        }


        // GET: tb_building/Delete/5
        public ActionResult DeleteLot(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            tb_building tb_building = db.tb_building.Find(id);
            if (tb_building == null)
            {
                return HttpNotFound();
            }
            return View(tb_building);
        }

        // POST: tb_building/Delete/5
        [HttpPost, ActionName("DeleteLot")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLot(int id)
        {
            tb_building tb_building = db.tb_building.Find(id);
            var buildingCheck = db.tb_investor.Where(i=>i.i_lot == id).FirstOrDefault();
            if (buildingCheck == null)
            {
                db.tb_building.Remove(tb_building);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "sorry, but you cannot delete a lot that has investors.";
                return View(tb_building);
            }
            
            return RedirectToAction("ViewLot");
        }

    }


}
