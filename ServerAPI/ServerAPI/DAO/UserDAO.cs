using ServerAPI.CF_Models;
using ServerAPI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace ServerAPI.DAO
{
    public class UserDAO
    {
    }

    public class SessionDAO
    {
        /// <summary>
        /// Authorization request header from client.
        /// </summary>
        /// <param name="header">header from client</param>
        /// <returns>-401/-403/{userId}</returns>
        public static int Authorization(HttpRequestHeaders header)
        {
            string authorization = header.GetValues("Authorization").FirstOrDefault();
            if (authorization == null)
                {
                    return -401;
                }
            using (var db = new CF_FamsamEntities())
            {
                string token = authorization.Split(null)[1];
                    Session session = db.Session.Find(token);
                    Debug.WriteLine("____________________________" + session.token);
                    if (session == null) return -403;

                    if (session.expired < DateTime.Now)
                    {
                        Debug.WriteLine("____________________________ session mili:" + session.expired.Millisecond);
                        Debug.WriteLine("____________________________ now mili:" + DateTime.Now.Millisecond);
                        //session expired
                        db.Session.Remove(session);
                        db.SaveChanges();
                        return -403;
                    }
                    else
                    {
                        return session.User.id;
                    }


                

            }
        }
    }
}