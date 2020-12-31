using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;


namespace GestioLib
{
    public class Gestio
    {
        SQLiteConnection cnx;
        string nom, datahora;
        int alç, amp;

        public void GetDades(string nom, int alç, int amp) {
            this.nom = nom;
            this.alç = alç;
            this.amp = amp;
        }

        //Iniciar la conexiío amb la base de dades "baseimatges"
        public int Conectar() {
            string DataSource = "Data Source = ..\\..\\..\\DATABASE\\baseimatge.db";
            cnx = new SQLiteConnection(DataSource);
            try
            {
                cnx.Open();
                return 0;
            }
            catch {
                return -1;
            }
        }

        //Desconectar de la base de dades
        public void Desconectar() {
            cnx.Close();
        }

        //Agafar de la base de dades
        public DataTable GetDadesImatges() {
            SQLiteDataAdapter adp;
            DataTable dt = new DataTable();
            string sql;
            sql = "SELECT * FROM imatges";
            adp = new SQLiteDataAdapter(sql, cnx);
            adp.Fill(dt);
            return dt;
        }
        
        //Insertar a la base de dades
        public void InsertarDades(string n, int alç, int amp, string datahora) {
            SQLiteDataAdapter adp;
            DataTable dt = new DataTable();
            string sql;
            sql = "SELECT * FROM imatges WHERE NOM = '"+n+"'";
            adp = new SQLiteDataAdapter(sql, cnx);
            adp.Fill(dt);
            if (dt.Rows.Count == 0)
            { // Insertar si no hi es
                SQLiteCommand command;
                sql = "INSERT INTO imatges VALUES ('" + n + "', " + alç + ", " + amp + ", '" + datahora + "')";
                command = new SQLiteCommand(sql, cnx);
                command.ExecuteNonQuery();
            }
            else { //Actualitzar
                SQLiteCommand command;
                sql = "UPDATE imatges SET alçada = "+alç+", amplada = "+amp+", datahora = '"+datahora+"' WHERE NOM = '"+n+"'";
                command = new SQLiteCommand(sql, cnx);
                command.ExecuteNonQuery();
            }
        }		

        //Mostra en la base de dades tots els fitxer amb el mateix nom que ha introduit l'usuari
        public DataTable MostrarPerNom_Dades(string n) {
            SQLiteDataAdapter adp;
            DataTable dt = new DataTable();
            string sql;
            sql = "SELECT * FROM imatges WHERE NOM = '" + n + "'";
            adp = new SQLiteDataAdapter(sql, cnx);
            adp.Fill(dt);
            return dt;
        }

        //Mostra en la base de dades tots els fitxers modificats a la mateixa data que ha introduit l'usuari
        public DataTable MostrarPerData_Dades(string data) {
            SQLiteDataAdapter adp;
            DataTable dt = new DataTable();
            string sql;
            sql = "SELECT * FROM imatges WHERE datahora = '" + data + "'";
            adp = new SQLiteDataAdapter(sql, cnx);
            adp.Fill(dt);
            return dt;
        }

        //Agafar la Data de la base de dades
        public string GetDataHora (string n){

            SQLiteDataAdapter adp;
            DataTable dt = new DataTable();
            string sql;
            sql = "SELECT datahora FROM imatges WHERE NOM = '" + Path.GetFileName(n) + "'";
            adp = new SQLiteDataAdapter(sql, cnx);
            adp.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else {
                string datahora = dt.Rows[0]["datahora"].ToString();
                return datahora;
            }
        }
        
        //Eliminar dades del registre
        public void EliminarDades(string nom) {
            
            string sql;
            sql = "DELETE FROM imatges WHERE NOM = '" + nom + "'";
            SQLiteCommand command;
            command = new SQLiteCommand(sql, cnx);
            command.ExecuteNonQuery();
        }

        //-----------------------Ampliació 2 Albert Compte----------------
        //Eliminar dades anteriors dades
        public int EliminarPerData(string data)
        {

            string sql;
            sql = "DELETE FROM imatges WHERE datahora < '" + data+ "'";
            SQLiteCommand command;
            command = new SQLiteCommand(sql, cnx);
            int canvis = command.ExecuteNonQuery();
            return canvis;
        }

    }
}

