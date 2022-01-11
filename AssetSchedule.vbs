using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

Call ScheduleTask() 

Sub ScheduleTask()
On Error Resume Next

Dim objRequest
Dim URL

Set objRequest = CreateObject("Microsoft.XMLHTTP")
URL = "https://localhost:44371/Asset/EmailExport"

objRequest.open "GET", URL , false

objRequest.Send

Set objRequest = Nothing

End Sub