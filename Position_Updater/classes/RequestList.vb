Public Class RequestVehicle
    Private m_Registration As String
    Public Property Registration As String
        Get
            Return m_Registration
        End Get
        Set(ByVal value As String)
            m_Registration = value
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
    Private m_ID As Integer
    Public Property ID As Integer
        Get
            Return m_ID
        End Get
        Set(ByVal value As Integer)
            m_ID = value
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
    Private m_StartDate As DateTime
    Public Property StartDate As DateTime
        Get
            Return m_StartDate
        End Get
        Set(ByVal value As DateTime)
            m_StartDate = value
        End Set
    End Property
    Private m_EndDate As DateTime
    Public Property EndDate As DateTime
        Get
            Return m_EndDate
        End Get
        Set(ByVal value As DateTime)
            m_EndDate = value
        End Set
    End Property

    Private m_RecordCount As Integer
    Public Property RecordCount As Integer
        Get
            Return m_RecordCount
        End Get
        Set(value As Integer)
            m_RecordCount = value
        End Set
    End Property

    Private m_user As String
    Public Property user As String
        Get
            Return m_user
        End Get
        Set(ByVal value As String)
            m_user = value
        End Set
    End Property
End Class
