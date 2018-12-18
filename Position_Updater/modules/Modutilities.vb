Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Threading.Tasks
Imports Position_Updater.jobservice
Imports Position_Updater.PositioningWS
Imports Telerik.WinControls.UI

Module Modutilities
    Public appname As String = "Multiupdater"
    Public movements As New List(Of movement)
    Public vehicleRegistrations As New List(Of String)()
    Public locations As New List(Of VehicleEvent)
    Public joblist As New List(Of Job)
    Private ReadOnly _login As String = "gba_services_v2"
    '  Private oTimer As System.Threading.Timer
    Private ReadOnly _password As String = "8798"
    Public progresstxt As String
    Public progress As Int16
    Public MixRecCount As Integer
    Public SentList As List(Of GPSsent)
    Public Sub PopUpAlert(ByVal bodytext As String, ByVal caption As String, ByVal imagedID As Int16, ByVal img As Image)
        Try
            Dim Popup As New RadDesktopAlert
            Popup.ContentImage = img

            Popup.ContentImage.Tag = 1
            Popup.CaptionText = caption
            Popup.ContentText = bodytext '"<html><i>The Job has been sent to " & DriverName & "</i>.<b><span><color=Red> Please call " & Mobile & " to confirm receipt!. </span></b></html>"
            Popup.FixedSize = New System.Drawing.Size(280, 100)
            ' Popup.IsPinned = True
            Popup.ThemeName = "TelerikMetroBlue" 'Office2010"
            Popup.PopupAnimationDirection = RadDirection.Up

            Popup.FadeAnimationType = Popup.FadeAnimationType Or FadeAnimationType.FadeIn
            'Popup.ButtonItems.Add(bt)
            Popup.ScreenPosition = AlertScreenPosition.BottomLeft
            Popup.AutoCloseDelay = 10
            Popup.Show()
        Catch ex As Exception
            MessageBox.Show("error setting desktop alert!")
        End Try


    End Sub
    Public Function GetTransportVehicles()
        Return Task.Factory.StartNew(Of Integer)(
            Function()
                vehlist = New ObservableCollection(Of myVehicle)
                Try
                    progresstxt = "Getting Vehicles"
                    Dim dB As New mydataDataContext

                    Dim veh = (From V In dB.VehicleBases Order By V.dwClassificationIdFK Select V).ToList


                    For I As Integer = 0 To veh.Count - 1

                        Dim ve As New myVehicle

                        ve.ID = veh(I).dwVehicleId

                        'If veh(I).szRegistration = "J17GBA" Then
                        '    Debug.Print("J17")
                        'End If
                        'Dim LastLoc = (From L In dB.GPSPositions Where L.VehID.Equals(ve.ID) Order By L.dateoffix Descending Take 1 Select L).FirstOrDefault
                        'If Not LastLoc Is Nothing Then
                        '    ve.LatLong = LastLoc.LatLong
                        '    ve.DateofFix = LastLoc.dateoffix
                        '    ve.Address = LastLoc.Location
                        '    If Not LastLoc.TrackedBY Is Nothing Then ve.TrackedBy = LastLoc.TrackedBY
                        'Else
                        '    '  Debug.Print(ve.ID & " " & veh(I).szRegistration)
                        'End If
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

                    MessageBox.Show("Get Transport Veh " & Err.Description & vehlist(vehlist.Count - 1).Registration, appname)
                    Return 0

                Finally
                    progresstxt = "Vehicles Complete"
                End Try
            End Function)
    End Function
    Private Function getpositions()
        Return Task.Factory.StartNew(Of Boolean)(
            Function()
                Try
                    Dim positions As New List(Of Fleetfence.Position)()
                    Dim start As DateTime = Format(Now.AddHours(-6), "dd/MM/yyyy HH:mm")
                    Dim [end] As DateTime = Format(Now, "dd/MM/yyyy HH:mm")

                    Dim Units As DistanceUnit
                    Units = DistanceUnit.Mile

                    Dim eventsToReturn As New List(Of EventType)()
                    Dim eventTypeLocation As New EventType()
                    eventTypeLocation.Code = EventCode.VehicleLocation
                    eventsToReturn.Add(eventTypeLocation)
                    Dim eventTypeignition = New EventType
                    eventTypeignition.Code = EventCode.Ignition
                    eventsToReturn.Add(eventTypeignition)


                    '  Dim reg As String = Me.VehicleDropDown.Text
                    '  vehicleRegistrations.Add(reg)
                    Dim webservice As New jobservice.jobService

                    Dim vehicleHistorys As VehicleHistoryV2() = webservice.GetHistoryV2(_login, _password, vehicleRegistrations.ToArray(), VehicleCollectionType.Vehicles, start, [end],
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
            End Function)
    End Function
    Public Sub CopyStream(input As Stream, output As Stream)
        Dim buffer As Byte() = New Byte(8 * 1024 - 1) {}
        Dim len As Integer
        While (InlineAssignHelper(len, input.Read(buffer, 0, buffer.Length))) > 0
            output.Write(buffer, 0, len)
        End While
    End Sub
    Private Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
    Public Function SavedTimezone(ByVal Country As String) As Int16
        Try
            Country = Country.ToLower
            Select Case (Country)
                Case "albania"
                    Return 1
                Case "algeria"
                    Return 1
                Case "andorra"
                    Return 1
                Case "austria"
                    Return 1
                Case "österreich"
                    Return 1
                Case "belgium"
                    Return 1
                Case "belgique"
                    Return 1
                Case "belgië"
                    Return 1
                Case "bosnia and herzegovina"
                    Return 1
                Case "croatia"
                    Return 1
                Case "czech republic"
                    Return 1
                Case "česko "
                    Return 1
                Case "denmark"
                    Return 1
                Case "france"
                    Return 1
                Case "germany"
                    Return 1
                Case "deutschland"
                    Return 1
                Case "holland"
                    Return 1
                Case "nederland"
                    Return 1
                Case "netherlands"
                    Return 1
                Case "hungary"
                    Return 1
                Case "magyarország"
                    Return 1
                Case "ireland"
                    Return 0
                Case "italy"
                    Return 1
                Case "italia"
                    Return 1
                Case "kosovo"
                    Return 1
                Case "liechtenstein"
                    Return 1
                Case "luxembourg"
                    Return 1
                Case "macedonia"
                    Return 1
                Case "malta"
                    Return 1
                Case "mauritania"
                    Return 0
                Case "monaco"
                    Return 1
                Case "montenegro"
                    Return 1
                Case "norway"
                    Return 1
                Case "poland"
                    Return 1
                Case "polska"
                    Return 1
                Case "portugal"
                    Return 0
                Case "românia"
                    Return 0
                Case "san marino"
                    Return 1
                Case "serbia"
                    Return 1
                Case "slovakia"
                    Return 1
                Case "slovenia"
                    Return 1
                Case "spain"
                    Return 1
                Case "españa"
                    Return 1
                Case "sweden"
                    Return 1
                Case "switzerland"
                    Return 1
                Case "Vatican city"
                    Return 1
                Case " българия"
                    Return 1
                Case "п. југословенска репуб. македонија" 'Yugoslavia
                    Return 1
                Case "ukraine"
                    Return 1
                Case Else
                    Console.WriteLine("Saved Time Zones : " & Country & " Not Found!")
            End Select
        Catch ex As Exception
            MessageBox.Show("Saved Time Zones error: " & Err.Description)
        End Try
    End Function
    ''' <summary>
    ''' Get a list of active(status 27) jobs that are in the Active Fleet Fence table. set the date submitted field in the vehicle collection to the start date for the job
    ''' and the job number to the job.
    ''' </summary>
    ''' <returns>Returns true if completed, false if not.</returns>
    Public Function GetCompletedVehicles()
        Return Task.Factory.StartNew(Of Boolean)(
          Function()
              Try
                  SentList = New List(Of GPSsent)
                  Using dB As New mydataDataContext
                      Dim acclist As New List(Of Integer)
                      'Retrieve active accounts from the ActiveFleetfence table in transport test database and add to the filter _
                      'used against the active base table views to return current jobs. 
                      Dim fleetAcc = (From acc In dB.ActiveFleetfences Where acc.GBAcompany.Equals(1) AndAlso acc.active.Equals(1) Select acc.dwaccountidfk).ToList
                      If Not fleetAcc Is Nothing Then
                          For I As Integer = 0 To fleetAcc.Count - 1
                              acclist.Add(fleetAcc(I))
                          Next
                      End If

                      Dim query1 = (From gba In dB.ActiveBaseGBAs Where acclist.Contains(gba.dwAccountIdFK) Select gba.ColDate, gba.dwJobStatusIdFK, gba.DelDate, gba.Vehicle, gba.dwJobNumber, gba.szName, gba.Trailers).ToList

                      If Not query1.Count = 0 Then
                          Dim active = (From A In query1 Where A.dwJobStatusIdFK.Equals(27) Or A.dwJobStatusIdFK.Equals(28) Select A).ToList ' is on route to delivery
                          Dim coltDiff, deltDiff As Integer
                          If Not active.Count = 0 Then
                              For i As Integer = 0 To active.Count - 1
                                  coltDiff = DateDiff(DateInterval.Minute, CDate(active(i).ColDate), DateTime.Now)
                                  deltDiff = DateDiff(DateInterval.Minute, CDate(active(i).DelDate), DateTime.Now)
                                  If deltDiff > 39 And deltDiff < 120 Then
                                      Dim veh = (From V In vehlist Where V.Registration.Equals(active(i).Vehicle) Or V.Registration.Equals(active(i).Trailers) Select V).FirstOrDefault
                                      If Not veh Is Nothing Then
                                          Dim sdate As DateTime = active(i).ColDate
                                          Dim edate As DateTime = active(i).DelDate
                                          Dim gpsSent = (Aggregate g In dB.GPSPositions Where g.VehID.Equals(veh.ID) AndAlso g.dateoffix >= CDate(active(i).ColDate) AndAlso g.dateoffix <= CDate(active(i).DelDate) Into Count)

                                          If gpsSent > 0 Then

                                          End If

                                          '                Dim londonCustomerCount = Aggregate cust In dB.Customers
                                          'Where cust.City = "London"
                                          'Into Count()
                                      End If
                                      Continue For
                                  Else
                                      Dim veh = (From V In vehlist Where V.Registration.Equals(active(i).Vehicle) Or V.Registration.Equals(active(i).Trailers) Select V).FirstOrDefault
                                      If Not veh Is Nothing Then
                                          Dim gpsSent = (Aggregate g In dB.GPSPositions Where g.VehID.Equals(veh.ID) AndAlso g.dateoffix >= CDate(active(i).ColDate) AndAlso g.dateoffix <= CDate(active(i).DelDate) Into Count)
                                          If gpsSent > 0 Then
                                              Dim FirstSent = (From f In dB.GPSPositions Where f.VehID.Equals(veh.ID) AndAlso f.dateoffix >= CDate(active(i).ColDate) AndAlso f.dateoffix <= CDate(active(i).DelDate) Order By f.dateoffix Ascending Select f.dateoffix).FirstOrDefault
                                              Dim LastSent = (From f In dB.GPSPositions Where f.VehID.Equals(veh.ID) AndAlso f.dateoffix >= CDate(active(i).ColDate) AndAlso f.dateoffix <= CDate(active(i).DelDate) Order By f.dateoffix Descending Select f.dateoffix).FirstOrDefault
                                              Dim bo As New GPSsent
                                              bo.ColDate = active(i).ColDate
                                              bo.DelDate = active(i).DelDate
                                              bo.JobNumber = active(i).dwJobNumber
                                              bo.Registration = veh.Registration
                                              bo.Count = gpsSent
                                              bo.StartDate = FirstSent
                                              bo.EndDate = LastSent
                                              SentList.Add(bo)
                                          End If
                                      End If
                                      '                Dim londonCustomerCount = Aggregate cust In dB.Customers
                                      'Where cust.City = "London"
                                      'Into Count()
                                  End If
                                     Next
                          End If
                      End If
                  End Using

                  'Dim veh = (From V In vehlist Where V.Registration.Equals(active(i).Vehicle) Or V.Registration.Equals(active(i).Trailers) Select V).FirstOrDefault
                  'If Not veh Is Nothing Then
                  '    Dim gpsSent = (Aggregate g In dB.GPSPositions Where g.VehID.Equals(veh.Registration) AndAlso g.dateoffix >= active(i).ColDate AndAlso g.dateoffix <= active(i).DelDate Into Count)
                  '    If gpsSent > 0 Then

                  '    End If

                  '    '                Dim londonCustomerCount = Aggregate cust In dB.Customers
                  '    'Where cust.City = "London"
                  '    'Into Count()
                  'End If
                  'End If

                  '                    If coltDiff > -29 Then 'Job fulfills required activity criteria and data is exposed.
                  '                    Dim veh = (From V In vehlist Where V.Registration.Equals(active(i).Vehicle) Or V.Registration.Equals(active(i).Trailers) Select V).FirstOrDefault
                  '                    If Not veh Is Nothing Then
                  '                        veh.FleetFence = 1
                  '                        If Not veh.LatLong Is Nothing Then
                  '                            veh.expose = True
                  '                            veh.Company = 1
                  '                            veh.JobNumber = active(i).dwJobNumber
                  '                            veh.Statusid = active(i).dwJobStatusIdFK
                  '                            veh.Acc_Name = active(i).szName
                  '                            If veh.DateSubmitted = Nothing Then
                  '                                veh.DateSubmitted = active(i).ColDate
                  '                                If DateDiff(DateInterval.Minute, CDate(active(i).ColDate), DateTime.Now) > 60 Then
                  '                                    veh.DateSubmitted = DateAdd(DateInterval.Hour, -1, DateTime.Now)
                  '                                End If
                  '                            End If
                  '                            If Not active(i).Trailers Is Nothing Then
                  '                                veh.Trailer = active(i).Trailers
                  '                                Dim trailer = active(i).Trailers
                  '                                Dim temp = (From T In vehlist Where T.Registration.Equals(trailer) Select T).FirstOrDefault
                  '                                If Not temp Is Nothing Then
                  '                                    If Not temp.MixTempID = 0 Then
                  '                                        veh.MixTempID = temp.MixTempID
                  '                                        veh.Temperature = temp.Temperature
                  '                                        veh.DateofEvent = temp.DateofEvent
                  '                                        veh.NewEvt = temp.NewEvt
                  '                                        veh.expose = True
                  '                                    ElseIf Not temp.mDateofEvent = Nothing Then
                  '                                        veh.MTemperature = temp.MTemperature
                  '                                        veh.mDateofEvent = temp.mDateofEvent
                  '                                        veh.mNewEvt = temp.mNewEvt
                  '                                        veh.expose = True
                  '                                    End If
                  '                                End If
                  '                            End If
                  '                        Else
                  '                            veh.expose = False
                  '                        End If
                  '                    End If
                  '                End If
                  '            Next
                  '        End If
                  '    End If
                  '    Dim inactive = (From B In query1 Where B.dwJobStatusIdFK < 27 AndAlso B.dwJobStatusIdFK > 18 Select B).ToList
                  '    If Not inactive Is Nothing Then
                  '        For i As Integer = 0 To inactive.Count - 1
                  '            Dim coltDiff, deltDiff As Integer
                  '            coltDiff = DateDiff(DateInterval.Minute, CDate(inactive(i).ColDate), DateTime.Now)
                  '            deltDiff = DateDiff(DateInterval.Minute, CDate(inactive(i).DelDate), DateTime.Now)
                  '            If coltDiff > -29 AndAlso deltDiff < 60 Then 'Job fulfills required activity criteria and data is exposed.
                  '                Dim veh = (From V In vehlist Where V.Registration.Equals(inactive(i).Vehicle) Select V).FirstOrDefault
                  '                If Not veh Is Nothing Then
                  '                    If Not veh.LatLong Is Nothing Then
                  '                        veh.expose = True
                  '                        veh.Company = 1
                  '                        veh.JobNumber = inactive(i).dwJobNumber
                  '                        veh.Statusid = inactive(i).dwJobStatusIdFK
                  '                        veh.Acc_Name = inactive(i).szName
                  '                        If Not inactive(i).Trailers Is Nothing Then
                  '                            veh.Trailer = inactive(i).Trailers
                  '                            veh.expose = False
                  '                        End If
                  '                    End If
                  '                End If
                  '            End If
                  '        Next
                  '    End If
                  '    acclist.Clear()
                  '    fleetAcc = (From acc In dB.ActiveFleetfences Where acc.GBAcompany.Equals(2) AndAlso acc.active.Equals(1) Select acc.dwaccountidfk).ToList
                  '    If Not fleetAcc Is Nothing Then
                  '        For I As Integer = 0 To fleetAcc.Count - 1
                  '            acclist.Add(fleetAcc(I))
                  '        Next
                  '    End If

                  '    Dim query2 = (From gba In dB.ActiveBaseEuropes Where acclist.Contains(gba.dwAccountIdFK) Select gba.ColDate, gba.dwJobStatusIdFK, gba.DelDate, gba.Vehicle, gba.dwJobNumber, gba.szName, gba.Trailers).ToList

                  '    If Not query2.Count = 0 Then
                  '        Dim active = (From A In query2 Where A.dwJobStatusIdFK.Equals(27) Or A.dwJobStatusIdFK.Equals(28) Select A).ToList
                  '        Dim coltDiff, deltDiff As Integer
                  '        If Not active Is Nothing Then
                  '            For i As Integer = 0 To active.Count - 1
                  '                coltDiff = DateDiff(DateInterval.Minute, CDate(active(i).ColDate), DateTime.Now)
                  '                deltDiff = DateDiff(DateInterval.Minute, CDate(active(i).DelDate), DateTime.Now)
                  '                Dim xx As Integer = i
                  '                If coltDiff > -29 AndAlso deltDiff < 90 Then 'Job fulfills required activity criteria and data is exposed.
                  '                    Dim veh = (From V In vehlist Where V.Registration.Equals(active(xx).Vehicle) Select V).FirstOrDefault
                  '                    If Not veh Is Nothing Then
                  '                        veh.FleetFence = 1
                  '                        If Not veh.LatLong Is Nothing Then
                  '                            veh.expose = True
                  '                            veh.Company = 2
                  '                            veh.JobNumber = active(i).dwJobNumber
                  '                            veh.Statusid = active(i).dwJobStatusIdFK
                  '                            veh.Acc_Name = active(i).szName
                  '                        Else
                  '                            veh.expose = False
                  '                        End If
                  '                    End If
                  '                End If
                  '            Next
                  '        End If
                  '    End If
                  'End Using
                  'Dim query3 = (From gba In dB.ActiveBaseEuropes Where acclist.Contains(gba.dwAccountIdFK) Select gba).ToList
                  'Dim inactiveEU = (From B In query3 Where B.dwJobStatusIdFK < 27 Select B).ToList
                  'If Not inactiveEU Is Nothing Then
                  '    For i As Integer = 0 To inactiveEU.Count - 1
                  '        'The job is not past its delivery time but the status is not active,
                  '        'send the jobs details!
                  '        Dim coltDiff, deltDiff As Integer
                  '        coltDiff = DateDiff(DateInterval.Minute, CDate(inactiveEU(i).ColDate), DateTime.Now)
                  '        deltDiff = DateDiff(DateInterval.Minute, CDate(inactiveEU(i).DelDate), DateTime.Now)
                  '        If deltDiff < 60 Then 'Job fulfills required activity criteria and data is exposed.
                  '            Dim veh = (From V In vehlist Where V.Registration.Equals(inactiveEU(i).Vehicle) Select V).FirstOrDefault
                  '            If Not veh Is Nothing Then
                  '                veh.expose = True
                  '                veh.Company = 2
                  '                veh.JobNumber = inactiveEU(i).dwJobNumber
                  '                veh.Statusid = inactiveEU(i).dwJobStatusIdFK
                  '                veh.Acc_Name = inactiveEU(i).szName
                  '            End If
                  '        End If
                  '    Next
                  'End If
                  Return True
              Catch ex As Exception
                  MessageBox.Show("Get Fleetfence Comp " & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
                  Return False
              End Try
          End Function)
    End Function
End Module
