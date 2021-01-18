# WSDLParser

Welcome, developer!!

This is an C# (.net 4.6.1) WSDL Parser that generates xml request/response based on it's wsdl schema.

Since i needed a wsdlparser that generates a xml sample request/response (like soapui does), i have found nothing that simple as possible and decided to develop this by myself and i could get some functional code in a very simple way using a recursive method that read the service schema and outputs a xml string based on it.

In the sample below, you can parse a wsdl generating an object with all information about the soap ws, in a simplified way, including the request/response xml sample (template).

Code Sample:

string WSDL = "http://pathtowsdl.com.br?WSDL"

System.Net.WebClient client = new System.Net.WebClient();

System.IO.Stream stream = client.OpenRead(WSDL);

ServiceDescription description = ServiceDescription.Read(stream);

// add using Six; at the top of script

List<WSDLServiceType> WSDLServices = WSDLParser.Parse(description, false);
