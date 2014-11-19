Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml

Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        DoThat()
    End Sub


    Private Sub DoThat()

        Dim soapAction As String = ""
        Dim uriString As String = ""
        Dim returnValue As String = ""

        uriString = TextBox2.Text
        soapAction = TextBox3.Text

        Try

            Dim buffer As Byte() = System.Text.Encoding.UTF8.GetBytes(TextBox1.Text)

            ' Setup the request for SOAP
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(New Uri(uriString)), HttpWebRequest)
            request.Credentials = CredentialCache.DefaultCredentials
            request.Method = "POST"
            request.ContentType = "text/xml; charset=utf-8"
            request.Headers.Add([String].Format("SOAPAction: ""{0}""", soapAction))
            request.ContentLength = buffer.Length
            request.ReadWriteTimeout = 10
            request.ProtocolVersion = HttpVersion.Version11

            ' Create the request stream and write the buffer to the stream
            Dim newStream As Stream = request.GetRequestStream()
            newStream.Write(buffer, 0, buffer.Length)
            newStream.Close()

            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            ' Get the stream associated with the response. 
            Dim receiveStream As Stream = response.GetResponseStream()

            ' Pipes the stream to a higher level stream reader with the required encoding format
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)

            ' Write the stream to the textbox control
            TextBox4.Text = readStream.ReadToEnd()

            ' Close the response object
            response.Close()

            ' Close the StreamReader object
            readStream.Close()

        Catch ex As Exception
            returnValue = "Error : " + ex.ToString()
            MsgBox(returnValue, MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        TextBox4.Clear()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        Dim openFileDialog1 As New OpenFileDialog()

        'openFileDialog1.InitialDirectory = "C:\"
        openFileDialog1.Title = "Browse XML Files"

        openFileDialog1.CheckFileExists = True
        openFileDialog1.CheckPathExists = True

        openFileDialog1.DefaultExt = "xml"
        openFileDialog1.Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        openFileDialog1.ReadOnlyChecked = True
        openFileDialog1.ShowReadOnly = True

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            'TextBox1.Text = openFileDialog1.FileName

            Dim fileS As FileStream
            fileS = New FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            'declaring a FileStream to open the file named file.doc with access mode of reading
            Dim i As New StreamReader(fileS)
            'creating a new StreamReader and passing the filestream object fs as argument
            i.BaseStream.Seek(0, SeekOrigin.Begin)
            'Seek method is used to move the cursor to different positions in a file, in this code, to
            'the beginning
            While i.Peek() > -1
                'peek method of StreamReader object tells how much more data is left in the file
                TextBox1.Text &= i.ReadLine()
                'displaying text from doc file in the RichTextBox
            End While
            i.Close()


        End If

    End Sub


    Protected Function FormatXml(xmlNode As Xml.XmlNode) As String
        Dim bob As New StringBuilder()


        ' We will use stringWriter to push the formated xml into our StringBuilder bob.
        Using stringWriter As New StringWriter(bob)
            ' We will use the Formatting of our xmlTextWriter to provide our indentation.
            Using xmlTextWriter As New XmlTextWriter(stringWriter)
                xmlTextWriter.Formatting = Formatting.Indented
                xmlNode.WriteTo(xmlTextWriter)
            End Using
        End Using


        Return bob.ToString()
    End Function

End Class

