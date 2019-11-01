using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;





namespace Parser
{
    public partial class Form1 : Form
    {
      private string filePin;
      private string filePout;
        private string strLine;
        public string strLineOut
        {
            get { return strLine; }
            set { value = strLine;}
        }
        public string fIn
        {
            get { return filePin; }
            set { value = filePin; }
        }
        public string fOut  // Имя выходного файла
        {
            get { return filePout; }
            set { value = filePout; }
        }
        private string fContent;
        //Parstxt pars;
        public string fContentF
        {
            get { return fContent; }
            set { value = fContent; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                // openFileDialog1.Reset();
                fIn = openFileDialog1.FileName;
                try
                {
                    using (Stream fileStream = openFileDialog1.OpenFile())
                    {
                        StreamReader readINfile = new StreamReader(fileStream);


                        fContentF = readINfile.ReadToEnd();
                       // richTextBox1.AppendText(fContent);
                    }
                }
                catch(Exception k){ MessageBox.Show(k.Message);}

                var pat = @"\b\d\.\d*|w{2}\b";  // \b\d\d\.\d*\b   паттрерн для числового значения разультата проверки.
                var objP = new Parstxt();
                  objP.ParstxtSearch(fContentF, pat);
                if (fOut.Length > 0 & File.Exists(fOut)) // если выходной файл выбран и он существует то парсим его на предмет наличия маркера шаблона
                {
                    using (Stream fileOut = openFileDialog1.OpenFile())
                    {
                        StreamReader sReaderOut = new StreamReader(fileOut);
                        strLineOut = sReaderOut.ReadToEnd();
                        // парсим выходной файл шаблона, ищим маркер после чего заменяем маркер на данные
                        var patre = @"@\d";
                        var StrHtmlOut = objP.InsertDataPattern(objP.resArr, strLineOut, patre) ; // выдача выходной строки Html отчета.

                        webBrowser1.Navigate(StrHtmlOut);
                    }

                }
                else { MessageBox.Show("Выходной файл не выбран, либо он не существует"); }
                   // richTextBox1.Lines = objP.GetMResLine() ; // заполняем richTextBox1 результатами   
           
           }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

         // получаем шаблон HTML
        private void шаблонHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.Filter = "(html) |*.html|(*.*)|*.*";
             if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                fOut = openFileDialog1.FileName; // сохраняем имя выходного файла-шаблона 
                

                //var fileHTMLStream = openFileDialog1.OpenFile();
                //using(StreamReader readHTML = new StreamReader(fileHTMLStream))
                //{
                //  var htmlContent = readHTML.ReadToEnd(); // Прочитать HTML шаблон.
                //    webBrowser1.Navigate(fileHTML);
                //}


            }
               
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}
