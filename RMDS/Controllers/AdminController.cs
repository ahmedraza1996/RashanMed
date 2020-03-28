using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;

namespace RMDS.Controllers
{
    public class AdminController :Controller
    {



        //public HttpResponseMessage GetPendingDonation()
        //{


        //}
        //user personal info
        //donation list pending
        //assigndonationto rider
        //add rider

        public ActionResult Login()

        {

            if (Session[Shared.Constants.SESSION_ADMIN] != null)
            {

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public string Login(string username, string password)
        {
            string ret = Shared.Constants.MSG_ERR_NOUSEREXIST.Text;
            string whereclause = "EMAIL='" + username + "'" + "or username='" + username + "'";
            List<Admin> lstAdmin = NGOmemberManager.GetNGOmember(whereclause, null);
            if (lstAdmin.Count > 0)
            {
                if (lstAdmin.First().PASSWORD.Equals(password))
                {
                    SetSessionAdmin(lstAdmin.First());
                    ret = Shared.Constants.MSG_SUCCESS.Text;

                }
                else
                {
                    ret = Shared.Constants.MSG_ERR_INVALIDCRED.Text;
                }
            }
            return ret;

        }

    }
}
