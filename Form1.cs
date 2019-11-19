using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SelectPdf;





namespace Parser
{
    public partial class Form1 : Form
    {
      string FileName, PatchF, FileNamePdf;
      private string filePin;
      private string filePout;
      private string strLine;
        public string strLineOut
        {
            get { return strLine; }
            set { strLine = value;}
        }
        public string fIn
        {
            get { return filePin; }
            set {filePin = value; }
        }
        public string fOut  // Имя выходного файла
        {
            get { return filePout; }
            set {filePout = value; }
        }
        private string fContent;
        //Parstxt pars;
        public string fContentF
        {
            get { return fContent; }
            set {fContent = value; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.Filter = "(*.*) | *.*" ;
           if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                // openFileDialog1.Reset();
                fIn = openFileDialog1.FileName;
                try
                {
                    using (Stream fileStream = openFileDialog1.OpenFile())
                    {
                        StreamReader readINfile = new StreamReader(fileStream);
                        fContentF = readINfile.ReadToEnd(); // загружаем основной файл для парсинга в поток fContentF
                        //richTextBox1.AppendText(fContentF);  
                        MessageBox.Show("Файл загружен");   
                        
                    }
                }
                catch(Exception k){ MessageBox.Show(k.Message);}
                     
                
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
            openFileDialog1.Filter = "(html) |*.html";
             if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                fOut = openFileDialog1.FileName; // сохраняем имя выходного файла-шаблона 
                Stream SreamReadOut = openFileDialog1.OpenFile(); //читаем поток шаблона
                StreamReader StReadOut = new StreamReader(SreamReadOut);
                strLineOut = StReadOut.ReadToEnd();
                if (strLineOut.Length > 0)
                {
                    MessageBox.Show("Шаблон выходного файла загружен");
                   
                }
                else
                { 
                    MessageBox.Show("Шаблон выходного файла не загружен");
                }
            } 
               
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
         // выполнение преобразования в html
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Выполняется !");
            var pat = @"\b\d\.\d{3,6}";  // \b\d\d\.\d*\b   паттрерн для числового значения разультата проверки.
            var objP = new Parstxt();
            objP.ParstxtSearch(fContentF, pat);
            if (strLineOut.Length > 0 & File.Exists(fOut)) // если выходной файл выбран и он существует то парсим его на предмет наличия маркера шаблона
            {
                // парсим выходной файл шаблона, ищим маркер после чего заменяем маркер на данные
                var patre = @"@\d*";  // необходимо отработать ошибку 0.7 
                var StrHtmlOut = objP.InsertDataPattern(objP.ArrListRes, strLineOut, patre); // выдача выходной строки Html отчета.
                string DayT = DateTime.Now.ToShortDateString();
                DayT = DayT.Replace('.', '_');
                string TimeT = DateTime.Now.ToShortTimeString() ;
                FileName = "FileReport_" + DayT +"_"+ TimeT + ".html";
                FileName = FileName.Replace(':', '_');

                using (StreamWriter Sw = new StreamWriter(FileName))
                {
                    Sw.WriteLine(StrHtmlOut); // пишим строку целиком
                }
                if (File.Exists(FileName))
                {
                     PatchF = new FileInfo(FileName).FullName;
                    webBrowser1.Navigate(@PatchF);
                }
                else MessageBox.Show("Выходного файла не существует!");
            }
            else { MessageBox.Show("Выходной файл не выбран, либо он не существует"); }

        }
        /// <summary>
        /// // конвертирование Html в pdf 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // progressBar1.Select();
            progressBar1.Visible = true;
           HtmlToPdf conv = new HtmlToPdf();
            PdfDocument doc = conv.ConvertUrl(PatchF);
             PdfViewerPreferences PdfVif = doc.ViewerPreferences;
            PdfVif.PageMode = PdfViewerPageMode.UseThumbs;
            progressBar1.Value = 50;
       FileNamePdf = PatchF + ".pdf";

            doc.Save(FileNamePdf);
            doc.Close();
            progressBar1.Value = 100;
            progressBar1.Visible = false;
        }
    }
}
