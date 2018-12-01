Imports System.Threading.Tasks

Public Class ShapedForm1
    Public isrunning As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        'Dim NewUser As New AliasAccount("mick.Candela", "Twat1234", "gba.local")
        'NewUser.BeginImpersonation()

    End Sub

    Private Async Sub ShapedForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim sdate As DateTime = Nothing

            Timer1.Start()
            '  Parallel.Invoke(New Action(AddressOf GetTransportVehicles), New Action(AddressOf loginMix), New Action(AddressOf timerStart))
            ' Exit Sub
            '
            Dim vehTask As Task(Of Integer) = GetTransportVehicles()
            progresstxt = "Getting Vehicles"
            Dim vehresult As Integer = Await vehTask
            'If loginresult = True Then
            '    progresstxt = "Mix Logging In"
            '    Dim mixtask As Task(Of Boolean) = getlatestMixPositions()
            '    Dim mixResult As Boolean = Await mixtask
            '    If mixResult = True Then
            '        progresstxt = "Mix Logged In"
            '    Else
            '        Throw New System.Exception("Failed to get mix login")
            '    End If
            'End If

            If vehresult > 0 Then
                progresstxt = "Vehicles Completed"
                'Dim logintask As Task(Of Boolean) = loginMix()
                'Dim loginresult As Boolean = Await logintask
                'If loginresult = True Then
                progresstxt = "Mix Logging In"
                Dim logintask As Task(Of Boolean) = loginMix()
                Dim loginresult As Boolean = Await logintask
                If loginresult = True Then
                    ' Dim img As Image = ImageList1.Images(0)
                    'PopUpAlert("Logged into mix", "Mix Logged On", 1, img)
                    Dim BlueTask As Task(Of Boolean) = GetBlueeTreeEvents()
                    progresstxt = "Getting Blue Tree Vehicle ID's"
                    Dim N As Int16 = 0
                    Dim blueResult As Boolean = Await BlueTask
                    If blueResult = True Then
                        System.Threading.Thread.Sleep(500)
                        Position_Updater.Show()
                        Timer1.Stop()
                    Else
                        progresstxt = "Failed to get Blue Tree Vehicle ID's"
                        System.Threading.Thread.Sleep(500)
                        Position_Updater.Show()
                        Timer1.Stop()
                    End If
                Else
                    MessageBox.Show("Error Loging onto mix")
                End If

            End If
            ' End If


        Catch ex As Exception

        End Try
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If isrunning = False Then

            isrunning = True
        End If
        If RadProgressBar1.Value1 >= 100 Then
            progress = 0
            If progresstxt = "Mix Logged In" Then


                Me.Visible = False
            ElseIf progresstxt = "Getting Vehicles" Or progresstxt = "Vehicles Complete" Then
                progress = 0
            ElseIf progresstxt = "Mix Logging In" Then
                progress = 0
            End If
            RadProgressBar1.Value1 = progress
        Else
            RadProgressBar1.Value1 = progress
            ' RadProgressBar1.Value2 += 1
            RadProgressBar1.Text = progresstxt

        End If
        progress += 1
    End Sub

    Private Sub ShapedForm1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Position_Updater.Visible = False Then
                Position_Updater.Show()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
Public Class AliasAccount
    Private _username, _password, _domainname As String

    Private _tokenHandle As New IntPtr(0)
    Private _dupeTokenHandle As New IntPtr(0)
    Private _impersonatedUser As System.Security.Principal.WindowsImpersonationContext


    Public Sub New(ByVal username As String, ByVal password As String)
        Dim nameparts() As String = username.Split("\")
        If nameparts.Length > 1 Then
            _domainname = nameparts(0)
            _username = nameparts(1)
        Else
            _username = username
        End If
        _password = password
    End Sub

    Public Sub New(ByVal username As String, ByVal password As String, ByVal domainname As String)
        _username = username
        _password = password
        _domainname = domainname
    End Sub


    Public Sub BeginImpersonation()
        Const LOGON32_PROVIDER_DEFAULT As Integer = 0
        Const LOGON32_LOGON_INTERACTIVE As Integer = 2
        Const SecurityImpersonation As Integer = 2

        Dim win32ErrorNumber As Integer

        _tokenHandle = IntPtr.Zero
        _dupeTokenHandle = IntPtr.Zero

        If Not LogonUser(_username, _domainname, _password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, _tokenHandle) Then
            win32ErrorNumber = System.Runtime.InteropServices.Marshal.GetLastWin32Error()
            Throw New ImpersonationException(win32ErrorNumber, GetErrorMessage(win32ErrorNumber), _username, _domainname)
        End If

        If Not DuplicateToken(_tokenHandle, SecurityImpersonation, _dupeTokenHandle) Then
            win32ErrorNumber = System.Runtime.InteropServices.Marshal.GetLastWin32Error()

            CloseHandle(_tokenHandle)
            Throw New ImpersonationException(win32ErrorNumber, "Unable to duplicate token!", _username, _domainname)
        End If

        Dim newId As New System.Security.Principal.WindowsIdentity(_dupeTokenHandle)
        _impersonatedUser = newId.Impersonate()
    End Sub


    Public Sub EndImpersonation()
        If Not _impersonatedUser Is Nothing Then
            _impersonatedUser.Undo()
            _impersonatedUser = Nothing

            If Not System.IntPtr.op_Equality(_tokenHandle, IntPtr.Zero) Then
                CloseHandle(_tokenHandle)
            End If
            If Not System.IntPtr.op_Equality(_dupeTokenHandle, IntPtr.Zero) Then
                CloseHandle(_dupeTokenHandle)
            End If
        End If
    End Sub


    Public ReadOnly Property username() As String
        Get
            Return _username
        End Get
    End Property

    Public ReadOnly Property domainname() As String
        Get
            Return _domainname
        End Get
    End Property


    Public ReadOnly Property currentWindowsUsername() As String
        Get
            Return System.Security.Principal.WindowsIdentity.GetCurrent().Name
        End Get
    End Property


#Region "Exception Class"
    Public Class ImpersonationException
        Inherits System.Exception

        Public ReadOnly win32ErrorNumber As Integer

        Public Sub New(ByVal win32ErrorNumber As Integer, ByVal msg As String, ByVal username As String, ByVal domainname As String)
            MyBase.New(String.Format("Impersonation of {1}\{0} failed! [{2}] {3}", username, domainname, win32ErrorNumber, msg))
            Me.win32ErrorNumber = win32ErrorNumber
        End Sub
    End Class
#End Region


#Region "External Declarations and Helpers"
    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String],
            ByVal lpszDomain As [String], ByVal lpszPassword As [String],
            ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer,
            ByRef phToken As IntPtr) As Boolean


    Private Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal ExistingTokenHandle As IntPtr,
                ByVal SECURITY_IMPERSONATION_LEVEL As Integer,
                ByRef DuplicateTokenHandle As IntPtr) As Boolean


    Private Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean


    <System.Runtime.InteropServices.DllImport("kernel32.dll")>
    Private Shared Function FormatMessage(ByVal dwFlags As Integer, ByRef lpSource As IntPtr,
            ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByRef lpBuffer As [String],
            ByVal nSize As Integer, ByRef Arguments As IntPtr) As Integer
    End Function


    Private Function GetErrorMessage(ByVal errorCode As Integer) As String
        Dim FORMAT_MESSAGE_ALLOCATE_BUFFER As Integer = &H100
        Dim FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
        Dim FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000

        Dim messageSize As Integer = 255
        Dim lpMsgBuf As String
        Dim dwFlags As Integer = FORMAT_MESSAGE_ALLOCATE_BUFFER Or FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS

        Dim ptrlpSource As IntPtr = IntPtr.Zero
        Dim prtArguments As IntPtr = IntPtr.Zero

        Dim retVal As Integer = FormatMessage(dwFlags, ptrlpSource, errorCode, 0, lpMsgBuf, messageSize, prtArguments)
        If 0 = retVal Then
            Throw New System.Exception("Failed to format message for error code " + errorCode.ToString() + ". ")
        End If

        Return lpMsgBuf
    End Function

#End Region

End Class