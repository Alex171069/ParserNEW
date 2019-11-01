using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace Parser
{
    class xmlconfig
    {
        private XmlDocument xmlFConf;
        public XmlDocument xmlF
            { 
             get => xmlFConf;
             set => xmlFConf = value;
            }
      public xmlconfig(string xmlfilename)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlfilename);
            xmlF = xDoc;
        }
      public string xmlSearchTag(string searchTag) //searchTag - значение атрибута ["n"]...  поиск значение атрибута в XML файле
        {
            var TagInt = Convert.ToInt32(searchTag);

            foreach (XmlNode xConfNode in xmlFConf.SelectNodes("/rep/row")) 
            {
                 if(TagInt == Convert.ToInt32(xConfNode.Attributes["id"].Value)) // при совпадении значения с икомым выводим значение тега
                {
                    return xConfNode.Value;
                }
                 
            } 
            return null; // вывод значения null если узла не нашли.
        }
    }
}
