<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PTVUpdater
    Inherits Telerik.WinControls.UI.ShapedForm

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
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RadDock1 = New Telerik.WinControls.UI.Docking.RadDock()
        Me.Jobs = New Telerik.WinControls.UI.Docking.ToolWindow()
        Me.RadMenu1 = New Telerik.WinControls.UI.RadMenu()
        Me.RadMenuItem1 = New Telerik.WinControls.UI.RadMenuItem()
        Me.JobsGrid = New Telerik.WinControls.UI.RadGridView()
        Me.ToolTabStrip1 = New Telerik.WinControls.UI.Docking.ToolTabStrip()
        Me.DocumentContainer1 = New Telerik.WinControls.UI.Docking.DocumentContainer()
        Me.ToolTabStrip2 = New Telerik.WinControls.UI.Docking.ToolTabStrip()
        Me.TrackingWindow = New Telerik.WinControls.UI.Docking.ToolWindow()
        CType(Me.RadDock1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadDock1.SuspendLayout()
        Me.Jobs.SuspendLayout()
        CType(Me.RadMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.JobsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.JobsGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ToolTabStrip1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolTabStrip1.SuspendLayout()
        CType(Me.DocumentContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ToolTabStrip2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolTabStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'RadDock1
        '
        Me.RadDock1.ActiveWindow = Me.Jobs
        Me.RadDock1.CausesValidation = False
        Me.RadDock1.Controls.Add(Me.ToolTabStrip1)
        Me.RadDock1.Controls.Add(Me.DocumentContainer1)
        Me.RadDock1.Controls.Add(Me.ToolTabStrip2)
        Me.RadDock1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadDock1.DragDropAllowedDockStates = Telerik.WinControls.UI.Docking.AllowedDockState.Docked
        Me.RadDock1.DragDropMode = Telerik.WinControls.UI.Docking.DragDropMode.Preview
        Me.RadDock1.IsCleanUpTarget = True
        Me.RadDock1.Location = New System.Drawing.Point(0, 0)
        Me.RadDock1.MainDocumentContainer = Me.DocumentContainer1
        Me.RadDock1.Name = "RadDock1"
        Me.RadDock1.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RadDock1.Padding = New System.Windows.Forms.Padding(8)
        '
        '
        '
        Me.RadDock1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.RadDock1.Size = New System.Drawing.Size(535, 526)
        Me.RadDock1.TabIndex = 1
        Me.RadDock1.TabStop = False
        Me.RadDock1.ThemeName = "Office2007Black"
        '
        'Jobs
        '
        Me.Jobs.Caption = Nothing
        Me.Jobs.Controls.Add(Me.RadMenu1)
        Me.Jobs.Controls.Add(Me.JobsGrid)
        Me.Jobs.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!)
        Me.Jobs.Location = New System.Drawing.Point(1, 26)
        Me.Jobs.Margin = New System.Windows.Forms.Padding(4)
        Me.Jobs.Name = "Jobs"
        Me.Jobs.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked
        Me.Jobs.Size = New System.Drawing.Size(517, 230)
        Me.Jobs.Text = "Jobs"
        '
        'RadMenu1
        '
        Me.RadMenu1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadMenuItem1})
        Me.RadMenu1.Location = New System.Drawing.Point(0, 0)
        Me.RadMenu1.Name = "RadMenu1"
        Me.RadMenu1.Size = New System.Drawing.Size(517, 20)
        Me.RadMenu1.TabIndex = 1
        '
        'RadMenuItem1
        '
        Me.RadMenuItem1.Name = "RadMenuItem1"
        Me.RadMenuItem1.Text = "Get Jobs"
        '
        'JobsGrid
        '
        Me.JobsGrid.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.JobsGrid.Location = New System.Drawing.Point(0, 27)
        '
        '
        '
        Me.JobsGrid.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.JobsGrid.Name = "JobsGrid"
        Me.JobsGrid.Size = New System.Drawing.Size(517, 203)
        Me.JobsGrid.TabIndex = 0
        '
        'ToolTabStrip1
        '
        Me.ToolTabStrip1.CanUpdateChildIndex = True
        Me.ToolTabStrip1.CausesValidation = False
        Me.ToolTabStrip1.Controls.Add(Me.Jobs)
        Me.ToolTabStrip1.Location = New System.Drawing.Point(8, 8)
        Me.ToolTabStrip1.Name = "ToolTabStrip1"
        '
        '
        '
        Me.ToolTabStrip1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.ToolTabStrip1.SelectedIndex = 0
        Me.ToolTabStrip1.Size = New System.Drawing.Size(519, 258)
        Me.ToolTabStrip1.SizeInfo.AbsoluteSize = New System.Drawing.Size(764, 258)
        Me.ToolTabStrip1.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, 668)
        Me.ToolTabStrip1.TabIndex = 1
        Me.ToolTabStrip1.TabStop = False
        Me.ToolTabStrip1.ThemeName = "Office2007Black"
        '
        'DocumentContainer1
        '
        Me.DocumentContainer1.Name = "DocumentContainer1"
        Me.DocumentContainer1.Padding = New System.Windows.Forms.Padding(8)
        '
        '
        '
        Me.DocumentContainer1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.DocumentContainer1.SizeInfo.AbsoluteSize = New System.Drawing.Size(764, 2)
        Me.DocumentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill
        Me.DocumentContainer1.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, -1020)
        Me.DocumentContainer1.TabIndex = 2
        Me.DocumentContainer1.ThemeName = "Office2007Black"
        '
        'ToolTabStrip2
        '
        Me.ToolTabStrip2.CanUpdateChildIndex = True
        Me.ToolTabStrip2.Controls.Add(Me.TrackingWindow)
        Me.ToolTabStrip2.Location = New System.Drawing.Point(8, 276)
        Me.ToolTabStrip2.Name = "ToolTabStrip2"
        '
        '
        '
        Me.ToolTabStrip2.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.ToolTabStrip2.SelectedIndex = 0
        Me.ToolTabStrip2.Size = New System.Drawing.Size(519, 242)
        Me.ToolTabStrip2.SizeInfo.AbsoluteSize = New System.Drawing.Size(391, 242)
        Me.ToolTabStrip2.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, 171)
        Me.ToolTabStrip2.TabIndex = 3
        Me.ToolTabStrip2.TabStop = False
        Me.ToolTabStrip2.ThemeName = "Office2007Black"
        '
        'TrackingWindow
        '
        Me.TrackingWindow.Caption = Nothing
        Me.TrackingWindow.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None
        Me.TrackingWindow.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!)
        Me.TrackingWindow.Location = New System.Drawing.Point(1, 26)
        Me.TrackingWindow.Name = "TrackingWindow"
        Me.TrackingWindow.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked
        Me.TrackingWindow.Size = New System.Drawing.Size(517, 214)
        Me.TrackingWindow.Text = "Tracking"
        '
        'PTVUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(535, 526)
        Me.Controls.Add(Me.RadDock1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "PTVUpdater"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = "PTVUpdater"
        CType(Me.RadDock1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadDock1.ResumeLayout(False)
        Me.Jobs.ResumeLayout(False)
        Me.Jobs.PerformLayout()
        CType(Me.RadMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.JobsGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.JobsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ToolTabStrip1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolTabStrip1.ResumeLayout(False)
        CType(Me.DocumentContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ToolTabStrip2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolTabStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadDock1 As Telerik.WinControls.UI.Docking.RadDock
    Friend WithEvents DocumentContainer1 As Telerik.WinControls.UI.Docking.DocumentContainer
    Friend WithEvents ToolTabStrip1 As Telerik.WinControls.UI.Docking.ToolTabStrip
    Friend WithEvents Jobs As Telerik.WinControls.UI.Docking.ToolWindow
    Friend WithEvents TrackingWindow As Telerik.WinControls.UI.Docking.ToolWindow
    Friend WithEvents ToolTabStrip2 As Telerik.WinControls.UI.Docking.ToolTabStrip
    Friend WithEvents JobsGrid As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadMenu1 As Telerik.WinControls.UI.RadMenu
    Friend WithEvents RadMenuItem1 As Telerik.WinControls.UI.RadMenuItem
End Class

