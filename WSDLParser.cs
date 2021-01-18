using System.Collections.Generic;
using System.Linq;
using BUS.Models;
using System.Data;
using System.Web.Services.Description;
using System.Xml.Schema;

namespace Six
{
    class WSDLParser
    {

        public static XmlSchemaObjectCollection SchemaItemsRoot { get; set; }
        public static List<WSDLServiceDescription> Parse(ServiceDescription Description, bool ParamArray)
        {

            var Services = new List<WSDLServiceDescription>();

            foreach(Service service in Description.Services)
            {
                WSDLServiceDescription wsd = new WSDLServiceDescription();
                wsd.name = service.Name;
                wsd.port = new List<PortWSDL>();

                foreach(Port port in service.Ports)
                {
                    PortWSDL pw = new PortWSDL();
                    pw.name = port.Name;
                    pw.address = ((SoapAddressBinding)port.Extensions[0]).Location;
                    pw.operations = new List<OperationWSDL>();
                    Binding binding = Description.Bindings.Cast<Binding>().Where(x => x.Name == port.Binding.Name).FirstOrDefault();
                    PortType porttype = Description.PortTypes.Cast<PortType>().Where(x => x.Name == binding.Type.Name).FirstOrDefault();
                    foreach (OperationBinding operationbinding in binding.Operations)
                    {
                        Operation operation = porttype.Operations.Cast<Operation>().Where(x => x.Name == operationbinding.Name).FirstOrDefault();
                        Message messageinput = Description.Messages.Cast<Message>().Where(x => x.Name == operation.Messages[0].Message.Name).FirstOrDefault();
                        Message messageoutput = Description.Messages.Cast<Message>().Where(x => x.Name == operation.Messages[1].Message.Name).FirstOrDefault();
                        OperationWSDL ow = new OperationWSDL();
                        ow.name = operation.Name;
                        ow.soapaction = ((SoapOperationBinding)operationbinding.Extensions[0]).SoapAction;

                        SchemaItemsRoot = Description.Types.Schemas[0].Items;
                        ow.input = GenerateXmlFromWSDLSchema(SchemaItemsRoot, ParamArray, messageinput.Parts[0].Element.Name);
                        ow.output = GenerateXmlFromWSDLSchema(SchemaItemsRoot, ParamArray, messageoutput.Parts[0].Element.Name);
                        
                        pw.operations.Add(ow);
                    }

                    wsd.port.Add(pw);
                }

                Services.Add(wsd);
            }
            
            return Services;
        }

        public static string GenerateXmlFromWSDLSchema(XmlSchemaObjectCollection SchemaItems, bool ParamArray, string ElementName=null)
        {
            var xmlstring = "";

            foreach(var item in SchemaItems)
            {
                if (item is XmlSchemaComplexType)
                {
                    if ((item as XmlSchemaComplexType).Name == ElementName)
                    {
                        XmlSchemaComplexType complextype = item as XmlSchemaComplexType;
                        
                        if (complextype.Particle == null)
                        {
                            XmlSchemaComplexContentExtension xscce = complextype.ContentModel.Content as XmlSchemaComplexContentExtension;
                            if (xscce.BaseTypeName != null)
                            {
                                xmlstring += GenerateXmlFromWSDLSchema(SchemaItemsRoot, ParamArray, xscce.BaseTypeName.Name);
                            }
                            xmlstring += GenerateXmlFromWSDLSchema(((XmlSchemaSequence)xscce.Particle).Items, ParamArray);
                        } else
                        {
                            xmlstring += GenerateXmlFromWSDLSchema(((XmlSchemaSequence)complextype.Particle).Items, ParamArray);
                        }
                        
                    }
                }
                else
                {
                    if (((item as XmlSchemaElement).Name == ElementName) || ElementName == null)
                    {
                        XmlSchemaElement element = item as XmlSchemaElement;

                        var unbounded = false;
                        if (element.MaxOccursString != null)
                        {
                            if (element.MaxOccursString == "unbounded")
                            {
                                xmlstring += "<!-- 1 ou mais repetições (list/array) -->";
                                unbounded = ParamArray == true ? true : false;
                            }
                        }

                        xmlstring += unbounded == true ? "<" + element.Name + " isArray=\"true\">" : "<" +element.Name+">";
                        if (element.SchemaType != null)
                        {
                            XmlSchemaComplexType complextype = element.SchemaType as XmlSchemaComplexType;
                            
                            if ((XmlSchemaSequence)complextype.Particle != null)
                            {
                                xmlstring += GenerateXmlFromWSDLSchema(((XmlSchemaSequence)complextype.Particle).Items, ParamArray);
                            }
                            
                        }
                        else
                        {
                            xmlstring += GenerateXmlFromWSDLSchema(SchemaItemsRoot, ParamArray, element.SchemaTypeName.Name);
                        }
                        xmlstring += "</" + element.Name + ">";
                    }
                }
            }
            
            return xmlstring;
        }
    }
}
