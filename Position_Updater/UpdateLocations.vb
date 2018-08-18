Imports System.ComponentModel

Public Class UpdateLocations
    Implements INotifyPropertyChanged
#Region "INotifyPropertyChanged Members"

    Public Event PropertyChanged As PropertyChangedEventHandler _
       Implements INotifyPropertyChanged.PropertyChanged

    Private Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))

    End Sub

#End Region
    '  Private CIDField As Integer
    Private m_AccID As Integer

    Public Property AccID As Integer
        Get
            Return m_AccID
        End Get
        Set(value As Integer)
            Me.m_AccID = value
        End Set
    End Property
    ' Private ModemIDField As Long
    Private m_vehID As Integer

    Public Property vehID As Integer
        Get
            Return m_vehID
        End Get
        Set(value As Integer)
            Me.m_vehID = value
        End Set
    End Property

    Private m_DateOfFix As DateTime
    Public Property DateOfFix() As DateTime
        Get
            Return Me.m_DateOfFix
        End Get
        Set(value As DateTime)
            If Me.m_DateOfFix <> value Then
                Me.m_DateOfFix = value
                Me.OnPropertyChanged("DateOfFix")
            End If
        End Set
    End Property

    Private m_Registration As String

    Public Property Registration() As String
        Get
            Return Me.m_Registration
        End Get
        Set(value As String)
            If Me.m_Registration <> value Then
                Me.m_Registration = value
                Me.OnPropertyChanged("Registration")
            End If
        End Set
    End Property

    'Private m_jobnum As Integer

    'Public Property jobnum As Integer
    '    Get
    '        Return m_jobnum
    '    End Get
    '    Set(value As Integer)
    '        Me.m_jobnum = value
    '    End Set
    'End Property

    Private m_LatLng As String
    Public Property LatLng() As String
        Get
            Return Me.m_LatLng
        End Get
        Set(value As String)
            If Me.m_LatLng <> value Then
                Me.m_LatLng = value
                Me.OnPropertyChanged("LatLng")
            End If
        End Set
    End Property

    Private m_timelag As Integer
    Public Property timelag As Integer
        Get
            Return m_timelag
        End Get
        Set(value As Integer)
            Me.m_timelag = value
        End Set
    End Property
    
End Class
