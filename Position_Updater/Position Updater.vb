Imports Position_Updater.jobservice
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Xml
Imports System.IO
Imports Telerik.WinControls.UI
Imports Position_Updater.Fleetfence

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
            If vehlist.Count <> 0 Then

            End If
            startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
            enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
            RadWaitingBar1.StartWaiting()
            RadWaitingBar1.Text = "Attempting to get Records"
            RadWaitingBar1.Visible = True
            If enddatep < startdatep Then
                MsgBox("The end date mus be after the start date", MsgBoxStyle.Critical)
                Exit Sub

            Else
                Dim reg As String = VehicleDropDown.Text
                Dim veh = (From v In vehlist Where v.Registration = reg Select v).FirstOrDefault
                If veh Is Nothing Then
                    MessageBox.Show("Error finding vehicle", Application.ProductName, MessageBoxButtons.OK)
                    Exit Sub
                End If
                If Trackinglbl.Text.IndexOf("Mix") > 0 Then
                    movements = New List(Of movement)
                    Dim positionM As Task(Of Boolean) = GetMixTripsbyVehicle(veh.MixID, startdatep, enddatep)
                    Dim positionresult As Boolean = Await positionM
                    If positionresult = True Then
                        If movements.Count = 1000 Then
                            Dim d As Date = movements(999).DateofFix
                            If DateDiff("n", d, enddatep) > 50 Then
                                positionM = GetMixTripsbyVehicle(veh.MixID, d, enddatep)
                                positionresult = Await positionM
                                If positionresult = True Then

                                End If
                            End If
                        End If
                        RadGridView1.DataSource = movements
                        RadGridView1.BestFitColumns()
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
                                    m.ID = o
                                    m.DateofFix = reader.GetAttribute("RecordTimeStamp")
                                    m.Latitude = reader.GetAttribute("Latitude")
                                    m.Longitude = reader.GetAttribute("Longitude")
                                    m.Address = reader.GetAttribute("LocationDescription") & reader.GetAttribute("Country")
                                    movements.Add(m)
                                    o += 1
                                End If
                            End While

                            Dim lastRec = (From M In movements Order By M.DateofFix Select M).LastOrDefault
                            Dim newTask As Task(Of Boolean) = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", vehId, o)
                            Dim newResult As Boolean = Await newTask
                            If newResult = True Then
                                lastRec = (From M In movements Order By M.DateofFix Select M).LastOrDefault
                                newTask = GPStestforLast(lastRec.DateofFix, enddatep, "BlueTree", vehId, o)
                                newResult = Await newTask
                            End If
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
    Private Async Function GPStestforLast(ByVal lastRec As DateTime, enddatep As DateTime, ByVal caller As String, ByVal vehid As Int16, ByVal o As Int16) As Task(Of Boolean)
        Try
            Debug.Print(DateDiff(DateInterval.Minute, lastRec, EndDate.Value))
            If DateDiff(DateInterval.Minute, lastRec, DateTime.Now) > 15 Then
                Me.RadGridView1.DataSource = Nothing
                If caller = "BlueTree" Then
                    Dim GPSTask As Task(Of String) = GetGPSForVehicle(My.Settings.BlueTreeUser, My.Settings.BlueTreePassword, vehid, lastRec, enddatep)
                    Dim GPSResult As String = Await GPSTask
                    If GPSResult.Length > 50 Then
                        Dim reader As XmlReader = XmlReader.Create(New StringReader(GPSResult))
                        While reader.Read
                            If ((reader.NodeType = XmlNodeType.Element) _
                                 AndAlso (reader.Name = "row")) Then
                                Dim m As New movement
                                o += 1
                                m.ID = o
                                '     Debug.Print(reader.GetAttribute("RecordTimeStamp"))
                                m.DateofFix = reader.GetAttribute("RecordTimeStamp")
                                m.Latitude = reader.GetAttribute("Latitude") '& "," & reader.GetAttribute("Longitude")
                                m.Longitude = reader.GetAttribute("Longitude")
                                m.Address = reader.GetAttribute("LocationDescription") & "," & reader.GetAttribute("Country")
                                movements.Add(m)
                            End If
                        End While
                    End If
                End If
            End If
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

    Private Async Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton1.Click
        Try
            Dim password As String = "systemsOnly"

            ' Dim input = InputBox("Password", "Update Password")
            If Not passtxt.Text = password Then
                MsgBox("Only authorised users are allowed to update positions!")
                Me.RadButton1.Enabled = False
                Exit Sub
            End If

            Me.RadButton1.Enabled = True

            Dim sendTask As Task(Of Boolean) = SendToFleetFeence()
            Dim sendResult As Boolean = Await sendTask
            If sendResult = True Then
                MsgBox("Results out of date! Not added = " & notSent & vbCrLf & "Positions added " & sendNumber)
            Else
                MsgBox("All results were added  " & vbCrLf & "Positions added " & sendNumber)

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
                If movements.Count > 0 Then
                    Me.RadGridView1.DataSource = Nothing
                End If

                If ListBuilder.Checked = True Then
                    startdatep = Format(Me.StartDate.Value, "dd/MM/yyyy") & " " & Format(Me.StartTime.Value, "HH:mm")
                    enddatep = Format(Me.EndDate.Value, "dd/MM/yyyy") & " " & Format(Me.EndTime.Value, "HH:mm")
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
                    RadGridView1.Refresh()
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
    Private Sub AutoUpdater_Click(sender As Object, e As EventArgs) Handles AutoUpdater.Click
        Try
            If RadGridView1.Rows.Count > 0 Then
                RadGridView1.DataSource = Nothing

            End If
        Catch ex As Exception

        End Try
    End Sub

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
End Class
