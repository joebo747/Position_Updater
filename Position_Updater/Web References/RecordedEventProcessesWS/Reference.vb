﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace RecordedEventProcessesWS
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="RecordedEventProcessesWSSoap", [Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData"),  _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(EntityBase))>  _
    Partial Public Class RecordedEventProcessesWS
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private tokenHeaderValueField As TokenHeader
        
        Private GetVehicleEventsInDateRangeOperationCompleted As System.Threading.SendOrPostCallback
        
        Private GetVehicleEventsXMostRecentOperationCompleted As System.Threading.SendOrPostCallback
        
        Private GetEventsSinceIDOperationCompleted As System.Threading.SendOrPostCallback
        
        Private GetEventsInIDRangeOperationCompleted As System.Threading.SendOrPostCallback
        
        Private GetEventsInDateRangeForVehiclesOperationCompleted As System.Threading.SendOrPostCallback
        
        Private GetEventsInDateRangeForDriversOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.Position_Updater.My.MySettings.Default.Position_Updater_RecordedEventProcessesWS_RecordedEventProcessesWS
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Property TokenHeaderValue() As TokenHeader
            Get
                Return Me.tokenHeaderValueField
            End Get
            Set
                Me.tokenHeaderValueField = value
            End Set
        End Property
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event GetVehicleEventsInDateRangeCompleted As GetVehicleEventsInDateRangeCompletedEventHandler
        
        '''<remarks/>
        Public Event GetVehicleEventsXMostRecentCompleted As GetVehicleEventsXMostRecentCompletedEventHandler
        
        '''<remarks/>
        Public Event GetEventsSinceIDCompleted As GetEventsSinceIDCompletedEventHandler
        
        '''<remarks/>
        Public Event GetEventsInIDRangeCompleted As GetEventsInIDRangeCompletedEventHandler
        
        '''<remarks/>
        Public Event GetEventsInDateRangeForVehiclesCompleted As GetEventsInDateRangeForVehiclesCompletedEventHandler
        
        '''<remarks/>
        Public Event GetEventsInDateRangeForDriversCompleted As GetEventsInDateRangeForDriversCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetVehicleEventsInDateRange", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetVehicleEventsInDateRange(ByVal VehicleID As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetVehicleEventsInDateRange", New Object() {VehicleID, StartDateTime, EndDateTime, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetVehicleEventsInDateRangeAsync(ByVal VehicleID As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short)
            Me.GetVehicleEventsInDateRangeAsync(VehicleID, StartDateTime, EndDateTime, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetVehicleEventsInDateRangeAsync(ByVal VehicleID As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetVehicleEventsInDateRangeOperationCompleted Is Nothing) Then
                Me.GetVehicleEventsInDateRangeOperationCompleted = AddressOf Me.OnGetVehicleEventsInDateRangeOperationCompleted
            End If
            Me.InvokeAsync("GetVehicleEventsInDateRange", New Object() {VehicleID, StartDateTime, EndDateTime, EventDescriptionIDs}, Me.GetVehicleEventsInDateRangeOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetVehicleEventsInDateRangeOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetVehicleEventsInDateRangeCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetVehicleEventsInDateRangeCompleted(Me, New GetVehicleEventsInDateRangeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetVehicleEventsXMostRecent", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetVehicleEventsXMostRecent(ByVal VehicleID As Short, ByVal X As Integer, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetVehicleEventsXMostRecent", New Object() {VehicleID, X, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetVehicleEventsXMostRecentAsync(ByVal VehicleID As Short, ByVal X As Integer, ByVal EventDescriptionIDs() As Short)
            Me.GetVehicleEventsXMostRecentAsync(VehicleID, X, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetVehicleEventsXMostRecentAsync(ByVal VehicleID As Short, ByVal X As Integer, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetVehicleEventsXMostRecentOperationCompleted Is Nothing) Then
                Me.GetVehicleEventsXMostRecentOperationCompleted = AddressOf Me.OnGetVehicleEventsXMostRecentOperationCompleted
            End If
            Me.InvokeAsync("GetVehicleEventsXMostRecent", New Object() {VehicleID, X, EventDescriptionIDs}, Me.GetVehicleEventsXMostRecentOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetVehicleEventsXMostRecentOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetVehicleEventsXMostRecentCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetVehicleEventsXMostRecentCompleted(Me, New GetVehicleEventsXMostRecentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetEventsSinceID", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetEventsSinceID(ByVal ID As Integer, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetEventsSinceID", New Object() {ID, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetEventsSinceIDAsync(ByVal ID As Integer, ByVal EventDescriptionIDs() As Short)
            Me.GetEventsSinceIDAsync(ID, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetEventsSinceIDAsync(ByVal ID As Integer, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetEventsSinceIDOperationCompleted Is Nothing) Then
                Me.GetEventsSinceIDOperationCompleted = AddressOf Me.OnGetEventsSinceIDOperationCompleted
            End If
            Me.InvokeAsync("GetEventsSinceID", New Object() {ID, EventDescriptionIDs}, Me.GetEventsSinceIDOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetEventsSinceIDOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetEventsSinceIDCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetEventsSinceIDCompleted(Me, New GetEventsSinceIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetEventsInIDRange", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetEventsInIDRange(ByVal StartID As Integer, ByVal EndID As Integer, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetEventsInIDRange", New Object() {StartID, EndID, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetEventsInIDRangeAsync(ByVal StartID As Integer, ByVal EndID As Integer, ByVal EventDescriptionIDs() As Short)
            Me.GetEventsInIDRangeAsync(StartID, EndID, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetEventsInIDRangeAsync(ByVal StartID As Integer, ByVal EndID As Integer, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetEventsInIDRangeOperationCompleted Is Nothing) Then
                Me.GetEventsInIDRangeOperationCompleted = AddressOf Me.OnGetEventsInIDRangeOperationCompleted
            End If
            Me.InvokeAsync("GetEventsInIDRange", New Object() {StartID, EndID, EventDescriptionIDs}, Me.GetEventsInIDRangeOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetEventsInIDRangeOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetEventsInIDRangeCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetEventsInIDRangeCompleted(Me, New GetEventsInIDRangeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetEventsInDateRangeForVehicle"& _ 
            "s", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetEventsInDateRangeForVehicles(ByVal VehicleIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetEventsInDateRangeForVehicles", New Object() {VehicleIDs, StartDateTime, EndDateTime, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetEventsInDateRangeForVehiclesAsync(ByVal VehicleIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short)
            Me.GetEventsInDateRangeForVehiclesAsync(VehicleIDs, StartDateTime, EndDateTime, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetEventsInDateRangeForVehiclesAsync(ByVal VehicleIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetEventsInDateRangeForVehiclesOperationCompleted Is Nothing) Then
                Me.GetEventsInDateRangeForVehiclesOperationCompleted = AddressOf Me.OnGetEventsInDateRangeForVehiclesOperationCompleted
            End If
            Me.InvokeAsync("GetEventsInDateRangeForVehicles", New Object() {VehicleIDs, StartDateTime, EndDateTime, EventDescriptionIDs}, Me.GetEventsInDateRangeForVehiclesOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetEventsInDateRangeForVehiclesOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetEventsInDateRangeForVehiclesCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetEventsInDateRangeForVehiclesCompleted(Me, New GetEventsInDateRangeForVehiclesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("TokenHeaderValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.omnibridge.com/SDKWebServices/AssetData/GetEventsInDateRangeForDrivers"& _ 
            "", RequestNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", ResponseNamespace:="http://www.omnibridge.com/SDKWebServices/AssetData", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GetEventsInDateRangeForDrivers(ByVal DriverIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short) As RecordedEvent()
            Dim results() As Object = Me.Invoke("GetEventsInDateRangeForDrivers", New Object() {DriverIDs, StartDateTime, EndDateTime, EventDescriptionIDs})
            Return CType(results(0),RecordedEvent())
        End Function
        
        '''<remarks/>
        Public Overloads Sub GetEventsInDateRangeForDriversAsync(ByVal DriverIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short)
            Me.GetEventsInDateRangeForDriversAsync(DriverIDs, StartDateTime, EndDateTime, EventDescriptionIDs, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub GetEventsInDateRangeForDriversAsync(ByVal DriverIDs() As Short, ByVal StartDateTime As Date, ByVal EndDateTime As Date, ByVal EventDescriptionIDs() As Short, ByVal userState As Object)
            If (Me.GetEventsInDateRangeForDriversOperationCompleted Is Nothing) Then
                Me.GetEventsInDateRangeForDriversOperationCompleted = AddressOf Me.OnGetEventsInDateRangeForDriversOperationCompleted
            End If
            Me.InvokeAsync("GetEventsInDateRangeForDrivers", New Object() {DriverIDs, StartDateTime, EndDateTime, EventDescriptionIDs}, Me.GetEventsInDateRangeForDriversOperationCompleted, userState)
        End Sub
        
        Private Sub OnGetEventsInDateRangeForDriversOperationCompleted(ByVal arg As Object)
            If (Not (Me.GetEventsInDateRangeForDriversCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent GetEventsInDateRangeForDriversCompleted(Me, New GetEventsInDateRangeForDriversCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData"),  _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData", IsNullable:=false)>  _
    Partial Public Class TokenHeader
        Inherits System.Web.Services.Protocols.SoapHeader
        
        Private tokenField As String
        
        Private anyAttrField() As System.Xml.XmlAttribute
        
        '''<remarks/>
        Public Property Token() As String
            Get
                Return Me.tokenField
            End Get
            Set
                Me.tokenField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAnyAttributeAttribute()>  _
        Public Property AnyAttr() As System.Xml.XmlAttribute()
            Get
                Return Me.anyAttrField
            End Get
            Set
                Me.anyAttrField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(RecordedEvent)),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData")>  _
    Partial Public MustInherit Class EntityBase
        
        Private idField As String
        
        '''<remarks/>
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData")>  _
    Partial Public Class RecordedEvent
        Inherits EntityBase
        
        Private vehicleIDField As Short
        
        Private eventIDField As Short
        
        Private eventTypeField As EventTypes
        
        Private startSeqField As Integer
        
        Private endSeqField As Integer
        
        Private driverIDField As Short
        
        Private originalDriverIDField As Short
        
        Private startField As Date
        
        Private endField As Date
        
        Private recordedDateTimeField As Date
        
        Private startOdoField As Single
        
        Private endOdoField As Single
        
        Private odometerField As Single
        
        Private startGPSIDField As Integer
        
        Private endGPSIDField As Integer
        
        Private totalTimeField As Integer
        
        Private totalOccursField As Integer
        
        Private valueField As Double
        
        Private litresField As Single
        
        Private f3ParameterField As Short
        
        Private f3ValueField As Double
        
        '''<remarks/>
        Public Property VehicleID() As Short
            Get
                Return Me.vehicleIDField
            End Get
            Set
                Me.vehicleIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property EventID() As Short
            Get
                Return Me.eventIDField
            End Get
            Set
                Me.eventIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property EventType() As EventTypes
            Get
                Return Me.eventTypeField
            End Get
            Set
                Me.eventTypeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property StartSeq() As Integer
            Get
                Return Me.startSeqField
            End Get
            Set
                Me.startSeqField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property EndSeq() As Integer
            Get
                Return Me.endSeqField
            End Get
            Set
                Me.endSeqField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property DriverID() As Short
            Get
                Return Me.driverIDField
            End Get
            Set
                Me.driverIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property OriginalDriverID() As Short
            Get
                Return Me.originalDriverIDField
            End Get
            Set
                Me.originalDriverIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Start() As Date
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property [End]() As Date
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property RecordedDateTime() As Date
            Get
                Return Me.recordedDateTimeField
            End Get
            Set
                Me.recordedDateTimeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property StartOdo() As Single
            Get
                Return Me.startOdoField
            End Get
            Set
                Me.startOdoField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property EndOdo() As Single
            Get
                Return Me.endOdoField
            End Get
            Set
                Me.endOdoField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Odometer() As Single
            Get
                Return Me.odometerField
            End Get
            Set
                Me.odometerField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property StartGPSID() As Integer
            Get
                Return Me.startGPSIDField
            End Get
            Set
                Me.startGPSIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property EndGPSID() As Integer
            Get
                Return Me.endGPSIDField
            End Get
            Set
                Me.endGPSIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property TotalTime() As Integer
            Get
                Return Me.totalTimeField
            End Get
            Set
                Me.totalTimeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property TotalOccurs() As Integer
            Get
                Return Me.totalOccursField
            End Get
            Set
                Me.totalOccursField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Value() As Double
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Litres() As Single
            Get
                Return Me.litresField
            End Get
            Set
                Me.litresField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property F3Parameter() As Short
            Get
                Return Me.f3ParameterField
            End Get
            Set
                Me.f3ParameterField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property F3Value() As Double
            Get
                Return Me.f3ValueField
            End Get
            Set
                Me.f3ValueField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.omnibridge.com/SDKWebServices/AssetData")>  _
    Public Enum EventTypes
        
        '''<remarks/>
        Detail
        
        '''<remarks/>
        Summary
        
        '''<remarks/>
        Notify
        
        '''<remarks/>
        Database
        
        '''<remarks/>
        DatabaseNotify
        
        '''<remarks/>
        System
        
        '''<remarks/>
        SystemDetail
        
        '''<remarks/>
        SystemSummary
        
        '''<remarks/>
        SystemNotify
        
        '''<remarks/>
        SystemGenerated
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetVehicleEventsInDateRangeCompletedEventHandler(ByVal sender As Object, ByVal e As GetVehicleEventsInDateRangeCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetVehicleEventsInDateRangeCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetVehicleEventsXMostRecentCompletedEventHandler(ByVal sender As Object, ByVal e As GetVehicleEventsXMostRecentCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetVehicleEventsXMostRecentCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetEventsSinceIDCompletedEventHandler(ByVal sender As Object, ByVal e As GetEventsSinceIDCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetEventsSinceIDCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetEventsInIDRangeCompletedEventHandler(ByVal sender As Object, ByVal e As GetEventsInIDRangeCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetEventsInIDRangeCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetEventsInDateRangeForVehiclesCompletedEventHandler(ByVal sender As Object, ByVal e As GetEventsInDateRangeForVehiclesCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetEventsInDateRangeForVehiclesCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")>  _
    Public Delegate Sub GetEventsInDateRangeForDriversCompletedEventHandler(ByVal sender As Object, ByVal e As GetEventsInDateRangeForDriversCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GetEventsInDateRangeForDriversCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As RecordedEvent()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),RecordedEvent())
            End Get
        End Property
    End Class
End Namespace
