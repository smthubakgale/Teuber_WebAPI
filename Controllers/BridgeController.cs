using Desktop.Database.offline;
using System;
using System.Web.Http;
using Web.Deploy.Models;
using Web.Deploy.Views.Layout;
using WebAPIsameDomain.Models;

namespace WebAPIsameDomain.Controllers
{
    public class BridgeController : ApiController
    {
        public string Get(string json)
        {
            var ret = new object(); 
            var dt = new JC<Bridge>().ToObj(json);

            try
            {
                var cl = (new JC<Model>().ToObj(dt.param)).Client;

                if (dt.func != "log") 
                { 
                    //new Logger().Clear(cl);
                    new Logger().Log(cl, "BRIDGE :: " + dt.control + " , " + dt.func + " , " + dt.param);
                }
                
                Type type = Type.GetType("Web.API." + dt.control);
                var obj = Activator.CreateInstance(type);
                var method = obj.GetType().GetMethod(dt.func);
                ret = method.Invoke(obj, new object[1] { dt.param });
            }
            catch (Exception ex)
            {
                ret = "[]";
                new Logger().Log("Bridge Error" , "BRIDGE : " + ex.Message);
            }
             
            return (string)ret;
        }
    }
}
