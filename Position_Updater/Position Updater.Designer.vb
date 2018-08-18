<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Position_Updater
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Position_Updater))
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel4 = New Telerik.WinControls.UI.RadLabel()
        Me.GetPositons = New Telerik.WinControls.UI.RadButton()
        Me.VehicleDropDown = New Telerik.WinControls.UI.RadDropDownList()
        Me.StartDate = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.EndDate = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.StartTime = New Telerik.WinControls.UI.RadTimePicker()
        Me.EndTime = New Telerik.WinControls.UI.RadTimePicker()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel5 = New Telerik.WinControls.UI.RadLabel()
        Me.Office2007BlackTheme1 = New Telerik.WinControls.Themes.Office2007BlackTheme()
        Me.positonCountlbl = New Telerik.WinControls.UI.RadLabel()
        Me.RadWaitingBar1 = New Telerik.WinControls.UI.RadWaitingBar()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.passtxt = New Telerik.WinControls.UI.RadTextBox()
        Me.Passlbl = New Telerik.WinControls.UI.RadLabel()
        Me.selectedVehicle = New Telerik.WinControls.UI.RadLabel()
        Me.vehicle_lbl = New Telerik.WinControls.UI.RadLabel()
        Me.vehLock_chk = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadLabel6 = New Telerik.WinControls.UI.RadLabel()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Trackinglbl = New Telerik.WinControls.UI.RadLabel()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GetPositons, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleDropDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StartTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EndTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.positonCountlbl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadWaitingBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.passtxt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Passlbl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.selectedVehicle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vehicle_lbl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vehLock_chk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Trackinglbl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadLabel1
        '
        Me.RadLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel1.Location = New System.Drawing.Point(168, 82)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(31, 16)
        Me.RadLabel1.TabIndex = 0
        Me.RadLabel1.Text = "Date"
        '
        'RadLabel2
        '
        Me.RadLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel2.Location = New System.Drawing.Point(67, 107)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(31, 16)
        Me.RadLabel2.TabIndex = 1
        Me.RadLabel2.Text = "Start"
        '
        'RadLabel3
        '
        Me.RadLabel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel3.Location = New System.Drawing.Point(296, 82)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(32, 16)
        Me.RadLabel3.TabIndex = 1
        Me.RadLabel3.Text = "Time"
        '
        'RadLabel4
        '
        Me.RadLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel4.Location = New System.Drawing.Point(67, 145)
        Me.RadLabel4.Name = "RadLabel4"
        Me.RadLabel4.Size = New System.Drawing.Size(27, 16)
        Me.RadLabel4.TabIndex = 2
        Me.RadLabel4.Text = "End"
        '
        'GetPositons
        '
        Me.GetPositons.Location = New System.Drawing.Point(420, 105)
        Me.GetPositons.Name = "GetPositons"
        Me.GetPositons.Size = New System.Drawing.Size(100, 24)
        Me.GetPositons.TabIndex = 3
        Me.GetPositons.Text = "Get Positons"
        Me.GetPositons.ThemeName = "Office2007Black"
        '
        'VehicleDropDown
        '
        Me.VehicleDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.VehicleDropDown.BackColor = System.Drawing.Color.White
        Me.VehicleDropDown.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InOutQuad
        Me.VehicleDropDown.Location = New System.Drawing.Point(146, 16)
        Me.VehicleDropDown.Name = "VehicleDropDown"
        Me.VehicleDropDown.Size = New System.Drawing.Size(103, 24)
        Me.VehicleDropDown.TabIndex = 5
        Me.VehicleDropDown.ThemeName = "Office2007Black"
        '
        'StartDate
        '
        Me.StartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.StartDate.Location = New System.Drawing.Point(146, 107)
        Me.StartDate.Name = "StartDate"
        Me.StartDate.Size = New System.Drawing.Size(103, 24)
        Me.StartDate.TabIndex = 6
        Me.StartDate.TabStop = False
        Me.StartDate.Text = "18/04/2013"
        Me.StartDate.ThemeName = "Office2007Black"
        Me.StartDate.Value = New Date(2013, 4, 18, 13, 58, 23, 478)
        '
        'EndDate
        '
        Me.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EndDate.Location = New System.Drawing.Point(146, 145)
        Me.EndDate.Name = "EndDate"
        Me.EndDate.Size = New System.Drawing.Size(103, 24)
        Me.EndDate.TabIndex = 7
        Me.EndDate.TabStop = False
        Me.EndDate.Text = "18/04/2013"
        Me.EndDate.ThemeName = "Office2007Black"
        Me.EndDate.Value = New Date(2013, 4, 18, 13, 58, 23, 478)
        '
        'StartTime
        '
        Me.StartTime.Location = New System.Drawing.Point(285, 107)
        Me.StartTime.MaxValue = New Date(9999, 12, 31, 23, 59, 59, 0)
        Me.StartTime.MinValue = New Date(CType(0, Long))
        Me.StartTime.Name = "StartTime"
        Me.StartTime.Size = New System.Drawing.Size(100, 24)
        Me.StartTime.TabIndex = 8
        Me.StartTime.TabStop = False
        Me.StartTime.ThemeName = "Office2007Black"
        Me.StartTime.Value = New Date(2013, 4, 18, 13, 59, 41, 0)
        '
        'EndTime
        '
        Me.EndTime.Location = New System.Drawing.Point(285, 145)
        Me.EndTime.MaxValue = New Date(9999, 12, 31, 23, 59, 59, 0)
        Me.EndTime.MinValue = New Date(CType(0, Long))
        Me.EndTime.Name = "EndTime"
        Me.EndTime.Size = New System.Drawing.Size(100, 24)
        Me.EndTime.TabIndex = 9
        Me.EndTime.TabStop = False
        Me.EndTime.ThemeName = "Office2007Black"
        Me.EndTime.Value = New Date(2013, 4, 18, 13, 59, 41, 0)
        '
        'RadGridView1
        '
        Me.RadGridView1.Location = New System.Drawing.Point(12, 208)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.EnableGrouping = False
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
        Me.RadGridView1.Size = New System.Drawing.Size(518, 242)
        Me.RadGridView1.TabIndex = 10
        Me.RadGridView1.ThemeName = "Office2007Black"
        '
        'RadButton1
        '
        Me.RadButton1.Enabled = False
        Me.RadButton1.Location = New System.Drawing.Point(430, 466)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(100, 24)
        Me.RadButton1.TabIndex = 11
        Me.RadButton1.Text = "Send Positons"
        Me.RadButton1.ThemeName = "Office2007Black"
        '
        'RadLabel5
        '
        Me.RadLabel5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel5.Location = New System.Drawing.Point(67, 19)
        Me.RadLabel5.Name = "RadLabel5"
        Me.RadLabel5.Size = New System.Drawing.Size(45, 16)
        Me.RadLabel5.TabIndex = 12
        Me.RadLabel5.Text = "Vehicle"
        '
        'positonCountlbl
        '
        Me.positonCountlbl.Location = New System.Drawing.Point(12, 466)
        Me.positonCountlbl.Name = "positonCountlbl"
        Me.positonCountlbl.Size = New System.Drawing.Size(36, 18)
        Me.positonCountlbl.TabIndex = 13
        Me.positonCountlbl.Text = "Count"
        '
        'RadWaitingBar1
        '
        Me.RadWaitingBar1.Location = New System.Drawing.Point(54, 466)
        Me.RadWaitingBar1.Name = "RadWaitingBar1"
        Me.RadWaitingBar1.Size = New System.Drawing.Size(370, 24)
        Me.RadWaitingBar1.TabIndex = 14
        Me.RadWaitingBar1.Text = "RadWaitingBar1"
        Me.RadWaitingBar1.ThemeName = "Office2007Black"
        Me.RadWaitingBar1.WaitingDirection = Telerik.WinControls.ProgressOrientation.Left
        Me.RadWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.Throbber
        '
        'Timer1
        '
        '
        'passtxt
        '
        Me.passtxt.AcceptsReturn = True
        Me.passtxt.Location = New System.Drawing.Point(284, 172)
        Me.passtxt.Name = "passtxt"
        Me.passtxt.NullText = "Password:"
        Me.passtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.passtxt.Size = New System.Drawing.Size(100, 24)
        Me.passtxt.TabIndex = 15
        Me.passtxt.TabStop = False
        Me.passtxt.ThemeName = "Office2007Black"
        Me.passtxt.Visible = False
        '
        'Passlbl
        '
        Me.Passlbl.ForeColor = System.Drawing.Color.Red
        Me.Passlbl.Location = New System.Drawing.Point(146, 174)
        Me.Passlbl.Name = "Passlbl"
        Me.Passlbl.Size = New System.Drawing.Size(126, 18)
        Me.Passlbl.TabIndex = 16
        Me.Passlbl.Text = "Password Required --->"
        Me.Passlbl.Visible = False
        '
        'selectedVehicle
        '
        Me.selectedVehicle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.selectedVehicle.Location = New System.Drawing.Point(296, 16)
        Me.selectedVehicle.Name = "selectedVehicle"
        Me.selectedVehicle.Size = New System.Drawing.Size(112, 16)
        Me.selectedVehicle.TabIndex = 17
        Me.selectedVehicle.Text = "Selected Vehicle -->"
        Me.selectedVehicle.Visible = False
        '
        'vehicle_lbl
        '
        Me.vehicle_lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.vehicle_lbl.Location = New System.Drawing.Point(420, 16)
        Me.vehicle_lbl.Name = "vehicle_lbl"
        Me.vehicle_lbl.Size = New System.Drawing.Size(45, 16)
        Me.vehicle_lbl.TabIndex = 18
        Me.vehicle_lbl.Text = "Vehicle"
        Me.vehicle_lbl.Visible = False
        '
        'vehLock_chk
        '
        Me.vehLock_chk.Location = New System.Drawing.Point(488, 16)
        Me.vehLock_chk.Name = "vehLock_chk"
        Me.vehLock_chk.Size = New System.Drawing.Size(43, 18)
        Me.vehLock_chk.TabIndex = 19
        Me.vehLock_chk.Text = "Lock"
        '
        'RadLabel6
        '
        Me.RadLabel6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel6.ForeColor = System.Drawing.Color.Red
        Me.RadLabel6.Location = New System.Drawing.Point(15, 63)
        Me.RadLabel6.Name = "RadLabel6"
        Me.RadLabel6.Size = New System.Drawing.Size(502, 16)
        Me.RadLabel6.TabIndex = 20
        Me.RadLabel6.Text = "By Selecting lock the vehicle shown in the Selected Vehicle will be used as the s" &
    "ending vehicle!"
        Me.RadLabel6.Visible = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "transparent.png")
        '
        'Trackinglbl
        '
        Me.Trackinglbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Trackinglbl.Location = New System.Drawing.Point(146, 46)
        Me.Trackinglbl.Name = "Trackinglbl"
        Me.Trackinglbl.Size = New System.Drawing.Size(51, 16)
        Me.Trackinglbl.TabIndex = 21
        Me.Trackinglbl.Text = "Tracking"
        '
        'Position_Updater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(542, 518)
        Me.Controls.Add(Me.Trackinglbl)
        Me.Controls.Add(Me.RadLabel6)
        Me.Controls.Add(Me.vehLock_chk)
        Me.Controls.Add(Me.vehicle_lbl)
        Me.Controls.Add(Me.selectedVehicle)
        Me.Controls.Add(Me.Passlbl)
        Me.Controls.Add(Me.passtxt)
        Me.Controls.Add(Me.RadWaitingBar1)
        Me.Controls.Add(Me.positonCountlbl)
        Me.Controls.Add(Me.RadLabel5)
        Me.Controls.Add(Me.RadButton1)
        Me.Controls.Add(Me.RadGridView1)
        Me.Controls.Add(Me.EndTime)
        Me.Controls.Add(Me.StartTime)
        Me.Controls.Add(Me.EndDate)
        Me.Controls.Add(Me.StartDate)
        Me.Controls.Add(Me.VehicleDropDown)
        Me.Controls.Add(Me.GetPositons)
        Me.Controls.Add(Me.RadLabel4)
        Me.Controls.Add(Me.RadLabel3)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadLabel1)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximumSize = New System.Drawing.Size(550, 550)
        Me.MinimumSize = New System.Drawing.Size(550, 550)
        Me.Name = "Position_Updater"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.RootElement.MaxSize = New System.Drawing.Size(550, 550)
        Me.ShowIcon = False
        Me.Text = "Position_Updater"
        Me.ThemeName = "Office2007Black"
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GetPositons, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleDropDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StartTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EndTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.positonCountlbl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadWaitingBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.passtxt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Passlbl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.selectedVehicle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vehicle_lbl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vehLock_chk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Trackinglbl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel4 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents GetPositons As Telerik.WinControls.UI.RadButton
    Friend WithEvents VehicleDropDown As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents StartDate As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents EndDate As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents StartTime As Telerik.WinControls.UI.RadTimePicker
    Friend WithEvents EndTime As Telerik.WinControls.UI.RadTimePicker
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel5 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Office2007BlackTheme1 As Telerik.WinControls.Themes.Office2007BlackTheme
    Friend WithEvents positonCountlbl As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadWaitingBar1 As Telerik.WinControls.UI.RadWaitingBar
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents passtxt As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents Passlbl As Telerik.WinControls.UI.RadLabel
    Friend WithEvents selectedVehicle As Telerik.WinControls.UI.RadLabel
    Friend WithEvents vehicle_lbl As Telerik.WinControls.UI.RadLabel
    Friend WithEvents vehLock_chk As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents RadLabel6 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents Trackinglbl As Telerik.WinControls.UI.RadLabel
End Class

