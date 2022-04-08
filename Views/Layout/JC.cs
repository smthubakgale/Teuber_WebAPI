using Desktop.Database.offline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WebAPIsameDomain.Models;

namespace Web.Deploy.Views.Layout
{
    public class JC<T>
    {
        //------------- Json Conversion  
        public JC() { }
        public T ToObj(string json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }
        public List<T> ToObjArr(string json)
        {
            List<T> obj = JsonConvert.DeserializeObject<List<T>>(json);

            return obj;
        }
        public string ToJson(T obj)
        {
            var json = (new JavaScriptSerializer() { }).Serialize(obj);

            return json;
        }
        public string ToJsonArr(List<T> obj)
        {
            var json = "[";
            var cnt = 0;
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    cnt++;

                    json += (new JavaScriptSerializer() { }).Serialize(item);

                    if (cnt != obj.Count)
                    {
                        json += " , ";
                    }

                }
            }
            json += "]";

            return json;
        }
        //-------------- Error Display  
        public bool isfinal = true;
        public string client = "JC";
        public void Clear(string cl)
        { 
            try
            {
                var a = (new Sqlbase<Logg>());
                var c = a.Read();
                var d = c.Where(item => item.client == cl);
                foreach (var b in d)
                {
                    (new Sqlbase<Logg>()).Delete(b.Idx);
                }
            }
            catch (Exception ex)
            {
                if (!isfinal)
                {
                    new LogLogger().Log(ex.Message);
                }
            }
        }
        public void Log(string cl, string msg)
        {
            try
            {
                var k = (new Sqlbase<Logg>()).Read().Select(item => item.Idx);
                var i = (k.Count() == 0) ? 1 : Int32.Parse(k.Max()) + 1;
                var a = new Logg()
                {
                    Idx = i.ToString(),
                    client = cl,
                    exception = msg,
                    datetime = DateTime.Now.ToString()
                };
                var param = new JC<Logg>().ToJson(a);
                new Sqlbase<Logg>().Create(param);
            }
            catch (Exception ex)
            {
                if (!isfinal)
                {
                    new LogLogger().Log(ex.Message);
                }
            }
        }
        public void Show(string message)
        {
            var a = message.IndexOf("Logg").ToString();
            if(a != "0")
            {
                var b = message.IndexOf("JCLog_");
                if (b != -1)
                {
                    if (!isfinal)
                    {
                        Log(client, "JCLog_" + message);
                        new LogLogger().Log("JCLog_" + message);
                    }
                    else
                    {
                        Log(client, "JCLog_" + message);
                    }
                } 
            }
        }
        //
    }
}
