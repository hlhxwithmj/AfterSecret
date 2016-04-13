using AfterSecret.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AfterSecret
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private object Lock = new Object();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var log4NetPath = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4NetPath));

            Database.SetInitializer<ASDbContext>(new DbInitializer());
            using (var context = new ASDbContext())
            {
                System.Data.Entity.Core.Objects.ObjectContext objcontext = ((IObjectContextAdapter)context).ObjectContext;
            }

            System.Timers.Timer timer1 = new System.Timers.Timer(10000); // This will raise the event ev ery 10 secs.
            timer1.Enabled = true;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(ExpireOrder);
        }

        private void ExpireOrder(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(Lock))
            {
                //log.Info("DownloadWeChatImg execute......");
                using (UnitOfWork uw = new UnitOfWork())
                {
                    var result = uw.OrderRepository.Get(false)
                        .Where(a => a.OrderStatus == Models.Constant.OrderStatus.Unpaid)
                        .Where(a => a.ExpireTime < DateTime.Now).ToList();
                    try
                    {
                        result.ForEach(a => a.OrderStatus = Models.Constant.OrderStatus.Expired);
                        uw.context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        log4net.ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication).FullName);
                        log.Warn(ex);
                    }
                    finally
                    {
                        Monitor.Exit(Lock);
                    }
                }
            }
            else
                return;
        }

    }
}