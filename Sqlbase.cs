
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using Web.Deploy.Views.Layout;

namespace Web.Deploy
{
    public class Sqlbase<T>
    {
        //:: sqlserver
        private string sql = "";
        private string connetionString = @"Data Source=sql.bsite.net\MSSQL2016;Initial Catalog=sammyguy7_;Persist Security Info=True;User ID=sammyguy7_;Password=Sammyguy7";
        // Constructor
        public Sqlbase()
        {
            // Create Table
            try
            {
                //new JC<int>().Show("a");
                sql = "Select * from " + typeof(T).Name;

                var connection = new SqlConnection(connetionString);
                connection.Open();
                var command = new SqlDataAdapter(sql, connection);

                DataTable datatbl = new DataTable();
                command.Fill(datatbl);
                var a = datatbl.Rows.Count.ToString();

                command.Dispose();
                connection.Close();
                //new JC<int>().Show("b");
            }
            catch (Exception ex1)
            {
                new JC<int>().Show(ex1.Message);

                try
                {
                    sql = "CREATE TABLE " + typeof(T).Name + "( ";
                    PropertyInfo[] myPropertyInfo = typeof(T).GetProperties();
                    for (int i = 0; i < myPropertyInfo.Length; i++)
                    {
                        var a = "c_" + myPropertyInfo[i].Name;

                        if (a == "Idx")
                        {
                            sql += a + " INTEGER PRIMARY KEY ";
                        }
                        else
                        {
                            sql += a + " VARCHAR(MAX) ";
                        }

                        if (i != myPropertyInfo.Length - 1)
                        {
                            sql += " , ";
                        }
                    }
                    sql += " )";

                    var connection = new SqlConnection(connetionString);
                    connection.Open();

                    var cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    connection.Close();
                }
                catch (Exception ex2)
                {
                    new JC<int>().Show("CN > " + ex2.Message);
                }
            }
        }
        // Methods
        public void Create(string data)
        {
            var inp = new JC<T>().ToObj(data);
            sql = "INSERT INTO " + typeof(T).Name + "( ";
            PropertyInfo[] myPropertyInfo2 = typeof(T).GetProperties();
            for (int i = 0; i < myPropertyInfo2.Length; i++)
            {
                sql += "c_" + myPropertyInfo2[i].Name;

                if (i != myPropertyInfo2.Length - 1)
                {
                    sql += " , ";
                }
            }
            sql += " ) VALUES(";
            for (int i = 0; i < myPropertyInfo2.Length; i++)
            {
                var z = myPropertyInfo2[i].GetValue(inp, null);
                var a = (z == null) ? "null" : z.ToString();

                if (z != null)
                {
                    if (z.GetType().FullName.Contains("List"))
                    {
                        a = (new JavaScriptSerializer()).Serialize(z);
                    }
                }

                sql += "'" + a + "'";

                if (i != myPropertyInfo2.Length - 1)
                {
                    sql += " , ";
                }
            }
            sql += " )";

            new JC<int>().Show(sql);
            try
            {
                var connection = new SqlConnection(connetionString);
                connection.Open();

                var cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                new JC<int>().Show("Insert > " + t);
                // Error 1
                var t1 = "Invalid column name";
                if(t.Contains(t1)) 
                {
                    t = t.Replace("'", "");
                    try
                    {
                        var col = t.Substring(t1.Length, t.Length - t1.Length - 1).Trim();
                        new JC<int>().Show(col);
                        sql = "ALTER TABLE " + typeof(T).Name + " ADD " + col ;

                        if (col == "c_Idx")
                        {
                            sql += " INTEGER DEFAULT 0 NOT NULL";
                        }
                        else
                        {
                            sql += " VARCHAR(MAX) NULL";
                        }

                        new JC<int>().Show(sql);

                        var connection = new SqlConnection(connetionString);
                        connection.Open();

                        var cmd = new SqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();

                        cmd.Dispose();
                        connection.Close();

                        Create(data);
                    } 
                    catch(Exception ex3)
                    {
                        new JC<int>().Show("Insert > Add Col : " + ex3.Message);
                    }
                }
            }
        }
        public List<T> Read()
        {
            var list = new List<T>();
            PropertyInfo[] myPropertyInfo3 = typeof(T).GetProperties();
            sql = "Select * from " + typeof(T).Name;
            try
            {
                var connection = new SqlConnection(connetionString);
                connection.Open();
                var cmd = new SqlCommand(sql, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                sql = "[";
                while (dr.Read())
                {
                    //---
                    sql += "{";
                    for (int i = 0; i < myPropertyInfo3.Length; i++)
                    {
                        var z = dr["c_" + myPropertyInfo3[i].Name];

                        if (myPropertyInfo3[i].PropertyType.FullName.Contains("List"))
                        {
                            sql += myPropertyInfo3[i].Name + " : " + z.ToString();
                        }
                        else if (z.ToString() == "null")
                        {
                            sql += myPropertyInfo3[i].Name + " : " + z.ToString();
                        }
                        else
                        {
                            sql += myPropertyInfo3[i].Name + " : '" + z.ToString() + "'";
                        }

                        if (i != myPropertyInfo3.Length - 1)
                        {
                            sql += " ,";
                        }
                    }
                    sql += "}";
                    //--- 

                    sql += " ,";
                }
                sql += "]";
                sql = sql.Replace(",}", "}");
                sql = sql.Replace(",]", "]");

                new JC<int>().Show(typeof(T).Name + " > " + sql);
                list = JsonConvert.DeserializeObject<List<T>>(sql);

                cmd.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                new JC<int>().Show("Read > " + typeof(T).Name + " >> " + ex.Message);
            }

            return list;
        }
        public void Update(string inp) 
        {
            var data = new JC<T>().ToObj(inp);

            sql = "UPDATE " + typeof(T).Name + " SET ";
            var myPropertyInfo4 = data.GetType().GetProperties();
            for (int i = 0; i < myPropertyInfo4.Length; i++)
            {
                var a = myPropertyInfo4[i].Name;
                sql += "c_" + a + "='@c_" + a + "'";

                if (i != myPropertyInfo4.Length - 1)
                {
                    sql += " , ";
                }
            }
            sql += " WHERE c_Idx='@c_Idx'";
            new JC<int>().Show(sql);

            for (int i = 0; i < myPropertyInfo4.Length; i++)
            {
                var z = myPropertyInfo4[i].GetValue(data);
                var a = (z == null) ? "null" : z.ToString();

                if (z != null)
                {
                    if (z.GetType().FullName.Contains("List"))
                    {
                        a = (new JavaScriptSerializer()).Serialize(z);
                    }
                }

                new JC<int>().Show(myPropertyInfo4[i].Name + " : " + a);

                var b = "@c_" + myPropertyInfo4[i].Name;
                if (b == "@c_Idx")
                {
                    sql = sql.Replace(b, a);
                }
                else
                {
                    sql = sql.Replace(b, a);
                }
            }

            new JC<int>().Show(sql);
            try
            {
                var connection = new SqlConnection(connetionString);
                connection.Open();

                var cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            { 
                var t = ex.Message;
                new JC<int>().Show("Update > " + t);
                // Error 1
                var t1 = "Invalid column name";
                if (t.Contains(t1))
                {
                    t = t.Replace("'", "");
                    try
                    {
                        var col = t.Substring(t1.Length, t.Length - t1.Length - 1).Trim();
                        new JC<int>().Show(col);
                        sql = "ALTER TABLE " + typeof(T).Name + " ADD " + col;

                        if (col == "c_Idx")
                        {
                            sql += " INTEGER DEFAULT 0 NOT NULL";
                        }
                        else
                        {
                            sql += " VARCHAR(MAX) NULL";
                        }

                        new JC<int>().Show(sql);

                        var connection = new SqlConnection(connetionString);
                        connection.Open();

                        var cmd = new SqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();

                        cmd.Dispose();
                        connection.Close();

                        Update(inp);
                    }
                    catch (Exception ex3)
                    {
                        new JC<int>().Show("Insert > Add Col : " + ex3.Message);
                    }
                }
            }
        }
        public void Delete(string Idx) 
        {
            sql = "DELETE FROM " + typeof(T).Name + " WHERE c_Idx='@c_Idx'";
            sql = sql.Replace("@c_Idx", Idx);

            new JC<int>().Show(sql);
            try
            {
                var connection = new SqlConnection(connetionString);
                connection.Open();

                var cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                new JC<int>().Show("Delete > " + ex.Message);
            }
        }
        public void DeleteAll() 
        {
            sql = "TRUNCATE TABLE " + typeof(T).Name;

            new JC<int>().Show(sql);
            try
            {
                var connection = new SqlConnection(connetionString);
                connection.Open();

                var cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                new JC<int>().Show("Delete All > " + ex.Message);
            }
        }
    }
}
