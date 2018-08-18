Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports Telerik.WinControls.UI

Module Modutilities
    Public appname As String = "Multiupdater"
    Public movements As New List(Of movement)
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

                    MessageBox.Show("Get Transport Veh " & Err.Description & vehlist(vehlist.Count - 1).Registration, appname)
                    Return 0
                End Try
            End Function)
    End Function
End Module
