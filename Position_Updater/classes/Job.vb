Public Class Job
    Private _jobnum As String
    ''' <summary>
    ''' The Job Number. (dwjobnumber)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property jobnum As String
        Get
            Return _jobnum
        End Get
        Set(ByVal value As String)
            _jobnum = value
        End Set
    End Property

    Private _vehicle As String
    ''' <summary>
    ''' The vehicle Registration.
    ''' </summary>
    ''' <value>Last Assigned registration.</value>
    ''' <remarks></remarks>
    Public Property vehicle As String
        Get
            Return _vehicle
        End Get
        Set(ByVal value As String)
            _vehicle = value
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

    Private _RecordsSent As String
    ''' <summary>
    ''' Number of items on the load.
    ''' </summary>
    ''' <value>No longer used</value>
    ''' <remarks></remarks>
    Public Property RecordsSent As String
        Get
            Return _RecordsSent
        End Get
        Set(ByVal value As String)
            _RecordsSent = value
        End Set
    End Property

    Private _ReSend As Boolean
    ''' <summary>
    ''' The Job Re-Send at time of sending.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property ReSend As Boolean
        Get
            Return _ReSend
        End Get
        Set(ByVal value As Boolean)
            _ReSend = value
        End Set
    End Property

End Class
