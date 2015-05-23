' MenuSample - C# console application
' This console application accepts 2 argument: 
' first, a path to a menu .xml file (e.g. “c:\schedulemenu1.xml”);
' second,  an activePath to match (e.g. “/default.aspx”). 
' This module parses through the xml document ignoring any XML content not required for this application.
' It identifies currently active menu items if it or any of its children paths match the activePath argument.
' It writes the parsed menu to the output showing the displayName and path structure for each menu item, 
' it indents subMenu items, and prints the word "ACTIVE" next to any active menu items.
' ©2014 Christopher C. Stultz
' 
' Requirements:
' Requires an accessible menu .xml document 
'
' Usage:
' Dim strMenuOutput to collect all menu items to be written to the console until after parsing is complete
' Dim strSubMenuOutput to collect all subMenu items to be written to the console until after parsing is complete
' Dim strIndent for parring of the subMenuOutput
' Dim activePath holds the path value attribute to be matched during the parse of menu .xml from the 2nd argument passed during exe call
' Dim xmld holds the current document loaded from filePath
' Dim xmlRoot holds the root element of xmld
' Dim argsRequired holds the required amount of arguments passed during execution
' Dim argv holds the argument(s) passed during exe call
' Dim filePath holds the path of the menu .xml file from the 1st argument passed during exe call
' Call sub ReadXML(root As XmlNode) is the recursive subroutine that parses the menu .xml document
' Call sub DoWork(xmln As XmlNode) performs the logic necessary to parse XML menu and write items to the console
' Call function evaluateArgs(argv As System.Collections.ObjectModel.ReadOnlyCollection(Of String), argsRequired As Integer) as Boolean checks to see if the argument(s) provided (if any) meet argument requirements of the exe
' Call function Load_XML_Document(filePath As String) as xmlDocument to load the menu .xml document 
' Call Sub Print to write strOutput to console.
' 
' Limitations:
' Sub Main written specifically to parse a menu .xml (e.g. "schedulemenu1.xml" or ""schedulemenu2.xml").

Imports System
Imports System.Collections.Generic
Imports System.Xml

Module modMain
    Dim strMenuOutput As String
    Dim strSubMenuOutput As String
    Dim strIndent As String = ""
    Dim activePath As String = ""

    Sub Main()
        'initialize the XML document
        Dim xmld As XmlDocument = New XmlDocument
        Dim xmlRoot As XmlNode
        'set argument requirement(s)
        Dim argsRequired As Integer = 2
        Dim filePath As String = ""
        Dim argv As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        'accept and store arguments from the command line exe call
        argv = My.Application.CommandLineArgs
        'evaluate if argument(s) provided meet argument requirement(s)
        If evaluateArgs(argv, argsRequired) Then
            'accept and store 1st argument as path to menu .xml
            filePath = argv(0)
            'verify path provided exists on the network
            If LCase(My.Computer.FileSystem.FileExists(LCase(filePath))) Then
                'accept and store the 2nd argument as the activePath
                activePath = argv(1)
                'accept and store the XML document
                xmld = Load_XML_Document(filePath)
                'set the root element for parsing
                xmlRoot = xmld.DocumentElement
                'read the XML document starting from the root
                ReadXML(xmlRoot)
                'check to see if this is the last menu item
                If strMenuOutput = "" Then
                    'do nothing
                Else
                    'write line feed
                    strMenuOutput += vbCrLf
                    'print the menu item
                    Print(strMenuOutput)
                    'reset for the next menu item
                    strMenuOutput = ""
                    'check to see if this is the last subMenu item
                    If strSubMenuOutput = "" Then
                        'do nothing
                    Else
                        'print the subMenu item
                        Print(strSubMenuOutput)
                        'reset for the next subMenu item
                        strSubMenuOutput = ""
                        'reset for the next sub menu
                        strIndent = ""
                    End If
                End If
            Else
                'inform user file path does not exist or is inaccessible on the network 
                Console.Write("1st argument 'file path' provided does not exist or is inaccessible.")
                'terminate the subroutine
                Exit Sub
            End If
            'argument requirement not met
        Else
            'inform user argument requirements during exe call
            Console.Write(vbCrLf & _
                          "2 string arguments are required when calling MenuSample.exe" & vbCrLf & vbCrLf & _
                          "1st argument: a 'file path' to a menu .xml" & vbCrLf & vbCrLf & _
                          "(eg. ""c:\schedulemenu1.xml"" or ""c:\schedulemenu2.xml"");" & vbCrLf & vbCrLf & _
                          "2nd argument: a 'path value' subMenu item found in the 'file path' provided" & vbCrLf)
            'terminate the subroutine
            Exit Sub
        End If
    End Sub

    'recursively parses the menu .xml document
    Sub ReadXML(root As XmlNode)
        'ensure the node is an element
        If (root.NodeType = XmlNodeType.Element) Then
            'perform the work on the element
            DoWork(root)
            'detect if node has children nodes
            If (root.HasChildNodes) Then
                'check to see if element name is subMenu
                If root.Name = "subMenu" Then
                    'indent the subMenu
                    strIndent += vbTab
                    'read the first child of the current subMenu node
                    ReadXML(root.FirstChild)
                    'check to see if element name is menu
                ElseIf root.Name = "menu" Then
                    'read the first child of the current menu node
                    ReadXML(root.FirstChild)
                Else
                    'read the first child of the current non menu and non subMenu node
                    ReadXML(root.FirstChild)
                End If
            End If
        End If
        'check to see if the next element in the current node is nothing
        If root.NextSibling Is Nothing Then
            'do nothing
        Else
            'read the next node in the element
            ReadXML(root.NextSibling)
        End If
    End Sub

    'performs the logical work necessay to write items to console
    Sub DoWork(xmln As XmlNode)
        'get the name of the element
        Select Case (xmln.Name)
            'check element name for displayName
            Case "displayName"
                'check ancestor to see if node is a subMenu item
                If xmln.ParentNode.ParentNode.Name = "subMenu" Then
                    'update output with indent and displayName subMenuItem
                    strSubMenuOutput += strIndent & xmln.InnerText
                    'update output with comma seperator
                    strSubMenuOutput += ", "
                    'check ancestor to see if node is a subMenu item
                ElseIf xmln.ParentNode.ParentNode.Name = "menu" Then
                    'check to see if this is the first menu item
                    If strMenuOutput = "" Then
                        'do nothing
                    Else
                        'line drop for last menuItem
                        strMenuOutput += vbCrLf
                        'print menuItem
                        Print(strMenuOutput)
                        'reset strMenuOutput for next menuItem
                        strMenuOutput = ""
                        'check to see if this is the first subMenu item
                        If strSubMenuOutput = "" Then
                        Else
                            'print subMenuItem(s)
                            Print(strSubMenuOutput)
                            'reset strSubMenuOutput for next subMenuItem(s)
                            strSubMenuOutput = ""
                            'reset strIndent for next subMenu
                            strIndent = ""
                        End If
                    End If
                    'updated output with menu displayName
                    strMenuOutput += xmln.InnerText
                    'update output with comma seperator
                    strMenuOutput += ", "
                End If
                'check to see if xmln.Name is path
            Case "path"
                'check ancestor to see if node is a subMenu item
                If xmln.ParentNode.ParentNode.Name = "subMenu" Then
                    'update output with path structure
                    strSubMenuOutput += xmln.Attributes.GetNamedItem("value").Value
                    'check to see if current path structure matches the activePath argument for a subMenuItem
                    If LCase(activePath) = LCase(xmln.Attributes.GetNamedItem("value").Value) Then
                        'test to see if menu is already active 
                        If strMenuOutput.Contains("ACTIVE") Then
                            'do nothing
                        Else
                            'update menuItem to active
                            strMenuOutput += " ACTIVE"
                        End If
                        'update subMenuItem to active
                        strSubMenuOutput += " ACTIVE"
                    End If
                    'line drop for subMenuItem
                    strSubMenuOutput += vbCrLf
                    'check ancestor to see if node is a menuItem
                ElseIf xmln.ParentNode.ParentNode.Name = "menu" Then
                    'update output with path structure
                    strMenuOutput += xmln.Attributes.GetNamedItem("value").Value
                    'check to see if current path structure matches the activePath argument for a menuItem
                    If LCase(activePath) = LCase(xmln.Attributes.GetNamedItem("value").Value) Then
                        'updated strMenuOutput to active
                        strMenuOutput += " ACTIVE"
                    End If
                End If
        End Select
    End Sub

    'loads the xmlDocument from passed filePath variable
    'returns the xmlDocument
    Function Load_XML_Document(filePath As String) As XmlDocument
        Dim doc_xmld As XmlDocument
        'Create the XML Document
        doc_xmld = New XmlDocument()
        'Catch load error and report to console.
        Try
            'Load the Xml file
            doc_xmld.Load(filePath)
        Catch errorVariable As Exception
            'Error trapping
            Console.Write(errorVariable.ToString())
        End Try
        'Return the XML document
        Return doc_xmld
    End Function

    'evaluates arg(s) accepted from command line against argsRequired
    'returns boolean
    Function evaluateArgs(argv As System.Collections.ObjectModel.ReadOnlyCollection(Of String), argsRequired As Integer) As Boolean
        'check to see if argument array matches argsRequired
        If argv.Count = argsRequired Then
            'argsRequirement(s) met
            Return True
        Else
            'argsRequirement(s) not met
            Return False
        End If
    End Function

    'prints to console passed strOutput
    Sub Print(strOutput As String)
        'write to console
        Console.Write(strOutput)
    End Sub

End Module
