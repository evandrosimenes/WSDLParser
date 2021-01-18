# WSDLParser

That's an C# WSDL Parser that generates xml request/response based on it's wsdl schema.

Sample:

string WSDL = "http://pathtowsdl.com.br?WSDL"
System.Net.WebClient client = new System.Net.WebClient();
System.IO.Stream stream = client.OpenRead(WSDL);
ServiceDescription description = ServiceDescription.Read(stream);

// add using Six; at the top of script
List<WSDLServiceType> WSDLServices = WSDLParser.Parse(description, false);
