using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace GlobalDataAccess
{
    public class ADODataAccess
    {
        private string _connectionString;
        private SqlConnection _conn;
        public ADODataAccess(string connectionString)
        {
            _connectionString = connectionString;
            _conn = new SqlConnection(_connectionString);
        }


        public bool RunQuery(DataSet ds, string sql)
        {
            bool ret = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                ret = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error running RunQuery method", ex);
            }
            return ret;
        }
        public bool ExecuteStoredProc(DataSet ds, string sql, List<SqlParameter> paramList)
        {
            bool ret = false;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sql;

            try
            {
                if (paramList != null && paramList.Count > 0)
                {
                    foreach (SqlParameter p in paramList)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                ret = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error running ExecuteStoredProc method", ex);
            }
            return ret;
        }

        public bool ExecuteNonQuery(string sql, List<SqlParameter> paramList)
        {
                        bool ret = false;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sql;

            try
            {
                if (paramList != null)
                {
                    foreach (SqlParameter p in paramList)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
                ret = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error running ExecuteNonQuery method", ex);
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return ret;
        }

        //protected override void RunStoredProc(DataSet ds, string sql, List<SqlParameter> paramList)
        //{
        //    int returnValue = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = _conn;
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    if (paramList != null)
        //    {
        //        foreach (SqlParameter p in paramList)
        //        {
        //            cmd.Parameters.Add(p);
        //        }
        //    }
        //    int.TryParse(cmd.ExecuteScalar().ToString(),out returnValue);
        //}
    }
}
