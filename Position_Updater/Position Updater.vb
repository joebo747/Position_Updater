Imports Position_Updater.FMApiNamespace
Imports Position_Updater.JobsService
Imports Position_Updater.PositonService
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports BlueTreeSystems.FleetManagerAPI.WebServiceTestClient.bluetree.fmapi
Imports SampleClient.bluetree.fmapi
Imports BlueTreeTrial.fleetmanager
Imports System.Threading.Tasks
Imports System.Xml
Imports System.IO
Imports Telerik.WinControls.UI

Public Class Position_Updater
    Private vehicle As IObservable(Of Vehicle)
    Dim startdatep As DateTime
    Dim enddatep As DateTime
    Private ReadOnly _login As String = "gba_services_v2"
    Private oTimer As System.Threading.Timer
    Private ReadOnly _password As String = "8798"
    Private Shared Positonlist As New ObservableCollection(Of UpdateLocations)
    Dim vehicleRegistrations As New List(Of String)()
    Public locations As New List(Of VehicleEvent)
    Private myBackgroundWorker As BackgroundWorker

    Private authenticationToken As String = Nothing

    Private accessed As Boolean = False
    Private lastPositionId As Integer = Nothing
    Private lastTempId As Integer = Nothing
    Public WithEvents BGworker As New BackgroundWorker
    Private WithEvents loginBackground As System.ComponentModel.BackgroundWorker
    Private stillLogginIn As Boolean = False
    Private _isloaded As Boolean = False
    Private Async Sub Position_Updater_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.RadWaitingBar1.Visible = False
            Me.RadWaitingBar1.StartWaiting()
            Me.Text = "  Position Updater Handle with care"
            Me.positonCountlbl.Text = ""
            'Dim DB As New PositonsDataContext
            'Dim Veh = (From V In DB.Vehicles Where V.bSold.Equals(0) Order By V.szRegistration Ascending Select V).ToList
            Dim vehTask As Task(Of Integer) = GetTransportVehicles()
            Me.RadWaitingBar1.Visible = True
            Dim vehresult As Integer = Await vehTask
            If vehresult > 0 Then
                Me.VehicleDropDown.DataSource = vehlist
                Me.VehicleDropDown.DataMember = "ID"
                Me.VehicleDropDown.DisplayMember = "Registration"
                Me.VehicleDropDown.ValueMember = "ID"

                Me.VehicleDropDown.SelectedIndex = -1
            End If
            myBackgroundWorker = New BackgroundWorker()
            myBackgroundWorker.WorkerReportsProgress = True
            myBackgroundWorker.WorkerSupportsCancellation = True
            AddHandler myBackgroundWorker.DoWork, AddressOf myBackgroundWorker1_DoWork
            AddHandler myBackgroundWorker.RunWorkerCompleted, AddressOf myBackgroundWorker1_RunWorkerCompleted

            Me.StartDate.Value = DateTime.Now
            Me.EndDate.Value = DateTime.Now

            Dim logintask As Task(Of Boolean) = loginMix()
            Dim loginresult As Boolean = Await logintask
            If loginresult = True Then

                Dim mixtask As Task(Of Boolean) = getlatestMixPositions()
                Dim mixResult As Boolean = Await mixtask
                If mixResult = True Then

                Else
                    Throw New System.Exception("Failed to get mix login")
                End If
                Dim mixvehicles As Task(Of Boolean) = getlatestMixPositions()
                mixResult = Await mixvehicles
                If mixResult = True Then
                    Dim img As Image = ImageList1.Images(0)
                    PopUpAlert("Mix Telematics login succesfull", "Mix Login", 5, img)
                    RadWaitingBar1.StopWaiting()
                    Me.Text = "Mix logged in succesfully!"
                    Me.RadWaitingBar1.Visible = False
                End If
            End If
        Catch ex As Exception
            MsgBox(Err.Description)
            RadWaitingBar1.StopWaiting()
            Me.Text = "An Error has ocurred!"
            Me.RadWaitingBar1.Visible = False
        End Try
    End Sub
    Public Function GetTransportVehicles()
        Return Task.Factory.StartNew(Of Integer)(
            Function()
                vehlist = New ObservableCollection(Of myVehicle)
                Try
                    Dim dB As New mydataDataContext

                    Dim veh = (From V In dB.VehicleBases Order By V.dwClassificationIdFK Select V).ToList

                    For I As Integer = 0 To veh.Count - 1
                        'If veh(I).szRegistration = "H20GBA" Then
                        '    MessageBox.Show("Required Vehicle")
                        'End If
                        Dim ve As New myVehicle
                        ve.ID = veh(I).dwVehicleId
                        Dim LastLoc = (From L In dB.GPSPositions Where L.VehID.Equals(ve.ID) Order By L.dateoffix Descending Take 1 Select L).FirstOrDefault
                        If Not LastLoc Is Nothing Then
                            ve.LatLong = LastLoc.LatLong
                            ve.DateofFix = LastLoc.dateoffix
                            ve.Address = LastLoc.Location
                            If Not LastLoc.TrackedBY Is Nothing Then ve.TrackedBy = LastLoc.TrackedBY
                        Else
                            '  Debug.Print(ve.ID & " " & veh(I).szRegistration)
                        End If
                        ve.Registration = Replace(veh(I).szRegistration, " ", "")
                        ve.vehclass = veh(I).vehClass
                        ve.Trailer = " "

                        ve.NewLoc = False

                        vehlist.Add(ve)
                    Next

                    Dim tra = (From T In dB.fridgeTrailers Select T).ToList
                    For I As Integer = 0 To tra.Count - 1
                        Dim ve As New myVehicle
                        ve.ID = tra(I).dwTrailerId
                        If Not tra(I).szTrailerCode Is Nothing Then ve.Registration = tra(I).szTrailerCode
                        ve.vehclass = "Fridge Trailer"
                        vehlist.Add(ve)
                    Next
                    Dim x = vehlist.Count
                    Return x
                Catch ex As Exception
                    MessageBox.Show("Get Transport Veh " & Err.Description & vehlist(vehlist.Count - 1).Registration, "Postiton Updater", MessageBoxButtons.OK)
                    Return 0
                End Try
            End Function)
    End Function


    Private Function GetBlueeTreeEvents()
        Return Task.Factory.StartNew(Of Boolean)(
         Function()
             Dim ListVeh As Vehicle
             Try
                 Dim test As String = GetLatestVehicleReadings(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword)
                 If InStr(test, "Error") > 0 Then Return Nothing
                 Dim reader As XmlReader = XmlReader.Create(New StringReader(test))
                 While reader.Read
                     ' .Using db As New mydataDataContext
                     If ((reader.NodeType = XmlNodeType.Element) _
                                     AndAlso (reader.Name = "row")) Then
                         'If Not reader.GetAttribute(1).Replace(" ", Nothing) Is Nothing Then
                         '    ListVeh = (From v In vehlist Where v.Registration.Equals(reader.GetAttribute(1).Replace(" ", Nothing)) Select v).FirstOrDefault

                         '    If Not ListVeh Is Nothing Then
                         '        Try
                         '            If ListVeh.Registration = "N20GBA" Then
                         '                MessageBox.Show("N20")

                         '            End If
                         '            ListVeh.TrackedBy = "BlueTree"
                         '            If ListVeh.DateofFix <> CDate(reader.GetAttribute("PositionTimeStamp")) Then
                         '                ListVeh.DateofFix = CDate(reader.GetAttribute("PositionTimeStamp"))
                         '                ListVeh.LatLong = reader.GetAttribute("Latitude") & "," & reader.GetAttribute("Longitude")
                         '                ListVeh.Address = reader.GetAttribute("LocationDescription") & "," & reader.GetAttribute("Country")
                         '                If ListVeh.Address.Length > 145 Then ListVeh.Address = ListVeh.Address.Substring(0, 145)
                         '                If Not reader.GetAttribute("Speed") = String.Empty Then ListVeh.Speed = reader.GetAttribute("Speed")
                         '                ListVeh.expose = True
                         '                ListVeh.NewLoc = True
                         '                If Not reader.GetAttribute("Odometer") = String.Empty Then ListVeh.odo = reader.GetAttribute("Odometer")
                         '            End If
                         '            If Not reader.GetAttribute("FridgeInfoTimeStamp") = String.Empty Then
                         '                If ListVeh.DateofEvent <> CDate(reader.GetAttribute("FridgeInfoTimeStamp")) Then
                         '                    ListVeh.DateofEvent = CDate(reader.GetAttribute("FridgeInfoTimeStamp"))
                         '                    Dim fridgeTemp As String = 0
                         '                    Console.WriteLine(("FridgeInfoTimeStamp = " + reader.GetAttribute(14)))
                         '                    Console.WriteLine(("FridgeOn = " + reader.GetAttribute(15)))
                         '                    For I As Int16 = 16 To 19
                         '                        If Not reader.GetAttribute(I) Is Nothing Then
                         '                            fridgeTemp = reader.GetAttribute(I) & "|"
                         '                            ListVeh.NewEvt = True
                         '                        End If
                         '                    Next
                         '                    If ListVeh.NewEvt = True Then ListVeh.Temperature = fridgeTemp.Substring(0, fridgeTemp.Length - 1)

                         '                End If
                         '            End If
                         '        Catch ex As Exception
                         '            MessageBox.Show("GetBlueeTreeEvents Loop error: " & ListVeh.Registration & " " & Err.Description)
                         '        End Try

                         '    End If
                         'End If
                     End If
                     ' End Using
                 End While
                 MessageBox.Show("GetBlueeTreeEvents Completed Sucessfully")
                 Return True
             Catch ex As Exception
                 MessageBox.Show("GetBlueeTreeEvents MAIN error: " & Err.Description)
             End Try
             Return True
         End Function)
    End Function
    Public Function GetLatestVehicleReadings(ByVal usn As String, ByVal pwd As String) As String
        Dim fmapi As FMApiWrapper = New FMApiWrapper(usn, pwd, True, Nothing)
        ' Console.WriteLine(("Connecting to: " + fmapi.FleetManagerAPI.Url))
        Dim strError As String
        Dim strInfo As String
        Dim columns As List(Of String) = New List(Of String)
        columns.Add("VehicleID")
        columns.Add("VehicleName")
        'columns.Add("VehicleTypeID")
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
        columns.Add("AlertOn")
        columns.Add("PTOStatus")
        columns.Add("IdleStatus")
        'columns.Add("VehicleWeight")
        strInfo = fmapi.GetLatestVehicleReadings(columns, strError)
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

    End Function
    Private Async Sub GetPositons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetPositons.Click
        Try
            startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
            enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
            RadWaitingBar1.StartWaiting()
            RadWaitingBar1.Text = "Attempting to get Records"
            RadWaitingBar1.Visible = True
            If enddatep < startdatep Then
                MsgBox("The end date mus be after the start date", MsgBoxStyle.Critical)
                Exit Sub

            Else
                Dim reg As String = Me.VehicleDropDown.Text
                Dim veh = (From v In vehlist Where v.Registration = reg Select v).FirstOrDefault
                If veh Is Nothing Then
                    MessageBox.Show("Error finding vehicle", Application.ProductName, MessageBoxButtons.OK)
                    Exit Sub
                End If
                If Trackinglbl.Text.IndexOf("Mix") > 0 Then
                    Dim positionM As Task(Of Boolean) = GetMixTripsbyVehicle(veh.MixID, startdatep, enddatep)
                    Dim positionresult As Boolean = Await positionM
                    If positionresult = True Then
                        RadGridView1.DataSource = movements
                        RadGridView1.BestFitColumns()
                        Dim img As Image = ImageList1.Images(0)
                        PopUpAlert(movements.Count & " Records retuned!", "Mix Login", 5, img)
                        RadWaitingBar1.StopWaiting()
                        RadWaitingBar1.Visible = False
                        RadWaitingBar1.Text = String.Empty
                        Exit Sub
                    End If
                ElseIf Trackinglbl.Text.IndexOf("Blue") > 0 Then

                Else
                    If Not myBackgroundWorker.IsBusy Then
                        Me.RadWaitingBar1.StartWaiting()
                        Me.positonCountlbl.Text = 1
                        Timer1.Interval = 1000
                        Timer1.Start()
                        myBackgroundWorker.RunWorkerAsync(Me.positonCountlbl.Text)
                        Me.RadWaitingBar1.Visible = True
                        Me.RadWaitingBar1.Text = "Loading positions please wait!"
                        Me.RadWaitingBar1.StartWaiting()
                        BackgroundWorker1.WorkerReportsProgress = True
                        BackgroundWorker1.RunWorkerAsync()
                        Application.DoEvents()
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ON_timeEvent()
        Try

        Catch ex As Exception

        End Try

    End Sub
    Private Sub getpositions()
        Try
            Dim positions As New List(Of Position)




            Dim start As DateTime = Format(startdatep, "dd/MM/yyyy HH:mm")
            Dim [end] As DateTime = Format(enddatep, "dd/MM/yyyy HH:mm")

            Dim Units As DistanceUnit
            Units = DistanceUnit.Mile

            Dim eventsToReturn As New List(Of EventType)()
            Dim eventTypeLocation As New EventType()
            eventTypeLocation.Code = EventCode.VehicleLocation
            eventsToReturn.Add(eventTypeLocation)
            Dim eventTypeignition = New EventType
            eventTypeignition.Code = EventCode.Ignition
            eventsToReturn.Add(eventTypeignition)


            Dim reg As String = Me.VehicleDropDown.Text
            vehicleRegistrations.Add(reg)
            Dim webService As New jobServiceSoapClient

            Dim vehicleHistorys As VehicleHistoryV2() = webService.GetHistoryV2(_login, _password, vehicleRegistrations.ToArray(), VehicleCollectionType.Vehicles, start, [end],
                             eventsToReturn.ToArray(), True)

            Dim EV As Int16 = 0
            locations = New List(Of VehicleEvent)
            For Each vehicleHistory As VehicleHistoryV2 In vehicleHistorys
                Dim moveRec
                If vehicleHistory.Events.Count > 0 Then
                    For Each vehicleEvent As VehicleEvent In vehicleHistory.Events
                        locations.Add(vehicleEvent)
                    Next



                    'Exit Sub
                End If

            Next

        Catch ex As Exception
            MsgBox(Err.Description)
        End Try
    End Sub
    Private Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton1.Click
        Try
            Dim password As String = "systemsOnly"

            ' Dim input = InputBox("Password", "Update Password")
            If Not passtxt.Text = password Then
                MsgBox("Only authorised users are allowed to update positions!")
                Exit Sub
            End If
            Me.RadButton1.Enabled = False
            Dim positions As New List(Of Position)
            Dim EV As Integer
            For I As Integer = 0 To Me.RadGridView1.Rows.Count - 1
                Dim position As Position = New Position()
                position = New Position()
                position.CID = 701
                ' Dim lastevent As Integer = VehicleHistory.Events.Count - 1
                ' Dim lastEventTime As DateTime = VehicleHistory.Events(lastevent).PositionDateTime

                Dim Pdate As Date = RadGridView1.Rows(I).Cells("PositionDateTime").Value
                Dim tlag As Int16 = DateDiff(DateInterval.Minute, Pdate, DateTime.Now)
                Dim LatLng As String = RadGridView1.Rows(I).Cells("Latitude").Value & "," & RadGridView1.Rows(I).Cells("Longitude").Value
                position.Lat = RadGridView1.Rows(I).Cells("Latitude").Value * 36000
                position.Lon = RadGridView1.Rows(I).Cells("Longitude").Value * 36000
                If Me.vehLock_chk.Checked = False Then
                    position.ModemID = Me.VehicleDropDown.SelectedItem.Value
                Else
                    position.ModemID = Me.vehicle_lbl.Tag
                End If

                Dim u As DateTime = Pdate.ToUniversalTime

                position.Time = DateTime.Parse(u).ToString("yyyy-MM-ddTHH:mm:ss").ToString
                ' RadGridView1.Rows.Add(DateTime.Now, "Position Added", item.Registration, position.ModemID & " " & item.Registration, position.Lat & "," & position.Lon)
                'Filelog = Filelog & vbCrLf & position.CID & "," & position.ModemID & "," & position.Lat & "," & position.Lon & "," & position.Time & vbCrLf
                'If DateDiff(DateInterval.Minute, Pdate, DateTime.Now) < 35 Then
                positions.Add(position)
                EV += 1
                'moveRec = New DHLVehicleUpdate With {.Registration = item.Registration, _
                '                                 .LastSentdate = Pdate, _
                '                                 .DwVehicleidfk = item.ID,
                '                                 .accID = item.Company,
                '                                 .latlong = LatLng,
                '                                 .timelag = tlag,
                '                                 .accepted = 1}
                'DB.DHLVehicleUpdates.InsertOnSubmit(moveRec)
            Next
            Dim AddPos As Boolean = True
            Dim G As Integer = 0
            Dim b As Integer = 0
            If EV > 0 Then
                Dim o_result() As Result
                Dim CallWebService As New PositionServiceSoapClient()

                o_result = CallWebService.AddPosition("GBA", "positions", "L]V)M_''={w2#^r", positions.ToArray)

                For I = 0 To UBound(o_result)
                    If o_result(I).ResultCode = 0 Then
                        G += 1
                    ElseIf o_result(I).ResultCode = 1 Then
                        b += 1
                        AddPos = False
                    End If
                Next

            End If
            If AddPos = False Then
                MsgBox("Results out of date! Not added = " & b & vbCrLf & "Positions added " & G)
            Else
                MsgBox("All results were added  " & vbCrLf & "Positions added " & G)
            End If

        Catch ex As Exception
        Finally
            Me.passtxt.Visible = False
            Me.Passlbl.Visible = False
        End Try
    End Sub

    'Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
    '    Try

    '        getpositions()
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub myBackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Try
            Try
                getpositions()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub
    Private Sub myBackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Try

            'Me.RadWaitingBar1.Visible = False

            If locations.Count > 0 Then
                Timer1.Stop()
                Me.positonCountlbl.Text = locations.Count & " Records"
                Me.RadGridView1.DataSource = locations
                Me.RadGridView1.Columns(0).IsVisible = False
                Me.RadGridView1.Columns("Latitude").IsVisible = False
                Me.RadGridView1.Columns("Longitude").IsVisible = False
                Me.RadGridView1.Columns("Type").IsVisible = False
                Me.RadGridView1.Columns("AdditionalEventInfo").IsVisible = False
                Me.passtxt.Visible = True
                Me.Passlbl.Visible = True
                For I As Int16 = 0 To Me.RadGridView1.Columns.Count - 1
                    Me.RadGridView1.Columns(I).BestFit()
                Next
                Me.RadButton1.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(Err.Description)
        Finally
            Me.RadWaitingBar1.StopWaiting()
            Me.RadWaitingBar1.Visible = False
        End Try
    End Sub
    'Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
    '    Try
    '        Me.RadWaitingBar1.StopWaiting()
    '        'Me.RadWaitingBar1.Visible = False

    '        If locations.Count > 0 Then
    '            Me.positonCountlbl.Text = locations.Count & " Records"
    '            Me.RadGridView1.DataSource = locations
    '            Me.RadGridView1.Columns(0).IsVisible = False
    '            Me.RadGridView1.Columns("Latitude").IsVisible = False
    '            Me.RadGridView1.Columns("Longitude").IsVisible = False
    '            Me.RadGridView1.Columns("Type").IsVisible = False
    '            Me.RadGridView1.Columns("AdditionalEventInfo").IsVisible = False
    '            For I As Int16 = 0 To Me.RadGridView1.Columns.Count - 1
    '                Me.RadGridView1.Columns(I).BestFit()
    '            Next
    '            Me.RadButton1.Enabled = True
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub RadDropDownList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs) Handles VehicleDropDown.SelectedIndexChanged
        Try

            If vehLock_chk.Checked = False Then
                vehicle_lbl.Text = VehicleDropDown.SelectedText
                vehicle_lbl.Tag = VehicleDropDown.SelectedValue
            End If

            Dim veh = (From v In vehlist Where v.Registration.Equals(VehicleDropDown.SelectedText) Select v).FirstOrDefault
            If Not veh Is Nothing Then
                Trackinglbl.Text = "Tracked by " & veh.TrackedBy

            End If
            If locations.Count > 0 Then
                locations.Clear()
            End If
            If vehLock_chk.Checked = False Then
                vehicle_lbl.Text = VehicleDropDown.SelectedText
                vehicle_lbl.Tag = VehicleDropDown.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Dim I As Integer = CInt(Me.positonCountlbl.Text)
            I += 1
            Me.positonCountlbl.Text = I
        Catch ex As Exception

        End Try
    End Sub

    Private Sub vehLock_chk_ToggleStateChanged(ByVal sender As System.Object, ByVal args As Telerik.WinControls.UI.StateChangedEventArgs) Handles vehLock_chk.ToggleStateChanged
        If Me.vehLock_chk.Checked = True Then
            Me.vehicle_lbl.Visible = True
            Me.selectedVehicle.Visible = True
            Me.RadLabel6.Visible = True
        Else
            Me.vehicle_lbl.Visible = False
            Me.selectedVehicle.Visible = False
            Me.RadLabel6.Visible = False
        End If
    End Sub
    Private font As New Font("Consolas", 14, FontStyle.Bold)
    Private Sub VehicleDropDown_VisualListItemFormatting(sender As Object, args As VisualItemFormattingEventArgs) Handles VehicleDropDown.VisualListItemFormatting
        'If _isloaded = False Then Exit Sub
        If args.VisualItem.Selected Then
            args.VisualItem.NumberOfColors = 1
            args.VisualItem.BackColor = Color.LightGreen
            args.VisualItem.ForeColor = Color.Red
            args.VisualItem.BorderColor = Color.Blue
            args.VisualItem.Font = font
        Else
            args.VisualItem.ResetValue(LightVisualElement.NumberOfColorsProperty, Telerik.WinControls.ValueResetFlags.Local)
            args.VisualItem.ResetValue(LightVisualElement.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local)
            args.VisualItem.ResetValue(LightVisualElement.ForeColorProperty, Telerik.WinControls.ValueResetFlags.Local)
            args.VisualItem.ResetValue(LightVisualElement.BorderColorProperty, Telerik.WinControls.ValueResetFlags.Local)
            args.VisualItem.ResetValue(LightVisualElement.FontProperty, Telerik.WinControls.ValueResetFlags.Local)
        End If
    End Sub
End Class
