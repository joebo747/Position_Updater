Imports System.Threading.Tasks
Imports Position_Updater.FMApiNamespace
Imports System.Xml
Imports System.IO
Imports System.Net

Module BlueTreeModule
    Public stringReturn As String
    Public dsGPS As DataSet
    Public BlueTreeComplete As Boolean = False
    Public Function GetBlueeTreeEvents()
        Return Task.Factory.StartNew(Of Boolean)(
         Function()
             Dim ListVeh As myVehicle
             Try

                 Dim test As String = GetLatestVehicleReadings(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword)
                 stringReturn = test
                 If InStr(test, "Error") > 0 Then
                     MessageBox.Show("Error getting Blue Tree records " & test)
                     progresstxt = "Error getting Blue Tree!"
                 End If
                 Dim reader As XmlReader = XmlReader.Create(New StringReader(test))
                 While reader.Read
                     Using db As New mydataDataContext
                         If ((reader.NodeType = XmlNodeType.Element) _
                                     AndAlso (reader.Name = "row")) Then
                             If Not reader.GetAttribute(1).Replace(" ", Nothing) Is Nothing Then
                                 ListVeh = (From v In vehlist Where v.Registration.Equals(reader.GetAttribute(1).Replace(" ", Nothing)) Select v).FirstOrDefault

                                 If Not ListVeh Is Nothing Then
                                     Try
                                         'If ListVeh.Registration = "N20GBA" Then
                                         '    MessageBox.Show("N20")

                                         'End If
                                         ListVeh.TrackedBy = "BlueTree"
                                         Dim vid As Integer = CInt(reader.GetAttribute("VehicleID"))
                                         If ListVeh.DateofFix <> CDate(reader.GetAttribute("PositionTimeStamp")) Then
                                             ListVeh.DateofFix = CDate(reader.GetAttribute("PositionTimeStamp"))
                                             ListVeh.LatLong = reader.GetAttribute("Latitude") & "," & reader.GetAttribute("Longitude")
                                             ListVeh.Address = reader.GetAttribute("LocationDescription") & "," & reader.GetAttribute("Country")
                                             If ListVeh.Address.Length > 145 Then ListVeh.Address = ListVeh.Address.Substring(0, 145)
                                             If Not reader.GetAttribute("Speed") = String.Empty Then ListVeh.Speed = reader.GetAttribute("Speed")
                                             ListVeh.expose = True
                                             ListVeh.NewLoc = True
                                             If Not reader.GetAttribute("Odometer") = String.Empty Then ListVeh.odo = reader.GetAttribute("Odometer")
                                             ListVeh.BluetreeID = vid
                                         End If
                                         If Not reader.GetAttribute("FridgeInfoTimeStamp") = String.Empty Then
                                             If ListVeh.DateofEvent <> CDate(reader.GetAttribute("FridgeInfoTimeStamp")) Then
                                                 ListVeh.DateofEvent = CDate(reader.GetAttribute("FridgeInfoTimeStamp"))
                                                 Dim fridgeTemp As String = 0
                                                 Console.WriteLine(("FridgeInfoTimeStamp = " + reader.GetAttribute(14)))
                                                 Console.WriteLine(("FridgeOn = " + reader.GetAttribute(15)))
                                                 For I As Int16 = 16 To 19
                                                     If Not reader.GetAttribute(I) Is Nothing Then
                                                         fridgeTemp = reader.GetAttribute(I) & "|"
                                                         ListVeh.NewEvt = True
                                                     End If
                                                 Next
                                                 If ListVeh.NewEvt = True Then ListVeh.Temperature = fridgeTemp.Substring(0, fridgeTemp.Length - 1)

                                             End If
                                         End If
                                     Catch ex As Exception
                                         MessageBox.Show("GetBlueeTreeEvents Loop error: " & ListVeh.Registration & " " & Err.Description)
                                     End Try

                                 End If
                             End If
                         End If
                     End Using
                 End While
                 progresstxt = "Bluetree Vehicles Updated"
                 Return True
             Catch ex As Exception
                 MessageBox.Show("GetBlueeTreeEvents MAIN error: " & Err.Description)
             End Try
             Return True
         End Function)
    End Function
    Public Function GetGPSForVehicle(ByVal usn As String, ByVal pwd As String, ByVal VehicleId As Integer, ByVal sdate As DateTime, ByVal edate As DateTime)
        Return Task.Factory.StartNew(Of String)(
         Function()
             Dim strError As String
             Dim strinfo As String
             Try
                 Dim AdjRow As Integer
                 'Dim fmapi As FMApiWrapper = New FMApiWrapper(usn, pwd, True, Nothing)
                 'Console.WriteLine(("Connecting to: " + fmapi.FleetManagerAPI.Url))
                 Dim columns As List(Of String) = New List(Of String)
                 'columns.Add("VehicleName")
                 columns.Add("LocationDescription")
                 columns.Add("Country")
                 'columns.Add("Speed")
                 'columns.Add("VehicleDirection")
                 'columns.Add("DirectionDescription")

                 columns.Add("RecordTimeStamp")
                 ' columns.Add("StoppedDuration")
                 '  columns.Add("RCOMTrackerStatus")
                 columns.Add("Latitude")
                 columns.Add("Longitude")
                 '   strinfo = fmapi.GetGpsReadingsForVehicle_v1(usn, pwd, 1, columns, sdate, "", VehicleId, edate, strError, AdjRow)
                 Dim webService As New fleetmanager.FleetManagerAPI
                 strinfo = webService.GetGpsReadingsForVehicle_v1(usn, pwd, 1, columns.ToArray, sdate, "", VehicleId, edate, strError, AdjRow)
                 'Debug.Print(strinfo)
                 If (strError.Length > 0) Then
                     Return ("Error has happened:" & vbLf + strError)
                     ' Console.WriteLine("Error has happened:\n" + strError);
                 Else
                     Return strinfo
                 End If

             Catch ex As Exception
                 MessageBox.Show(strError)
             End Try
         End Function)
    End Function


    Public Function GetLatestVehicleReadings(ByVal usn As String, ByVal pwd As String) As String
        'Return Task.Factory.StartNew(Of Boolean)(
        ' Function()


        '  Dim fmapi As FMApiWrapper = New FMApiWrapper(usn, pwd, True, Nothing)

        ' Console.WriteLine(("Connecting to: " + fmapi.FleetManagerAPI.Url))
        Dim strError As String
        Dim strInfo As String
        Dim columns As List(Of String) = New List(Of String)
        columns.Add("VehicleID")
        columns.Add("VehicleName")
        columns.Add("VehicleTypeID")
        columns.Add("VehicleDescription")
        'columns.Add("FolderID")
        ' columns.Add("FolderName")
        ' columns.Add("DriverID")
        columns.Add("DriverName")
        ' columns.Add("DriverFolderID")
        ' columns.Add("DriverFolderName")
        'columns.Add("LastCommTime")
        columns.Add("PositionTimeStamp")
        ' columns.Add("LastMovementTime")
        columns.Add("RecordTimeStamp")
        ' columns.Add("StoppedDuration")
        columns.Add("RCOMTrackerStatus")
        columns.Add("Latitude")
        columns.Add("Longitude")
        columns.Add("LocationDescription")
        columns.Add("Country")
        columns.Add("Speed")
        ' columns.Add("VehicleDirection")
        'columns.Add("DirectionDescription")
        columns.Add("TrackerBatteryVoltageTime")
        columns.Add("TrackerBatteryVoltage")
        ' columns.Add("LocationID")
        ' columns.Add("IsNamedLocation")

        'columns.Add("LocationUserData")
        columns.Add("FridgeInfoTimeStamp")
        columns.Add("FridgeOn")
        'columns.Add("TrackerDoorSwitch1Open")
        ' columns.Add("TrackerDoorSwitch2Open")

        'columns.Add("Zone1Setpoint")
        'columns.Add("Zone1Return")
        'columns.Add("Zone1Discharge")
        'columns.Add("Zone2Setpoint")
        'columns.Add("Zone2Return")
        'columns.Add("Zone2Discharge")
        'columns.Add("Zone3Setpoint")
        'columns.Add("Zone3Return")
        'columns.Add("Zone3Discharge")
        columns.Add("Sensor1")
        columns.Add("Sensor2")
        columns.Add("Sensor3")
        columns.Add("Sensor4")
        'columns.Add("Sensor5")
        'columns.Add("Sensor6")
        columns.Add("ActiveAlarms")
        columns.Add("FridgeEngineModeOn")
        columns.Add("FridgeCycleSentryOn")
        'columns.Add("Zone1On")
        'columns.Add("Zone2On")
        'columns.Add("Zone3On")
        'columns.Add("DoorSwitch1Open")
        'columns.Add("DoorSwitch2Open")
        'columns.Add("DoorSwitch3Open")
        'columns.Add("FridgeFuelLevel")
        columns.Add("FridgeBatteryVoltage")
        'columns.Add("TotalHourmeter")
        'columns.Add("DieselHourmeter")
        'columns.Add("ElectricHourmeter")
        'columns.Add("HourmeterTimeStamp")
        columns.Add("IgnitionOn")
        columns.Add("IgnitionReadingTime")
        columns.Add("Odometer")
        columns.Add("FuelLevel")
        columns.Add("FuelReadingTime")
        columns.Add("CardPresent")
        columns.Add("DriversStartOfDay")
        columns.Add("LastValidBreakEndTime")
        columns.Add("DrivingDurationMinutes")
        columns.Add("DrivingTimeRemaining")
        columns.Add("WorkingStateTypeID")
        columns.Add("WorkingStateDescription")
        'columns.Add("FMSFuelBurnt")
        'columns.Add("AlertOn")
        'columns.Add("PTOStatus")
        'columns.Add("IdleStatus")
        'columns.Add("VehicleWeight")
        'strInfo = fmapi.GetLatestVehicleReadings(columns, strError)
        If ((columns Is Nothing) _
                        OrElse (columns.Count = 0)) Then
            Throw New ArgumentException("invalid input parameter")
        End If
        Dim webService As New fleetmanager.FleetManagerAPI
        strInfo = webService.GetLatestVehicleReadings_v1(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword, columns.ToArray, "", strError)
        If (strError.Length > 0) Then
            Return ("Error has happened:" & vbLf + strError)
            ' Console.WriteLine("Error has happened:\n" + strError);
        Else
            'Dim doc As XmlDocument = New XmlDocument
            'doc.LoadXml(strInfo)
            'doc.Save("CurrentInfo.xml")
            'doc = Nothing

            Return strInfo
            ' Console.WriteLine(strInfo);
        End If
        ' End Function)
    End Function
End Module
