Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports System.Threading.Tasks

Module MixModule
    Public userName As String = My.Settings.mixUser
    Public password As String = My.Settings.mixPass
    Public Myfile As String = "\\gba.local\GBA_Share\GBA_Sharedwork\DFS_Applications\DMUsers\MixCookie\mixTestcookie.txt"
    Public appId As String = My.Computer.Name + "_GBA_Positon_Updater_" + Now.Year.ToString + "_" + Now.Month.ToString() + "_" + Now.Day.ToString()
    Public vehlist As New ObservableCollection(Of myVehicle)
    Public thisLogin As New CoreWS.CoreWS
    Public thisOrgHeader As New CoreWS.TokenHeader
    Public thisOrgUserContext As New CoreWS.UserContext()
    Public MaxMixID As Integer = 0 'To hold the Identity of the greatest record returned during GetTripRecords as some records may not be written back and the function will be calling a false record
    Public vehicleDetail As New VehicleProcessesWS.VehicleProcessesWS()

    Public token As CoreWS.WebLoginResult
    Public _login1 As CoreWS.WebLoginResult

    Public thisUser As CoreWS.CoreWS = New CoreWS.CoreWS
    Public userContext As CoreWS.UserContext = New CoreWS.UserContext
    Public loginTokenHeader As CoreWS.TokenHeader = New CoreWS.TokenHeader
    Public loginResult As CoreWS.WebLoginResult = New CoreWS.WebLoginResult
    Public vehicleProcessesWS As VehicleProcessesWS.VehicleProcessesWS = New VehicleProcessesWS.VehicleProcessesWS
    Public vehicleTokenHeader As VehicleProcessesWS.TokenHeader = New VehicleProcessesWS.TokenHeader
    Public positionProcessesWS As PositioningWS.PositioningWS = New PositioningWS.PositioningWS
    Public positionTokenHeader As PositioningWS.TokenHeader = New PositioningWS.TokenHeader


    Public eventTokenHeader As RecordedEventProcessesWS.TokenHeader = New RecordedEventProcessesWS.TokenHeader
    Public RecordedEventProcessesWS As RecordedEventProcessesWS.RecordedEventProcessesWS = New RecordedEventProcessesWS.RecordedEventProcessesWS

    Public driverTokenHeader As DriverProcessesWS.TokenHeader = New DriverProcessesWS.TokenHeader
    Public DriverProcessesWS As DriverProcessesWS.DriverProcessesWS = New DriverProcessesWS.DriverProcessesWS

    Public TripTokenHeader As TripProcessesWS.TokenHeader = New TripProcessesWS.TokenHeader
    Public TripProcessesWS As TripProcessesWS.TripProcessesWS = New TripProcessesWS.TripProcessesWS

    Public EventNotificationTokenHeader As EventNotificationWS.TokenHeader = New EventNotificationWS.TokenHeader
    Public TripEventNotificationWS As EventNotificationWS.EventNotification = New EventNotificationWS.EventNotification


    Public vehicleList() As VehicleProcessesWS.Vehicle
    ' Public positionList() As PositioningWS.GPSPosition
    Public PositionDetails As New PositioningWS.PositioningWS
    Public PositionHeader As New PositioningWS.TokenHeader

    Public EventDescriptionProcessWS As EventDescriptionProcessWS.EventDescriptionProcessWS = New EventDescriptionProcessWS.EventDescriptionProcessWS
    Public ListsHeader As EventDescriptionProcessWS.TokenHeader = New EventDescriptionProcessWS.TokenHeader

    Public authenticationToken As String = Nothing
    Public organisationID As Integer = 4225
    Public accessed As Boolean = False
    Public lastPositionId As Integer = Nothing
    Public lastTempId As Integer = Nothing
    Public WithEvents BGworker As New BackgroundWorker
    Public WithEvents loginBackground As System.ComponentModel.BackgroundWorker
    Public stillLogginIn As Boolean = False
    Public _isloaded As Boolean = False

    Private Function login(ByVal thisUsername As String, ByVal thisPassword As String, ByVal thisAppId As String) As Boolean
        Dim loggedIn As Boolean = False
        Try
            loginResult = thisUser.Login(thisUsername, thisPassword, thisAppId)
            If loginResult.CoreLoginResult.Indicator.Equals(CoreWS.Indicators.Success) Then
                'Write auth token to file
                Using fs As FileStream = File.Create(Myfile)
                    Dim Info As Byte() = New UTF8Encoding(True).GetBytes(loginResult.Token)
                    fs.Write(Info, 0, Info.Length)
                End Using
                authenticationToken = loginResult.Token
                loginTokenHeader.Token = authenticationToken
                loginResult.CoreLoginResult.CurrentUserContext = thisUser.GetUserContext(authenticationToken)
                loggedIn = True
            End If
        Catch ex As Exception
            MessageBox.Show("Login error" & vbCrLf & ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return loggedIn
    End Function
    Public Function loginMix()
        Return Task.Factory.StartNew(Of Boolean)(
                  Function()
                      Try

                          Dim inforeader As System.IO.FileInfo
                          inforeader = My.Computer.FileSystem.GetFileInfo(Myfile)
                          Dim cookieDate As Date = inforeader.LastWriteTime
                          If DateDiff(DateInterval.Hour, cookieDate, Now.Date()) > 18 Then
                              accessed = login(userName, password, appId)
                              If accessed Then
                                  '  mainFormText("Monitoring: Logged in")
                                  If populateWSDL(authenticationToken) Then
                                      'Do While progresstxt = "Getting Vehicles"
                                      '    System.Threading.Thread.Sleep(500)
                                      'Loop
                                      progresstxt = "Mix Logging In Populating wsdl"
                                      ' mainFormText("Monitoring: WSDL APIs populated")
                                      vehicleList = vehicleProcessesWS.GetVehiclesList()
                                      If vehicleList.Length > 0 Then
                                          For I As Integer = 0 To vehicleList.Length - 1
                                              Dim mixVeh As String = Replace(vehicleList(I).RegistrationNumber, " ", Nothing)
                                              Dim veh = (From V In vehlist Where V.Registration.Equals(mixVeh) Select V).FirstOrDefault
                                              If Not veh Is Nothing Then
                                                  'Debug.Print(veh.Registration.ToString)
                                                  'If veh.Registration = "P19GBA" Then
                                                  '    MsgBox("")
                                                  'End If
                                                  ' veh.TrackedBy = "Mix Telematics"
                                                  veh.MixID = vehicleList(I).ID
                                                  veh.NewLoc = False
                                              End If
                                          Next

                                      End If
                                      'positionList = positionProcessesWS.GetGPSPositionsInDateRange(Now().Date.AddHours(-1), Now.Date())
                                      'lastPositionId = Integer.Parse(positionList.ElementAt(positionList.Length - 1).ID)
                                      progresstxt = "Mix Logged In New Cookie"
                                      ' GetBlueeTreeEvents()
                                      Return True
                                  Else
                                      ' mainFormText("Monitoring: Unable to populate WSDL APIs")
                                      System.IO.File.Delete(Myfile)
                                      progresstxt = "Mix Failed To Log In"
                                      Return False
                                      ' MessageBox.Show("Please double click the form to retry, do not click on the text area", "Restart application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                  End If
                              Else
                                  '  mainFormText("Monitoring: Unable to login")
                                  progresstxt = "Mix Failed To Log In"
                                  Return False
                              End If

                          Else
                              If System.IO.File.Exists(Myfile) Then
                                  authenticationToken = File.ReadAllText(Myfile)

                                  If Not authenticationToken Is Nothing Then
                                      If populateWSDL(authenticationToken) Then
                                          ' mainFormText("Monitoring: WSDL APIs populated")
                                          vehicleList = vehicleProcessesWS.GetVehiclesList()
                                          If vehicleList.Length > 0 Then
                                              ' positionList = positionProcessesWS.GetGPSPositionsInDateRange(Now().Date.AddHours(-1), Now.Date())
                                              For I As Integer = 0 To vehicleList.Length - 1

                                                  Dim mixVeh As String = Replace(vehicleList(I).RegistrationNumber, " ", Nothing)
                                                  Debug.Print(mixVeh & " " & vehicleList(I).GroupID)
                                                  If mixVeh.Length = 7 Then
                                                      If mixVeh.Substring(0, 3) = "GBA" Then
                                                          mixVeh = mixVeh.Replace("GBA", Nothing)
                                                      End If
                                                  End If
                                                  ' Debug.Print(mixVeh.Substring(0, 3))

                                                  Dim veh = (From V In vehlist Where V.Registration.Equals(mixVeh) Select V).FirstOrDefault
                                                  If Not veh Is Nothing Then
                                                      veh.odo = vehicleList(I).LastOdometer
                                                      veh.MixID = vehicleList(I).ID
                                                      veh.NewLoc = False
                                                      veh.TrackedBy = "Mix"
                                                      ' Debug.Print(veh.Registration & vbTab)
                                                  End If
                                              Next

                                          End If

                                          progresstxt = "Mix Logged In file exists!"
                                          ' GetBlueeTreeEvents()
                                          ' lastPositionId = Integer.Parse(positionList.ElementAt(positionList.Length - 1).ID)
                                          Return True
                                      Else
                                          ' mainFormText("Monitoring: Unable to populate WSDL APIs")
                                          System.IO.File.Delete(Myfile)
                                          Return False
                                          'MessageBox.Show("Please double click the form to retry, do not click on the text area", "Restart application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                      End If
                                      progresstxt = "Mix Logged In"
                                      'GetBlueeTreeEvents()
                                      Return True
                                  End If
                              End If
                          End If
                      Catch ex As Exception
                          'MessageBox.Show("Error starting application" & vbCrLf & ex.Message, "Error starting app", MessageBoxButtons.OK, MessageBoxIcon.Error)
                          progresstxt = "Mix Failed To Log In" & ex.Message
                          Debug.Print(progresstxt)
                          Return False
                      End Try


                  End Function)
    End Function
    Private Function GetReverseGeo(ByVal records As Array)
        Try
            Dim revGEO
            PositionDetails.TokenHeaderValue = PositionHeader
            revGEO = PositionDetails.GetReverseGeoForGPSPositions(records)
            Return revGEO
        Catch ex As Exception
            MessageBox.Show(Err.Description)
        End Try

    End Function
    Public Function GetMixTripsbyVehicle(ByVal vehid As Int16, ByVal sdate As Date, ByVal edate As Date)
        Return Task.Factory.StartNew(Of Boolean)(
                  Function()
                      Try
                          Dim vehicle

                          PositionHeader.Token = authenticationToken 'valid token
                          PositionDetails.TokenHeaderValue = PositionHeader
                          Dim vehPos As PositioningWS.GPSPosition()
                          Dim localZone As TimeZone = TimeZone.CurrentTimeZone
                          vehPos = PositionDetails.GetGPSPositionsForVehicleInDateRange(vehid, sdate, edate)
                          If vehPos.Length > 0 Then
                              Dim revGEO
                              If vehPos.Length > 1000 Then
                                  vehicle = Array.CreateInstance(GetType(Int64), 1000)
                                  For I As Integer = 0 To 999
                                      vehicle.SetValue(CInt(vehPos(I).ID), I)
                                  Next
                              Else
                                  vehicle = Array.CreateInstance(GetType(Int64), vehPos.Length)
                                  For I As Integer = 0 To vehPos.Length - 1
                                      vehicle.SetValue(CInt(vehPos(I).ID), I)
                                  Next
                              End If
                              revGEO = PositionDetails.GetReverseGeoForGPSPositions(vehicle)

                              If revGEO.Length > 0 Then
                                  For x As Integer = 0 To vehPos.Count - 1
                                      Try
                                          If x > revGEO.length - 1 Then Exit For
                                          Dim xx As Int16 = vehPos(x).VehicleID
                                          Dim m As New movement
                                          Dim compAdd As String = revGEO(x).Road & "," & revGEO(x).Town & "," & revGEO(x).Zip & "," & revGEO(x).Country
                                          If Not m.Address = compAdd Then
                                              '  Debug.Print("Comparison Add: " & compAdd & " " & "Current Add: " & m.Address)
                                              If Not revGEO(x).Road Is Nothing Then m.Address = revGEO(x).Road
                                              If Not revGEO(x).Town Is Nothing Then m.Address += "," & revGEO(x).Town
                                              If Not revGEO(x).Zip Is Nothing Then m.Address += "," & revGEO(x).Zip
                                              If Not revGEO(x).Country Is Nothing Then m.Address += "," & revGEO(x).Country
                                              '  m.Address = revGEO(x).Road & "," & revGEO(x).Town & "," & revGEO(x).Zip & "," & revGEO(x).Country
                                              'replaced 19Feb16 If DateDiff(DateInterval.Minute, m.DateofFix, DateTime.Now) < 60 Then m.NewLoc = True
                                              m.ID = x
                                              Dim currentUTC As DateTime = localZone.ToUniversalTime(vehPos(x).Time)
                                              Dim currentOffset As TimeSpan = localZone.GetUtcOffset(vehPos(x).Time)
                                              m.Latitude = vehPos(x).Latitude
                                              m.Longitude = vehPos(x).Longitude
                                              Dim truedate As DateTime = currentUTC
                                              m.DateofFix = truedate 'vehPos(x).Time
                                          End If

                                          movements.Add(m)
                                      Catch ex As Exception
                                          MessageBox.Show("error adding movement geting positions from Mix " & Err.Description, "Position Updater")
                                      End Try
                                  Next
                              End If

                          End If

                          If movements.Count > 0 Then
                                              Return True
                                          End If
                                          Return False
                                      Catch ex As Exception
                          MessageBox.Show("error geting positions from Mix " & Err.Description, "Position Updater")
                          Return False
                                      End Try
                  End Function)
    End Function


    Public Function populateWSDL(ByVal token As String) As Boolean
        Dim populated As Boolean = False
        Dim result As New CoreWS.LoginResult
        Try
            thisUser = New CoreWS.CoreWS
            loginTokenHeader.Token = token
            thisUser.TokenHeaderValue = loginTokenHeader
            thisUser.SetCurrentOrgID(organisationID)
            result.CurrentUserContext = thisUser.GetUserContext(token)
            vehicleTokenHeader.Token = token
            vehicleProcessesWS.TokenHeaderValue = vehicleTokenHeader
            positionTokenHeader.Token = token
            positionProcessesWS.TokenHeaderValue = positionTokenHeader
            eventTokenHeader.Token = token
            ListsHeader.Token = token
            driverTokenHeader.Token = token
            TripTokenHeader.Token = token
            EventNotificationTokenHeader.Token = token
            populated = True
        Catch ex As Exception
            MessageBox.Show("Unable to populate the WSDLs in use" & vbCrLf & ex.Message, "Populate WSDL APIs", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        result = Nothing
        GC.Collect()
        Return populated
    End Function

    Public Function getlatestMixPositions()

        Return Task.Factory.StartNew(Of Boolean)(
              Function()

                  Dim localZone As TimeZone = TimeZone.CurrentTimeZone
                  Try
                      Dim vehicle
                      PositionHeader.Token = authenticationToken 'valid token
                      PositionDetails.TokenHeaderValue = PositionHeader
                      Dim allPos As PositioningWS.GPSPosition()


                      Dim nulval() As Int16
                      allPos = PositionDetails.GetLatestPositionPerVehicle(nulval)

                      If allPos.Length > 0 Then
                          vehicle = Array.CreateInstance(GetType(Int64), allPos.Length)
                          For I As Integer = 0 To allPos.Length - 1
                              vehicle.SetValue(CInt(allPos(I).ID), I)
                          Next
                      End If

                      Dim revGEO = PositionDetails.GetReverseGeoForGPSPositions(vehicle)
                      If revGEO.Length > 0 Then
                          For x As Integer = 0 To allPos.Count - 1
                              Try
                                  Dim xx As Int16 = allPos(x).VehicleID
                                  Dim m As New movement
                                  Dim compAdd As String = revGEO(x).Road & "," & revGEO(x).Town & "," & revGEO(x).Zip & "," & revGEO(x).Country
                                  If Not m.Address = compAdd Then
                                      '  Debug.Print("Comparison Add: " & compAdd & " " & "Current Add: " & m.Address)
                                      If Not revGEO(x).Road Is Nothing Then m.Address = revGEO(x).Road
                                      If Not revGEO(x).Town Is Nothing Then m.Address += "," & revGEO(x).Town
                                      If Not revGEO(x).Zip Is Nothing Then m.Address += "," & revGEO(x).Zip
                                      If Not revGEO(x).Country Is Nothing Then m.Address += "," & revGEO(x).Country
                                      '  m.Address = revGEO(x).Road & "," & revGEO(x).Town & "," & revGEO(x).Zip & "," & revGEO(x).Country
                                      'replaced 19Feb16 If DateDiff(DateInterval.Minute, m.DateofFix, DateTime.Now) < 60 Then m.NewLoc = True

                                      Dim currentUTC As DateTime = localZone.ToUniversalTime(allPos(x).Time)
                                      Dim currentOffset As TimeSpan = localZone.GetUtcOffset(allPos(x).Time)

                                      Dim truedate As DateTime = currentUTC


                                      m.DateofFix = currentUTC

                                      'End If

                                  End If
                              Catch ex As Exception

                              End Try
                          Next
                      End If
                      Return True
                  Catch ex As Exception
                      MsgBox(Err.Description)
                      Return False
                  End Try

              End Function)
    End Function
End Module
