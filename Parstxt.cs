﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Parser
{
    
    public struct MRes
    {
        public int MResIndex   ;
        public string MResValue;
    }
    class Parstxt
    {
       string[] ArrOutStr;  
       string[] ArrInStr;
       public MRes resArr;
        ArrayList ArrListRes;
        public Parstxt()
        {
             resArr = new MRes();
             ArrListRes = new ArrayList();
        }
        public void ParstxtSearch(string FileF, string pattern)  // FileF распарсиваемая строка .
        {
      try { 
            ArrInStr = FileF.Split('\n'); // \t \n
            int CountIn = 1;
            foreach (string strFile in ArrInStr) // перебираем по строкам входной поток
            {
                Match matchR = Regex.Match(EncodToUTF(strFile), pattern); // разбираем строку на предмет совпадений с паттерном и за одно конвертируем в UTF-8
                if (matchR.Value != "") // если строка совпадения не найдена то в массив не заносим
                {
                    resArr.MResValue = matchR.Value;
                    resArr.MResIndex = CountIn;
                    ArrListRes.Add(resArr);
                    CountIn++;
                }
            }
          }
            catch(Exception a)
            {
                MessageBox.Show(a.Message);  
            }


        }
        /// <summary>
        /// Процедура замены маркеров паттерна patternIns в выходном файле 
        /// FileNameOut на реальные данные из массива ArrData
        /// </summary>
        /// <param name="ArrData">дата массив</param>
        /// <param name="InStr">строка в которой осуществляется замена</param>
        public string InsertDataPattern(MRes ArrData, string InStr, string patternO)
        {
            string FinalStrOut = null;
            ArrOutStr = InStr.Split('\n');
            foreach (string StrO in ArrOutStr)
            {
                Match matchO = Regex.Match(StrO, patternO);
                if (matchO.Success) // если паттерн в строке найден то сверяем его с номером индекса ArrData 
                {
                    for (int i = 0; i < ArrData.MResIndex; i++)
                    {
                        if (Convert.ToInt32(matchO.Value.Substring(1)) == i)  // берем 1 символ 
                            StrO.Replace(matchO.Value, ArrData.MResValue);
                    }
                }
                FinalStrOut += StrO;
            }
            return FinalStrOut;
        }
        /// <summary>
        ///  метод конвертирования входной строки в кодировку UTF-8 из кодировки win1251
        /// </summary>
        /// <param name="inputtxt"></param>
        /// <returns></returns>
        public string EncodToUTF(string inputtxt)  // конвертирование входной строки в UTF-8
        {
          Encoding utf8str = Encoding.GetEncoding("UTF-8");
          Encoding win1251str = Encoding.GetEncoding("Windows-1251");
          byte[] utf8Bytes = win1251str.GetBytes(inputtxt);
          byte[] win1251Bytes = Encoding.Convert(win1251str, utf8str, utf8Bytes);
          char[] charAns = new char[win1251str.GetCharCount(win1251Bytes, 0, win1251Bytes.Length)];
            utf8str.GetChars(win1251Bytes, 0, win1251Bytes.Length, charAns, 0);
            string OutStr = new string(charAns);
            return OutStr;
        }

    }
}