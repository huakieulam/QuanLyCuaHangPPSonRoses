using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace QuanLyCuaHangPPSonRoses.DK_DN_QMK
{
    internal class KetNoiDuLieu
    {
        private static KetNoiDuLieu instance;
        public static KetNoiDuLieu Instance
        {
            get { if (instance == null) instance = new KetNoiDuLieu(); return KetNoiDuLieu.instance; }
            set { KetNoiDuLieu.instance = value; }
        }
        private KetNoiDuLieu()
        {

        }
        public string connectionSTR = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=123;";
        public DataTable ExcuteQuery(string query, object[] para = null)
        {
            DataTable data = new DataTable();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionSTR))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (para != null)
                    {
                        string[] listPara = query.Split(' ');

                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains("@"))
                            {
                                command.Parameters.AddWithValue(item, para[i]);
                                i++;
                            }
                        }
                    }

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);

                    adapter.Fill(data);
                }
            }
            return data;
        }

        public int ExcuteNonQuery(string query, object[] para = null)
        {
            int data = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionSTR))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (para != null)
                    {
                        string[] listPara = query.Split(' ');

                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains("@")) // phần tử nào chứa @ thì là tham số
                            {
                                command.Parameters.AddWithValue(item, para[i]);
                                i++;
                            }
                        }
                    }

                    data = command.ExecuteNonQuery();
                }
            }
            return data;
        }

        public object ExcuteScalar(string query, object[] para = null)
        {
            object data = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionSTR))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (para != null)
                    {
                        string[] listPara = query.Split(' ');

                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains("@"))
                            {
                                command.Parameters.AddWithValue(item, para[i]);
                                i++;
                            }
                        }
                    }

                    data = command.ExecuteScalar();
                }
            }
            return data;
        }
    }
}