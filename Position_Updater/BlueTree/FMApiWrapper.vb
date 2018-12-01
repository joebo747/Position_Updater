Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Xml
Imports System.Threading
Imports BlueTreeSystems.FleetManagerAPI.WebServiceTestClient.bluetree.fmapi
Imports SampleClient.bluetree.fmapi
Imports BlueTreeTrial.fleetmanager
Imports System.Net

Namespace FMApiNamespace

    ''' <summary>
    ''' This class wraps calls to the webservice and separates the
    ''' webservice related logic from the rest of the application
    ''' </summary>
    Partial Class FMApiWrapper

        Private _usn As String

        Private _pwd As String

        Private _callerContext As Object = Nothing

        ''' <summary>
        ''' If this function returns false then the loop stops otherwise the loop continues.
        ''' </summary>
        ''' <param name="xml">Contains the actual data.</param>
        ''' <param name="lastRowId">The id of the last row in this chunk.</param>
        ''' <param name="bMoreFlag">Specifies if there are currently more chunks of data to be retrieved
        '''     for the executed query.</param>
        ''' <param name="callerContext">the invoker of this class can use this parameter, this reference is
        '''     returned in every callback to the caller</param>
        Public Delegate Function NewChunkCB(ByVal callerContext As Object, ByVal xml As XmlDocument, ByVal lastGenerationNumber As Long, ByVal bMoreFlag As Boolean) As Boolean

        ''' <summary>
        ''' If this function returns false then the loop stops otherwise the loop continues.
        ''' </summary>
        ''' <param name="bServerError">it is true if this error message was communicated from the webservice</param>
        ''' <param name="errorMsg">the error description</param>
        Public Delegate Function ErrorCB(ByVal callerContext As Object, ByVal bServerError As Boolean, ByVal errorMsg As String) As Boolean

        ''' <summary>
        ''' from the bluetree.fmapi namespace, FleetManagerAPI is the
        ''' generated proxy class for the webservice
        ''' </summary>
        Private _fmapi As FleetManagerAPI

        Friend ReadOnly Property FleetManagerAPI As FleetManagerAPI
            Get
                Return Me._fmapi
            End Get
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="usn"></param>
        ''' <param name="pwd"></param>
        ''' <param name="enableCompression"></param>
        ''' <param name="callerContext">Caller gets back a reference in each callback to this object</param>
        Public Sub New(ByVal usn As String, ByVal pwd As String, ByVal enableCompression As Boolean, ByVal callerContext As Object)
            MyBase.New
            If ((usn Is Nothing) _
                        OrElse (pwd Is Nothing)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Me._usn = usn
            Me._pwd = pwd
            Me._callerContext = callerContext
            ' from the bluetree.fmapi namespace, FleetManagerAPI is the
            ' generated proxy class for the webservice
            Me._fmapi = New FleetManagerAPI
            ' The webservice supports gzip compression, it can compress the data that a client retrieves.
            ' (compression in the other way is not supported now)
            ' For this to happen, in the http request header gzip has to be specified.
            ' With the EnableDecompression flag turned on the proxy code will generate the proper
            ' http header and perform the decompression in the lower layer so if you want compression
            ' you can just set this flag and do not need to do anything else.
            Me._fmapi.EnableDecompression = enableCompression
        End Sub

        ''' <summary>
        ''' This function returns all the vehicles in one call.
        ''' </summary>
        ''' <param name="cols">String array of the requested columns.</param>
        ''' <param name="error">The parameter that will contain the error message if something goes wrong</param>
        Public Function GetVehicles(ByVal cols As IEnumerable(Of String), ByRef [error] As String) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                Return Me._fmapi.GetVehicles_v1(Me._usn, Me._pwd, cols.ToArray, "", [error])
            Catch ex As Exception
                [error] = ex.Message
                Return ""
            End Try

        End Function

        ''' <summary>
        ''' This function returns all the vehicles in one call.
        ''' </summary>
        ''' <param name="cols">String array of the requested columns.</param>
        ''' <param name="error">The parameter that will contain the error message if something goes wrong</param>
        Public Function GetVehicleFolders(ByVal cols As IEnumerable(Of String), ByRef [error] As String) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                Return Me._fmapi.GetVehicleFolders_v1(Me._usn, Me._pwd, cols.ToArray, "", [error])
            Catch ex As Exception
                [error] = ex.Message
                Return ""
            End Try

        End Function

        ''' <summary>
        ''' This function returns the status of all vehicles in one call.
        ''' </summary>
        ''' <param name="cols">String array of the requested columns.</param>
        ''' <param name="[error]">The parameter that will contain the [error] message if something goes wrong</param>
        Public Function GetCurrentVehicleStatus(ByVal cols As IEnumerable(Of String), ByRef [error] As String) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                Return Me._fmapi.GetCurrentVehicleStatus_v1(Me._usn, Me._pwd, cols.ToArray, "", [error])
            Catch ex As Exception
                [error] = ex.Message
                Return ""
            End Try

        End Function
        Public Function GetLatestVehicleReadings(ByVal cols As IEnumerable(Of String), ByRef err As String) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                'Dim wReq As HttpWebRequest = DirectCast(WebRequest.Create("http://FleetManagerAPI.bluetree.ie/GetLatestVehicleReadings_v1"), HttpWebRequest)
                'Dim px As IWebProxy = WebRequest.GetSystemWebProxy()
                'wReq.Proxy = px
                'wReq.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials
                'wReq.Credentials = CredentialCache.DefaultNetworkCredentials

                '_fmapi.Credentials.Equals(wReq)


                Return _fmapi.GetLatestVehicleReadings_v1(_usn, _pwd, cols.ToArray, "", err)
            Catch ex As Exception
                err = ex.Message
                'Return 
            End Try

        End Function
        Public Function GetServiceInfo(ByRef errorMsg As String) As String
            Try
                errorMsg = ""
                Return Me._fmapi.GetServiceInfo_v1(Me._usn, Me._pwd, "", errorMsg)
            Catch ex As Exception
                errorMsg = ex.Message
                Return ""
            End Try

        End Function
        ''' <summary>
        ''' Not amongst the original wrapper items.
        ''' </summary>
        ''' <param name="usn"></param>
        ''' <param name="pwd"></param>
        ''' <param name="requestedRowNumber"></param>
        ''' <param name="cols"></param>
        ''' <param name="startTimeStamp"></param>
        ''' <param name="options"></param>
        ''' <param name="lastTimeStamp"></param>
        ''' <param name="errorMsg"></param>
        ''' <param name="ajustedRowNumber"></param>
        ''' <returns></returns>
        Public Function Get_GPSReadings_v1(ByVal usn As String, ByVal pwd As String, ByVal requestedRowNumber As Integer, cols As IEnumerable(Of String), ByVal startTimeStamp As Date, ByVal options As String, ByRef lastTimeStamp As Date, ByRef errorMsg As String, ByRef ajustedRowNumber As Integer) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                Return Me._fmapi.GetGPSReadings_v1(Me._usn, Me._pwd, 1, cols.ToArray, startTimeStamp, "", lastTimeStamp, errorMsg, 1)
            Catch ex As Exception
                errorMsg = ex.Message
                Return ""
            End Try
        End Function
        ''' <summary>
        ''' Not amongst the original wrapper items.
        ''' </summary>
        ''' <param name="usn"></param>
        ''' <param name="pwd"></param>
        ''' <param name="requestedRowNumber"></param>
        ''' <param name="cols"></param>
        ''' <param name="startTimeStamp"></param>
        ''' <param name="options"></param>
        ''' <param name="vehicleID"></param>
        ''' <param name="lastTimeStamp"></param>
        ''' <param name="errorMsg"></param>
        ''' <param name="ajustedRowNumber"></param>
        ''' <returns></returns>
        Public Function GetGpsReadingsForVehicle_v1(ByVal usn As String, ByVal pwd As String, ByVal requestedRowNumber As Integer, cols As IEnumerable(Of String), ByVal startTimeStamp As Date, ByVal options As String, ByVal vehicleID As Integer, ByRef lastTimeStamp As Date, ByRef errorMsg As String, ByRef ajustedRowNumber As Integer) As String
            If ((cols Is Nothing) _
                        OrElse (cols.Count = 0)) Then
                Throw New ArgumentException("invalid input parameter")
            End If

            Try
                Return Me._fmapi.GetGpsReadingsForVehicle_v1(Me._usn, Me._pwd, 1, cols.ToArray, startTimeStamp, "", vehicleID, lastTimeStamp, errorMsg, 1)
            Catch ex As Exception
                errorMsg = ex.Message
                Return ""
            End Try

        End Function
    End Class
End Namespace