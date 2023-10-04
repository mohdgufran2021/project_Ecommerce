using project_Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace project_Ecommerce.Controllers
{
    public class homeController : Controller
    {
        // GET: home
        datalayer dl=new datalayer();
        dblayer db = new dblayer();
        public ActionResult Index(int ? id)
        {
            if(id.HasValue)
            {
                DataTable dt = dl.SelectAllcategories();
                DataTable dt1 = dl.SelectSubCategory(Convert.ToInt32(id));
                DataTable dt2 = dl.SelectProductOfOneCategory(User.Identity.Name, Convert.ToInt32(id));
                ViewBag.subcategory = dt1;
                ViewBag.products = dt2;
                Session["sp_cart"] = dl.GetCartItem(User.Identity.Name);
                return View(dt);
            }
            else
            {
                return RedirectToAction("product_category");
            }
           
     
        }

        public ActionResult product_category()
        {
            DataTable dt = dl.SelectAllcategories();
            DataTable dt2 = dl.SelectDiscountedProduct(User.Identity.Name);
            ViewBag.products = dt2;
            Session["sp_cart"] = dl.GetCartItem(User.Identity.Name);
            return View(dt);
        }

        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddRegisteration(AddRegister re)
        {
            SqlParameter[] sp = new SqlParameter[] 
            {
            new SqlParameter("@action",1),
            new SqlParameter("@name",re.name),
            new SqlParameter("@email",re.email),
            new SqlParameter("@mobile",re.mobile),
            new SqlParameter("@password",re.password),
            new SqlParameter("@address",re.address),
            new SqlParameter("@pincode",re.pincode),
            new SqlParameter("@landmark",re.landmark),
            new SqlParameter("@picture",re.picture.FileName)
           
            };
            object res = db.ExcuteScalar("proc_Register", sp);
            if (res !=null)
            {
                re.picture.SaveAs(Server.MapPath("/content/RegisterImage/") + re.picture.FileName);
                string msg = "Welcome to World of <h2>Email</h2><br/>Your Login Id is " + re.email + "<br/>Your Password is : " + re.password + "<br/>So,We are happy";
                dl.SendEmail(re.email, msg);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost,AllowAnonymous]
        public ActionResult Login(string email, string password)
        {
            SqlParameter[] login = new SqlParameter[]
                {
                    new SqlParameter("@email",email),
                    new SqlParameter("@password",password)
                };
            DataTable dt = db.ExcuteGetTable("sp_login", login);
            if (dt.Rows.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(email, false);
                Session["user"] = dt.Rows[0]["email"].ToString();
                string msg = " " + email + " Login Successfully";
                dl.SendEmail(email, msg);
                return RedirectToAction("product_category");
            }
            else
            {
                ViewBag.error = "Error Message Occured";
                return View();
            }
        }
        [HttpPost]        
        public ActionResult AddToCart(int product_id,int quantity)
        {
            if(User.Identity.IsAuthenticated)
            {
                string email = User.Identity.Name;
                int row = dl.AddToCart(product_id,email,quantity);
                Session["sp_cart"] = dl.GetCartItem(User.Identity.Name);
                return Json(Session["sp_cart"], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("sp_cart");
            return RedirectToAction("product_category");
        }
        [Authorize]
        public ActionResult Cart()
        {
            DataTable st = dl.GetCartItems(User.Identity.Name);
            return View(st);
        }
        public ActionResult PaymentGateway()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult SendOtp(string email)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@action",4),
                new SqlParameter("@userid",email)
            };
                object em = db.ExcuteScalar("proc_register", parameters);
                if(email!=null)
                {
                    Random random = new Random();
                    int otp = random.Next(10000, 99999);
                    string message = "<h3>You Have requested to change your password. Kindly use " + otp + " as your OTP to change your password. </h3><br/> Thanks";
                    dl.SendEmail(email.ToString(), message);
                    Session["otp"] = otp;
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("User Does not found", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Error! Please try again", JsonRequestBehavior.AllowGet);
            }
                
        }

       
        

    }


}
