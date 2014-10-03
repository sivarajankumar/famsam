using ServerAPI.CF_Models;
using ServerAPI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;

namespace ServerAPI.DAO
{
    public class UserDAO
    {
        public static FamsamPrinciple CheckLogin(string email, string password)
        {
            using (var db = new CF_FamsamEntities())
            {
                User user = (from u in db.User
                             where u.email == email && u.password == password
                             select u).FirstOrDefault();
                if (user != null)
                {
                    //TODO - create session
                    return new FamsamPrinciple(new FamsamIdentity(user.email), user.UserRole.rolename);
                }
            }
            return null;
        }
    }

    public class FamsamIdentity : IIdentity
    {
        public FamsamIdentity(string name){
            this.name = name;
        }
        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { throw new NotImplementedException(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }
    }

    public class FamsamPrinciple : IPrincipal
    {
        public FamsamPrinciple(FamsamIdentity identity, string role)
        {
            this.identity = identity;
            this.role = role;
        }

        private FamsamIdentity identity;
        public IIdentity Identity
        {
            get { return identity; }
        }

        public string role;
        public bool IsInRole(string role)
        {
            return this.role == role;
        }
    }

    public class SessionDAO
    {
        /// <summary>
        /// Authorization request header from client.
        /// </summary>
        /// <param name="header">header from client</param>
        /// <returns>-401/-403/{userId}</returns>
        public static long Authentication(HttpRequestHeaders header)
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

        public static long Authentication(String token){
            using (var db = new CF_FamsamEntities())
            {
                    Session session = db.Session.Find(token);
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