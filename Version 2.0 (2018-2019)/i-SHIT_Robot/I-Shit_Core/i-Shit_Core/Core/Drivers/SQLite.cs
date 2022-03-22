using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    public static partial class Driver
    {
        public static SQLiteConnection SQLite_OpenDB(string DBName)
        {
            string path = @"..\Data\" + DBName + ".db";
            SQLiteConnection cn = new SQLiteConnection("data source=" + path);
            cn.Open();
            return cn;
        }

        


        public static void SQLite_Insert(SQLiteConnection connection, string tablename, string[] column, string[] data)
        {
            if (column.Length != data.Length) { Console.WriteLine("SQLite Insert Error: Count of Column and Data NOT matched!"); return; }
            string queryString = @"insert into ";
            queryString += tablename + " (";
            //遍历每个colunm，生成sql语句
            foreach (string item in column)
            {
                queryString += (item + ",");

            }
            queryString = queryString.Substring(0, queryString.Length - 1);//去掉最后一个逗号
            //--遍历每个colunm，生成sql语句
            queryString += " ) values (";
            //遍历每个data，生成sql语句
            foreach (string item in data)
            {
                queryString += ("'" + item + "',");

            }
            queryString = queryString.Substring(0, queryString.Length - 1);//去掉最后一个逗号
            //--遍历每个data，生成sql语句
            queryString += ")";

            Console.WriteLine("SQLite Insert: " + queryString);
            new SQLiteCommand(queryString, connection).ExecuteNonQuery();//执行
            return;
        }

        public static DataSet SQLite_Query(SQLiteConnection connection, string tablename, string[] condition_column, string[] condition_columnData)
        {
            if (condition_column.Length != condition_columnData.Length) { Console.WriteLine("SQLite Query Error: Count of Column and Data NOT matched!"); return null; }
            string queryString = "SELECT * FROM " + tablename + " WHERE ";

            //遍历每个column和columnData，生成where条件
            for (int i = 0; i < condition_column.Length; i++)
            {
                queryString += " " + condition_column[i] + "='" + condition_columnData[i] + "' AND";
            }
            queryString = queryString.Substring(0, queryString.Length - 3);//去掉最后一个AND
            //--遍历每个column和columnData，生成where条件


            Console.WriteLine("SQLite Query: " + queryString);

            DataSet ds = new DataSet();
            try
            {
                SQLiteDataAdapter sr = new SQLiteDataAdapter(queryString, connection);
                sr.Fill(ds);
            }
            catch { }
            return ds;
        }

        public static void SQLite_Delete(SQLiteConnection connection, string tablename, string[] condition_column, string[] condition_columnData)
        {
            if (condition_column.Length != condition_columnData.Length) { Console.WriteLine("SQLite Query Error: Count of Column and Data NOT matched!"); return; }
            string queryString = "DELETE FROM " + tablename + " WHERE ";

            //遍历每个column和columnData，生成where条件
            for (int i = 0; i < condition_column.Length; i++)
            {
                queryString += " " + condition_column[i] + "='" + condition_columnData[i] + "' AND";
            }
            queryString = queryString.Substring(0, queryString.Length - 3);//去掉最后一个AND
            //--遍历每个column和columnData，生成where条件
            Console.WriteLine("SQLite Delete:" + queryString);


            new SQLiteCommand(queryString, connection).ExecuteNonQuery();
        }

        public static void SQLite_Update(SQLiteConnection connection, string tablename, string[] target_column, string[] target_data, string[] condition_column, string[] condition_columnData)
        {
            if (condition_column.Length != condition_columnData.Length) { Console.WriteLine("SQLite Query Error: Count of Column and Data NOT matched!"); return; }
            string queryString = "UPDATE " + tablename + " SET ";


            //遍历每个column和columnData，生成SET
            for (int i = 0; i < target_column.Length; i++)
            {
                queryString += target_column[i] + "='" + target_data[i] + "',";
            }
            queryString = queryString.Substring(0, queryString.Length - 1);//去掉最后一个逗号
            //--遍历每个column和columnData，生成SET

            queryString += " WHERE ";

            //遍历每个column和columnData，生成where条件
            for (int i = 0; i < condition_column.Length; i++)
            {
                queryString += " " + condition_column[i] + "='" + condition_columnData[i] + "' AND";
            }
            queryString = queryString.Substring(0, queryString.Length - 3);//去掉最后一个AND
            //--遍历每个column和columnData，生成where条件

            Console.WriteLine("SQLite Update:" + queryString);

            new SQLiteCommand(queryString, connection).ExecuteNonQuery();
        }

    }
}
