using MySql.Data.MySqlClient;
using RMDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using System.Web.Routing;

namespace RMDS.Controllers
{
    public class AdminController : Controller
    {
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (GetSessionAdmin() != null)
        //    {
        //        if (filterContext.ActionDescriptor.ActionName.Equals("Login"))
        //        {
        //            filterContext.Result = new RedirectResult("/Admin/GetAllPendingDonation");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        if (!filterContext.ActionDescriptor.ActionName.Equals("Login"))
        //        {
        //            //filterContext.Result = RedirectToAction("Login", "Admin");
        //            //filterContext.Result = new RedirectToRouteResult(       
        //            //                                                    new RouteValueDictionary
        //            //                                                    {
        //            //                                                        {"controller", "Admin"},
        //            //                                                        {"action", "Login"}
        //            //                                                    }
        //            //                                                );
        //            filterContext.Result = new RedirectResult("/Admin/Login");
        //            return;
        //        }
        //    }
        //}




        public ActionResult Login()

        {

            if (Session[Shared.Constants.SESSION_ADMIN] != null)
            {

                return RedirectToAction("GetAllPendingDonation");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin obj)
        {
            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;

            string whereclause = "EMAIL='" + obj.EMAIL + "'" + "or username='" + obj.EMAIL + "'";
            List<Admin> lstAdmin = AdminManager.GetAdmin(whereclause, null);
            if (lstAdmin.Count > 0)
            {
                if (lstAdmin.First().APASSWORD.Equals(obj.APASSWORD))
                {
                    SetSessionAdmin(lstAdmin.First());
                    ret = Shared.Constants.MSG_SUCCESS.Text;

                }
                else
                {
                    ret = Shared.Constants.MSG_ERR_INVALIDCRED.Text;
                }
            }
            if (ret.Equals(Shared.Constants.MSG_SUCCESS.Text))
                return RedirectToAction("GetAllPendingDonation");
            else
                return RedirectToAction("Login");
        }
        public Admin GetSessionAdmin()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] != null)
            {
                return Session[Shared.Constants.SESSION_ADMIN] as Admin;
            }

            return null;
        }

        public void SetSessionAdmin(Admin obj)
        {
            Session[Shared.Constants.SESSION_ADMIN] = obj;
        }

        public ActionResult GetAllPendingDonation()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            //select* from userdonation d, user u where d.donationstatus = 'pending ' and d.userid = u.userid
            List<UserDonation> lstDonation = UserDonationManager.GetPendingDonation("", null);

            return View("PendingDonations", lstDonation);

        }
        public ActionResult GetAllDonations()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            //select* from userdonation d, user u where d.donationstatus = 'pending ' and d.userid = u.userid
            List<UserDonation> lstDonation = UserDonationManager.GetAll("", null);

            return View(lstDonation);

        }
        public ActionResult GetDonationDetails(string id)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<DonationDetails> lstDonation = DonationDetailManager.GetDonationDetails("donationid='" + id + "'", null);

            return View(lstDonation);
        }
        public ActionResult GetUserDetail(string id)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<User> lstUser = UserManager.GetUser("userid='" + id + "'", null);

            return View(lstUser.First());
        }
        public ActionResult EditDonation(string id)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<UserDonation> lstDonation = UserDonationManager.GetUserDonation("DonationID='" + id + "'", null);

            return View(lstDonation.First());

        }
        [HttpPost]

        public ActionResult EditDonation(UserDonation obj)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            ret = UserDonationManager.SaveUserDonation(obj);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetAllPendingDonation", "Admin");

            }
            return RedirectToAction("EditDonation", "Admin");


        }
        public ActionResult AddRider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRider(Rider objRider)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            ret = RiderManager.SaveRider(objRider);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetRiders", "Admin");

            }
            return RedirectToAction("AddRider", "Admin");

        }
        public ActionResult GetRiders()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }

            List<Rider> lstRider = RiderManager.GetRider("", null);

            return View("Riders", lstRider);
        }
        public ActionResult GetAllDonator()

        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<User> lstDonators = UserManager.GetUser("UserTypeId=1");

            return View("Donators", lstDonators);

        }

        public ActionResult GetAllDistributors()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<User> lstDistributors = UserManager.GetUser("UserTypeId=2");

            return View("Distributors", lstDistributors);
        }


        public ActionResult GetDistributionRequest()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            //select* from userdonation d, user u where d.donationstatus = 'pending ' and d.userid = u.userid
            List<DistributorRequest> lstDist = DistributorRequestManager.GetDistributionRequest("RequestStatus='Pending'", null);

            return View(lstDist);


        }
        public ActionResult GetAllRequest()
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            //select* from userdonation d, user u where d.donationstatus = 'pending ' and d.userid = u.userid
            List<DistributorRequest> lstDist = DistributorRequestManager.GetDistributionRequest("", null);

            return View(lstDist);


        }

        public ActionResult SignOut()
        {

            SetSessionAdmin(null);

            return RedirectToAction("Login", "Admin");

        }
        //public ActionResult AddDistributor()
        //{

        //}

        //public ActionResult AddDonator()
        //{

        //}

        public ActionResult AddDonator(string id="1")
        {
            ViewBag.UserTypeId = id;

            return View();
        }
        [HttpPost]
        public ActionResult AddDonator(User objUser)
        {
            //if (Session[Shared.Constants.SESSION_ADMIN] == null)
            //{

            //    return RedirectToAction("Login");
            //}

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            User temp = new User();
            if (objUser.Email == null)
            {
                objUser.Email = "";
            }
            if (objUser.UPassword == null)
            {
                objUser.UPassword = "";
            }
            if (objUser.Username == null)
            {
                objUser.Username = "";
            }
            objUser.UserTypeId = 1;
            ret = UserManager.SaveUser(objUser);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetAllDonator", "Admin");

            }
            return RedirectToAction("AddDonator", "Admin");

        }
        public ActionResult AddDistributor(string id = "2")
        {
            ViewBag.UserTypeId = id;

            return View();
        }
        [HttpPost]
        public ActionResult AddDistributor(User objUser)
        {
            //if (Session[Shared.Constants.SESSION_ADMIN] == null)
            //{

            //    return RedirectToAction("Login");
            //}

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            
            if (objUser.Email == null)
            {
                objUser.Email = "";
            }
            if (objUser.UPassword == null)
            {
                objUser.UPassword = "";
            }
            if (objUser.Username == null)
            {
                objUser.Username = "";
            }
            objUser.UserTypeId = 2;
            ret = UserManager.SaveUser(objUser);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetAllDistributors", "Admin");

            }
            return RedirectToAction("AddDistributor", "Admin");

        }

        public ActionResult AddDonation()
        {
            

            return View();
        }
        [HttpPost]
        public ActionResult AddDonation(UserDonation objDonation)
        {
            objDonation.RequestDate = DateTime.Now.Date;
            objDonation.DonationStatus = Shared.Constants.DonationStatusPending;
            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            MySqlConnection conn = Shared.BaseManager.PrimaryConnection();
            conn.Open();
            var transaction = conn.BeginTransaction();
            ret = UserDonationManager.SaveUserDonation(objDonation, conn,transaction);
            bool err = false;
            if (!ret.Equals(Shared.Constants.MSG_ERR_DBSAVE.Text))
            {
               for(int i =0;i<objDonation.ItemName.Count; i++)
                {
                    DonationDetails objDD = new DonationDetails();
                    objDD.DONATIONID = Convert.ToInt32(ret);
                    objDD.ITEMNAME = objDonation.ItemName[i];
                    objDD.QUANTITY = objDonation.Quantity[i];

                    string res = DonationDetailManager.SaveDonationDetail(objDD, conn, transaction);
                    if (res.Equals(Shared.Constants.MSG_ERR_DBSAVE.Text))
                    {
                        err = true;
                        break; }
                }
                if (!err)
                {
                    transaction.Commit();
                    conn.Close();
                    conn.Dispose();
                    return RedirectToAction("GetAllPendingDonation", "Admin");
                }


                //return RedirectToAction("GetAllPendingDonation", "Admin");

            }
            transaction.Rollback();
            conn.Close();
            conn.Dispose();
            return RedirectToAction("AddDonation", "Admin");


        }
        public ActionResult AddDistribution()
        {



            return View();
        }
        [HttpPost]
        public ActionResult AddDistribution(DistributorRequest objDist)
        {
            //if (Session[Shared.Constants.SESSION_ADMIN] == null)
            //{

            //    return RedirectToAction("Login");
            //}

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            
            objDist.RequestDate = DateTime.Now.Date;
            objDist.RequestStatus = Shared.Constants.ReqStatusPending;
           
            ret = DistributorRequestManager.SaveDistributionRequest(objDist);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetDistributionRequest", "Admin");
            }
            return RedirectToAction("AddDistribution", "Admin");

        }
        public ActionResult EditRequest(string id)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }
            List<DistributorRequest> lstRequest = DistributorRequestManager.GetDistributionRequest("RequestID='" + id + "'", null);

            return View(lstRequest.First());

        }
        [HttpPost]

        public ActionResult EditRequest(DistributorRequest obj)
        {
            if (Session[Shared.Constants.SESSION_ADMIN] == null)
            {

                return RedirectToAction("Login");
            }

            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            ret = DistributorRequestManager.SaveDistributionRequest(obj);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetDistributionRequest", "Admin");

            }
            return RedirectToAction("EditRequest", "Admin");


        }

    }
}
