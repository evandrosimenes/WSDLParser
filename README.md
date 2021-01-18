# WSDLParser

Welcome, developer!!

This is an C# (.net 4.6.1) WSDL Parser that generates xml request/response based on it's wsdl schema.

Since i needed a wsdlparser that generates a xml sample request/response (like soapui does), i have found nothing that simple as possible and decided to develop this by myself and i could get some functional code in a very simple way using a recursive method that read the service schema and outputs a xml string based on it.
I was so frustrated because a couldn't found anything that really helps me, that i decided to share this code here to help others who could use it and save time.

In the sample below, you can parse a wsdl generating an object with all information about the soap ws, in a simplified way, including the request/response xml sample (template).


ADD the 2 classes WSDLServiceType and WSDLParser to your c# project.
// add using Six; at the top of script

Code Sample:

string WSDL = "http://pathtowsdl.com.br?WSDL"; //WSDL url

System.Net.WebClient client = new System.Net.WebClient();

System.IO.Stream stream = client.OpenRead(WSDL); //open WSDL url

ServiceDescription description = ServiceDescription.Read(stream); //load service description .net native object.

List<WSDLServiceType> WSDLServices = WSDLParser.Parse(description, false); //parse wsdl
