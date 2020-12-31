using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PixelLib;
using System.Drawing;
using GestioLib;
using System.Data.SQLite;
using System.Media;
using System.Threading;

namespace ImagenLib
{
    public class Imagen
    {
        //~~Atributs~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private string nomfitxer, identificador;
        private int alçada, amplada;
        private int prevalçada, prevamplada;
        private byte nivell;
        private Pixel[,] dades;
        private Pixel[,] previsualització;
        Gestio mi_base = new Gestio();

        //Atributs Navi
        SoundPlayer VeuNavi = new SoundPlayer();
        string RutaNavi = "Navi\\Veu\\";
        string[] AudioNavi = { "Hello.wav", "Hey.wav", "Listen.wav", "Look.wav", "Watch Out.wav", "Flotar.wav", "Entra.wav", "Surt.wav", "Golpe.wav", "CritCop.wav" };

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //~~Metode~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Obtenir/Donar valors
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public int GetAlçada() 
        {
            return this.alçada;
        }

        public int GetAmplada() 
        {
            return this.amplada;
        }

        public string GetNomFitxer()
        {
            return this.nomfitxer;
        }

        public Pixel[,] GetMatriu()
        {
            return this.dades;
        }

        public int GetAmpladaPrev()
        {
            return this.prevamplada;
        }

        public int GetAlçadaPrev()
        {
            return this.prevalçada;
        }

        public Pixel[,] GetMatriuPrev()
        {
            return this.previsualització;
        }

        public void SetMatriu(Pixel[,] matriu, int alçada, int amplada)
        {
            this.dades = matriu;
            this.alçada = alçada;
            this.amplada = amplada;
        }

        public void SetMatriuPrev(Pixel[,] matriu, int alçada, int amplada)
        {
            this.previsualització = matriu;
            this.prevalçada = alçada;
            this.prevamplada = amplada;
        }

        public byte GetNivell()
        {
            return this.nivell;
        }

        public string GetNom()
        {
            return this.nomfitxer;
        }

        public string GetIdentificador()
        {
            return this.identificador;
        }

        public void SetDadesPPM(byte nivell, string nomfitxer, string identificador)
        {
            this.nivell = nivell;
            this.nomfitxer = nomfitxer;
            this.identificador = identificador;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Desar documents
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Desat general
        public int GuardarGeneral(string nom) 
        {
            if (Path.GetExtension(nom) == ".ppm" || Path.GetExtension(nom) == ".PPM") {
                GuardarComoPPM(nom);
                mi_base.Conectar();
                mi_base.InsertarDades(Path.GetFileName(nom), this.alçada, this.amplada, DateTime.Now.ToString("yyyy/MM/dd"));
                mi_base.Desconectar();
                return 0;
            }
            else if (Path.GetExtension(nom) == ".jpg" || Path.GetExtension(nom) == ".JPG" || Path.GetExtension(nom) == ".png" || Path.GetExtension(nom) == ".PNG" || Path.GetExtension(nom) == ".jpeg" || Path.GetExtension(nom) == ".JPEG")
            {
                GuardarComoJPG_PNG(nom);
                mi_base.Conectar();
                mi_base.InsertarDades(Path.GetFileName(nom), this.alçada, this.amplada, DateTime.Now.ToString("yyyy/MM/dd"));
                mi_base.Desconectar();
                return 1;
            }
            else {//No guarda imatge
                return -1;
            }

        }

        //Funció per guardar la imatge png i jpg
        public int GuardarComoJPG_PNG(string nom) {
            Bitmap btp = CrearBitmap();
            btp.Save(nom);
            return 0;
        }

        //Funcion para guardar la imagen en ppm
        public int GuardarComoPPM(string nom)
        {
            if (this.amplada == 0 || this.alçada == 0)
                return -1;

            
            try
            {
                StreamWriter E = new StreamWriter(nom);
                E.WriteLine(this.identificador);
                E.WriteLine("{0} {1}", this.amplada, this.alçada);
                E.WriteLine(this.nivell);

                for (int i = 0; i < this.alçada; i++)
                {
                    E.Write("{0} {1} {2}", this.dades[i,0].GetR(), this.dades[i,0].GetG(), this.dades[i,0].GetB());
                
                    for (int j = 1; j < this.amplada; j++)
                    {
                        E.Write(" ");
                        E.Write("{0} {1} {2}", this.dades[i, j].GetR(), this.dades[i, j].GetG(), this.dades[i, j].GetB());
                    }

                    E.WriteLine();
                }

                E.Close();
                return 0;
            }

            catch (IOException)
            {
                return -2;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("ERROR! No has introduït res");
                return -3;
            }
        }

        //Funció per guardar la imatge de forma directe sense guardar
        public int GuardarGeneral()
        {
            return (GuardarGeneral(this.nomfitxer));
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Carregar documents
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Càrrega de qualsevol imatge
        public int CarregarGeneral(string nom) {
            if (Path.GetExtension(nom) == ".ppm" || Path.GetExtension(nom) == ".PPM") { 
                int res = CarregarPPM(nom);
                if (res == -1)
                    return -1;
                else if (res == -2)
                    return -2;
                else if (res == -3)
                    return -3;
                else if (res == -4)
                    return -4;
                else if (res == -5)
                    return -5;
                else if (res == -6)
                    return -6;
                else
                    return 0;
            }
            else if (Path.GetExtension(nom) == ".jpg" || Path.GetExtension(nom) == ".JPG" || Path.GetExtension(nom) == ".png" || Path.GetExtension(nom) == ".PNG" || Path.GetExtension(nom) == ".jpeg" || Path.GetExtension(nom) == ".JPEG")
            {
                CarregarJPG_PNG(nom);
                return 1;
            }
            else { //No carrega imatge
                return -2;
            }
            
        }

        //Càrrega del png i jpg
        public int CarregarJPG_PNG(string nom) 
        {

            Bitmap btp = new Bitmap(nom);
            this.nomfitxer = nom;
            this.amplada = btp.Width;
            this.alçada = btp.Height;
            this.identificador = "P3";
            this.nivell = 255;
            this.dades = new Pixel[alçada, amplada];

            //Definir el color de cada pixel
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i,j] = new Pixel();
                    Color micolor = btp.GetPixel(j, i);
                    this.dades[i, j].SetR(micolor.R);
                    this.dades[i, j].SetG(micolor.G);
                    this.dades[i, j].SetB(micolor.B);
                }
            }

            btp.Dispose();

            return 0;

        }
        // Funcion para cargar una imagen, pedida por pantalla
        public int CarregarPPM(string nom) {

            StreamReader F;
            try
            {
                F = new StreamReader(nom);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR! fitxer no trobat");
                return -1;
            }
            catch (FileLoadException)
            {
                Console.WriteLine("ERRROR! El format de fitxer no és correcte");
                return -2;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("ERROR! No has introduït res");
                return -3;
            }

            this.nomfitxer = nom;
            this.identificador = F.ReadLine();
            if (this.identificador != "P3") {
                F.Close();
                return -3;
            }
            string [] mides= new string [2];
            string [] fila;
            mides= F.ReadLine().Split(' ');
            this.amplada = Convert.ToInt32(mides[0]);
            this.alçada = Convert.ToInt32(mides[1]);
            dades = new Pixel[this.alçada, this.amplada];
            try
            {
                this.nivell = Convert.ToByte(F.ReadLine());
            }
            catch(FormatException)
            {
                F.Close();
                return -4;
            }
            string n= F.ReadLine();
            for (int i = 0; n != null; i++) {
                fila = n.Split(' ');
                if (fila.Length % 3 != 0) {
                    F.Close();
                    return -5;
                }
                for (int j = 0; j < fila.Length; j=j+3) {
                    try
                    {
                        this.dades[i, j/3] = new Pixel();
                        this.dades[i, j/3].SetR(Convert.ToByte(fila[j]));
                        this.dades[i, j/3].SetG(Convert.ToByte(fila[j + 1]));
                        this.dades[i, j/3].SetB(Convert.ToByte(fila[j + 2]));
                    }
                    catch (FormatException) {
                        F.Close();
                        return -6;
                    }
                    
                }
                n=F.ReadLine();
            }
            return 0;
        }


        // Funcion para modificar un pixel, pideindo posicion i valor por pantalla
        public void Pixel(int x, int y, byte R, byte G, byte B)
        {
            this.dades[x, y].SetR(R);
            this.dades[x, y].SetG(G);
            this.dades[x, y].SetB(B);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Simetria
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Funcion para girar verticalmente la imagen
        public void Simetria_Vertical()
        {
            
            Pixel[,] matriu = new Pixel[this.alçada,this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[i, j] = this.dades[i, this.amplada-1-j];

                }
            }

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];

                }
            }
            
        }

        // Función para girar horizontalmente la imagen
        public void Simetria_Horitzontal()
        {

            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[i, j] = this.dades[this.alçada - 1 - i, j];

                }
            }

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];

                }
            }

        }

        //Funcion pra rotar la imagen 90 grados en sentido horario
        public void Rotar90()
        {

            Pixel[,] matriu = new Pixel[this.amplada, this.alçada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[j, i] = this.dades[i, this.amplada - 1 - j];

                }
            }

            int ampladaRot = this.alçada;
            int alçadaRot = this.amplada;
            this.alçada = alçadaRot;
            this.amplada = ampladaRot;
            this.dades = matriu;

        }

        //Funcion pra rotar la imagen 90 grados en sentido antihorario
        public void RotarN90()
        {

            Pixel[,] matriu = new Pixel[this.amplada, this.alçada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[j, i] = this.dades[this.alçada -1 - i,j];

                }
            }

            int ampladaRot = this.alçada;
            int alçadaRot = this.amplada;
            this.alçada = alçadaRot;
            this.amplada = ampladaRot;
            this.dades = matriu;

        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Capçalera i Console
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Funcion para mostrar la cabezera de la imagen
        public void Capçalera()
        {
            Console.WriteLine(this.identificador);
            Console.WriteLine(this.amplada + " " + this.alçada);
            Console.WriteLine(this.nivell);
            Console.WriteLine();

        }

        public string GetCapçalera()
        {
            Gestio dh = new Gestio();
            dh.Conectar();
            string datahora = dh.GetDataHora(this.nomfitxer);
            if (datahora == null)
            {
                string capçalera = "Identificador: " + this.identificador + " || Amplada: " + this.amplada + " px | Alçada: " + this.alçada + " px || Nivell: " + this.nivell;
                dh.Desconectar();
                return capçalera; ;

            }
            else
            {
                string capçalera = "Identificador: " + this.identificador + " || Amplada: " + this.amplada + " px | Alçada: " + this.alçada + " px || Nivell: " + this.nivell + " || Data i Hora : " + datahora;
                dh.Desconectar();
                return capçalera;
            }
        }
        
        //Mostrar matriu
        public int MostrarMatriu()
        {
            if (this.amplada == 0 || this.alçada == 0){
                return -1;
            }
            for (int i = 0; i < this.alçada; i++){
                Console.Write("{0} {1} {2}", this.dades[i,0].GetR(), this.dades[i, 0].GetG(), this.dades[i, 0].GetB());
                for (int j = 1; j < this.amplada; j++){
                    Console.Write(" ");
                    Console.Write("{0} {1} {2}", this.dades[i, j].GetR(), this.dades[i, j].GetG(), this.dades[i, j].GetB());
                }
                Console.WriteLine();
            }
            return 0;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Modifica Bitmap
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Crear Bitmap
        public Bitmap CrearBitmap()
        {
            //Crear un Bitmap de la imatge
            Bitmap bmp = new Bitmap(this.amplada, this.alçada);

            //Definir el color de cada pixel
            for(int i = 0; i < this.alçada; i++)
            {
                for(int j = 0; j < this.amplada; j++)
                {
                    
                    Color micolor = Color.FromArgb(this.dades[i,j].GetR(), this.dades[i,j].GetG(),this.dades[i,j].GetB());
                    bmp.SetPixel(j, i, micolor);
                }
            }
            return bmp;
        }

        //CrearPrevBitmap
        public Bitmap CrearPrevBitmap()
        {
            //Crear un Bitmap de la imatge
            Bitmap bmp = new Bitmap(this.prevamplada, this.prevalçada);

            //Definir el color de cada pixel
            for (int i = 0; i < this.prevalçada; i++)
            {
                for (int j = 0; j < this.prevamplada; j++)
                {

                    Color micolor = Color.FromArgb(this.previsualització[i, j].GetR(), this.previsualització[i, j].GetG(), this.previsualització[i, j].GetB());
                    bmp.SetPixel(j, i, micolor);
                }
            }
            return bmp;
        }

        //Fes una còpia
        public Imagen Còpia()
        {
            Imagen còpia = new Imagen();
            Pixel[,] matriuCòpia = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriuCòpia[i, j] = new Pixel(Convert.ToByte(this.dades[i,j].GetR()),Convert.ToByte(this.dades[i,j].GetG()),Convert.ToByte(this.dades[i,j].GetB()));

                }
            }
            còpia.SetMatriu(matriuCòpia,this.alçada, this.amplada);
            Pixel[,] matriuCòpiaPrev = new Pixel[this.prevalçada, this.prevamplada];
            for (int i = 0; i < this.prevalçada; i++)
            {
                for (int j = 0; j < this.prevamplada; j++)
                {
                    matriuCòpiaPrev[i, j] = new Pixel(Convert.ToByte(this.previsualització[i, j].GetR()), Convert.ToByte(this.previsualització[i, j].GetG()), Convert.ToByte(this.previsualització[i, j].GetB()));

                }
            }
            còpia.SetMatriuPrev(matriuCòpiaPrev, this.prevalçada, this.prevamplada);
            còpia.SetDadesPPM(this.nivell, this.nomfitxer, this.identificador);
            return còpia;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Modifica colors
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Escala de grisos
        public void EscalaGrisos()
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    byte gris = Convert.ToByte((Convert.ToInt32(this.dades[i, j].GetR()) + Convert.ToInt32(this.dades[i, j].GetG()) + Convert.ToInt32(this.dades[i, j].GetB())) / 3);
                    matriu[i, j] = new Pixel(gris, gris, gris);
                }
            }
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];
                }
            }
        }

        //Fes un esbós
        //Passa la imatge en una escala de pocs grisos
        public void Esboix()
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    byte gris = Convert.ToByte((Convert.ToInt32(this.dades[i, j].GetR()) + Convert.ToInt32(this.dades[i, j].GetG()) + Convert.ToInt32(this.dades[i, j].GetB())) / 3);
                    if (gris >= 150)
                        matriu[i, j] = new Pixel(255, 255, 255);
                    if ((gris < 150) && (gris > 100))
                        matriu[i, j] = new Pixel(150, 150, 150);
                    if ((gris <= 100) && (gris > 50))
                        matriu[i, j] = new Pixel(100, 100, 100);
                    if (gris <= 50)
                        matriu[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];
                }
            }
        }

        
        //Modifica colors
        //Canvia el nivell d'un color
        public void Modifica(int r, int g, int b) {

            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            byte R, G, B;
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    //ROIG
                    
                    if ((r + Convert.ToInt32(this.dades[i, j].GetR())) > 255) {
                        R = 255;
                    }
                    else if ((r + Convert.ToInt32(this.dades[i, j].GetR())) < 0)
                    {
                         R = 0;
                    }
                    else {
                         R = Convert.ToByte(r + Convert.ToInt32(this.dades[i, j].GetR()));
                    }

                    //VERD
                    
                    if ((g + Convert.ToInt32(this.dades[i, j].GetG())) > 255)
                    {
                         G = 255;
                    }
                    else if ((g + Convert.ToInt32(this.dades[i, j].GetG())) < 0)
                    {
                         G = 0;
                    }
                    else
                    {
                        G = Convert.ToByte(g + Convert.ToInt32(this.dades[i, j].GetG()));
                    }

                    //BLAU
                    
                    if ((b + Convert.ToInt32(this.dades[i, j].GetB())) > 255)
                    {
                         B = 255;
                    }
                    else if ((b + Convert.ToInt32(this.dades[i, j].GetB())) < 0)
                    {
                         B = 0;
                    }
                    else
                    {
                         B = Convert.ToByte(b + Convert.ToInt32(this.dades[i, j].GetB()));
                    }

                    matriu[i, j] = new Pixel(R, G, B);
                }
            }

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];

                }
            }
        }

        //Inverteix colors
        public void Invertir()
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    byte R = Convert.ToByte(255 - Convert.ToInt32(this.dades[i, j].GetR()));
                    byte G = Convert.ToByte(255 - Convert.ToInt32(this.dades[i, j].GetG()));
                    byte B = Convert.ToByte(255 - Convert.ToInt32(this.dades[i, j].GetB()));
                    matriu[i, j] = new Pixel(R, G, B);
                }
            }

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];

                }
            }
        }

        //Lineat
        //Mira els pixels del seu voltant per decidir si és una linia o no
        //Provoca linies amb gruix de 2 pixels
        public int Lineat(int detall)
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            int area = this.alçada * this.amplada;
            int negre = 0;

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    int grisPixel = (Convert.ToInt32(this.dades[i, j].GetR()) + Convert.ToInt32(this.dades[i, j].GetG()) + Convert.ToInt32(this.dades[i, j].GetB())) / 3;

                    int grisPixelAmunt;
                    if (i != this.alçada -1)
                        grisPixelAmunt = (Convert.ToInt32(this.dades[i + 1, j].GetR()) + Convert.ToInt32(this.dades[i + 1, j].GetG()) + Convert.ToInt32(this.dades[i + 1, j].GetB())) / 3;
                    else
                        grisPixelAmunt = grisPixel;

                    int  grisPixelAvall;
                    if (i != 0)
                        grisPixelAvall = (Convert.ToInt32(this.dades[i-1, j].GetR()) + Convert.ToInt32(this.dades[i-1, j].GetG()) + Convert.ToInt32(this.dades[i-1, j].GetB())) / 3;
                    else
                        grisPixelAvall = grisPixel;

                    int grisPixelDret;
                    if (j != this.amplada - 1)
                        grisPixelDret = (Convert.ToInt32(this.dades[i, j+1].GetR()) + Convert.ToInt32(this.dades[i, j+1].GetG()) + Convert.ToInt32(this.dades[i, j+1].GetB())) / 3;
                    else
                        grisPixelDret = grisPixel;

                    int  grisPixelEsq;
                    if (j != 0)
                        grisPixelEsq = (Convert.ToInt32(this.dades[i, j-1].GetR()) + Convert.ToInt32(this.dades[i, j-1].GetG()) + Convert.ToInt32(this.dades[i, j-1].GetB())) / 3;
                    else
                        grisPixelEsq = grisPixel;

                    if (((Math.Abs(grisPixel - grisPixelAmunt) > detall) && (Math.Abs(grisPixel - grisPixelAmunt) < 255)) || ((Math.Abs(grisPixel - grisPixelAvall) > detall) && (Math.Abs(grisPixel - grisPixelAvall) < 255)) || ((Math.Abs(grisPixel - grisPixelDret) > detall) && (Math.Abs(grisPixel - grisPixelDret) < 255)) || ((Math.Abs(grisPixel - grisPixelEsq) > detall) && (Math.Abs(grisPixel - grisPixelEsq) < 255)))
                    {
                        matriu[i, j] = new Pixel(0, 0, 0);
                        negre++;
                    }
                    else if ((Math.Abs(grisPixel - grisPixelAmunt) == 255) || (Math.Abs(grisPixel - grisPixelAvall) == 255) || (Math.Abs(grisPixel - grisPixelDret) == 255) && (Math.Abs(grisPixel - grisPixelEsq) < 255))
                        matriu[i, j] = new Pixel(Convert.ToByte(grisPixel), Convert.ToByte(grisPixel), Convert.ToByte(grisPixel));
                    else
                        matriu[i, j] = new Pixel(255, 255, 255);
                }
            }
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];
                }
            }
            int percent = negre * 100 / area;

            return percent;
        }

        //Lineat amb color
        //Fa el mateix que el liniat però les arees amb blanc tindran la mitjana de la seva àrea

        public void LineatColor(int detall)
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            int area = this.alçada * this.amplada;
            int negre = 0;

            for (int i = 0; i < this.alçada; i++)
            {
                //Liniat sense els espais en blanc
                for (int j = 0; j < this.amplada; j++)
                {
                    int grisPixel = (Convert.ToInt32(this.dades[i, j].GetR()) + Convert.ToInt32(this.dades[i, j].GetG()) + Convert.ToInt32(this.dades[i, j].GetB())) / 3;

                    int grisPixelAmunt;
                    if (i != this.alçada - 1)
                        grisPixelAmunt = (Convert.ToInt32(this.dades[i + 1, j].GetR()) + Convert.ToInt32(this.dades[i + 1, j].GetG()) + Convert.ToInt32(this.dades[i + 1, j].GetB())) / 3;
                    else
                        grisPixelAmunt = grisPixel;

                    int grisPixelAvall;
                    if (i != 0)
                        grisPixelAvall = (Convert.ToInt32(this.dades[i - 1, j].GetR()) + Convert.ToInt32(this.dades[i - 1, j].GetG()) + Convert.ToInt32(this.dades[i - 1, j].GetB())) / 3;
                    else
                        grisPixelAvall = grisPixel;

                    int grisPixelDret;
                    if (j != this.amplada - 1)
                        grisPixelDret = (Convert.ToInt32(this.dades[i, j + 1].GetR()) + Convert.ToInt32(this.dades[i, j + 1].GetG()) + Convert.ToInt32(this.dades[i, j + 1].GetB())) / 3;
                    else
                        grisPixelDret = grisPixel;

                    int grisPixelEsq;
                    if (j != 0)
                        grisPixelEsq = (Convert.ToInt32(this.dades[i, j - 1].GetR()) + Convert.ToInt32(this.dades[i, j - 1].GetG()) + Convert.ToInt32(this.dades[i, j - 1].GetB())) / 3;
                    else
                        grisPixelEsq = grisPixel;

                    if (((Math.Abs(grisPixel - grisPixelAmunt) > detall) && (Math.Abs(grisPixel - grisPixelAmunt) < 255)) || ((Math.Abs(grisPixel - grisPixelAvall) > detall) && (Math.Abs(grisPixel - grisPixelAvall) < 255)) || ((Math.Abs(grisPixel - grisPixelDret) > detall) && (Math.Abs(grisPixel - grisPixelDret) < 255)) || ((Math.Abs(grisPixel - grisPixelEsq) > detall) && (Math.Abs(grisPixel - grisPixelEsq) < 255)))
                    {
                        matriu[i, j] = new Pixel(0, 0, 0);
                        negre++;
                    }
                    else if ((Math.Abs(grisPixel - grisPixelAmunt) == 255) || (Math.Abs(grisPixel - grisPixelAvall) == 255) || (Math.Abs(grisPixel - grisPixelDret) == 255) && (Math.Abs(grisPixel - grisPixelEsq) < 255))
                        matriu[i, j] = new Pixel(Convert.ToByte(grisPixel), Convert.ToByte(grisPixel), Convert.ToByte(grisPixel));
                }
            }

            //Bucle de reomplir els espais en null
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    if (matriu[i, j] == null)
                    {
                        this.Àrea_Lineat(matriu,i,j);
                    }
                }
            }

            //Canvia imatge
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];
                }
            }
        }

        public void Àrea_Lineat(Pixel[,] matriu, int i, int j)
        {
            List<Point> àrea = new List<Point>();
            Queue<Point> píxelsComprovar = new Queue<Point>();
            àrea.Add(new Point(j, i));
            píxelsComprovar.Enqueue(new Point(j, i));
            int Rmitja = 0, Gmitja = 0, Bmitja = 0;

            Point p = new Point(j, i);

            //Píxel dret
            if (p.X + 1 < this.amplada)
            {
                try
                {
                    if (matriu[p.Y, p.X + 1] == null)
                    {
                        àrea.Add(new Point(p.X + 1, p.Y));
                        píxelsComprovar.Enqueue(new Point(p.X + 1, p.Y));

                        if (p.X + 2 < this.amplada)
                        {
                            if (matriu[p.Y, p.X + 2] == null)
                            {
                                àrea.Add(new Point(p.X + 2, p.Y));
                                píxelsComprovar.Enqueue(new Point(p.X + 2, p.Y));
                            }
                        }
                    }
                }
                catch { }
            }

            //Píxel esquerra
            if (p.X - 1 >= 0)
            {
                try
                {
                    if (matriu[p.Y, p.X - 1] == null)
                    {
                        àrea.Add(new Point(p.X - 1, p.Y));
                        píxelsComprovar.Enqueue(new Point(p.X - 1, p.Y));

                        if (p.X - 2 >= 0)
                        {
                            if (matriu[p.Y, p.X - 2] == null)
                            {
                                àrea.Add(new Point(p.X - 2, p.Y));
                                píxelsComprovar.Enqueue(new Point(p.X - 2, p.Y));
                            }
                        }
                    }
                }
                catch { }
            }

            //Píxel superior
            if (p.Y - 1 >= 0)
            {
                try
                {
                    if (matriu[p.Y - 1, p.X] == null)
                    {
                        àrea.Add(new Point(p.X, p.Y - 1));
                        píxelsComprovar.Enqueue(new Point(p.X, p.Y - 1));

                        if (p.Y - 2 >= 0)
                        {
                            if (matriu[p.Y - 2, p.X] == null)
                            {
                                àrea.Add(new Point(p.X, p.Y - 2));
                                píxelsComprovar.Enqueue(new Point(p.X, p.Y - 2));
                            }
                        }
                    }
                }
                catch { }
            }

            //Píxel inferior
            if (p.Y + 1 < this.alçada)
            {
                try
                {
                    if (matriu[p.Y + 1, p.X] == null)
                    {
                        àrea.Add(new Point(p.X, p.Y + 1));
                        píxelsComprovar.Enqueue(new Point(p.X, p.Y + 1));

                        if (p.Y + 2 < this.alçada)
                        {
                            if (matriu[p.Y + 2, p.X] == null)
                            {
                                àrea.Add(new Point(p.X, p.Y + 2));
                                píxelsComprovar.Enqueue(new Point(p.X, p.Y + 2));
                            }
                        }
                    }
                }
                catch { }
            }

            //Píxels diagonals
            if ((p.Y + 1 < this.alçada)&&(p.X +1 < this.alçada))
            {
                try
                {
                    if (matriu[p.Y + 1, p.X + 1] == null)
                    {
                        àrea.Add(new Point(p.X + 1, p.Y + 1));
                        píxelsComprovar.Enqueue(new Point(p.X + 1, p.Y + 1));
                    }
                }
                catch
                {
                }
            }

            if ((p.Y + 1 < this.alçada) && (p.X - 1 >= 0))
            {
                try
                {
                    if (matriu[p.Y + 1, p.X - 1] == null)
                    {
                        àrea.Add(new Point(p.X - 1, p.Y + 1));
                        píxelsComprovar.Enqueue(new Point(p.X - 1, p.Y + 1));
                    }
                }
                catch
                {

                }
            }

            if ((p.Y - 1 >= 0) && (p.X + 1 < this.alçada))
            {
                try
                {
                    if (matriu[p.Y - 1, p.X + 1] == null)
                    {
                        àrea.Add(new Point(p.X + 1, p.Y - 1));
                        píxelsComprovar.Enqueue(new Point(p.X + 1, p.Y - 1));
                    }
                }
                catch { }
            }

            if ((p.Y - 1 >= 0) && (p.X - 1 >= 0))
            {
                try
                {
                    if (matriu[p.Y - 1, p.X - 1] == null)
                    {
                        àrea.Add(new Point(p.X - 1, p.Y - 1));
                        píxelsComprovar.Enqueue(new Point(p.X - 1, p.Y - 1));
                    }
                }
                catch { }
            }

            foreach (Point Pmitja in àrea)
            {
                Rmitja += this.dades[Pmitja.Y, Pmitja.X].GetR();
                Gmitja += this.dades[Pmitja.Y, Pmitja.X].GetG();
                Bmitja += this.dades[Pmitja.Y, Pmitja.X].GetB();
            }

            Rmitja = Rmitja / àrea.Count();
            Gmitja = Gmitja / àrea.Count();
            Bmitja = Bmitja / àrea.Count();

            foreach (Point Ppintar in àrea)
            {
                matriu[Ppintar.Y, Ppintar.X] = new Pixel(Convert.ToByte(Rmitja), Convert.ToByte(Gmitja), Convert.ToByte(Bmitja));
            }
        }

        //Llindar ------- Ampliació individual ------------ Albert Compte

        public void Llindar(byte Llindar)
        {
            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    byte roig;
                    byte verd;
                    byte blau;

                    if (this.dades[i, j].GetR() >= Llindar)
                        roig = 255;
                    else
                        roig = 0;

                    if (this.dades[i, j].GetG() >= Llindar)
                        verd = 255;
                    else
                        verd = 0;

                    if (this.dades[i, j].GetB() >= Llindar)
                        blau = 255;
                    else
                        blau = 0;

                    matriu[i, j] = new Pixel(roig, verd, blau);
                }
            }

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    this.dades[i, j] = matriu[i, j];
                }
            }

        }

        //Filtre Roig --------- Ampliació Individual ----------- Joel Compte

        public void Filtre_Roig(int umbral, int color)
        {

            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            matriu = this.dades;

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    if ((color == 0)&&((umbral > Convert.ToInt32(this.dades[i, j].GetR())) || (umbral <= Convert.ToInt32(this.dades[i, j].GetG())) || (umbral <= Convert.ToInt32(this.dades[i, j].GetB()))))
                    {
                        matriu[i, j] = new Pixel(0, 0, 0);
                    }
                    else if ((color == 1)&&((umbral <= Convert.ToInt32(this.dades[i, j].GetR())) || (umbral > Convert.ToInt32(this.dades[i, j].GetG())) || (umbral <= Convert.ToInt32(this.dades[i, j].GetB()))))
                    {
                        matriu[i, j] = new Pixel(0, 0, 0);
                    }
                    else if ((color == 2)&&((umbral <= Convert.ToInt32(this.dades[i, j].GetR())) || (umbral <= Convert.ToInt32(this.dades[i, j].GetG())) || (umbral > Convert.ToInt32(this.dades[i, j].GetB()))))
                    {
                        matriu[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        byte R = Convert.ToByte(Convert.ToInt32(this.dades[i, j].GetR()));
                        byte G = Convert.ToByte(Convert.ToInt32(this.dades[i, j].GetG()));
                        byte B = Convert.ToByte(Convert.ToInt32(this.dades[i, j].GetB()));
                        matriu[i, j] = new Pixel(R, G, B);
                    }
                }


                for (int x = 0; x < this.alçada; x++)
                {
                    for (int j = 0; j < this.amplada; j++)
                    {
                        this.dades[x, j] = matriu[x, j];

                    }
                }
            }
        }

        //Tons roig ------------------- Ampliació individual ---------------- Joel Delgado
        public void TonosRojo(int r, int g, int b, int tipus)
        {
            byte R, G, B;
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    if (tipus == 0)
                    {
                        if (Convert.ToInt32(this.dades[i, j].GetR()) >= 128 && Convert.ToInt32(this.dades[i, j].GetG()) < 128 && Convert.ToInt32(this.dades[i, j].GetB()) < 128)
                        {
                            R = Convert.ToByte(r);
                            G = Convert.ToByte(g);
                            B = Convert.ToByte(b);
                            this.dades[i, j] = new Pixel(R, G, B);
                        }
                    }
                    else if (tipus == 1)
                    {
                        if (Convert.ToInt32(this.dades[i, j].GetG()) >= 128 && Convert.ToInt32(this.dades[i, j].GetR()) < 128 && Convert.ToInt32(this.dades[i, j].GetB()) < 128)
                        {
                            R = Convert.ToByte(r);
                            G = Convert.ToByte(g);
                            B = Convert.ToByte(b);
                            this.dades[i, j] = new Pixel(R, G, B);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(this.dades[i, j].GetB()) >= 128 && Convert.ToInt32(this.dades[i, j].GetG()) < 128 && Convert.ToInt32(this.dades[i, j].GetR()) < 128)
                        {
                            R = Convert.ToByte(r);
                            G = Convert.ToByte(g);
                            B = Convert.ToByte(b);
                            this.dades[i, j] = new Pixel(R, G, B);
                        }
                    }
                }
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Marc
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Marc previsualització
        public int Marcprevisualitza(bool tipusMarc, int gruix, byte R, byte G, byte B)
        {
            //Marc interior
            if (tipusMarc == false)
            {
                this.prevalçada = this.alçada;
                this.prevamplada = this.amplada;
                this.previsualització = new Pixel[this.prevalçada, this.prevamplada];

                if(((2*gruix) > this.prevalçada)||((2*gruix) > this.prevamplada))
                    return -1;
                for (int i = 0; i < this.prevalçada; i++)
                {
                    for (int j = 0; j < this.prevamplada; j++)
                    {
                        if ((i < gruix) || (j < gruix) || (i >= (this.prevalçada - gruix)) || (j >= (this.prevamplada - gruix)))
                        {
                            this.previsualització[i, j] = new Pixel(R, G, B);
                        }
                        else
                            this.previsualització[i, j] = this.dades[i,j];
                    }
                }
               
            }

            //Marc exterior
            else if (tipusMarc == true)
            {
                this.prevalçada = this.alçada + (2 * gruix);
                this.prevamplada = this.amplada + (2 * gruix);
                this.previsualització = new Pixel[this.prevalçada, this.prevamplada];
                for (int i = 0; i < this.prevalçada; i++)
                {
                    for (int j = 0; j < this.prevamplada; j++)
                    {
                        if ((i < gruix) || (j < gruix) || (i >= (this.prevalçada - gruix)) || (j >= (this.prevamplada - gruix)))
                            this.previsualització[i, j] = new Pixel(R, G, B);
                        else
                            this.previsualització[i, j] = this.dades[i - gruix, j - gruix];
                    }
                }
            }
            return 0;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Seccionar
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Seccionar
        public int Seccionar(int x1, int x4, int y1, int y4, int opcions, int desX, int desY)
        {
            Pixel[,] matriu = new Pixel[Math.Abs(y1 - y4) +1, Math.Abs(x1 - x4) +1];

            if ((x1 == x4) || (y1 == y4))
                return -1;

            if (x1 > x4)
            {
                int x = x1;
                x1 = x4;
                x4 = x;
            }
            if (y1 > y4)
            {
                int y = y1;
                y1 = y4;
                y4 = y;
            }

            for (int i = 0; i <= Math.Abs(y1 - y4); i++)
            {
                for (int j = 0; j <= Math.Abs(x1 - x4); j++)
                {

                    matriu[i, j] = new Pixel();
                    matriu[i, j] = this.dades[y1+i, x1 + j];
                    this.dades[y1 + i, x1 + j] = new Pixel(255, 255, 255);

                }
            }

            this.previsualització = matriu;

            //Borrar exterior
            if (opcions == 1)
            {
                for (int i = 0; i < this.alçada; i++)
                {
                    for (int j = 0; j < this.amplada; j++)
                    {
                        this.dades[i, j] = new Pixel(255, 255, 255);

                        if ((x1 <= j)&&(x4 >= j)&&(y1 <= i)&&(y4 >= i))
                            this.dades[i, j] = this.previsualització[i - y1 , j - x1];
                    }
                }
            }

            //Desplaçar
            if (opcions == 2)
            {
                for (int i = 0; i < Math.Abs(y1 - y4); i++)
                {
                    for (int j = 0; j < Math.Abs(x1 - x4); j++)
                    {
                        if(((i + desY) < this.alçada)&&((j + desX) < this.amplada))
                            this.dades[i + desY, j + desX] = this.previsualització[i, j];
                    }
                }
            }

            return 0;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Retallar
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public int Retallar(int x1, int y1, int x2, int y2)
        {
            if ((x1 == x2) || (y1 == y2))
                return -1;
            if ((x1 < 0) || (x2 < 0) || (y1 < 0) || (y2 < 0) || (x1 >= this.amplada) || (x2 >= this.amplada) || (y1 >= this.alçada) || (y2 >= this.alçada))
                return -2;

            this.prevalçada = Math.Abs(y1 - y2);
            this.prevamplada = Math.Abs(x1 - x2);

            if (x2 < x1)
            {
                int x = x1;
                x1 = x2;
                x2 = x;
            }
            if (y2 < y1)
            {
                int y = y1;
                y1 = y2;
                y2 = y;
            }

            this.previsualització = new Pixel[this.prevalçada, this.prevamplada];
            for (int i = 0; i < this.prevalçada; i++)
            {
                for (int j = 0; j < this.prevamplada; j++)
                {
                    this.previsualització[i,j] = this.dades[x1 + i, y1 + j];
                }
            }

            return 0;

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Funció Mirall~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //Eix vertical per fer el reflex
        public void Efecte_Mirall(int costat)
        {

            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[i, j] = this.dades[i, this.amplada - 1 - j];

                }
            }

            int meitat = this.amplada / 2;

            if (costat == 1)
            {
                for (int i = 0; i < this.alçada; i++)
                {
                    for (int j = meitat; j < this.amplada; j++)
                    {
                        this.dades[i, j] = matriu[i, j];

                    }
                }
            }

            if (costat == 0)
            {
                for (int i = 0; i < this.alçada; i++)
                {
                    for (int j = 0; j < meitat; j++)
                    {
                        this.dades[i, j] = matriu[i, j];

                    }
                }
            }

        }


        //Eix horitzontal per fer el reflex
        public void Efecte_Reflex(int banda)
        {

            Pixel[,] matriu = new Pixel[this.alçada, this.amplada];
            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    matriu[i, j] = this.dades[this.alçada - 1 - i, j];

                }
            }

            int meitat = this.alçada / 2;

            if (banda == 1)
            {
                for (int i = meitat; i < this.alçada; i++)
                {
                    for (int j = 0; j < this.amplada; j++)
                    {
                        this.dades[i, j] = matriu[i, j];

                    }
                }
            }

            if (banda == 0)
            {
                for (int i = 0; i < meitat; i++)
                {
                    for (int j = 0; j < this.amplada; j++)
                    {
                        this.dades[i, j] = matriu[i, j];

                    }
                }
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~NAVI~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        
        //Genera les frases que es mostran als 15 segons del canvi
        public string NaviGenérico()
        {
            Random Random = new Random();
            int fraseRnd = Random.Next(24);
            string Frase = "Hey Listen";

            switch (fraseRnd)
            {
                case 0:
                    Frase = "Hey! Vols que la imatge es vegi com un dibuix? \n Fes ús de la opció lineat a Filtres";
                    break;
                case 1:
                    Frase = "Hey! Aquesta imatge és per enmarcar-lo, vols\n afegir-li un marc? Vés a Editar per posar-li\n un marc";
                    break;
                case 2:
                    Frase = "Hey! Vols canviar-li el color de la imatge?\n La opció Modifica de Filtres ho pot fer";
                    break;
                case 3:
                    Frase = "Hey! Hi ha massa colors? Per què no proves\n la opció Llindar de Ampliacions?";
                    break;
                case 4:
                    Frase = "Hey! Saps què quedaria bé? Que la imatge es\n veies en blanc i negre com a les televisions antigues";
                    break;
                case 5:
                    Frase = "Hey! Si fas servir la opció Liniat, jo et\n recomenaré el valor que crec que és adient";
                    break;
                case 6:
                    Frase = "Hey! La de voltes que dóna la vida! Per què\n no fer el mateix amb la imatge? Tens l'opció\n a Edita";
                    break;
                case 7:
                    Frase = "Hey! Vols moure un tros de la imatge? L'opció\n Secciona d'Edita ho pot fer";
                    break;
                case 8:
                    Frase = "Hey! Creus que són una pesada?\n Desactiva'm quan vulguis a Ajuda...\n Potser no ho havia d'haver dit";
                    break;
                case 9:
                    Frase = "Hey! T'has equivocat? Ves a Enrere en edita per\n tornar a la imatge anterior";
                    break;
                case 10:
                    Frase = "Hey! Si em cliques jo treuré el comentari\n que hagi fet";
                    break;
                case 11:
                    Frase = "Hey! T'agrada portar la contraria als colors?\n Tria l'opció Negatiu de Filtres";
                    break;
                case 12:
                    Frase = "Hey! No t'agrada un color en específic?\n A Canvia RGB de Ampliacions el pot canviar";
                    break;
                case 13:
                    Frase = "Hey! Vols quedar-te amb un color en específic?\n Pots fer ús del Filtre RGB de Ampliacions";
                    break;
                case 14:
                    Frase = "Hey! Els creadors d'aquest projecte els podràs\n veure a informació en Equip";
                    break;
                case 15:
                    Frase = "Hey! Vols tenir una aplicació personalitzada?\n Canvia el color del programa a Visualització";
                    break;
                case 16:
                    Frase = "Hey!\n Tenim totes les teves accions registrades :)\nVols veure-les? Estàn a DataBase de Visualització";
                    break;
                case 17:
                    Frase = "Hey! Vols veure totes les imatges d'una carpeta?\n Presenta-les a mode presentació de Visualització";
                    break;
                case 18:
                    Frase = "Hey! Fa un dia radiant... Perquè no gires la imatge\n pi/2 radiants... Està a Edita";
                    break;
                case 19:
                    Frase = "Hey! Aquesta imatge és asimètrica :(\n Posa-li simetria amb l'efecte Mirall d'Edita";
                    break;
                case 20:
                    Frase = "Hey! Vull que la imatge sigui un dibuix amb color...\n T'agradaria posar un liniat amb color?";
                    break;
                case 21:
                    Frase = "Hey! Vols veure les imatges amb més detall?\n A la part inferior de l'aplicació pots canviar-li la mida";
                    break;
                case 22:
                    Frase = "Hey! Tinc por a les imatges grans...\n Si em poses una m'amagaré";
                    break;
                case 23:
                    Frase = "Hey! Enrecordat de desar la imatge quan acabis";
                    break;
                case 24:
                    Frase = "Hey! Vull col·laborar...\n Fes-me doble click";
                    break;

            }

            return Frase;
        }

        //Fa el soroll d'entrada
        public int EntradaNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[6];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }

        }

        //Fa el soroll de sortida
        public int SortidaNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[7];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Fa el soroll aleatori a les frases genèriques
        public int ExpressióNavi()
        {
            try
            {
                Random VeuRnd = new Random();
                int Rnd = VeuRnd.Next(3);
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[1];
                VeuNavi.Play();
                Thread.Sleep(600);
                switch (Rnd)
                {
                    case 0:
                        VeuNavi.SoundLocation = RutaNavi + AudioNavi[2];
                        VeuNavi.Play();
                        break;
                    case 1:
                        VeuNavi.SoundLocation = RutaNavi + AudioNavi[3];
                        VeuNavi.Play();
                        break;
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Diu Hola
        public int SaludaNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[0];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Diu Hey
        public int HeyNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[1];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Navi espantada
        public int EspantNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[9];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Navi Cop
        public int CopNavi()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[8];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Navi Avís
        public int NaviWatchOut()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[4];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Navi Flota
        public int NaviFlotar()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[5];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Navi Look
        public int NaviLook()
        {
            try
            {
                VeuNavi.SoundLocation = RutaNavi + AudioNavi[3];
                VeuNavi.Play();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        //Calcula el valor "òptim" pel liniat considerant que el 20% de la imatge ha de ser negre
        public int NaviLiniatCàlcul(int detall)
        {

            int area = this.alçada * this.amplada;
            int negre = 0;

            for (int i = 0; i < this.alçada; i++)
            {
                for (int j = 0; j < this.amplada; j++)
                {
                    int grisPixel = (Convert.ToInt32(this.dades[i, j].GetR()) + Convert.ToInt32(this.dades[i, j].GetG()) + Convert.ToInt32(this.dades[i, j].GetB())) / 3;

                    int grisPixelAmunt;
                    if (i != this.alçada - 1)
                        grisPixelAmunt = (Convert.ToInt32(this.dades[i + 1, j].GetR()) + Convert.ToInt32(this.dades[i + 1, j].GetG()) + Convert.ToInt32(this.dades[i + 1, j].GetB())) / 3;
                    else
                        grisPixelAmunt = grisPixel;

                    int grisPixelAvall;
                    if (i != 0)
                        grisPixelAvall = (Convert.ToInt32(this.dades[i - 1, j].GetR()) + Convert.ToInt32(this.dades[i - 1, j].GetG()) + Convert.ToInt32(this.dades[i - 1, j].GetB())) / 3;
                    else
                        grisPixelAvall = grisPixel;

                    int grisPixelDret;
                    if (j != this.amplada - 1)
                        grisPixelDret = (Convert.ToInt32(this.dades[i, j + 1].GetR()) + Convert.ToInt32(this.dades[i, j + 1].GetG()) + Convert.ToInt32(this.dades[i, j + 1].GetB())) / 3;
                    else
                        grisPixelDret = grisPixel;

                    int grisPixelEsq;
                    if (j != 0)
                        grisPixelEsq = (Convert.ToInt32(this.dades[i, j - 1].GetR()) + Convert.ToInt32(this.dades[i, j - 1].GetG()) + Convert.ToInt32(this.dades[i, j - 1].GetB())) / 3;
                    else
                        grisPixelEsq = grisPixel;

                    if (((Math.Abs(grisPixel - grisPixelAmunt) > detall) && (Math.Abs(grisPixel - grisPixelAmunt) < 255)) || ((Math.Abs(grisPixel - grisPixelAvall) > detall) && (Math.Abs(grisPixel - grisPixelAvall) < 255)) || ((Math.Abs(grisPixel - grisPixelDret) > detall) && (Math.Abs(grisPixel - grisPixelDret) < 255)) || ((Math.Abs(grisPixel - grisPixelEsq) > detall) && (Math.Abs(grisPixel - grisPixelEsq) < 255)))
                    {
                        negre++;
                    }

                }
            }

            int percent = negre * 100 / area;

            return percent;

        }

        //Respon el valor òptim del liniat
        public string NaviLineatResposta()
        {
            int detall = 0;
            string frase;
            bool detall_màxim = false;

            while ((!detall_màxim) && (this.NaviLiniatCàlcul(detall) > 20))
            {
                detall = detall + 10;
                if (detall > 255)
                {
                    detall_màxim = true;
                    detall = 255;
                }
            }
            while ((detall != 0) && (detall != 255) && (this.NaviLiniatCàlcul(detall) < 20))
            {
                detall--;
            }

            frase = "Hey! Et recomano un valor de lineat de " + detall;

            return frase;
        }

        //Resposta amb int
        public int NaviIntLineatResposta()
        {
            int detall = 0;
            bool detall_màxim = false;

            while ((!detall_màxim) && (this.NaviLiniatCàlcul(detall) > 20))
            {
                detall = detall + 10;
                if (detall > 255)
                {
                    detall_màxim = true;
                    detall = 255;
                }
            }
            while ((detall != 0) && (detall != 255) && (this.NaviLiniatCàlcul(detall) < 20))
            {
                detall--;
            }

            return detall;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    }
}
