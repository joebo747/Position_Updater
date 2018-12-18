Public Class AutoUpdater


    Private Sub AutoUpdater_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            Position_Updater.Show()
        Catch ex As Exception

        End Try
    End Sub

    Private Async Sub Last24HoursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Last24HoursToolStripMenuItem.Click
        Try
            Dim getRecords As Threading.Tasks.Task(Of Boolean) = GetCompletedVehicles()
            Dim recordResults As Boolean = Await getRecords
            If recordResults = True Then
                RecordGrid.AutoGenerateColumns = True
                RecordGrid.DataSource = SentList
                RecordGrid.BestFitColumns()
                RecordGrid.Refresh()
            End If
        Catch ex As Exception
            MessageBox.Show("Error Getting Records: " & Err.Description, "Position Updater", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
