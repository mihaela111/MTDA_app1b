using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace MTDA_app1b
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //stringul de conexiune 2 be updated with my own connection string
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-SVF1EHF\\SQLEXPRESS;" +
                "Initial Catalog=\'BPersonal\';" +
                "Integrated Security=True");

            /*stabilirea cerintelor pe baza de date, conform problemei*/
            //pe fiecare profesie in parte, numarul de salariati, fondul de salarii si  media salariala
            string a = "SELECT Profesia, COUNT(*) AS NumarSalariati, SUM(Salariu) AS FondSalarii, AVG(Salariu) AS SalariuMediu" + " FROM Salariati" + " GROUP BY Profesia";
            //numarul total de salariati din companie si fondul general de salarii
            SqlCommand[] b = new SqlCommand[2];
            b[0] = new SqlCommand("SELECT COUNT(*) AS NumarTotalSalariati " + "FROM Salariati");
            b[1] = new SqlCommand("SELECT SUM(Salariu) AS FondGeneralSalarii " + "FROM Salariati");

            /*solutionarea propriu-zisa a problemei*/
            try
            {
                //deschiderea conexiunii
                con.Open();
                //incarcarea datelor din baza de date in mijlocilor DataAdapter
                SqlDataAdapter da = new SqlDataAdapter(a, con);
                //se construieste obiectul DataSet
                DataTable dt = new DataTable();
                //se transmite informatia dinspre mijlocitor spre obiectul DataSet (DataTable) - se va folosi metoda Fill a unui obiect DataAdapter
                da.Fill(dt);
                //se parcurge intregul continut din DataTable - vom utliza subclasa DataRow
                foreach (DataRow dr in dt.Rows)
                    Console.WriteLine("Profesia: {0}\tNumar salariati: {1}\tFond salarii: {2}\tMedia salariala: {3}", dr["Profesia"], dr["NumarSalariati"], dr["FondSalarii"], dr["SalariuMediu"]);
                dt.Clear();
                //partea a doua a cerintei, si anume, numarul total de sariati si fondul general de salarii
                b[0].Connection = con;
                b[1].Connection = con;
                da = new SqlDataAdapter(b[0]);
                da.Fill(dt);
                da = new SqlDataAdapter(b[1]);
                da.Fill(dt);
                //transmit pe ecran cele doua situatii cerute
                Console.WriteLine("\nNumarul total de salariati din companie: " + dt.Rows[0]["NumarTotalSalariati"].ToString());
                Console.WriteLine("\nFondul general de salarii: " + dt.Rows[1]["FondGeneralSalarii"].ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Baza de date s-a deschis cu eroarea: " + ex.Message);
                Console.Read();
            }
            finally
            {
                con.Close();
            }
            Console.ReadKey();

        }
    }
}
