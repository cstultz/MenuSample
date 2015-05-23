
 Menu Sample: Parse XML Menu and Write Items to Console 

The scenario
There are two teams of developers. One team of .Net, and the other team Java. As end users navigate from page to page, they should not notice whether they’re on a .Net page or a Java page. Therefore both teams need to be able to draw the exact same menu, so there is a shared XML document to define the menu structure. 

The code sample 
Two text files with XML menus for each of the applications. The code sample is a C# console application written in Visual Studio that does all of the following: 

•	Accepts two arguments: first, a path to a menu .xml file (e.g. “c:\schedulemenu1.xml”); second an active path to match (e.g. “/default.aspx”) 
•	Parses the XML document, ignoring any XML content not required for this application 
•	Identifies currently-active menu items — a menu item is active if it or one of its children has a path matching the second argument 
•	Writes the parsed menu to the console:
1.	Shows the display name and the path structure for each menu item 
2.	Indents the submenu items 
3.	Prints the word “ACTIVE” next to any active menu items. 

When the application is ran from the command line, you will see something like this: 

c:>MenuSample.exe c:\ScheduleMenu1.xml /Requests/OpenQuotes.aspx
Home, /Default.aspx 
Trips, /Requests/Quotes/CreateQuote.aspx ACTIVE 
	Create Quote/Trip, /Requests/Quotes/CreateQuote.aspx 
	Open Quotes, /Requests/OpenQuotes.aspx ACTIVE 
	Scheduled Trips, /Requests/Trips/ScheduledTrips.aspx 
Company, /mvc/company/view 
	Customers, /customers/customers.aspx 
	Pilots, /pilots/pilots.aspx 
	Aircraft, /aircraft/Aircraft.aspx 
