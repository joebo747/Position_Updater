Public Class GPSsent
    Private m_JobNumber As Integer
    Public Property JobNumber As Integer
        Get
            Return m_JobNumber
        End Get
        Set(ByVal value As Integer)
            m_JobNumber = value
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
    Private m_Count As Int16
    Public Property Count As Int16
        Get
            Return m_Count
        End Get
        Set(value As Int16)
            m_Count = value
        End Set
    End Property
    Private _ColDate As DateTime
    ''' <summary>
    ''' Date the Job should be collected/Delivered
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property ColDate As DateTime
        Get
            Return _ColDate
        End Get
        Set(ByVal value As DateTime)
            _ColDate = value
        End Set
    End Property

    Private _DelDate As DateTime
    ''' <summary>
    ''' Date the Job should be collected/Delivered
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property DelDate As DateTime
        Get
            Return _DelDate
        End Get
        Set(ByVal value As DateTime)
            _DelDate = value
        End Set
    End Property
    Private _StartDate As DateTime
    ''' <summary>
    ''' Date the Job should be collected/Delivered
    ''' </summary>
    ''' <value></value>StartDate
    ''' <remarks></remarks>
    Public Property StartDate As DateTime
        Get
            Return _StartDate
        End Get
        Set(ByVal value As DateTime)
            _StartDate = value
        End Set
    End Property

    Private _EndDate As DateTime
    ''' <summary>
    ''' Date the Job should be collected/Delivered
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property EndDate As DateTime
        Get
            Return _EndDate
        End Get
        Set(ByVal value As DateTime)
            _EndDate = value
        End Set
    End Property
End Class
