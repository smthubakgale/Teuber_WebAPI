
using Desktop.Database.offline;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Deploy;
using Web.Deploy.Models;
using Web.Deploy.Views.Layout;
using WebAPIsameDomain.Models;

namespace Web.API
{
    public class ModelController
    {
        // LOG
        public string log(string param) 
        {
            var result = new List<Logg>();

            try
            {
                result = new Sqlbase<Logg>().Read();
                if (result.Count() != 0)
                {
                    result = result.OrderByDescending(o => DateTime.Parse(o.datetime)).ToList();
                }
            } 
            catch(Exception ex)
            {
                try { }
                catch { }
                var cl = (new JC<Model>().ToObj(param)).Client;
                new Logger().Log(cl , "get1() : " + ex.Message);
            }

            return new JC<Logg>().ToJsonArr(result);
        }
        // GET
        public string get1(string param) 
        {
            var result = new List<Model>();

            try
            {
                result = new Sqlbase<Model>().Read();
            } 
            catch(Exception ex)
            {
                try
                {
                    var cl = (new JC<Model>().ToObj(param)).Client;
                    new Logger().Log(cl, "get1() : " + ex.Message);
                }
                catch { }
            }

            return new JC<Model>().ToJsonArr(result);
        }
        // POST
        public string post1(string param)
        { 
            try
            {
                new Sqlbase<Model>().Create(param);
            }
            catch (Exception ex)
            {
                try
                {
                    var cl = (new JC<Model>().ToObj(param)).Client;
                    new Logger().Log(cl, "post1() : " + ex.Message);
                }
                catch { }
            }

            return "done";
        }
        public string post2(string param)
        {
            try
            {
                new Sqlbase<Model>().Update(param);
            }
            catch (Exception ex)
            {
                try
                {
                    var cl = (new JC<Model>().ToObj(param)).Client;
                    new Logger().Log(cl, "post2() : " + ex.Message);
                }
                catch { }
            }

            return "done";
        }
        public string post3(string param)
        {
            try
            {
                new Sqlbase<Model>().Delete(param);
            }
            catch (Exception ex)
            {
                try
                {
                    var cl = (new JC<Model>().ToObj(param)).Client;
                    new Logger().Log(cl, "post3() : " + ex.Message);
                }
                catch { }
            }

            return "done";
        }
        public string post4(string param)
        {
            try
            {
                new Sqlbase<Model>().DeleteAll();
            }
            catch (Exception ex)
            {
                try
                { 
                    var cl = (new JC<Model>().ToObj(param)).Client;
                    new Logger().Log(cl, "post4() : " + ex.Message);
                }
                catch { }
            }

            return "done";
        }
        //
    }
}
