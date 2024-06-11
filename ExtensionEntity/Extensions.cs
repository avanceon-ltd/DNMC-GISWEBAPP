using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System;

namespace WebAppForm.ExtensionEntity
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        ///     Executes the query, and returns the first result set as DataTable.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataTable that is equivalent to the first result set.</returns>
        public static DataTable ExecuteDataTable(this SqlCommand @this)
        {
            var dt = new DataTable();
            using (var dataAdapter = new SqlDataAdapter(@this))
            {
                dataAdapter.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        ///     Executes the query, and returns the result set as DataSet.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataSet that is equivalent to the result set.</returns>
        public static DataSet ExecuteDataSet(this SqlCommand @this)
        {
            var ds = new DataSet();
            using (var dataAdapter = new SqlDataAdapter(@this))
            {
                dataAdapter.Fill(ds);
            }

            return ds;
        }

        public static XmlElement AddNodeWithValue(this XmlDocument xmlRequest, string elementName, string elementValue)
        {
            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";
            XmlElement element = xmlRequest.CreateElement(prefix, elementName, ns);
            element.InnerText = elementValue;
            return element;
        }

        public static String RemoveEmptyNodes(this XmlDocument rootXml)
        {
            XDocument document = XDocument.Parse(rootXml.OuterXml);
            document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
            return document.ToString().Replace("NULL"," ");
        }
    }
}