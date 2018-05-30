using System;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.IO;
using System.Web.Util;

namespace TadaServer2
{
    /// <summary>
    /// Summary description for TadaServer2
    /// </summary>
    [WebService(Namespace = "urn:Microsoft.Search", Description = "TADA by FASIC")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TadaServer2 : System.Web.Services.WebService
    { 
        [WebMethod]
        public string Registration(string registrationXml)
    {
        MemoryStream stream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(
          stream, System.Text.Encoding.UTF8);

        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();

        // Registration Response Packet
        writer.WriteStartElement("ProviderUpdate",
          "urn:Microsoft.Search.Registration.Response");
        writer.WriteElementString("Status", "SUCCESS");

        // Provider
        writer.WriteStartElement("Providers");
        writer.WriteStartElement("Provider");
        writer.WriteElementString("Message", "TADA by FASIC");

        writer.WriteElementString("Id",
          "{C37EE888-D74E-47e5-B113-BA613D87F0B2}");
        writer.WriteElementString("Name",
          "TADA by FASIC");
        writer.WriteElementString("QueryPath",
          @"http://" + HttpContext.Current.Request.Url.Host + ":" +
          HttpContext.Current.Request.Url.Port.ToString() +
          HttpContext.Current.Request.Url.AbsolutePath);

        writer.WriteElementString("RegistrationPath",
          @"http://" + HttpContext.Current.Request.Url.Host + ":" +
          HttpContext.Current.Request.Url.Port.ToString() +
          HttpContext.Current.Request.Url.AbsolutePath);

        writer.WriteElementString("Type", "SOAP");

        // Services
        writer.WriteStartElement("Services");
        writer.WriteStartElement("Service");
        writer.WriteElementString("Id",
          "{8DD063CA-94FC-4514-8D83-3B36B12432BE}");

        writer.WriteElementString("Name",
          "TADA by FASIC");

        writer.WriteElementString("Description",
          "TADA by FASIC");

        writer.WriteElementString("Copyright", "(C) 2018");
        writer.WriteElementString("Display", "On");
        writer.WriteElementString("Category", "RESEARCH_GENERAL");
        writer.WriteEndElement(); // Service
        writer.WriteEndElement(); // Services
        writer.WriteEndElement(); // Provider
        writer.WriteEndElement(); // Providers
        writer.WriteEndElement(); // ProviderUpdate
        writer.WriteEndDocument();

        writer.Flush();

        stream.Position = 0;
        StreamReader reader = new StreamReader(stream);
        string result = reader.ReadToEnd();
        return result;
    }

    [WebMethod]
    public string Query(string queryXml)
    {
        XmlDocument xmlQuery = new XmlDocument();
        xmlQuery.LoadXml(queryXml);

        XmlNamespaceManager nm1 =
          new XmlNamespaceManager(xmlQuery.NameTable);
        nm1.AddNamespace("ns", "urn:Microsoft.Search.Query");
        nm1.AddNamespace("oc",
          "urn:Microsoft.Search.Query.Office.Context");
        string queryString = xmlQuery.SelectSingleNode(
          "//ns:QueryText", nm1).InnerText;

        XmlNamespaceManager nm2 = new XmlNamespaceManager(
          xmlQuery.NameTable);
        nm2.AddNamespace("msq", "urn:Microsoft.Search.Query");
        string domain = xmlQuery.SelectSingleNode(
          "/msq:QueryPacket/msq:Query",
          nm2).Attributes.GetNamedItem("domain").Value;
        string queryId = xmlQuery.SelectSingleNode(
          "/msq:QueryPacket/msq:Query/msq:QueryId",
          nm2).InnerText;

        MemoryStream stream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(stream, null);
        writer.Formatting = Formatting.Indented;

        // Compose the Query Response packet.
        writer.WriteStartDocument();
        writer.WriteStartElement("ResponsePacket",
          "urn:Microsoft.Search.Response");
        // The providerRevision attribute can be used
        // to update the service.
        writer.WriteAttributeString("providerRevision", "1");
        writer.WriteStartElement("Response");
        // The domain attribute identifies the service 
        // that executed the query.
        writer.WriteAttributeString("domain", domain);
        writer.WriteElementString("QueryID", queryId);
            //todo toto zmenit
        if (String.Compare("aaa", queryString, true) == 0)
        {
            writer.WriteStartElement("Range");
            writer.WriteStartElement("Results");


            // Begin Content element
            writer.WriteStartElement("Content",
              "urn:Microsoft.Search.Response.Content");
            writer.WriteStartElement("HorizontalRule");
            writer.WriteEndElement(); //Horizontal rule
            writer.WriteElementString("P",
              "FOKU MEEEEE");
            writer.WriteStartElement("HorizontalRule");
            writer.WriteEndElement(); //Horizontal rule
            writer.WriteEndElement(); //Content

            // Finish up.
            writer.WriteEndElement(); //Results
            writer.WriteEndElement(); //Range
        }
        writer.WriteElementString("Status", "SUCCESS");
        writer.WriteEndElement(); //Response
        writer.WriteEndElement(); //ResponsePacket
        writer.WriteEndDocument();

        writer.Flush();

        // Move the results into a string.
        stream.Position = 0;
        StreamReader reader = new StreamReader(stream);
        string result = reader.ReadToEnd();
        return result;
    }
}
}
