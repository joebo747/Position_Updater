Imports Position_Updater.jobservice
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Xml
Imports System.IO
Imports Telerik.WinControls.UI
Imports Position_Updater.Fleetfence
Imports Newtonsoft.Json
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net

Public Class Position_Updater
    Private path As String = "\\gba.local\gba_share\GBA_Sharedwork\DFS_Applications\DMUsers\RequestFile"
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
    Public vehlistx As New ObservableCollection(Of myVehicle)
    Private authenticationToken As String = Nothing
    Public Request As New ObservableCollection(Of RequestVehicle)
    Private accessed As Boolean = False
    Private lastPositionId As Integer = Nothing
    Private lastTempId As Integer = Nothing
    Public WithEvents BGworker As New BackgroundWorker
    Private WithEvents loginBackground As System.ComponentModel.BackgroundWorker
    Private stillLogginIn As Boolean = False
    Private _isloaded As Boolean = False
    Private sendNumber As Int16 = 0
    Private notSent As Int16 = 0
    Private html As String
    Private DocumentStarted As Boolean
    Private tooltiptxt As String = "Sends the positions to the service"
    Private tbSubscribed As Boolean = False
    Private lastRec
    Private lastDate As DateTime
    Public recordsetComplete As Boolean = False
    Private Async Sub Position_Updater_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.KeyPreview = True
            'If vehlist.Count = 0 Then
            ShapedForm1.Close()
            '    Me.Visible = False
            'End If
            'RadMenu1.Visible = False
            'Me.Height -= 28
            Me.RadWaitingBar1.Visible = True
            Me.RadWaitingBar1.StartWaiting()
            Me.Text = "  Position Updater Handle with care"
            Me.positonCountlbl.Text = ""
            'Dim DB As New PositonsDataContext
            'Dim Veh = (From V In DB.Vehicles Where V.bSold.Equals(0) Order By V.szRegistration Ascending Select V).ToList
            'Dim vehTask As Task(Of Integer) = GetTransportVehicles()
            'Me.RadWaitingBar1.Visible = True
            'Dim vehresult As Integer = Await vehTask
            'If vehresult > 0 Then
            vehlistx = New ObservableCollection(Of myVehicle)
            If vehlist.Count > 0 Then
                'For Each V As myVehicle In vehlist
                '    vehlistx.Add(V)
                'Next
            End If
            Me.VehicleDropDown.DataSource = vehlist
            Me.VehicleDropDown.DataMember = "ID"
            Me.VehicleDropDown.DisplayMember = "Registration"
            Me.VehicleDropDown.ValueMember = "ID"
            Me.VehicleDropDown.Refresh()
            Me.VehicleDropDown.SelectedIndex = -1
            ' End If
            myBackgroundWorker = New BackgroundWorker()
            myBackgroundWorker.WorkerReportsProgress = True
            myBackgroundWorker.WorkerSupportsCancellation = True
            AddHandler myBackgroundWorker.DoWork, AddressOf myBackgroundWorker1_DoWork
            AddHandler myBackgroundWorker.RunWorkerCompleted, AddressOf myBackgroundWorker1_RunWorkerCompleted

            Me.StartDate.Value = DateTime.Now.AddHours(-10)
            Me.EndDate.Value = DateTime.Now

            'Dim logintask As Task(Of Boolean) = loginMix()
            'Dim loginresult As Boolean = Await logintask
            'If loginresult = True Then

            '    Dim mixtask As Task(Of Boolean) = getlatestMixPositions()
            '    Dim mixResult As Boolean = Await mixtask
            '    If mixResult = True Then

            '    Else
            '        Throw New System.Exception("Failed to get mix login")
            '    End If
            '    Dim mixvehicles As Task(Of Boolean) = getlatestMixPositions()
            '    mixResult = Await mixvehicles
            '    If mixResult = True Then
            Dim img As Image = ImageList1.Images(0)
            PopUpAlert("Mix Telematics login succesfull", "Mix Login", 5, img)
            RadWaitingBar1.StopWaiting()
            Me.Text = "Mix logged in succesfully!"
            Me.RadWaitingBar1.Visible = False
            ' End If
            'End If
            'Dim BlueTask As Task(Of Boolean) = GetBlueeTreeEvents() '(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword)
            'Dim BlueResult As Boolean = Await BlueTask
            'If BlueResult = True Then

            'End If
            If Environment.UserName = "joe.bohen" Then
                RadMenu1.Visible = True
                Me.Height += 30
            End If
        Catch ex As Exception
            MsgBox(Err.Description)
            RadWaitingBar1.StopWaiting()
            Me.Text = "An Error has ocurred!"
            Me.RadWaitingBar1.Visible = False
        Finally
            _isloaded = True
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
                        Dim ve As New myVehicle
                        ve.ID = veh(I).dwVehicleId
                        If veh(I).szRegistration = "J17GBA" Then
                            Debug.Print("J17")
                        End If
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



    Private Async Sub GetPositons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetPositons.Click
        Try
            Dim recordfound As Boolean = False
            recordsetComplete = False
            If vehlist.Count <> 0 Then

            End If
            Dim hrs As Int16 = DateDiff(DateInterval.Hour, startdatep, enddatep)
            startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
            enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
            If DateDiff(DateInterval.Hour, startdatep, DateTime.Now) > 24 Or hrs > 24 Then
                MessageBox.Show("The service will not accept positions older than 24 hrs old! Please adjust the selected time band.", "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            'Exit Sub
            RadWaitingBar1.StartWaiting()
            RadWaitingBar1.Text = "Attempting to get Records"
            RadWaitingBar1.Visible = True
            If enddatep < startdatep Then
                MsgBox("The end date mus be after the start date", MsgBoxStyle.Critical)
                RadWaitingBar1.StopWaiting()
                RadWaitingBar1.Visible = False
                Exit Sub
            Else
                Dim reg As String = VehicleDropDown.Text
                Dim veh = (From v In vehlist Where v.Registration = reg Select v).FirstOrDefault
                If veh Is Nothing Then
                    MessageBox.Show("Error finding vehicle", Application.ProductName, MessageBoxButtons.OK)
                    RadWaitingBar1.StopWaiting()
                    RadWaitingBar1.Visible = False
                    Exit Sub
                End If
                If Trackinglbl.Text.IndexOf("Mix") > 0 Then
                    movements = New List(Of movement)
                    Dim positionM As Task(Of Boolean) = GetMixTripsbyVehicle(veh.MixID, startdatep, enddatep, veh.ID)
                    Dim positionresult As Boolean = Await positionM
                    If positionresult = True Then
                        If MixRecCount = 1000 Then
                            While MixRecCount = 1000
                                lastRec = (From M In movements Where M.ID.Equals(veh.ID) Order By M.DateofFix Descending Select M).FirstOrDefault
                                lastDate = CDate(lastRec.DateofFix)
                                positionM = GetMixTripsbyVehicle(veh.MixID, lastDate, enddatep, veh.ID)
                                positionresult = Await positionM
                            End While
                            'Dim d As Date = movements(999).DateofFix
                            'If DateDiff("n", d, enddatep) > 50 Then
                            '    positionM = GetMixTripsbyVehicle(veh.MixID, d, enddatep)
                            '    positionresult = Await positionM
                            '    If positionresult = True Then

                            '    End If
                            'End If
                        End If
                        RadGridView1.DataSource = movements
                        RadGridView1.TableElement.VScrollBar.PerformLast()
                        RadGridView1.BestFitColumns()
                        Me.RadGridView1.Refresh()
                        Dim img As Image = ImageList1.Images(0)
                        PopUpAlert(movements.Count & " Records retuned!", "Mix Positions", 5, img)
                        RadWaitingBar1.StopWaiting()
                        RadWaitingBar1.Visible = False
                        RadWaitingBar1.Text = String.Empty
                        Exit Sub
                    Else
                        Dim img As Image = ImageList1.Images(0)
                        PopUpAlert("No Records retuned for the selected period!", "Mix Positions", 5, img)
                        RadWaitingBar1.StopWaiting()
                        RadWaitingBar1.Visible = False
                        RadWaitingBar1.Text = String.Empty
                        Exit Sub
                    End If
                ElseIf Trackinglbl.Text.IndexOf("Blue") > 0 Then
                    Dim BT As Int16 = VehicleDropDown.SelectedValue
                    Dim vehId = (From v In vehlist Where v.ID.Equals(BT) Select v.BluetreeID).FirstOrDefault
                    If vehId > 0 Then
                        'dsGPS = New DataSet()
                        Dim GPSTask As Task(Of String) = GetGPSForVehicle(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword, vehId, startdatep, enddatep)
                        Dim GPSResult As String = Await GPSTask
                        movements.Clear()
                        If GPSResult.Length > 50 Then
                            Dim o As Int16 = 0
                            Dim reader As XmlReader = XmlReader.Create(New StringReader(GPSResult))
                            While reader.Read
                                If ((reader.NodeType = XmlNodeType.Element) _
                                     AndAlso (reader.Name = "row")) Then
                                    Dim m As New movement
                                    m.ID = BT
                                    m.DateofFix = reader.GetAttribute("RecordTimeStamp")
                                    m.Latitude = reader.GetAttribute("Latitude")
                                    m.Longitude = reader.GetAttribute("Longitude")
                                    m.Address = reader.GetAttribute("LocationDescription") & reader.GetAttribute("Country")
                                    m.Registration = VehicleDropDown.Text
                                    movements.Add(m)
                                    o += 1
                                End If
                            End While

                            lastRec = (From M In movements Where M.ID.Equals(BT) Order By M.DateofFix Select M).LastOrDefault
                            lastDate = CDate(lastRec.DateofFix)
                            While recordsetComplete = False
                                Dim newTask As Task(Of Boolean) = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", vehId, o)
                                Dim newResult As Boolean = Await newTask
                                If newResult = True Then
                                    lastRec = (From M In movements Where M.ID.Equals(vehId) Order By M.DateofFix Select M).LastOrDefault
                                    lastDate = CDate(lastRec.DateofFix)
                                    newTask = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", vehId, o)
                                    newResult = Await newTask
                                End If
                            End While
                        End If
                        Me.RadGridView1.DataSource = movements
                        RadGridView1.TableElement.VScrollBar.PerformLast()
                        RadGridView1.BestFitColumns()
                        Me.RadGridView1.Refresh()
                        Dim img As Image = ImageList1.Images(0)
                        PopUpAlert(movements.Count & " Records retuned!", "Blue Tree", 5, img)
                        RadWaitingBar1.StopWaiting()
                        RadWaitingBar1.Visible = False
                        RadWaitingBar1.Text = String.Empty
                        Exit Sub
                    Else
                        MessageBox.Show("The selected vehicle is not listed as a Blue Tree Vehicle!")
                        RadWaitingBar1.StopWaiting()
                        RadWaitingBar1.Visible = False
                        RadWaitingBar1.Text = String.Empty
                        Exit Sub
                    End If
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
            MessageBox.Show("Error Getting GPS Positions " & Err.Description)
        Finally
            If Err.Number = 0 And RadGridView1.Rows.Count > 0 Then
                Me.passtxt.Visible = True
                Me.Passlbl.Visible = True
                RadButton1.Enabled = True
            End If
        End Try
    End Sub
    Private Async Function GPStestforLast(ByVal lastenddate As DateTime, enddatep As DateTime, ByVal caller As String, ByVal vehid As Int16, ByVal o As Int16) As Task(Of Boolean)
        Try
            Debug.Print(DateDiff(DateInterval.Minute, enddatep, lastDate))
            If DateDiff(DateInterval.Minute, enddatep, lastDate) > 15 Then
                Me.RadGridView1.DataSource = Nothing
                If caller = "BlueTree" Then
                    Dim GPSTask As Task(Of String) = GetGPSForVehicle(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword, vehid, lastenddate, enddatep)
                    Dim GPSResult As String = Await GPSTask
                    If GPSResult.Length > 50 Then
                        Dim reader As XmlReader = XmlReader.Create(New StringReader(GPSResult))
                        While reader.Read
                            If ((reader.NodeType = XmlNodeType.Element) _
                                 AndAlso (reader.Name = "row")) Then
                                Dim m As New movement
                                o += 1
                                m.ID = vehid
                                '     Debug.Print(reader.GetAttribute("RecordTimeStamp"))
                                m.DateofFix = reader.GetAttribute("RecordTimeStamp")
                                m.Latitude = reader.GetAttribute("Latitude") '& "," & reader.GetAttribute("Longitude")
                                m.Longitude = reader.GetAttribute("Longitude")
                                m.Address = reader.GetAttribute("LocationDescription") & "," & reader.GetAttribute("Country")
                                If m.DateofFix >= enddatep Then
                                    recordsetComplete = True
                                    Exit Try
                                End If
                                movements.Add(m)
                            End If
                        End While
                    Else
                        recordsetComplete = True
                    End If
                End If
            Else
                recordsetComplete = True
            End If
            lastRec = (From M In movements Where M.ID.Equals(vehid) Order By M.DateofFix Select M).LastOrDefault
            lastDate = CDate(lastRec.DateofFix)
            'Me.RadGridView1.DataSource = movements
            'Me.RadGridView1.Refresh()

        Catch ex As Exception

        End Try
    End Function

    Private Sub ON_timeEvent()
        Try

        Catch ex As Exception

        End Try

    End Sub
    Private Function SendToFleetFeence()
        Return Task.Factory.StartNew(Of Boolean)(
            Function()
                Try
                    Dim lastSent As DateTime
                    sendNumber = 0
                    notSent = 0
                    Dim positions As New List(Of Fleetfence.Position)()
                    Dim EV As Integer
                    For I As Integer = 0 To Me.RadGridView1.Rows.Count - 1
                        Dim position As Position = New Position()
                        position = New Position()
                        position.CID = 701
                        ' Dim lastevent As Integer = VehicleHistory.Events.Count - 1
                        ' Dim lastEventTime As DateTime = VehicleHistory.Events(lastevent).PositionDateTime

                        Dim Pdate As Date = RadGridView1.Rows(I).Cells("DateOfFix").Value
                        Dim tlag As Int16 = DateDiff(DateInterval.Minute, Pdate, DateTime.Now)
                        Dim LatLng As String = RadGridView1.Rows(I).Cells("Latitude").Value '& "," & RadGridView1.Rows(I).Cells("Longitude").Value
                        position.Lat = RadGridView1.Rows(I).Cells("Latitude").Value * 36000
                        position.Lon = RadGridView1.Rows(I).Cells("Longitude").Value * 36000
                        If Me.vehLock_chk.Checked = False Then
                            position.ModemID = Me.VehicleDropDown.SelectedItem.Value
                        Else
                            position.ModemID = Me.vehicle_lbl.Tag
                        End If

                        Dim u As DateTime = Pdate.ToUniversalTime

                        position.Time = DateTime.Parse(u).ToString("yyyy-MM-ddTHH:mm:ss").ToString
                        '  If I > 0 Then Debug.Print(DateDiff("n", lastSent, CDate(RadGridView1.Rows(I).Cells("DateOfFix").Value)))

                        If DateDiff("n", lastSent, CDate(RadGridView1.Rows(I).Cells("DateOfFix").Value)) > 5 AndAlso I > 0 Then
                            positions.Add(position)
                            EV += 1
                            lastSent = RadGridView1.Rows(I).Cells("DateOfFix").Value
                            '  Debug.Print(lastSent & "  " & EV)
                        ElseIf I = 0 Then
                            positions.Add(position)
                            EV += 1
                            lastSent = RadGridView1.Rows(I).Cells("DateOfFix").Value
                        End If
                    Next
                    ' Exit Function '###
                    Dim AddPos As Boolean = True

                    If EV > 0 Then
                        Dim o_result() As Result
                        Dim CallWebService As New PositionServiceSoapClient()

                        o_result = CallWebService.AddPosition("GBA", "positions", "L]V)M_''={w2#^r", positions.ToArray)

                        For I = 0 To UBound(o_result)
                            If o_result(I).ResultCode = 0 Then
                                sendNumber += 1
                            ElseIf o_result(I).ResultCode = 1 Then
                                notSent += 1
                                AddPos = False
                            End If
                        Next
                    End If
                    If AddPos = False Then
                        Return False
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function)
    End Function
    ''' <summary>
    ''' Function used in GPSpassThrough service
    ''' </summary>
    ''' <returns></returns>
    Private Function SendPosToFleetFeence(ByVal GBAVehicleId As Integer)
        Return Task.Factory.StartNew(Of Boolean)(
            Function()
                Try
                    Dim lastSent As DateTime
                    sendNumber = 0
                    notSent = 0
                    If movements.Count = 0 Then Exit Function
                    Dim positions As New List(Of Fleetfence.Position)()
                    Dim EV As Integer
                    For I As Integer = 0 To movements.Count - 1
                        Dim position As Position = New Position()
                        position = New Position()
                        position.CID = 701
                        ' Dim lastevent As Integer = VehicleHistory.Events.Count - 1
                        ' Dim lastEventTime As DateTime = VehicleHistory.Events(lastevent).PositionDateTime

                        Dim Pdate As Date = movements(I).DateofFix
                        Dim tlag As Int16 = DateDiff(DateInterval.Minute, Pdate, DateTime.Now)
                        ' Dim LatLng As String = RadGridView1.Rows(I).Cells("Latitude").Value '& "," & RadGridView1.Rows(I).Cells("Longitude").Value
                        position.Lat = movements(I).Latitude * 36000
                        position.Lon = movements(I).Longitude * 36000

                        position.ModemID = GBAVehicleId

                        Dim u As DateTime = Pdate.ToUniversalTime

                        position.Time = DateTime.Parse(u).ToString("yyyy-MM-ddTHH:mm:ss").ToString
                        '  If I > 0 Then Debug.Print(DateDiff("n", lastSent, CDate(RadGridView1.Rows(I).Cells("DateOfFix").Value)))

                        If DateDiff("n", lastSent, CDate(movements(I).DateofFix)) > 5 AndAlso I > 0 Then
                            positions.Add(position)
                            EV += 1
                            lastSent = movements(I).DateofFix
                            '  Debug.Print(lastSent & "  " & EV)
                        ElseIf I = 0 Then
                            positions.Add(position)
                            EV += 1
                            lastSent = movements(I).DateofFix
                        End If
                    Next
                    ' Exit Function '###
                    Dim AddPos As Boolean = True

                    If EV > 0 Then
                        Dim o_result() As Result
                        Dim CallWebService As New PositionServiceSoapClient()

                        o_result = CallWebService.AddPosition("GBA", "positions", "L]V)M_''={w2#^r", positions.ToArray)

                        For I = 0 To UBound(o_result)
                            If o_result(I).ResultCode = 0 Then
                                sendNumber += 1
                            ElseIf o_result(I).ResultCode = 1 Then
                                notSent += 1
                                AddPos = False
                            End If
                        Next
                    End If
                    If AddPos = False Then
                        Return False
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function)
    End Function
    Private Async Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton1.Click
        Try
            Dim password As String = "systemsOnly"

            ' Dim input = InputBox("Password", "Update Password")
            If Not passtxt.Text = password Then
                MsgBox("Only authorised users are allowed to update positions!")
                Me.RadButton1.Enabled = False
                Exit Sub
            End If

            Me.RadButton1.Enabled = False
            If ListBuilder.Checked = False Then
                Dim sendTask As Task(Of Boolean) = SendToFleetFeence()
                Dim sendResult As Boolean = Await sendTask
                If sendResult <> True Then
                    MsgBox("Results out of date! Not added = " & notSent & vbCrLf & "Positions added " & sendNumber)
                Else
                    MsgBox("All results were added  " & vbCrLf & "Positions added " & sendNumber)

                End If
            ElseIf ListBuilder.Checked = True Then
                Dim DeserializationSettings = New JsonSerializerSettings()
                DeserializationSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat
                DeserializationSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc
                DeserializationSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                DeserializationSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
                ' DeserializationSettings.ContractResolver = New ReadOnlyJsonContractResolver(Unknown, Converters = newList < JsonConverterGreater{New Iso8601TimeSpanConverter(UnknownUnknown}

                Dim jsonSerializerSettings = New JsonSerializerSettings
                jsonSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat
                jsonSerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset
                jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
                ' Console.WriteLine(JSON.Parse(jo.ToString))
                Dim json = JsonConvert.SerializeObject(Request, jsonSerializerSettings)
                If Directory.Exists(path) Then
                    Dim filter As String = File.Exists(path & "\VehicleRequest.json")
                    If filter = True Then
                        File.Delete(path & "\VehicleRequest.json")

                        File.WriteAllText(path & "\VehicleRequest.json", json)
                    Else
                        File.WriteAllText(path & "\VehicleRequest.json", json)
                        RadButton1.Enabled = False
                        Dim img As Image = ImageList1.Images(1)
                        PopUpAlert("The request has been made to process the vehicles positions, you will recieve an email when this has been completed.", "GPS Updater", 5, img)

                    End If
                End If
            End If
        Catch ex As Exception
        Finally
            Me.passtxt.Visible = False
            Me.Passlbl.Visible = False
        End Try
    End Sub
    Private Function getpositions()

        Try
            Dim positions As New List(Of Position)
            'Dim start As DateTime = Format(Now.AddHours(-6), "dd/MM/yyyy HH:mm")
            'Dim [end] As DateTime = Format(Now, "dd/MM/yyyy HH:mm")
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

            vehicleRegistrations.Clear()
            Dim reg As String = Me.VehicleDropDown.Text
            vehicleRegistrations.Add(reg)
            Dim webService As New jobservice.jobService

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
            Return True
        Catch ex As Exception
            MsgBox(Err.Description)
            Return False
        End Try

    End Function

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
            If _isloaded = False Then Exit Sub
            If VehicleDropDown.SelectedValue = Nothing Then Exit Sub
            If vehLock_chk.Checked = False Then
                vehicle_lbl.Text = VehicleDropDown.SelectedText
                vehicle_lbl.Tag = VehicleDropDown.SelectedValue
                IDlabel.Text = "Vehicle ID " & VehicleDropDown.SelectedValue
            End If

            Dim veh = (From v In vehlist Where v.Registration.Equals(VehicleDropDown.SelectedText) Select v).FirstOrDefault
            If Not veh Is Nothing Then
                Trackinglbl.Text = "Tracked by " & veh.TrackedBy
                If veh.TrackedBy = "" Then
                    If veh.MixID > 0 Then
                        Trackinglbl.Text = "Mix"
                    ElseIf veh.BluetreeID > 0 Then
                        Trackinglbl.Text = "BlueTree"
                    Else
                        Trackinglbl.Text = "Masternaut"
                    End If
                End If
            End If
            If locations.Count > 0 Then
                locations.Clear()
            End If
            If vehLock_chk.Checked = False Then
                vehicle_lbl.Text = VehicleDropDown.SelectedText
                vehicle_lbl.Tag = VehicleDropDown.SelectedValue
            End If
            Try
                If Not movements Is Nothing Then
                    If movements.Count > 0 Then
                        Me.RadGridView1.DataSource = Nothing
                    End If
                Else
                    movements = New List(Of movement)
                End If
                If ListBuilder.Checked = True Then
                    startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
                    enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
                    If startdatep = enddatep Then
                        MessageBox.Show("The selected Start and End dzates are the same! Please correct this before adding vehicles.", "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        VehicleDropDown.SelectedIndex = -1
                        Exit Sub
                    End If
                    If Request Is Nothing Then
                        Request = New ObservableCollection(Of RequestVehicle)
                    End If
                    Dim reg As String = VehicleDropDown.Text
                    veh = (From v In vehlist Where v.Registration = reg Select v).FirstOrDefault
                    If veh Is Nothing Then
                        MessageBox.Show("Error finding vehicle", Application.ProductName, MessageBoxButtons.OK)
                        Exit Sub
                    End If
                    Dim bo As New RequestVehicle
                    bo.ID = veh.ID
                    bo.StartDate = startdatep
                    bo.EndDate = enddatep
                    bo.Registration = veh.Registration
                    bo.user = Environ("username") & "@gbaservices.com"
                    bo.RecordCount = 0
                    If veh.BluetreeID > 0 Then
                        bo.BluetreeID = veh.BluetreeID
                    ElseIf veh.MixID > 0 Then
                        bo.MixID = veh.MixID
                    End If
                    Request.Add(bo)
                End If
                If Not Request.Count = 0 Then
                    If RadGridView1.DataSource Is Nothing Then
                        RadGridView1.DataSource = Request
                        RadGridView1.TableElement.VScrollBar.PerformLast()
                        RadGridView1.BestFitColumns()
                        Me.RadGridView1.Refresh()
                        passtxt.Visible = True
                        RadButton1.Text = "Push Request"
                        tooltiptxt = "Pushes the request list to the service."
                    End If
                    RadGridView1.Refresh()
                    If RadGridView1.Rows.Count > 0 Then
                        passtxt.Visible = True
                        Passlbl.Visible = True
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("error adding vehicle :" & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            MessageBox.Show("error getting info:" & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        AddHandler RadGridView1.CellEditorInitialized, AddressOf radGridView1_CellEditorInitialized
        ' Add any initialization after the InitializeComponent() call.

    End Sub

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

    Private Sub passtxt_Leave(sender As Object, e As EventArgs) Handles passtxt.Leave
        Try
            Dim password As String = "systemsOnly"

            ' Dim input = InputBox("Password", "Update Password")
            If Not passtxt.Text = password Then
                ' MsgBox("Only authorised users are allowed to update positions!")
                passtxt.Text = String.Empty
                Passlbl.ForeColor = Color.Red
                Dim img As Image = ImageList1.Images(1)
                PopUpAlert("You have provided the wrong password, please try again!", "GPS Updater", 5, img)
                Passlbl.Text = "Password Required --->"
                Exit Sub
            Else
                Passlbl.Text = "Good password"
                Passlbl.ForeColor = Color.Green
                Dim img As Image = ImageList1.Images(0)
                PopUpAlert("Password correct please proceed, please try again!", "GPS Updater", 5, img)
                If ListBuilder.Checked = True Then
                    RadButton1.Enabled = True
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Position_Updater_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ' Close()
    End Sub



    'Private Sub main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    '    If (e.KeyCode = Keys.B AndAlso e.Modifiers = Keys.Control) Then
    '        MsgBox("CTRL + B Pressed !")
    '    End If
    'End Sub



    Private Sub Position_Updater_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (e.KeyCode = Keys.B AndAlso e.Modifiers = Keys.Control) Then
            RadGridView1.DataSource = vehlist
            Dim img As Image = ImageList1.Images(0)
            PopUpAlert("Getting Data", "List Key Pressed", 5, img)
        End If
    End Sub

    Private Sub PTVFormLoad_Click(sender As Object, e As EventArgs) Handles PTVFormLoad.Click
        Try


        Catch ex As Exception

        End Try
    End Sub

    Private Sub MultiRecordsProcess()
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
                Using db As New Comp1DataContext
                    Dim Jobs = (From J In db.JobItems Where J.dtStartDate >= startdatep And J.dtDeliverDate <= enddatep Select J).ToList

                End Using
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AutoUpdater_Click(sender As Object, e As EventArgs) Handles AutoUpdatermenu.Click
        Try
            'If RadGridView1.Rows.Count > 0 Then
            '    RadGridView1.DataSource = Nothing

            'End If
            If startdatep = enddatep Then
                MessageBox.Show("The selected Start and End dzates are the same! Please correct this before searcing for Jobs.", "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
                VehicleDropDown.SelectedIndex = -1
                Exit Sub
            End If
            ' GetCompletedVehicles()
            AutoUpdater.ShowDialog()
            Me.Visible = False

        Catch ex As Exception

        End Try
    End Sub
    Public Function AutoClose()
        Try
            Me.Visible = True
        Catch ex As Exception

        End Try
    End Function
    Private Sub RadGridView1_CellEndEdit(sender As Object, e As GridViewCellEventArgs) Handles RadGridView1.CellEndEdit
        Try
            If e.Column.HeaderText = "Registration" Then
                Dim reg As String = e.Value
                reg = reg.ToUpper
                Dim veh = (From v In vehlist Where v.Registration = reg Select v).FirstOrDefault
                If veh Is Nothing Then
                    MessageBox.Show("Could not find the vehicle", Application.ProductName, MessageBoxButtons.OK)
                    Exit Sub
                Else
                    Dim bo As New RequestVehicle
                    bo.ID = veh.ID
                    bo.StartDate = startdatep
                    bo.EndDate = enddatep
                    bo.Registration = veh.Registration
                    If veh.BluetreeID > 0 Then
                        bo.BluetreeID = veh.BluetreeID
                    ElseIf veh.MixID > 0 Then
                        bo.MixID = veh.MixID
                    End If
                    Request.Add(bo)
                End If
                If Not Request Is Nothing Then
                    If RadGridView1.DataSource Is Nothing Then
                        RadGridView1.DataSource = Request
                        RadGridView1.TableElement.VScrollBar.PerformLast()
                        RadGridView1.BestFitColumns()
                        Me.RadGridView1.Refresh()
                    End If
                    e = Nothing
                    RadGridView1.MasterTemplate.AllowAddNewRow = False
                    RadGridView1.MasterTemplate.AllowAddNewRow = True
                    RadGridView1.Refresh()
                    e = Nothing
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Function BuildAddHtmlDocument()
        Try

        Catch ex As Exception

        End Try
    End Function
    Private Function SendPositionsEmail(ByVal Reg As String, ByVal Count As Int16)
        Try

            If Not movements.Count = 0 Then
                html = "<center><br />"
                html = html & "<center><img src=""cid:Img1"" ><br />"
                html += "<div>Report Compiled Automatically from Request On" & Now & "</div><br />"
                html += "<table border=1 cellspacing=1 cellpadding=3><tbody>"
                html += "<tr><th bgcolor = LightGray > Registration</th>"
                'html += "<th bgcolor=LightGray>Date</th>"
                html += "<th bgcolor=LightGray > Records Sent</th>"
                html += "<th bgcolor=LightGray>Records Failed</th></tr><tr>"
                html += "<th>" & Reg & "</th>"
                html += "<th>" & sendNumber & "</th>"
                html += "<th>" & notSent & "</th>"
                html += "</tr></tbody></table>"
                Dim img1 As LinkedResource = New LinkedResource("C:\REPORT~2.gif", MediaTypeNames.Image.Jpeg)
                img1.ContentId = "Img1"
                Dim htmlView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(html, Nothing, "text/html")

                Dim mail As New System.Net.Mail.MailMessage()
                mail.AlternateViews.Add(htmlView)


                Dim msgtxt As String = "Received Results for myVehicle Check" & vbCrLf
                mail.Subject = "Positions Request Report"
                mail.From = New MailAddress("Systems@gbaservices.com", "Jobtrak")
                mail.To.Add(New MailAddress("j.bohen@gbaservices.com", "Jobtrak"))
                ' mail.To.Add(New MailAddress("ashford@gbaservices.com", "Jobtrak"))
                'mail.To.Add(New MailAddress("andrew.birkbeck@gbaservices.com", "Jobtrak"))
                Dim smtphost As New SmtpClient("smtp.star.co.uk")
                smtphost.Send(mail)
                Return True
            End If
        Catch ex As Exception
            MessageBox.Show("Error sending position report . " & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
    Private Async Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        Dim RecordsExist As Boolean = False
        Try
            Dim processList
            Dim jsonLiteral As String
            Dim VehId As Int16
            Dim o As Int16 = 0
            'GetlatestMixPositions()
            'Exit Sub '###
            Dim GBAVehicleId As Integer = Nothing
            Dim Reg As String = String.Empty
            Dim di As New OpenFileDialog
            If di.ShowDialog = Windows.Forms.DialogResult.OK Then
                jsonLiteral = File.ReadAllText(di.FileName)
                If InStr(di.FileName, "VehicleRequestResults") > 0 Then
                    processList = JsonConvert.DeserializeObject(Of List(Of movement))(jsonLiteral)
                    RadGridView1.DataSource = processList
                    RadGridView1.TableElement.VScrollBar.PerformLast()
                    RadGridView1.BestFitColumns()
                    Me.RadGridView1.Refresh()
                    Exit Sub
                End If
            End If
            If movements Is Nothing Then
                movements = New List(Of movement)
            End If

            RecordsExist = True

            If Not jsonLiteral Is Nothing Then
                processList = JsonConvert.DeserializeObject(Of List(Of RequestVehicle))(jsonLiteral)
                If Not processList Is Nothing Then
                    RadGridView1.DataSource = processList
                    For I As Int16 = 0 To processList.Count - 1
                        VehId = processList(I).ID
                        If processList(I).MixID > 0 Then
                            startdatep = processList(I).StartDate
                            enddatep = processList(I).EndDate
                            Dim positionM As Task(Of Boolean) = GetMixTripsbyVehicle(processList(I).MixID, processList(I).StartDate, processList(I).EndDate, VehId)
                            Dim positionresult As Boolean = Await positionM
                            If positionresult = True Then
                                If MixRecCount = 1000 Then
                                    While MixRecCount = 1000
                                        lastRec = (From M In movements Where M.ID.Equals(VehId) Order By M.DateofFix Descending Select M).FirstOrDefault
                                        lastDate = CDate(lastRec.DateofFix)
                                        positionM = GetMixTripsbyVehicle(processList(I).MixID, lastDate, enddatep, VehId)
                                        positionresult = Await positionM
                                    End While

                                End If

                            End If

                        ElseIf processList(I).BluetreeID > 0 Then
                            VehId = processList(I).ID
                            Dim GPSTask As Task(Of String) = GetGPSForVehicle(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword, processList(I).BluetreeID, processList(I).StartDate, processList(I).EndDate)
                            Dim GPSResult As String = Await GPSTask

                            If GPSResult.Length > 50 Then
                                Reg = processList(I).Registration

                                Dim reader As XmlReader = XmlReader.Create(New StringReader(GPSResult))
                                While reader.Read
                                    If ((reader.NodeType = XmlNodeType.Element) _
                                         AndAlso (reader.Name = "row")) Then
                                        Dim m As New movement
                                        m.ID = VehId
                                        m.DateofFix = reader.GetAttribute("RecordTimeStamp")
                                        m.Latitude = reader.GetAttribute("Latitude")
                                        m.Longitude = reader.GetAttribute("Longitude")
                                        m.Address = reader.GetAttribute("LocationDescription") & reader.GetAttribute("Country")
                                        m.Registration = Reg
                                        movements.Add(m)
                                        o += 1
                                        processList(I).RecordCount = movements.Count
                                    End If
                                End While
                            End If
                        End If
                    Next
                    lastRec = (From M In movements Where M.ID.Equals(VehId) Order By M.DateofFix Select M).LastOrDefault
                    lastDate = CDate(lastRec.DateofFix)
                    While recordsetComplete = False
                        Dim newTask As Task(Of Boolean) = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", VehId, o)
                        Dim newResult As Boolean = Await newTask
                        If newResult = True Then
                            lastRec = (From M In movements Where M.ID.Equals(VehId) Order By M.DateofFix Select M).LastOrDefault
                            lastDate = CDate(lastRec.DateofFix)
                            newTask = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", VehId, o)
                            newResult = Await newTask
                        End If
                    End While
                End If
            End If
            'End If
            If RecordsExist = True Then
                RadGridView1.DataSource = movements
                RadGridView1.TableElement.VScrollBar.PerformLast()
                RadGridView1.BestFitColumns()
                Me.RadGridView1.Refresh()
                Dim fileext As String = DateTime.Now.Hour & DateTime.Now.Minute & DateTime.Now.Second & DateTime.Now.Year
                Dim jsonSerializerSettings = New JsonSerializerSettings
                jsonSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat
                jsonSerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset
                jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
                Dim json = JsonConvert.SerializeObject(processList, jsonSerializerSettings)
                Dim MovementJson = JsonConvert.SerializeObject(movements, jsonSerializerSettings)
                If Directory.Exists(path) Then
                    File.WriteAllText(path & "\Completed" & "\VehicleRequest" & fileext & ".json", json)
                    File.WriteAllText(path & "\Completed" & "\VehicleRequestResults" & fileext & ".json", MovementJson)
                Else
                    File.WriteAllText(path & "\Completed" & "\VehicleRequest" & fileext & ".json", json)
                    RadButton1.Enabled = False
                    Dim img As Image = ImageList1.Images(1)
                    PopUpAlert("The request has been made to process the vehicles positions, you will recieve an email when this has been completed.", "GPS Updater", 5, img)
                End If
                ''File.Copy(path & "\Records" & "\VehicleRequest.json", path & "\Completed" & "\VehicleRequest.json", True)
                ''File.Delete(path & "\Records" & "\VehicleRequest.json")
            End If
        Catch ex As Exception

        Finally

        End Try
    End Sub

    Private Sub RadGridView1_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridView1.CellFormatting
        Try
            If e.Column.HeaderText = "Registration" Or e.Column.HeaderText = "RecordCount" Then
                'e.Column.IsVisible = False
                If e.CellElement.Text = "R30GBA" Then

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListBuilder_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ListBuilder.ToggleStateChanged
        Try
            If Request Is Nothing Then Exit Try
            If RadGridView1.Rows.Count > 0 Then
                Dim result As DialogResult = MessageBox.Show("This will clear the selected vehicles! Do you want to continue?", "Position Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                If result = DialogResult.No Then
                    Exit Sub
                ElseIf result = DialogResult.Yes Then
                    Request.Clear()
                    Request = Nothing
                    passtxt.Visible = False
                    RadButton1.Text = "Send Positions"
                    tooltiptxt = "Sends the positions to the service"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error clearing Request. " & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Try
            If movements.Count > 0 And RadGridView1.Rows.Count > 0 Then
                Dim result As DialogResult = MessageBox.Show("This will clear the Positions list! Do you want to continue?", "Position Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                If result = DialogResult.No Then
                    Exit Sub
                ElseIf result = DialogResult.Yes Then
                    movements.Clear()
                    RadGridView1.DataSource = Nothing
                    RadGridView1.Refresh()
                    passtxt.Visible = False
                    RadButton1.Text = "Send Positions"
                    tooltiptxt = "Sends the positions to the service"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error clearing Positions. " & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RadButton1_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadButton1.ToolTipTextNeeded
        e.ToolTipText = tooltiptxt
    End Sub
    Private Sub radGridView1_CellEditorInitialized(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.GridViewCellEventArgs)
        Dim tbEditor As RadTextBoxEditor = TryCast(Me.RadGridView1.ActiveEditor, RadTextBoxEditor)
        If Not tbEditor Is Nothing Then
            If (Not tbSubscribed) Then
                tbSubscribed = True
                'Dim tbElement As RadTextBoxEditorElement = CType(tbEditor.EditorElement, RadTextBoxEditorElement)
                'AddHandler tbElement.KeyDown, AddressOf tbElement_KeyDown
            End If
        End If
    End Sub
    Private Sub tbElement_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Control Then
            If e.KeyCode = Keys.L Then
                CType(sender, RadTextBoxEditorElement).Text = "Default text"
            End If
        End If
    End Sub
    Private Sub RadGridView1_CellValidating(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.CellValidatingEventArgs) Handles RadGridView1.CellValidating
        'Dim column As GridViewDataColumn = TryCast(e.Column, GridViewDataColumn)
        'If TypeOf e.Row Is GridViewDataRowInfo AndAlso column IsNot Nothing AndAlso column.Name = "Registration" Then
        '    Dim veh = (From v In Request Where v.Registration.Equals(e.Value) Select v).FirstOrDefault
        '    If Not veh Is Nothing Then
        '        e.Cancel = True
        '        DirectCast(e.Row, GridViewDataRowInfo).ErrorText = "Validation error!"
        '    Else
        '        DirectCast(e.Row, GridViewDataRowInfo).ErrorText = String.Empty
        '    End If
        'End If
    End Sub

    Private Sub radGridView1_CommandCellClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim cell As GridCommandCellElement = CType(sender, GridCommandCellElement)
        Dim column As GridViewDataColumn = CType(cell.ColumnInfo, GridViewCommandColumn)
        If (column.Name = "Registration") Then
            Me.RadGridView1.CancelEdit()
            Me.RadGridView1.MasterView.TableAddNewRow.CancelAddNewRow()
        End If

    End Sub

    Private Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click
        Try
            RadGridView1.DataSource = Nothing
            RadGridView1.Refresh()
            passtxt.Visible = False
            RadButton1.Text = "Send Positions"
            tooltiptxt = "Sends the positions to the service"
            VehicleDropDown.SelectedItem = Nothing
            movements = Nothing
            movements = Nothing
            If Request.Count > 0 Then
                Request.Clear()
                Request = New ObservableCollection(Of RequestVehicle)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub EndTime_ValueChanged(sender As Object, e As EventArgs) Handles EndTime.ValueChanged, StartTime.ValueChanged
        Try
            startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
            enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
        Catch ex As Exception

        End Try
    End Sub


End Class
