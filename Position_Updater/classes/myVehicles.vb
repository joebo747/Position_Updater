Imports System.Linq
Imports Position_Updater

Public Class myVehicle
    'Implements IComparer(Of myVehicle)

    Private m_ID As Integer
    Public Property ID As Integer
        Get
            Return m_ID
        End Get
        Set(ByVal value As Integer)
            m_ID = value
        End Set
    End Property

    Private m_BlueTreeID As Integer
    Public Property BluetreeID As Integer
        Get
            Return m_BlueTreeID
        End Get
        Set(ByVal value As Integer)
            m_BlueTreeID = value
        End Set
    End Property

    Private m_Registration As String
    Public Property Registration As String
        Get
            Return m_Registration
        End Get
        Set(ByVal value As String)
            m_Registration = value
        End Set
    End Property

    Private m_Driver As String
    Public Property Driver As String
        Get
            Return m_Driver
        End Get
        Set(ByVal value As String)
            m_Driver = value
        End Set
    End Property

    Private m_mixDriverID As Integer
    Public Property mixDriverID As Integer
        Get
            Return m_mixDriverID
        End Get
        Set(ByVal value As Integer)
            m_mixDriverID = value
        End Set
    End Property

    Private m_Trailer As String
    Public Property Trailer As String
        Get
            Return m_Trailer
        End Get
        Set(ByVal value As String)
            m_Trailer = value
        End Set
    End Property

    Private m_vehclass As String

    Public Property vehclass As String
        Get
            Return m_vehclass
        End Get
        Set(ByVal value As String)
            m_vehclass = value
        End Set
    End Property

    Private m_SendResult As String

    Public Property SendResult As String
        Get
            Return m_SendResult
        End Get
        Set(ByVal value As String)
            m_SendResult = value
        End Set
    End Property


    Private m_DateofFix As DateTime
    Public Property DateofFix As DateTime
        Get
            Return m_DateofFix
        End Get
        Set(ByVal value As DateTime)
            m_DateofFix = value
        End Set
    End Property

    Private m_mDateofFix As DateTime
    Public Property mDateofFix As DateTime
        Get
            Return m_mDateofFix
        End Get
        Set(ByVal value As DateTime)
            m_mDateofFix = value
        End Set
    End Property

    Private m_odo As Integer = 0
    Public Property odo As Integer
        Get
            Return m_odo
        End Get
        Set(value As Integer)
            m_odo = value
        End Set
    End Property

    Private m_distance As Integer = 0
    Public Property distance As Integer
        Get
            Return m_distance
        End Get
        Set(value As Integer)
            m_distance = value
        End Set
    End Property

    Private m_Statusid As Integer
    Public Property Statusid As Integer
        Get
            Return m_Statusid
        End Get
        Set(ByVal value As Integer)
            m_Statusid = value
        End Set
    End Property

    Private m_LatLong As String
    Public Property LatLong As String
        Get
            Return m_LatLong
        End Get
        Set(ByVal value As String)
            m_LatLong = value
        End Set
    End Property

    Private m_mLatLong As String
    Public Property mLatLong As String
        Get
            Return m_mLatLong
        End Get
        Set(ByVal value As String)
            m_mLatLong = value
        End Set
    End Property

    Private m_Company As Int16
    Public Property Company As Int16
        Get
            Return m_Company
        End Get
        Set(ByVal value As Int16)
            m_Company = value
        End Set
    End Property

    Private m_JobNumber As Integer
    Public Property JobNumber As Integer
        Get
            Return m_JobNumber
        End Get
        Set(ByVal value As Integer)
            m_JobNumber = value
        End Set
    End Property

    Private m_Temperature As String
    Public Property Temperature As String
        Get
            Return m_Temperature
        End Get
        Set(ByVal value As String)
            m_Temperature = value
        End Set
    End Property

    Private m_MTemperature As String
    Public Property MTemperature As String
        Get
            Return m_MTemperature
        End Get
        Set(ByVal value As String)
            m_MTemperature = value
        End Set
    End Property



    Private m_MixTempID As String
    Public Property MixTempID As String
        Get
            Return m_MixTempID
        End Get
        Set(ByVal value As String)
            m_MixTempID = value
        End Set
    End Property

    Private m_eLatLong As String
    Public Property eLatLong As String
        Get
            Return m_eLatLong
        End Get
        Set(ByVal value As String)
            m_eLatLong = value
        End Set
    End Property

    Private m_Dateofevent As DateTime
    Public Property DateofEvent As DateTime
        Get
            Return m_Dateofevent
        End Get
        Set(ByVal value As DateTime)
            m_Dateofevent = value
        End Set
    End Property

    Private m_mDateofevent As DateTime
    Public Property mDateofEvent As DateTime
        Get
            Return m_mDateofevent
        End Get
        Set(ByVal value As DateTime)
            m_mDateofevent = value
        End Set
    End Property

    Private m_Pos_DateofFix As String
    Public Property Pos_DateofFix As String
        Get
            Return m_Pos_DateofFix
        End Get
        Set(ByVal value As String)
            m_Pos_DateofFix = value
        End Set
    End Property

    Private m_trackedBy As String
    Public Property TrackedBy As String
        Get
            Return m_trackedBy
        End Get
        Set(ByVal value As String)
            m_trackedBy = value
        End Set
    End Property

    Private m_expose As Boolean
    Public Property expose As Boolean
        Get
            Return m_expose
        End Get
        Set(ByVal value As Boolean)
            m_expose = value
        End Set
    End Property

    Private m_NewLoc As Boolean
    Public Property NewLoc As Boolean
        Get
            Return m_NewLoc
        End Get
        Set(ByVal value As Boolean)
            m_NewLoc = value
        End Set
    End Property

    Private m_mNewLoc As Boolean
    Public Property mNewLoc As Boolean
        Get
            Return m_mNewLoc
        End Get
        Set(ByVal value As Boolean)
            m_mNewLoc = value
        End Set
    End Property

    Private m_NewEvt As Boolean
    Public Property NewEvt As Boolean
        Get
            Return m_NewEvt
        End Get
        Set(ByVal value As Boolean)
            m_NewEvt = value
        End Set
    End Property

    Private m_mNewEvt As Boolean
    Public Property mNewEvt As Boolean
        Get
            Return m_mNewEvt
        End Get
        Set(ByVal value As Boolean)
            m_mNewEvt = value
        End Set
    End Property

    Private m_Address As String
    Public Property Address As String
        Get
            Return m_Address
        End Get
        Set(ByVal value As String)
            m_Address = value
        End Set
    End Property

    Private m_Acc_Name As String
    Public Property Acc_Name As String
        Get
            Return m_Acc_Name
        End Get
        Set(ByVal value As String)
            m_Acc_Name = value
        End Set
    End Property

    Private m_acc_ID As Integer
    Public Property Acc_ID As Integer
        Get
            Return m_acc_ID
        End Get
        Set(value As Integer)
            m_acc_ID = value
        End Set
    End Property

    Private m_DateSubmitted As DateTime
    Public Property DateSubmitted As DateTime
        Get
            Return m_DateSubmitted
        End Get
        Set(ByVal value As DateTime)
            m_DateSubmitted = value
        End Set
    End Property

    Private m_MixID As Int16
    Public Property MixID As Int16
        Get
            Return m_MixID
        End Get
        Set(value As Int16)
            m_MixID = value
        End Set
    End Property

    Private m_MixRecID As Integer = 0
    Public Property MixRecID As Integer
        Get
            Return m_MixRecID
        End Get
        Set(value As Integer)
            m_MixRecID = value
        End Set
    End Property

    Public Property Speed() As Single
        Get
            Return m_Speed
        End Get
        Set
            m_Speed = Value
        End Set
    End Property
    Private m_Speed As Single

    Public Property Direction() As Single
        Get
            Return m_Direction
        End Get
        Set
            m_Direction = Value
        End Set
    End Property
    Private m_Direction As Single

    Public Property MultiJobs() As Single
        Get
            Return m_MultiJobs
        End Get
        Set
            m_MultiJobs = Value
        End Set
    End Property
    Private m_MultiJobs As Single

    Private m_Destination As String
    Public Property Destination As String
        Get
            Return m_Destination
        End Get
        Set(ByVal value As String)
            m_Destination = value
        End Set
    End Property

    Private m_FleetFence As Integer
    Public Property FleetFence As Integer
        Get
            Return m_FleetFence
        End Get
        Set(ByVal value As Integer)
            m_FleetFence = value
        End Set
    End Property
    Public Sub New()
        'Me.Registration = Registration 'Regoverride(ID, Registration)
        'Me.ID = ID
        'DateofFix = m_DateofFix
        'Statusid = m_Statusid
        'Exists = m_exisits
        'LatLong = m_LatLong
        Company = m_Company
    End Sub


    ' Public doc As XDocument = XDocument.Load("\\dc1\applications$\DMUsers\vehoverride.xml")
    '    Public Function Regoverride(ByVal vehID As Integer, ByVal Reg As String) As String
    '        Try
    '            Dim qVEh = (From xe In doc.Descendants("Vehicle") _
    '                       Where xe.Element("ID").Value.ToUpper = vehID _
    '                       Select New With { _
    '                            .vehID = xe.Element("ID").Value, _
    '                            .Reg = xe.Element("reg").Value, _
    '                            .Newreg = xe.Element("Newreg").Value _
    '                            } _
    '                       ).FirstOrDefault
    '            If Not qVEh Is Nothing Then
    '                Return qVEh.Newreg
    '            Else
    '                Return Reg
    '            End If
    '        Catch ex As Exception
    '            Return Reg
    '        End Try
    '    End Function
    '    Public Function Compare(ByVal x As Vehicle, ByVal y As Vehicle) As Integer Implements System.Collections.Generic.IComparer(Of Vehicle).Compare
    '        If x.ID < y.ID Then
    '            Return 1
    '        Else
    '            Return 0
    '        End If
    '    End Function
End Class


