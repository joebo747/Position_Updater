Public Class PTVUpdater
    Private Sub PTVUpdater_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim l As Point
            l.X = -5
            l.Y = -5
            RadDock1.Location = l
            RadDock1.Height = Me.Height
            Dim h As Int16 = Me.RadDock1.Height / 2
            Jobs.Height = h
            TrackingWindow.Height = h
        Catch ex As Exception
            MessageBox.Show("error loading " & Err.Description)
        End Try
    End Sub

    Private Sub RadDock1_DockWindowClosing(sender As Object, e As Telerik.WinControls.UI.Docking.DockWindowCancelEventArgs) Handles RadDock1.DockWindowClosing
        Try
            If Not Me.TrackingWindow.DockState = Telerik.WinControls.UI.Docking.DockState.Docked Then
                Me.Close()
                Dim l As New Point
                l.X = 0
                l.Y = 0
                TrackingWindow.Location = l

            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
