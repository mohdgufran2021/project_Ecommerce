using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace project_Ecommerce.Models
{
    public class datalayer
    {
        //function to select all categories
        dblayer db=new dblayer();

        public DataTable SelectAllcategories()
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter ("action",2)
            };
            DataTable dt = db.ExcuteGetTable("sp_category", sp);
            return dt;
        }


        //function to category wise sub-cate

        public DataTable SelectSubCategory(int cat_id)
        {
            SqlParameter[] sp = new SqlParameter[] {
             new SqlParameter("action",3),
             new SqlParameter("cat_id",cat_id)
            };
            DataTable dt = db.ExcuteGetTable("sp_subcategory", sp);
            return dt;         

        }
            //select max discounted product
        public DataTable SelectDiscountedProduct(string userid)
        {
            SqlParameter[] sp;
         
                sp = new SqlParameter[]
                 {
                new SqlParameter("action",6),
                new SqlParameter("user_id",userid)
                 };
            
            
            DataTable dt = db.ExcuteGetTable("sp_product", sp);
            return dt;
        }

        //select category wise product list
        public DataTable SelectProductOfOneCategory(string userid,int cat_id)
        {
            SqlParameter[] sp;

            sp = new SqlParameter[]
             {
                new SqlParameter("action",7),
                new SqlParameter("user_id",userid),
                new SqlParameter("cat_id",cat_id)
             };


            DataTable dt = db.ExcuteGetTable("sp_product", sp);
            return dt;
        }

        public int AddToCart(int product_id, string email, int quantity)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("action",1),
                new SqlParameter("product_id",product_id),
                new SqlParameter("user_id",email),
                new SqlParameter("quantity",quantity)
                
                };
            int row = db.ExcuteDML("sp_cart", sp);
            return row;

        }
        //method to select toatl items added in cart
        public Object GetCartItem(string userid)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("action",2),
                new SqlParameter("user_id",userid)
             };
           DataTable row= db.ExcuteGetTable("sp_cart", sp);
            string cart_msg = "";
            if(row.Rows.Count>0)
            {
                cart_msg += row.Rows[0]["quantity"] + " items <br/>&#x20B9;" + row.Rows[0]["salerate"];
            }
            return cart_msg;
        }
        //select cart data addedd item
        public DataTable  GetCartItems(string user_id)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@action",3),
                new SqlParameter("@user_id",user_id)
            };
            DataTable dt = db.ExcuteGetTable("sp_cart", sp);
            return dt;
        }
        //technobrain
        //send email to login time
        public void SendEmail(string email,string messagebody)
        {
            //compose mail message and set all properties
            MailMessage message = new MailMessage("sukratsukrat446@gmail.com",email);
            message.Subject = "Welcome Message from Sukrat";
            message.Body = messagebody;
            message.IsBodyHtml = true;
            //message.Bcc.Add("gopal20045@gmail.com");

            //send mail message by smtpclient
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = true;
            //smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new NetworkCredential("sukratsukrat446@gmail.com", "sbzggxlecuvjccsk");           
            smtp.Send(message);
        }
    }
}

